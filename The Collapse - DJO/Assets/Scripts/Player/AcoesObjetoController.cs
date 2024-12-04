using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcoesObjetoController : MonoBehaviour
{
    private IdentificarObjetoController idObjetos;
    private bool pegou = false;

    // Start is called before the first frame update
    void Start()
    {
        idObjetos = GetComponent<IdentificarObjetoController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjPegar() != null)
        {
            Pegar();
        }

        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjArrastar() != null)
        {
            if (!pegou)
            {
                Arrastar();
            } else
            {
                Soltar();
            }
            pegou = !pegou;
        }
    }

    private void Pegar()
    {
        // Aqui pode ser implementada a acao quando pegar o objeto
        IPegavel obj = idObjetos.GetObjPegar().GetComponent<IPegavel>();
        obj.Pegar();
        Debug.Log("Pegou o objeto");
        Destroy(idObjetos.GetObjPegar());
        idObjetos.EsconderTexto();
    }

    private void Arrastar()
    {
        // NAO SERA USADO
        GameObject obj = idObjetos.GetObjArrastar();
        obj.AddComponent<DragDrop>();
        obj.GetComponent<DragDrop>().Ativar();
        idObjetos.enabled = false;
    }

    private void Soltar()
    {
        GameObject obj = idObjetos.GetObjArrastar();
        Destroy(obj.GetComponent<DragDrop>());
        idObjetos.enabled = true;
    }
}
