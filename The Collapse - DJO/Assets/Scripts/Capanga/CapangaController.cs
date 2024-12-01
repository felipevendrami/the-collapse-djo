using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaController : MonoBehaviour
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaDoAtaque = 0.5f;
    private GameObject maoCapanga;
    private FieldOfView fov;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();

        maoCapanga = GameObject.FindWithTag("maoCapanga");
        maoCapanga.SetActive(false);
    }

    void Update()
    {
        // Implementar o morrer caso necessario

        if (fov.podeVerPlayer)
        {
            PersegueJogador();
        } else
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
            agente.isStopped = false;
        }
    }

    private void PersegueJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Capanga pode atacar
        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            agente.isStopped = true;
            maoCapanga.SetActive(true);
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigiEntrar();
        }

        // Player se afastou
        if (distanciaDoPlayer >= distanciaDoAtaque + 1)
        {
            maoCapanga.SetActive(false);
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
        }

        if (anim.GetBool("podeAndar"))
        {
            maoCapanga.SetActive(false);
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    private void CorrigirRigiEntrar()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OlharParaJogador()
    {
        Vector3 direcaoOlhar = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }
}