using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaController : MonoBehaviour
{
    [Header("IA Follow Navigation")]
    public float minDistanceToFollow = 0.5f;
    public float minDistanceToPoint = 1.0f;
    public float minDistanceToAttack = 2.0f;
    public float timeToStartNavegation = 3.0f;
    public float speedOfNavegation = 1.0f;
    public float speedOfFollow = 3.0f;

    [Header("IA Vision")]
    public Transform eyes;
    public float visionRadius = 10f;
    public float visionAngle = 45f;
    public float timeOfNavegation;
    public GameObject[] navegationsPoints;

    private float timerNav;
    private int pointIndex;
    private bool canSeePlayer = false;

    private Animator anim;
    private Transform target;
    private NavMeshAgent navMesh;

    [Header("Sound Effects")]
    private AudioSource audioSrc;
    public AudioClip runSound;
    public AudioClip walkSound;
    public AudioClip voicePunchSound;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navegationsPoints = GameObject.FindGameObjectsWithTag("NavegationPoints");
        audioSrc = GetComponent<AudioSource>();
        pointIndex = GetRandomPointIndex();

        navMesh.speed = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        timerNav = Mathf.Clamp(timerNav -= Time.deltaTime, 0, timeOfNavegation);
        canSeePlayer = CanSeePlayer();

        if (canSeePlayer || timerNav > 0)
        {
            Follow();
        } else
        {
            Navegation();
        }
    }

    // Raio de visao para avistar o jogador
    private bool CanSeePlayer()
    {
        if (target == null)
        {
            return false;
        }

        var directionToPlayer = (target.position - eyes.position).normalized;

        if (Vector3.Angle(eyes.forward, directionToPlayer) < visionAngle / 2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(eyes.position, directionToPlayer, out hit, visionRadius))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    timerNav = timeOfNavegation;
                    return true;
                }
            }
        }

        return false;
    }

    // Perseguir o jogador
    private void Follow()
    {
        navMesh.speed = speedOfFollow;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > minDistanceToAttack) // Capanga está longe, continua se aproximando
        {
            anim.SetBool("StopPunch", true);
            anim.SetBool("Follow", true);
            anim.SetBool("Navegation", false);
            anim.ResetTrigger("Punch");

            navMesh.enabled = true;
            navMesh.SetDestination(target.position);
        }
        else if (distanceToTarget <= minDistanceToAttack && distanceToTarget > minDistanceToFollow) // Dentro da distância para iniciar o soco
        {
            navMesh.enabled = false;

            anim.SetBool("Follow", false);
            anim.SetBool("StopPunch", false);
            anim.SetTrigger("Punch");

            // Capanga olha para o jogador ao atacar
            Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(lookPos);
            CorrigirRigiEntrar();
        }
        else // Distância muito pequena
        {
            navMesh.enabled = false;

            // Capanga evita bugs ficando muito perto
            Vector3 awayFromTarget = transform.position - target.position;
            transform.position += awayFromTarget.normalized * 0.1f;
        }
    }

    // Navegacao para novo destino
    private void Navegation()
    {
        anim.SetBool("Follow", false);
        navMesh.speed = speedOfNavegation;

        if (navMesh.enabled && Vector3.Distance(transform.position, navegationsPoints[pointIndex].transform.position) > minDistanceToPoint)
        {
            anim.SetBool("Navegation", true);
            navMesh.SetDestination(navegationsPoints[pointIndex].transform.position);
        } else
        {
            navMesh.enabled = false;
            anim.SetBool("Navegation", false);
            pointIndex = GetRandomPointIndex();
            StartCoroutine("StartNavegation");
        }
    }

    public void Walk()
    {
        CorrigirRigiSair();
        audioSrc.PlayOneShot(walkSound, 0.05f);
    }

    public void Run()
    {
        CorrigirRigiSair();
        audioSrc.PlayOneShot(runSound, 0.05f);
    }

    public void Punch()
    {
        audioSrc.clip = voicePunchSound;
        audioSrc.Play();
    }

    // Procura novo ponto para realizar a patrulha
    private int GetRandomPointIndex()
    {
        var i = UnityEngine.Random.Range(0, (navegationsPoints.Length - 1));

        if (pointIndex == i)
        {
            return GetRandomPointIndex();
        }

        var enemys = GameObject.FindGameObjectsWithTag("Capanga");

        foreach (GameObject enemy in enemys)
        {
            if (enemy.GetComponent<CapangaController>().pointIndex == i)
            {
                return GetRandomPointIndex();
            }
        }

        return i;
    }

    private IEnumerator StartNavegation()
    {
        yield return new WaitForSeconds(timeToStartNavegation);
        navMesh.enabled = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = canSeePlayer ? Color.blue : Color.red;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-visionAngle / 2f, transform.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(visionAngle / 2f, transform.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(eyes.position, leftRayDirection * visionRadius);
        Gizmos.DrawRay(eyes.position, rightRayDirection * visionRadius);
        Gizmos.DrawLine(eyes.position, eyes.position + leftRayDirection * visionRadius);
        Gizmos.DrawLine(eyes.position, eyes.position + rightRayDirection * visionRadius);
    }

        private void CorrigirRigiEntrar()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}