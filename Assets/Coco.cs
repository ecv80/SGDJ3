using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coco : MonoBehaviour
{
    public Vector3 destino=Vector3.zero;
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
        transform.position+=(destino-transform.position).normalized*Time.deltaTime*velocidad;
    }
}
