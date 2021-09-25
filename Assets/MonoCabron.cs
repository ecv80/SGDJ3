using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoCabron : MonoBehaviour
{
    public GameObject coco;

    float ultimoCocoT;
    float cadenciaTiradaCocos=3f;

    // Start is called before the first frame update
    void Start()
    {
        ultimoCocoT=Time.time;
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

                GameObject cocoInstance=Instantiate<GameObject>(coco, transform.position, Quaternion.identity);
                Coco c=cocoInstance.GetComponent<Coco>();
                c.destino=GameManager.instancia.prota.transform.position; //Somewhere over the rainbow
                c.velocidad=5f;
            }
        }
    }

}
