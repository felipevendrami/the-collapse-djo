using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10f;


    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // Encontra o objeto com a tag "Player"
            if (player != null)
            {
                target = player.transform;  
            }
            else
            {
                Debug.LogError("Nenhum objeto com a tag 'Player' foi encontrado na cena.");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Move a câmera do minimapa para seguir o Player
            transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Target (Player) não encontrado, a câmera do minimapa não pode seguir o jogador.");
        }
    }
}
