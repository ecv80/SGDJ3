using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject eslabon=null;

    // Start is called before the first frame update
    void Start()
    {
        //Llena diagonal pantalla de eslabones
        if (eslabon==null)
            return;
        
        Vector2 maxCoords=new Vector2(9f,5f);
        Vector2 minCoords=new Vector2(-9f,-5f);

        Vector2 pos=minCoords;
        Vector2 angulo=(maxCoords-minCoords).normalized;
        Vector2 incremento=angulo*eslabon.transform.localScale.x;

        int contador=0;

        while (pos.x<maxCoords.x || pos.y<minCoords.y) {
                
            Instantiate<GameObject>(eslabon, pos, Quaternion.identity);

            contador++;

            pos+=incremento;
        }

        print(contador+" eslabones.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
