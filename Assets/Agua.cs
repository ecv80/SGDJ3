using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agua : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector2(Camera.main.transform.position.x, transform.position.y); //Centrada en la camara siempre

        if (!GameManager.instancia.prota.subiendo)
            transform.position+=Vector3.up*Time.deltaTime*.1f;
    }

    public IEnumerator SubeAgua (float hastaAqui) { //Sube el agua rápidamente hasta donde le digas
        while (transform.position.y<hastaAqui-10f) {
            transform.position+=Vector3.up*Time.deltaTime*1f;
            if (transform.position.y>=hastaAqui-10f)
                transform.position=new Vector2(transform.position.x, hastaAqui-10f);

            yield return 0;
        }

        yield break;
    }
}
