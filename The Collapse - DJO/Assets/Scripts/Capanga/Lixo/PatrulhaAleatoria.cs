using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrulhaAleatoria : MonoBehaviour
{
    private NavMeshAgent agente;
    public float range;
    private float tempo;
    private CapangaController capangaController;
    
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        capangaController = GetComponent<CapangaController>();
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private bool ChegouAoDestino()
    {
        if (agente.remainingDistance <= agente.stoppingDistance)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool isDevePararDeAndar()
    {
        if(ChegouAoDestino() || (tempo >= 6.0f))
        {
            return true;
        } else
        {
            tempo += Time.deltaTime;
            return false;
        }
    }

    // Andar Original
    public void Andar()
    {
        while(true)
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                if (NavMesh.SamplePosition(point, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                {
                    agente.SetDestination(hit.position);
                    Debug.Log("Buscou o ponto de patrulha");
                    break;
                }
            }
        }

        /*if (ChegouAoDestino() || (tempo >= 6.0f))
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                agente.SetDestination(point);
                tempo = 0;
            }
        } else
        {
            tempo += Time.deltaTime;
        }*/

        Debug.DrawLine(transform.position, agente.destination, Color.magenta);
    }
}