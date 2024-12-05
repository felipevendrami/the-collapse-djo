using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMap : MonoBehaviour
{
    public GameObject mapa; 
    public GameObject mira;
    public static bool isActive; 

    void Start()
    {
        isActive = false; 
        mapa.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetButtonDown("Mapa"))
        {
            if (isActive)
            {
                CloseMap(); 
            }
            else
            {
                OpenMap(); 
            }
        }
    }

    public void OpenMap()
    {
        mapa.SetActive(true); 
        isActive = true; 
        mira.SetActive(false); 
    }

    // Método chamado para retomar o jogo
    public void CloseMap()
    {
        mapa.SetActive(false); 
        isActive = false; 
        mira.SetActive(true);
    }
}
