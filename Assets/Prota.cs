using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prota : MonoBehaviour
{
    public GameObject eslabon=null;

    bool disparando=false;
    bool subiendo=false;
    bool match=false;
    Vector2 matchAt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (match) {
            if (!subiendo)
                StartCoroutine(SubeCadena(transform.position, matchAt));
            return;
        }

        //Input
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos=Camera.main.ScreenToWorldPoint(mousePos);

            StartCoroutine(Dispara(new Vector2(transform.position.x, transform.position.y+transform.localScale.y/2f), mouseWorldPos));
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

        while ((destino-pos).magnitude>incremento.magnitude) {
                
            Instantiate<GameObject>(eslabon, pos, Quaternion.identity);

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

        disparando=false;
        matchAt=destino;
        match=true;
        yield break;
    }

    IEnumerator SubeCadena (Vector2 origen, Vector2 destino) {
        if (subiendo)
            yield break;
        subiendo=true;

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

        match=false;
        subiendo=false;
        yield break;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Eslabon")
            Destroy(other.gameObject);
    }


    
}
