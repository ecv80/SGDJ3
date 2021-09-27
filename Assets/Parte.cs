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

public class Parte : MonoBehaviour
{
    public SpriteRenderer p00;
    public SpriteRenderer p01;
    public SpriteRenderer p02;
    public SpriteRenderer p10;
    public SpriteRenderer p11;
    public SpriteRenderer p12;
    public SpriteRenderer p20;
    public SpriteRenderer p21;
    public SpriteRenderer p22;

    public bool[,] matriz; //Como de costumbre, respetando estrictamente las buenas
                            //prácticas de programación orientada a objetos ¬_¬

    Color invisible=new Color(0f, 0f, 0f, 0f);
    Color visible=new Color(0f, 0f, 0f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AsignaMatriz (bool[,] m) {
        if (m.GetLength(0)!=3 || m.GetLength(1)!=3)
            return;

        matriz=m;
        //Unity no muestra arrays multidimensionales en el inspector
        //y me da pereza ajustar de bidimensional a unidimensional solo pa esto
        p00.color=m[0,0]?visible:invisible;
        p01.color=m[0,1]?visible:invisible;
        p02.color=m[0,2]?visible:invisible;
        p10.color=m[1,0]?visible:invisible;
        p11.color=m[1,1]?visible:invisible;
        p12.color=m[1,2]?visible:invisible;
        p20.color=m[2,0]?visible:invisible;
        p21.color=m[2,1]?visible:invisible;
        p22.color=m[2,2]?visible:invisible;
    }
}
