using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDesafio3 : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("FpsController").GetComponent<GameManager>();
    }
    
    void OnTriggerEnter()
    {
        gameManager.TriggerDesafio3();
    }
}
