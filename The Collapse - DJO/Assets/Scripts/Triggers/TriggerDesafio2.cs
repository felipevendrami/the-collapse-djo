using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDesafio2 : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("FpsController").GetComponent<GameManager>();
    }
    void OnTriggerEnter()
    {
        gameManager.TriggerDesafio2();
    }
}
