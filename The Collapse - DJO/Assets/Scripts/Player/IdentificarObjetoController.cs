using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdentificarObjetoController : MonoBehaviour
{
    private float distanciaAlvo;
    private GameObject objArrastar, objPegar;
    private GameObject objAlvo;
    public Text textoTecla, textoMsg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            objArrastar = null;
            objPegar = null;

            int ignorarLayer = 7; // Ignoreplayercast
            ignorarLayer = 1 << ignorarLayer;
            ignorarLayer = ~ignorarLayer; // Somente o layer 7 sera ignorado

            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5, ignorarLayer))
            {
                distanciaAlvo = hit.distance;

                if (objAlvo != null && hit.transform.gameObject != objAlvo)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                }

                if (hit.transform.gameObject.tag == "Arrastar")
                {
                    objArrastar = hit.transform.gameObject;
                    objAlvo = objArrastar;

                    textoTecla.color = new Color(248 / 255f, 248 / 255f, 13 / 255f);
                    textoMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textoMsg.text = "Arrastar/Soltar";
                }

                if (hit.transform.gameObject.tag == "Pegar")
                {
                    objPegar = hit.transform.gameObject;
                    objAlvo = objPegar;

                    textoTecla.color = new Color(51/255f, 1, 0);
                    textoMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textoMsg.text = "Arrastar/Soltar";
                }
                
                if (objAlvo != null)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 5f;
                }
            } else
            {
                if (objAlvo != null)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                    EsconderTexto();
                }
            }
        }
    }

    public float GetDistanciaAlvo()
    {
        return distanciaAlvo;
    }

    public GameObject GetObjArrastar()
    {
        return objArrastar;
    }

    public GameObject GetObjPegar()
    {
        return objPegar;
    }

    public void EsconderTexto()
    {
        textoTecla.text = "";
        textoMsg.text = "";
    }
}
