using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaControllerOld : MonoBehaviour
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaDoAtaque = 0.5f;
    private GameObject maoCapanga;
    private FieldOfView fov;
    private PatrulhaAleatoria patrulhaAleatoria;
    private bool emPatrulha = true;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();

        maoCapanga = GameObject.FindWithTag("maoCapanga");
        maoCapanga.SetActive(false);
        patrulhaAleatoria = GetComponent<PatrulhaAleatoria>();
    }

    void Update()
    {
        // Implementar o morrer caso necessario

        /*if (fov.podeVerPlayer)
        {
            PersegueJogador();
        } else
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
            agente.isStopped = false;
        }

        patrulhaAleatoria.Andar();*/

        // Novo update
        
        if (fov.podeVerPlayer)
        {
            PersegueJogador();
        }

        patrulhaAleatoria.Andar();
    }

    public void LookArroundAnimation()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("lookingArround", true);
        CorrigirRigiSair();
        agente.isStopped = false;
    }

    public void IsWalkingAnimation()
    {
        anim.SetBool("lookingArround", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", true);
        CorrigirRigiSair();
        agente.isStopped = false;
    }

    public void IsRunningAnimation()
    {
        anim.SetBool("lookingArround", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", true);
        CorrigirRigiSair();
        agente.isStopped = false;
    }

    private void IsPunchingAnimation()
    {
        agente.isStopped = true;
        maoCapanga.SetActive(true);
        anim.SetTrigger("isPunching");
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("lookingArround", false);
        CorrigirRigiEntrar();
    }

    private void StopPunchAnimation()
    {
        maoCapanga.SetActive(false);
        CorrigirRigiSair();

        maoCapanga.SetActive(false);
        agente.isStopped = false;
        agente.SetDestination(player.transform.position);
        anim.ResetTrigger("isPunching");
    }

    /*private void PersegueJogador()
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
    }*/

    // Novo persegue jogador
    private void PersegueJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            IsPunchingAnimation();
        }
        else if (distanciaDoPlayer < distanciaDoAtaque + 1)
        {
            StopPunchAnimation();
            IsRunningAnimation(); 
        }
        else
        {
            IsRunningAnimation();
            agente.SetDestination(player.transform.position);
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