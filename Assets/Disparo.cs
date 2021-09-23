using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject eslabon=null;

    bool disparando=false;
    int disparo=0;

    // Start is called before the first frame update
    void Start()
    {
        //Llena diagonal pantalla de eslabones
        if (eslabon==null)
            return;
        
        Vector2 maxCoords=new Vector2(9f,5f);
        Vector2 minCoords=new Vector2(-9f,-5f);

        StartCoroutine(Dispara(minCoords, maxCoords));
        
    }

    // Update is called once per frame
    void Update()
    {
        if (disparando==false)
            switch(disparo) {
                case 0:
                    StartCoroutine(Dispara(new Vector2(-9f, -5f), new Vector2(9f, -5f)));
                    disparo++;
                    break;
                case 1:
                    StartCoroutine(Dispara(new Vector2(-9f, -5f), new Vector2(-9f, 5f)));
                    disparo++;
                    break;
                case 2:
                    StartCoroutine(Dispara(new Vector2(9f, 5f), new Vector2(-9f, 5f)));
                    disparo++;
                    break;
                default:
                    break;
            }

        //Input
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos=Camera.main.ScreenToWorldPoint(mousePos);

            StartCoroutine(Dispara(new Vector2(0f, -5f), mouseWorldPos));
        }
        
    }

    IEnumerator Dispara (Vector2 origen, Vector2 destino, int eslabonesPorFrame=10) {
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
        yield break;
    }

    
}
