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

public class MonoCabron : MonoBehaviour
{
    public GameObject coco;

    float ultimoCocoT;
    float cadenciaTiradaCocos=5f;

    // Start is called before the first frame update
    void Start()
    {
        cadenciaTiradaCocos+=cadenciaTiradaCocos/10f*Random.Range(-1f, 1f);
        ultimoCocoT=Time.time-cadenciaTiradaCocos; //Empieza a tirar cocos inmediatamente
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // print("OnTriggerStay2D");

        if (other.tag=="Prota") {//A hacer el cabrón lanzando cocos
            // print("Prota");
            if (Time.time>ultimoCocoT+cadenciaTiradaCocos) {//Han pasado mas de cadenciaTiradaCocos segundos desde la ultima tirada de cocos?
                ultimoCocoT=Time.time;

                LanzaCoco();
                float retraso2aTirada=Random.Range(cadenciaTiradaCocos/6f, cadenciaTiradaCocos/3f);
                Invoke("LanzaCoco", retraso2aTirada);
                Invoke("LanzaCoco", retraso2aTirada+Random.Range(cadenciaTiradaCocos/6f, cadenciaTiradaCocos/3f));

            }
        }
    }

    void LanzaCoco() {
        //Lanzar en direcciones inexactas
        float toleranciaAngulo=30f; //Desvío de +- toleranciaAngulo
        Vector2 destinoOriginal=GameManager.instancia.prota.transform.position;

        Vector2 destino=destinoOriginal-(Vector2)transform.position;
        
        if (destino.y>0) //Cocos hacia arriba, jamás
            return;

        destino=Quaternion.Euler(0f, 0f, Random.Range(-toleranciaAngulo, toleranciaAngulo))*destino;
        destino=(Vector2)transform.position+destino;

        GameObject cocoInstance=Instantiate<GameObject>(coco, transform.position, Quaternion.identity);
        Coco c=cocoInstance.GetComponent<Coco>();
        c.direccion=(destino-(Vector2)transform.position).normalized; //Somewhere p'ahi_p'allá (la i sin tilde)
        c.velocidad=Random.Range(2f, 8f);
    }

}
