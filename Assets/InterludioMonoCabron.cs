using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InterludioMonoCabron : MonoBehaviour
{
    public RectTransform molinillo;
    public TextMeshProUGUI texto1;
    public CanvasGroup panel2;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("MuestraTexto1", 2f);
        Invoke("MuestraPanel2", 5f);
        Invoke("VuelveAlJuego", 9f);
    }

    // Update is called once per frame
    void Update()
    {
        molinillo.Rotate(0f, 0f, -50f*Time.deltaTime);
    }

    void MuestraTexto1 () {
        Color c=texto1.color;
        c.a=1f;
        texto1.color=c;
    }
    void MuestraPanel2 () {
        panel2.alpha=1f;
    }
    void VuelveAlJuego() {
        SceneManager.LoadScene("Juego");
    }
}
