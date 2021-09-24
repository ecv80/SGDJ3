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
        AnadePlataformas(10, -5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {

        //Seguir al prota
        Vector3 destino=new Vector3(prota.transform.position.x, prota.transform.position.y+4.5f, camZ); 
        transform.position+=(destino-transform.position)*Time.deltaTime*3f;

        //Añadir plataformas si eso
        if (transform.position.y>plataformaMasAlta-10f) { //mejor 10 que 5, por dar más margen
            AnadePlataformas(10, transform.position.y+5f);
            //Si se atraganta, la convertimos en corutina luego
            //pero antes ver que tal funciona con pooling
        }
    }

    void AnadePlataformas (int cantidadPorTramo, float base_, int tramos=2) {
        float halfAncho=plataforma.transform.localScale.x/2f;
        float halfAlto=plataforma.transform.localScale.y/2f;

        //Siempre debe haber una plataforma por cada 10 de altura para que siempre haya una visible
        //en pantalla en todo momento, por lo que voy a partir la instanciación en trozos de a 10.
        for (int tramo=0; tramo<tramos; tramo++) {
            for (int i=0; i<cantidadPorTramo; i++) {
                Vector2 pos=new Vector2(Random.Range(prota.transform.position.x-9f+halfAncho, prota.transform.position.x+9f-halfAncho), Random.Range(base_+tramo*10f+1.25f+halfAlto, base_+(tramo+1)*10f-halfAlto));
                GameObject plat=Instantiate<GameObject>(plataforma, pos, Quaternion.identity);
                List<Collider2D> colliders=new List<Collider2D>();
                int plts=plat.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
                for (int j=0; j<plts; j++)
                    if (colliders[j].tag=="Plataforma") {
                        Destroy(plat);
                        continue;
                    }
                if (pos.y>plataformaMasAlta)
                    plataformaMasAlta=pos.y;

                //No vamos a reinstanciar indefinidamente las plataformas hasta dar con lugares donde no solapen a otras
                //porque así tendremos más variabilidad a costa de ningún recurso más
            }
        }
    }
}
