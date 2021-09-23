using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject plataforma;

    // Start is called before the first frame update
    void Start()
    {
        //Crea plataformas
        float halfAncho=plataforma.transform.localScale.x/2f;
        float halfAlto=plataforma.transform.localScale.y/2f;

        for (int i=0; i<10; i++) {
            Vector2 pos=new Vector2(Random.Range(-9f+halfAncho, 9f-halfAncho), Random.Range(-3.75f+halfAlto, 5f-halfAlto));
            GameObject plat=Instantiate<GameObject>(plataforma, pos, Quaternion.identity);
            List<Collider2D> colliders=new List<Collider2D>();
            int plts=plat.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
            for (int j=0; j<plts; j++)
                if (colliders[j].tag=="Plataforma")
                    Destroy(colliders[j].gameObject);

            //No vamos a reinstanciar indefinidamente las plataformas hasta dar con lugares donde no solapen a otras
            //porque así tendremos más variabilidad a costa de ningún recurso más
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
