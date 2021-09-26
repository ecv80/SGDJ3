using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameOver {Ahogado, Coco, Ganado};

public class GameManager : MonoBehaviour
{
    public Prota prota;
    public Agua agua;
    public TextMeshProUGUI gameOverText;

    public GameObject monoCabron;
    public GameObject plataforma;

    float camZ;

    Vector2 plataformaMasAlta=new Vector2(0f, -5f);
    Vector2 plataformaMasDcha=new Vector2(20f, -5f);
    Vector2 plataformaMasIzqda=new Vector2(-20f, -5f);

    public int plataformasPorSubir=0;

    public bool gameOver=false;

    int nivel=1;


    public static GameManager instancia=null;

    void Awake() { 
        if (!instancia) {
            instancia=this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(prota.gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        camZ=transform.position.z;

        //Crea plataformas
        AnadePlataformas(20, prota.transform.position);

        plataformasPorSubir=nivel;

        //Nivel
        gameOver=true; //bloquear
        gameOverText.text="Nivel "+nivel;
        gameOverText.gameObject.SetActive(true);
        Invoke("Desbloquear", 2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {

        //Seguir al prota
        Vector3 destino=new Vector3(prota.transform.position.x, prota.transform.position.y+4.5f, camZ); 
        transform.position+=(destino-transform.position)*Time.deltaTime*3f;

        //Resetear plataforma mas alta, segun se actualiza el eje x de la camara
        if (plataformaMasAlta.x<transform.position.x-10f || 
            plataformaMasAlta.x>transform.position.x+10f)
                plataformaMasAlta=transform.position;

        //Añadir plataformas si eso
        if (transform.position.y>plataformaMasAlta.y-10f) { //mejor 10 que 5, por dar más margen
            AnadePlataformas(20, new Vector2(transform.position.x, transform.position.y+5f));
            //Si se atraganta, la convertimos en corutina luego
            //pero antes a ver que tal funciona con pooling
        }

        //Resetear plataformas mas a la dcha e izqda, segun se actualiza la altura de la camara
        if (plataformaMasDcha.y<transform.position.y-5f)
            plataformaMasDcha=transform.position;
        if (plataformaMasIzqda.y<transform.position.y-5f)
            plataformaMasIzqda=transform.position;

        //Añadir plataformas a los lados si eso
        if (transform.position.x>plataformaMasDcha.x-20f)
            AnadePlataformas(20, new Vector2(transform.position.x+30f, transform.position.y-5f));
        if (transform.position.x<plataformaMasIzqda.x+20f)
            AnadePlataformas(20, new Vector2(transform.position.x-30f, transform.position.y-5f));

    }

    void AnadePlataformas (int cantidadPorTramo, Vector2 origen, int tramos=2) {
        float halfAncho=plataforma.transform.localScale.x/2f;
        float halfAlto=plataforma.transform.localScale.y/2f;

        //Siempre debe haber una plataforma por cada 10 de altura para que siempre haya una visible
        //en pantalla en todo momento, por lo que voy a partir la instanciación en trozos de a 10.
        for (int tramo=0; tramo<tramos; tramo++) {
            for (int i=0; i<cantidadPorTramo; i++) {
                Vector2 pos=new Vector2(Random.Range(origen.x-20f+halfAncho, origen.x+20f-halfAncho), Random.Range(origen.y+tramo*10f+1.25f+halfAlto, origen.y+(tramo+1)*10f-halfAlto));
                GameObject plat=Instantiate<GameObject>(plataforma, pos, Quaternion.identity);
                List<Collider2D> colliders=new List<Collider2D>();
                int plts=plat.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
                bool sigue=false;
                for (int j=0; j<plts; j++)
                    if (colliders[j].tag=="Plataforma") {
                        Destroy(plat);
                        sigue=true;
                        break;
                    }
                if (sigue)
                    continue;

                if (pos.y>plataformaMasAlta.y)
                    plataformaMasAlta=pos;
                if (pos.x>plataformaMasDcha.x)
                    plataformaMasDcha=pos;
                if (pos.x<plataformaMasIzqda.x)
                    plataformaMasIzqda=pos;

                //Poner a un mono cabron?
                if (Random.value<1f/20f) {
                    Instantiate<GameObject>(monoCabron, new Vector2(pos.x, pos.y+halfAlto+monoCabron.transform.localScale.y/2f), Quaternion.identity);
                }
                    

                //No vamos a reinstanciar indefinidamente las plataformas hasta dar con lugares donde no solapen a otras
                //porque así tendremos más variabilidad a costa de ningún recurso más
            }
        }
    }

    public void Fin(GameOver razon) {
        if (gameOver)
            return;
        gameOver=true;

        switch(razon) {
            case GameOver.Ahogado:
                gameOverText.text="AHOGADO";
                gameOverText.gameObject.SetActive(true);
            break;
            case GameOver.Coco:
                gameOverText.text="COCAZO t'has llevao, hamijo";
                gameOverText.gameObject.SetActive(true);
            break;
            case GameOver.Ganado:
                gameOverText.text="¡BRAVO!";
                gameOverText.gameObject.SetActive(true);
                Invoke("SiguienteNivel", 2f);
                return;
            break;
            default:
            break;
        }

        //Ir al inicio
        Invoke("Inicio", 2f);

    }

    void Inicio() {
        SceneManager.LoadScene("Inicio");
        Destroy(Camera.main.gameObject);
        Destroy(prota.gameObject);
    }

    void SiguienteNivel () {
        nivel++;
        SceneManager.LoadScene("Juego");
    }

    void Desbloquear() {
        gameOver=false;
        gameOverText.gameObject.SetActive(false);
    }

}
