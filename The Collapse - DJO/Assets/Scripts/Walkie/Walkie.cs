using UnityEngine;

public class Walkie : MonoBehaviour
{
    private Animator anim;
    private bool estahUsando;
    private AudioSource somWalkie;

    public GameObject imgCursor;
    public AudioClip[] clips;

    void Start()
    {
        estahUsando = false;
        anim = GetComponent<Animator>();
        somWalkie = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Bloqueia ações enquanto uma animação está em execução
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("AcaoOcorrendo"))
        {
            return;
        }

        if (Input.GetButtonDown("Walkie"))
        {
            if (!estahUsando && GameManager.Instance.ConsumirPilha())
            {
                UsarWalkie();
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
    }

    private void UsarWalkie()
    {
        somWalkie.clip = clips[0];
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

    private void SomSemPilha()
    {
        somWalkie.clip = clips[2];
        somWalkie.time = 0;
        somWalkie.Play();
    }
}
