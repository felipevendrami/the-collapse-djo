using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaDeVida : MonoBehaviour,IPegavel
{
    public void Pegar()
    {
        // Verificando a tag do objeto
        if (gameObject.CompareTag("CaixaVidaJogador")) 
        {
            GameManager.Instance.RecarregarVidaJogador();
        }
        else if (gameObject.CompareTag("CaixaVidaFamilia")) 
        {
            GameManager.Instance.RecarregarVidaFamilia();
        }
    }
}
