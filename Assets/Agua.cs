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

public class Agua : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main==null)
            return;
        
        transform.position=new Vector2(Camera.main.transform.position.x, transform.position.y); //Centrada en la camara siempre

        if (!GameManager.instancia.prota.subiendo && !GameManager.instancia.gameOver)
            transform.position+=Vector3.up*Time.deltaTime*.1f;
    }

    public IEnumerator SubeAgua (float hastaAqui) { //Sube el agua rápidamente hasta donde le digas
        while (transform.position.y<hastaAqui-10f) {
            transform.position+=Vector3.up*Time.deltaTime*1f;
            if (transform.position.y>=hastaAqui-10f)
                transform.position=new Vector2(transform.position.x, hastaAqui-10f);

            yield return 0;
        }

        yield break;
    }
}
