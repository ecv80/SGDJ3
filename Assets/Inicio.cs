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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour
{

    public TextMeshProUGUI start;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFlash());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;

            List <RaycastResult> hits=new List<RaycastResult>();
            EventSystem.current.RaycastAll(ped, hits);
            foreach (RaycastResult r in hits) {
                if (r.gameObject.name=="Start") {
                    //Empieza el juego
                    SceneManager.LoadScene("Juego");
                }
            }
        }
    }

    IEnumerator StartFlash() {
        Color visible=start.color;
        visible.a=1f;
        Color invisible=start.color;
        invisible.a=0f;

        yield return new WaitForSeconds(2); //Suspense tikitikitikitikitikiiii....

        while (true) {
            start.color=visible;
            yield return new WaitForSeconds(.75f);
            start.color=invisible;
            yield return new WaitForSeconds(.50f);

        }

    }
}
