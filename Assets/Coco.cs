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
        if (other.tag=="Eslabon") {
            Destroy(gameObject);

        }

    }
}
