using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour
{

    public TextMeshProUGUI start;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFlash());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;

            List <RaycastResult> hits=new List<RaycastResult>();
            EventSystem.current.RaycastAll(ped, hits);
            foreach (RaycastResult r in hits) {
                if (r.gameObject.name=="Start") {
                    //Empieza el juego
                    SceneManager.LoadScene("Juego");
                }
            }
        }
    }

    IEnumerator StartFlash() {
        Color visible=start.color;
        visible.a=1f;
        Color invisible=start.color;
        invisible.a=0f;

        yield return new WaitForSeconds(2); //Suspense tikitikitikitikitikiiii....

        while (true) {
            start.color=visible;
            yield return new WaitForSeconds(.75f);
            start.color=invisible;
            yield return new WaitForSeconds(.50f);

        }

    }
}
