using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prota;
    public GameObject plataforma;

    float camZ;

    float plataformaMasAlta=-5f;

    // Start is called before the first frame update
    void Start()
    {
        camZ=transform.position.z;

        //Crea plataformas
        AnadePlataformas(20, -5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {

        //Seguir al prota
        Vector3 destino=new Vector3(prota.transform.position.x, prota.transform.position.y+4.5f, camZ); 
        transform.position+=(destino-transform.position)*Time.deltaTime*3f;
    }

    void AnadePlataformas (int cantidad, float base_, float altura=20f) {
        float halfAncho=plataforma.transform.localScale.x/2f;
        float halfAlto=plataforma.transform.localScale.y/2f;

        for (int i=0; i<cantidad; i++) {
            Vector2 pos=new Vector2(Random.Range(-9f+halfAncho, 9f-halfAncho), Random.Range(base_+1.25f+halfAlto, altura-halfAlto));
            GameObject plat=Instantiate<GameObject>(plataforma, pos, Quaternion.identity);
            List<Collider2D> colliders=new List<Collider2D>();
            int plts=plat.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
            for (int j=0; j<plts; j++)
                if (colliders[j].tag=="Plataforma") {
                    Destroy(plat);
                    return;
                }
            if (pos.y>plataformaMasAlta)
                plataformaMasAlta=pos.y;

            //No vamos a reinstanciar indefinidamente las plataformas hasta dar con lugares donde no solapen a otras
            //porque así tendremos más variabilidad a costa de ningún recurso más
        }
    }
}
