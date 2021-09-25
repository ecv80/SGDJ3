﻿using System.Collections;
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
        ultimoCocoT=Time.time;
        cadenciaTiradaCocos+=cadenciaTiradaCocos/10f*Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag=="Prota") {//A hacer el cabrón lanzando cocos
            if (Time.time>ultimoCocoT+cadenciaTiradaCocos) {//Han pasado mas de cadenciaTiradaCocos segundos desde la ultima tirada de cocos?
                ultimoCocoT=Time.time;

                //TO-DO:
                //4. morir por cocazo

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
        destino=Quaternion.Euler(0f, 0f, Random.Range(-toleranciaAngulo, toleranciaAngulo))*destino;
        destino=(Vector2)transform.position+destino;

        GameObject cocoInstance=Instantiate<GameObject>(coco, transform.position, Quaternion.identity);
        Coco c=cocoInstance.GetComponent<Coco>();
        c.direccion=(destino-(Vector2)transform.position).normalized; //Somewhere p'ahi_p'allá (la i sin tilde)
        c.velocidad=Random.Range(2f, 8f);
    }

}
