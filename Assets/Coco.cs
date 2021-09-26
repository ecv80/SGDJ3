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

public class Coco : MonoBehaviour
{
    public Vector2 direccion=Vector2.zero;
    public float velocidad=1f;

    float nacimiento=0f;
    float expectativaVida=100f;

    // Start is called before the first frame update
    void Start()
    {
        nacimiento=Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time>nacimiento+expectativaVida) { //Has vivido demasiado, hamijo
            Destroy(gameObject);
            return;
        }

        //Vas p'ande t'an dicho y punto
        transform.position+=(Vector3)direccion*Time.deltaTime*velocidad;
    }

    void OnTriggerEnter2D (Collider2D other) {
        switch(other.tag) {
            case "Eslabon":
                Destroy(gameObject);
            break;
            case "Prota":
                if (GameManager.instancia.prota.efectoInvulnerable==false)
                    GameManager.instancia.Fin(GameOver.Coco);
            break;
            default:
            break;

        }

    }
}
