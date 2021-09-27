//Copyright © 2021- Eneko Castresana Vara. All rights reserved.
//License: No, you may NOT use any parts of the present and related work, including code and assets,
//for any purpose other than inspection of the work itself, without my express permission. 
//If you infringe this license your soul will burn in hell forever. 🔥😀👍
//
//Si te gusta lo que hago, contáctame a [X]ecv80vit[X]@[X]gmail.com[X] (sin las X ni corchetes)
//y dame trabajo. Mejor de videojuegos, pero cualquier trabajo de programación está bien.
//No dudes en contactarme si quieres que participe en un proyecto aunque sea sin sueldo
//pero con vistas a vender y repartir beneficios.
//Si quieres que participe contigo en una jam, pues lo mismo, me contactas.
//Gracias.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject parte;

    float camZ;

    Vector2 plataformaMasAlta=new Vector2(0f, -5f);
    Vector2 plataformaMasDcha=new Vector2(20f, -5f);
    Vector2 plataformaMasIzqda=new Vector2(-20f, -5f);

    public int plataformasPorSubir=0;

    public Slider barraProgreso;
    public TextMeshProUGUI textoProgreso;

    public ParteUI parte1;
    public ParteUI parte2;
    public ParteUI parte3;

    public bool gameOver=false;

    public static int nivel=1;

    static bool interludioMostrado=false;

    public List<bool[,]> matricesLlaves;

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
        barraProgreso=GameObject.Find("Barra").GetComponent<Slider>();
        textoProgreso=GameObject.Find("Progreso").GetComponent<TextMeshProUGUI>();
        parte1=GameObject.Find("Parte1").GetComponent<ParteUI>();
        // parte2=GameObject.Find("Parte2").GetComponent<ParteUI>();
        // parte3=GameObject.Find("Parte3").GetComponent<ParteUI>();

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
        barraProgreso.value=0;
        barraProgreso.minValue=0;
        barraProgreso.maxValue=nivel;
        textoProgreso.text="0/"+nivel;

        //Poner garfio-llaves
        GeneraGarfioLlaves(prota.DetectaCerraduras());

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
            default:
            break;
        }

        //Ir al inicio
        Invoke("Inicio", 4f);

    }

    void Inicio() {
        SceneManager.LoadScene("Inicio");
        Destroy(Camera.main.gameObject);
        Destroy(prota.gameObject);
    }

    void SiguienteNivel () {
        nivel++;
        if (nivel==6 && !interludioMostrado) {
            interludioMostrado=true;
            SceneManager.LoadScene("MonoCabron");
            Destroy(Camera.main.gameObject);
            Destroy(prota.gameObject);
        }
        else
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
            if (nivel>5)
                if (Random.value<1f/20f) {
                    Instantiate<GameObject>(monoCabron, new Vector2(pos.x, pos.y+halfAlto+monoCabron.transform.localScale.y/2f), Quaternion.identity);
                    //Si hay mono, no ponemos cerradura, para evitar situaciones complicadisimas. Ayyy dronjasdistoooo....
                    continue;
                }

            //Ponerle el cerrojo complementario que creará la UNIÓN
            Parte part=Instantiate<GameObject>(parte, pos-Vector2.up*.25f, Quaternion.identity).GetComponent<Parte>();
            part.AsignaMatriz(GenerarMatriz());
                

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

    bool[,] GenerarMatriz() {
        //Genera una matriz aleatoria de 3x3 de bools
        bool[,] matriz=new bool[3,3];
        for (int i=0; i<matriz.GetLength(0); i++)
            for (int j=0; j<matriz.GetLength(1); j++)
                matriz[i,j]=Random.value<.5f?false:true;
            
        return matriz;
    }

    bool[,] MatrizComplementaria(bool[,] m) {
        //Devuelve la matriz complementaria
        bool[,] matriz=new bool[m.GetLength(0),m.GetLength(1)];
        for (int i=0; i<matriz.GetLength(0); i++)
            for (int j=0; j<matriz.GetLength(1); j++)
                matriz[i,j]=m[i,j]?false:true;
            
        return matriz;
    }

    bool SonIguales(bool[,] m1, bool[,] m2) { //Leer en tono de pregunta, con mucha tontería
        if (m1.GetLength(0)!=m2.GetLength(0) || m1.GetLength(1)!=m2.GetLength(1))
            return false;

        for (int i=0; i<m1.GetLength(0); i++)
            for (int j=0; j<m1.GetLength(1); j++)
                if (m1[i,j]!=m2[i,j])
                    return false;
                
        return true;
    }

    public bool SonComplementarias(bool[,] m1, bool[,] m2) {
        //Hago esto porque generar una matriz complementaria y luego comprobar si son iguales,
        //tarda mas tiempo que comprobar directamente si son complementarias.
        //Que no? Y la interrupción de los loop? Piénsalo bien, piensa...

        if (m1.GetLength(0)!=m2.GetLength(0) || m1.GetLength(1)!=m2.GetLength(1))
            return false;

        for (int i=0; i<m1.GetLength(0); i++)
            for (int j=0; j<m1.GetLength(1); j++)
                if (m1[i,j]==m2[i,j])
                    return false;
                
        return true;
    }

    public void GeneraGarfioLlaves(List<bool[,]> matrices) {
        //Genera los garfios-llaves que encajarán con las cerraduras de las plataformas para crear uniones        
        //Acepta directamente cerraduras y genera las complementarias llaves
        matricesLlaves=new List<bool[,]>();

        foreach(bool[,] matriz in matrices) {
            //Aleatorizar la lista a la par que introducimos los complementarios
            matricesLlaves.Insert(Random.Range(0, matricesLlaves.Count), MatrizComplementaria(matriz));
        }
        
        parte1.AsignaMatriz(matricesLlaves[0]);
        // parte2.AsignaMatriz(matricesLlaves[1]);
        // parte3.AsignaMatriz(matricesLlaves[2]);
    }

    void circularLista<T>(List<T> l, int posiciones=1) {
        //Circula una lista poniendo las primeras posiciones al final de la lista
        for (int i=0; i<posiciones; i++)
            l.Add(l[i]);
        l.RemoveRange(0, posiciones);
    }

    

}
