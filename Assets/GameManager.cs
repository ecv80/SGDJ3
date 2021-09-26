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
    public TextMeshProUGUI texto;

    Color textoVisible;
    Color textoInvisible;

    public GameObject monoCabron;
    public GameObject plataforma;

    float camZ;

    Vector2 plataformaMasAlta=new Vector2(0f, -5f);
    Vector2 plataformaMasDcha=new Vector2(20f, -5f);
    Vector2 plataformaMasIzqda=new Vector2(-20f, -5f);

    public int plataformasPorSubir=0;

    public bool gameOver=false;

    int nivel=1;

    //Aqui va el mapeado de pantallas des/pobladas con plataformas
    //Para agilizar codigo y rendimiento, podría hacerlo con un simple array bidimensional de bools, 
    //de 10mil x 10mil puesto que es improbable que el usuario llegue tan lejos. El problema es que
    //los bool ocupan un byte entero en C# y resulta en una cantidad ingente de ram. 
    //No me parece aceptable. Así que opto por hacerlo con listas de objetos.
    //En lugar de usar una gran lista con objetos de todas las pantallas con sus correspondientes
    //coordenadas, para agilizar la busqueda voy a utilizar una lista de filas por altura y otra
    //de pantallas por fila.
    //La mera existencia de una pantalla, implica el hecho de que ha sido poblada con plataformas.
    public class Pantalla {
        public float x; 
        //La pantalla de inicio está en x=0
        //La pantalla de la izquierda, quedaría en x=-tamaño camara(altura)*ratio camara*2
        //La pantalla de la derecha, quedaría en x=tamaño camara(altura)*ratio camara*2
    }
    public class Fila {
        public float y;
        public List<Pantalla> pantallas=new List<Pantalla>();
        //La fila de inicio está en y=0
        //La fila de arriba está en y=tamaño camara(altura)*2
    }
    List<Fila> filas=new List<Fila>();


    public static GameManager instancia=null;

    void Awake() { 
        if (!instancia) {
            instancia=this;
            SceneManager.sceneLoaded+=OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void OnSceneLoaded(Scene escena, LoadSceneMode modo) {
        //Buscar referencias (los asignados en el inspector que no sean prefab)
        prota=FindObjectOfType<Prota>();
        agua=FindObjectOfType<Agua>();
        texto=GameObject.Find("Texto").GetComponent<TextMeshProUGUI>();

        transform.position=new Vector3(0f, 0f, -10f);

        textoVisible=texto.color;
        textoVisible.a=1f;
        textoInvisible=texto.color;
        textoInvisible.a=0f;

        camZ=transform.position.z;

        //Crea plataformas
        // AnadePlataformas(20, prota.transform.position);
        filas.Clear();
        Poblar(10, prota.transform.position);

        plataformasPorSubir=nivel;

        //Nivel
        gameOver=true; //bloquear
        texto.text="Nivel "+nivel;
        texto.color=textoVisible;

        Invoke("Desbloquear", 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {

        //Seguir al prota
        Vector3 destino=new Vector3(prota.transform.position.x, prota.transform.position.y+4.5f, camZ); 
        transform.position+=(destino-transform.position)*Time.deltaTime*3f;

        AnadirPlataformasAlrededor(10, transform.position);

        // //Resetear plataforma mas alta, segun se actualiza el eje x de la camara
        // if (plataformaMasAlta.x<transform.position.x-10f || 
        //     plataformaMasAlta.x>transform.position.x+10f)
        //         plataformaMasAlta=transform.position;

        // //Añadir plataformas si eso
        // if (transform.position.y>plataformaMasAlta.y-10f) { //mejor 10 que 5, por dar más margen
        //     AnadePlataformas(20, new Vector2(transform.position.x, transform.position.y+5f));
        //     //Si se atraganta, la convertimos en corutina luego
        //     //pero antes a ver que tal funciona con pooling
        // }

        // //Resetear plataformas mas a la dcha e izqda, segun se actualiza la altura de la camara
        // if (plataformaMasDcha.y<transform.position.y-5f)
        //     plataformaMasDcha=transform.position;
        // if (plataformaMasIzqda.y<transform.position.y-5f)
        //     plataformaMasIzqda=transform.position;

        // //Añadir plataformas a los lados si eso
        // if (transform.position.x>plataformaMasDcha.x-20f)
        //     AnadePlataformas(20, new Vector2(transform.position.x+30f, transform.position.y-5f));
        // if (transform.position.x<plataformaMasIzqda.x+20f)
        //     AnadePlataformas(20, new Vector2(transform.position.x-30f, transform.position.y-5f));

    }

    // void AnadePlataformas (int cantidadPorTramo, Vector2 origen, int tramos=2) {
    //     float halfAncho=plataforma.transform.localScale.x/2f;
    //     float halfAlto=plataforma.transform.localScale.y/2f;

    //     //Siempre debe haber una plataforma por cada 10 de altura para que siempre haya una visible
    //     //en pantalla en todo momento, por lo que voy a partir la instanciación en trozos de a 10.
    //     for (int tramo=0; tramo<tramos; tramo++) {
    //         for (int i=0; i<cantidadPorTramo; i++) {
    //             Vector2 pos=new Vector2(Random.Range(origen.x-20f+halfAncho, origen.x+20f-halfAncho), Random.Range(origen.y+tramo*10f+1.25f+halfAlto, origen.y+(tramo+1)*10f-halfAlto));
    //             GameObject plat=Instantiate<GameObject>(plataforma, pos, Quaternion.identity);
    //             List<Collider2D> colliders=new List<Collider2D>();
    //             int plts=plat.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
    //             bool sigue=false;
    //             for (int j=0; j<plts; j++)
    //                 if (colliders[j].tag=="Plataforma") {
    //                     Destroy(plat);
    //                     sigue=true;
    //                     break;
    //                 }
    //             if (sigue)
    //                 continue;

    //             if (pos.y>plataformaMasAlta.y)
    //                 plataformaMasAlta=pos;
    //             if (pos.x>plataformaMasDcha.x)
    //                 plataformaMasDcha=pos;
    //             if (pos.x<plataformaMasIzqda.x)
    //                 plataformaMasIzqda=pos;

    //             //Poner a un mono cabron?
    //             if (Random.value<1f/20f) {
    //                 Instantiate<GameObject>(monoCabron, new Vector2(pos.x, pos.y+halfAlto+monoCabron.transform.localScale.y/2f), Quaternion.identity);
    //             }
                    

    //             //No vamos a reinstanciar indefinidamente las plataformas hasta dar con lugares donde no solapen a otras
    //             //porque así tendremos más variabilidad a costa de ningún recurso más
    //         }
    //     }
    // }

    public void Fin(GameOver razon) {
        if (gameOver)
            return;
        gameOver=true;

        switch(razon) {
            case GameOver.Ahogado:
                texto.text="AHOGADO";
                texto.color=textoVisible;
            break;
            case GameOver.Coco:
                texto.text="COCAZO t'has llevao, hamijo";
                texto.color=textoVisible;
            break;
            case GameOver.Ganado:
                texto.text="¡BRAVO!";
                texto.color=textoVisible;
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
        prota.tiempoInvulnerable=3f;
        StartCoroutine(prota.EfectoInvulnerable());
        gameOver=false;
        texto.color=textoInvisible;
    }

    void OnDestroy() {
        SceneManager.sceneLoaded-=OnSceneLoaded;
    }

    bool Poblado (Vector2 pos) { //Comprueba si la pantalla de esas coordenadas está poblada
        foreach(Fila f in filas) {
            Vector2 lim=limitesFila(f.y);
            if (pos.y>=lim.x && pos.y<lim.y) {
                foreach(Pantalla p in f.pantallas) {
                    lim=limitesPantalla(p.x);
                    if (pos.x>=lim.x && pos.x<lim.y)
                        return true;
                }
                break;
            }
        }
        return false;
    }

    void Poblar (int cantidadPorPantalla, Vector2 origen) {
        if (Poblado(origen)) //Si ya esta poblada, no la repobles, joé
            return;
        Fila f=null;
        foreach(Fila fi in filas) { //Existe la fila? Si no, crearla
            Vector2 lim=limitesFila(fi.y);
            if (origen.y>=lim.x && origen.y<lim.y) { //La fila existe
                f=fi; 
                break;
            }
        }

        Pantalla p=new Pantalla();
        p.x=centraCoordPantalla(origen.x);

        if (f==null) {
            f=new Fila();
            f.y=centraCoordFila(origen.y);
            filas.Add(f);
        }
        f.pantallas.Add(p);
        
        Vector2 limX=limitesPantalla(origen.x);
        Vector2 limY=limitesFila(origen.y);
        
        float halfAncho=plataforma.transform.localScale.x/2f;
        float halfAlto=plataforma.transform.localScale.y/2f;

        for (int i=0; i<cantidadPorPantalla; i++) {
            Vector2 pos=new Vector2(Random.Range(limX.x+halfAncho, limX.y-halfAncho), Random.Range(limY.x+1.25f+halfAlto, limY.y-halfAlto));
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

            //Poner a un mono cabron?
            if (Random.value<1f/20f) {
                Instantiate<GameObject>(monoCabron, new Vector2(pos.x, pos.y+halfAlto+monoCabron.transform.localScale.y/2f), Quaternion.identity);
            }
                

            //No vamos a reinstanciar indefinidamente las plataformas hasta dar con lugares donde no solapen a otras
            //porque así tendremos más variabilidad a costa de ningún recurso más
        }

    }

    Vector2 limitesFila (float y) { 
        //Devuelve los límites inferior y superior (por ese orden) de la fila
        //en la que cae una coordenada y
        y=centraCoordFila(y);
        float offset=Camera.main.orthographicSize;
        return new Vector2(y-offset, y+offset);
    }

    Vector2 limitesPantalla (float x) { 
        //Devuelve los límites izquierdo y derecho (por ese orden) de la pantalla
        //en la que cae una coordenada x
        x=centraCoordPantalla(x);
        float offset=Camera.main.orthographicSize*Camera.main.aspect;
        return new Vector2(x-offset, x+offset);
    }

    float centraCoordFila (float y) {
        return Mathf.Round(y/(Camera.main.orthographicSize*2f))
            *Camera.main.orthographicSize*2f;
    }

    float centraCoordPantalla (float x) {
        return Mathf.Round(x/(Camera.main.orthographicSize*Camera.main.aspect*2f))
            *Camera.main.orthographicSize*Camera.main.aspect*2f;
    }

    void AnadirPlataformasAlrededor(int cantidadPorPantalla, Vector2 pos) {
        //Usa Poblar() para añadir plataformas en las pantallas
        //colindantes: superior, izquierda, derecha, sup-izqda y sup-dcha
        float xOffset=Camera.main.orthographicSize*Camera.main.aspect+1f; //El 1 es un margen de seguridad
        float yOffset=Camera.main.orthographicSize+1f;

        Poblar(cantidadPorPantalla, new Vector2(pos.x-xOffset, pos.y)); //Izqda
        Poblar(cantidadPorPantalla, new Vector2(pos.x+xOffset, pos.y)); //Dcha
        Poblar(cantidadPorPantalla, new Vector2(pos.x, pos.y+yOffset)); //Arriba
        Poblar(cantidadPorPantalla, new Vector2(pos.x-xOffset, pos.y+yOffset)); //Arriba Izqda
        Poblar(cantidadPorPantalla, new Vector2(pos.x+xOffset, pos.y+yOffset)); //Arriba Dcha

    }

    

}
