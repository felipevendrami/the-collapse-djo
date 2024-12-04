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
        EsconderTexto(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            objArrastar = null;
            objPegar = null;

            int ignorarLayer = LayerMask.GetMask("Minimap", "IgnorePlayercast"); 

            ignorarLayer = ~ignorarLayer;

            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5, ignorarLayer))
            {
                //Debug.Log("Objeto detectado: " + hit.transform.name + " " +hit.transform.gameObject.tag);
                distanciaAlvo = hit.distance;

                // Remover destaque do objeto anterior se não for mais o alvo
                if (objAlvo != null && hit.transform.gameObject != objAlvo)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                }

                // Verificar se o objeto é "Arrastar"
                if (hit.transform.gameObject.tag == "Arrastar")
                {
                    objArrastar = hit.transform.gameObject;
                    objAlvo = objArrastar;

                    textoTecla.color = new Color(248 / 255f, 248 / 255f, 13 / 255f);
                    textoMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textoMsg.text = "Arrastar/Soltar";
                }

                // Verificar se o objeto é "Pegar"
                if (hit.transform.gameObject.tag == "Pegar" || hit.transform.gameObject.tag == "CaixaVidaJogador" || hit.transform.gameObject.tag == "CaixaVidaFamilia")
                {
                    objPegar = hit.transform.gameObject;
                    objAlvo = objPegar;

                    textoTecla.color = new Color(51 / 255f, 1, 0);
                    textoMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textoMsg.text = "Pegar";
                }

                // Adicionar destaque ao objeto alvo
                if (objAlvo != null)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 5f;
                }
            }
            else
            {
                // Esconder os textos e remover destaque se nenhum objeto for identificado
                if (objAlvo != null)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                }

                EsconderTexto();
            }

            // Caso nenhum objeto de interesse seja identificado, garantir que os textos fiquem vazios
            if (objArrastar == null && objPegar == null)
            {
                EsconderTexto();
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
