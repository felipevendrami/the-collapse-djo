using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Walkie : MonoBehaviour
{
    public Text textoPilhas;
    private Animator anim;
    private bool estahUsando;
    private RaycastHit hit;

    private AudioSource somWalkie;
    private int carregador = 0;
    private int pilhas = 3;
    public GameObject imgCursor;
    public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        estahUsando = false;
        anim = GetComponent<Animator>();
        somWalkie = GetComponent<AudioSource>();
        AtualizarTextoPilhas();
    }

    // Update is called once per frame
    void Update()
    {
        // Bloqueia ações enquanto uma animação está em execução
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("AcaoOcorrendo"))
        {
            return;
        }

        if (Input.GetButtonDown("Walkie"))
        {
            if (!estahUsando && pilhas > 0)
            {
                UsarWalkie();
            }
            else if (!estahUsando && pilhas == 0 && carregador > 0)
            {
                Recarregar();
            }
            else if (estahUsando)
            {
                GuardarWalkie();
            }
            else
            {
                SomSemPilha();
            }
        }
        else if (Input.GetButtonDown("Recarregar"))
        {
            if (carregador > 0 && pilhas < 3)
            {
                Recarregar();
            }
            else
            {
                SomSemPilha();
            }
        }

        AtualizarTextoPilhas();
    }

    private void UsarWalkie()
    {
        somWalkie.clip = clips[0];
        pilhas--;
        estahUsando = true;
        anim.Play("UsarWalkie");
        somWalkie.time = 0;
        somWalkie.Play();
    }

    private void GuardarWalkie()
    {
        estahUsando = false;
        anim.Play("GuardarWalkie");
        somWalkie.Stop();
    }

    private void Recarregar()
    {
        somWalkie.clip = clips[1];
        somWalkie.time = 1.05f;
        somWalkie.Play();
        anim.Play("RecarregarWalkie");
        pilhas = 3;
        carregador--;
    }

    private void SomSemPilha()
    {
        somWalkie.clip = clips[2];
        somWalkie.time = 0;
        somWalkie.Play();
    }

    private void AtualizarTextoPilhas()
    {
        textoPilhas.text = pilhas.ToString() + "/" + carregador.ToString();
    }

    public void AddCarregador()
    {
        somWalkie.clip = clips[3];
        somWalkie.time = 0;
        somWalkie.Play();
        carregador++;
        AtualizarTextoPilhas();
    }
}
