//Copyright © 2021- Eneko Castresana Vara. All rights reserved.
//License: No, you may NOT use any parts of the present and related work, including code and assets,
//for any purpose other than inspection of the work itself, without my express permission. 
//If you infringe this license your soul will burn in hell forever. 🔥😀👍
//
//Si te gusta lo que hago, contáctame a [X]ecv80vit[X]@[X]gmail.com[X] (sin las X ni corchetes)
//y dame trabajo. Mejor de videojuegos, pero cualquier trabajo de programación está bien.
//No dudes en contactarme si quieres que participe en un proyecto aunque sea sin sueldo
//pero con vistas a vender y repartir beneficios.
//Si quieres que participe contigo en una jam, pues lo mismo, me contactas.
//Gracias.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prota : MonoBehaviour
{
    public GameObject eslabon=null;

    bool disparando=false;
    public bool subiendo=false;
    bool match=false;
    Vector2 matchAt;
    public float tiempoInvulnerable=0f;
    public bool efectoInvulnerable=false;

    public BoxCollider2D detectorCerraduras;

    float protaHalfAlto=0f;

    List<GameObject> eslabonesDisparo=new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        protaHalfAlto=transform.localScale.y/2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instancia.gameOver) {

        }
        else {
            if(!efectoInvulnerable) { //Muerte?
                if (GameManager.instancia.agua.transform.position.y+10f>transform.position.y+protaHalfAlto+.5f) { //Ahogado
                    GameManager.instancia.Fin(GameOver.Ahogado);
                }
            }

            if (!disparando && match) {
                if (!subiendo)
                    StartCoroutine(SubeCadena(transform.position, matchAt));
                    StartCoroutine(GameManager.instancia.agua.SubeAgua(matchAt.y+GameManager.instancia.plataforma.transform.localScale.y/2f));
                return;
            }

            //Input
            if (Input.GetButtonDown("Fire1"))
            {
                if (disparando)
                    return;
                
                Vector3 mousePos = Input.mousePosition;
                Vector3 mouseWorldPos=Camera.main.ScreenToWorldPoint(mousePos);         

                List <RaycastHit2D> hits=new List<RaycastHit2D>();
                int h = Physics2D.Raycast( mouseWorldPos, Vector2.zero, new ContactFilter2D().NoFilter(), hits);
                bool collidersRelevantes=false;
                for (int i=0; i<h; i++) {
                    if (hits[i].collider.tag=="Plataforma") {
                        collidersRelevantes=true;
                        //Aquí irían comprobaciones de que es la plataforma correcta
                        matchAt=hits[i].transform.position;
                        match=true;
                        StartCoroutine(Dispara(new Vector2(transform.position.x, transform.position.y+transform.localScale.y/2f), matchAt));
                    }
                }
                if (!collidersRelevantes) {//Disparo al aire
                    StartCoroutine(Dispara(new Vector2(transform.position.x, transform.position.y+transform.localScale.y/2f), mouseWorldPos));
                }
            }
        }
        
    }

    IEnumerator Dispara (Vector2 origen, Vector2 destino, int eslabonesPorFrame=10) {
        if (eslabon==null) //asi no
            yield break;
        if (disparando) //ignorar
            yield break;
        disparando=true;

        Vector2 pos=origen;
        Vector2 angulo=(destino-origen).normalized;
        Vector2 incremento=angulo*eslabon.transform.localScale.x;

        // int contador=0;

        int epf=eslabonesPorFrame;
        eslabonesDisparo.Clear();

        while ((destino-pos).magnitude>incremento.magnitude) {
                
            GameObject esl=Instantiate<GameObject>(eslabon, pos, Quaternion.identity);
            eslabonesDisparo.Add(esl);

            // contador++;

            pos+=incremento;
            epf--;

            // yield return new WaitForSeconds(cadencia);

            if (epf<=0) {
                epf=eslabonesPorFrame;
                yield return 0;
            }
        }

        // print(contador+" eslabones.");

        //Si no hay match, destruir eslabones del disparo al cabo de un tiempo
        if (match)
            disparando=false;
        else {
            Invoke("QuitaEslabones", .2f);
        }

        yield break;
    }

    IEnumerator SubeCadena (Vector2 origen, Vector2 destino) {
        if (subiendo)
            yield break;
        subiendo=true;
        tiempoInvulnerable+=1f;
        StartCoroutine(EfectoInvulnerable());

        Vector2 pos=origen;
        Vector2 angulo=(destino-origen).normalized;
        
        float lastDiff=(destino-pos).sqrMagnitude;
        while (true) {
            pos+=angulo*Time.deltaTime*20f; //Parece que a veces cuando el prota se pasa del limite de destino, aparece brevemente en pantalla, aunque no deberia...

            if ((destino-pos).sqrMagnitude>lastDiff) //que nos pasamos de vueltas
                pos=destino;
            
            transform.position=pos;

            if (pos==destino)
                break;

            lastDiff=(destino-pos).sqrMagnitude;       

            yield return 0;

        } //while (lastDiff>(lastDiff=(destino-pos).sqrMagnitude));

        //Una vez subida la cadena, se sube a la plataforma
        float platHalfAlto=GameManager.instancia.plataforma.transform.localScale.y/2f;
        //Salto
        while (transform.position.y<destino.y+platHalfAlto+protaHalfAlto+.5f) {
            transform.position=new Vector2(transform.position.x, transform.position.y+Time.deltaTime*20f);
            yield return 0;
        }
        //Aterrizaje
        while (transform.position.y>destino.y+platHalfAlto+protaHalfAlto) {
            transform.position=new Vector2(transform.position.x, transform.position.y-Time.deltaTime*10f);
            if (transform.position.y<=destino.y+platHalfAlto+protaHalfAlto) {
                transform.position=new Vector2(transform.position.x, destino.y+platHalfAlto+protaHalfAlto);
                break;
            }
            yield return 0;
        }
        tiempoInvulnerable=1f;
        GameManager.instancia.plataformasPorSubir--;
        GameManager.instancia.barraProgreso.value++;
        GameManager.instancia.textoProgreso.text=GameManager.instancia.barraProgreso.value+"/"+GameManager.instancia.barraProgreso.maxValue;
        if (GameManager.instancia.plataformasPorSubir<=0) {//Hemos ganado?
            GameManager.instancia.Fin(GameOver.Ganado);
        }

        //Actualizar garfio-llaves
        GameManager.instancia.GeneraGarfioLlaves(DetectaCerraduras());

        match=false;
        subiendo=false;
        yield break;
    }

    void QuitaEslabones() {
        foreach(GameObject esl in eslabonesDisparo)
            Destroy(esl);
        eslabonesDisparo.Clear();

        disparando=false;
    }

    public IEnumerator EfectoInvulnerable () {
        if (efectoInvulnerable)
            yield break;
        efectoInvulnerable=true;
        
        SpriteRenderer spr=GetComponent<SpriteRenderer>();
        Color visible=spr.color;
        Color invisible=visible;
        invisible.a=.25f;
        float tiempoInvisible=.05f;
        float tiempoVisible=.1f;

        while (tiempoInvulnerable>0f) {
            spr.color=invisible;
            yield return new WaitForSeconds(tiempoInvisible);

            spr.color=visible;
            yield return new WaitForSeconds(tiempoVisible);
            tiempoInvulnerable-=tiempoInvisible+tiempoVisible;
        }
        
        efectoInvulnerable=false;
        yield break;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Eslabon")
            Destroy(other.gameObject);
    }

    public List<bool[,]> DetectaCerraduras() {
        List<bool[,]> cerraduras=new List<bool[,]>();
        List<Collider2D> resultados=new List<Collider2D>();
        detectorCerraduras.OverlapCollider(new ContactFilter2D().NoFilter(), resultados);
        foreach(Collider2D col in resultados) {
            if (col.tag=="Union") {
                cerraduras.Add(col.GetComponent<Parte>().matriz);
            }
        }

        return cerraduras;
    }

    
}
