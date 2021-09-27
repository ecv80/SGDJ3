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
using UnityEngine.SceneManagement;
using TMPro;

public class InterludioMonoCabron : MonoBehaviour
{
    public RectTransform molinillo;
    public TextMeshProUGUI texto1;
    public CanvasGroup panel2;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("MuestraTexto1", 2f);
        Invoke("MuestraPanel2", 5f);
        Invoke("VuelveAlJuego", 9f);
    }

    // Update is called once per frame
    void Update()
    {
        molinillo.Rotate(0f, 0f, -50f*Time.deltaTime);
    }

    void MuestraTexto1 () {
        Color c=texto1.color;
        c.a=1f;
        texto1.color=c;
    }
    void MuestraPanel2 () {
        panel2.alpha=1f;
    }
    void VuelveAlJuego() {
        SceneManager.LoadScene("Juego");
    }
}
