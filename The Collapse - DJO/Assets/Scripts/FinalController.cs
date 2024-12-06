using UnityEngine;
using UnityEngine.UI; // Necessário para manipular os botões

public class FinalController : MonoBehaviour
{
    public AudioClip audioClip; // O áudio que será tocado
    private AudioSource audioSource; // Componente AudioSource
    public Button button1; // Botão 1
    public Button button2; // Botão 2

    void Start()
    {
        // Obtém o componente AudioSource
        audioSource = GetComponent<AudioSource>();

        // Adiciona o áudio ao AudioSource
        audioSource.clip = audioClip;

        // Desativa os botões inicialmente
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);

        // Toca o áudio
        audioSource.Play();

        // Chama a função CheckAudioEnd após o tempo do áudio
        Invoke("EnableButtons", audioSource.clip.length);
    }

    // Função chamada após o áudio terminar
    void EnableButtons()
    {
        button1.gameObject.SetActive(true); // Ativa o Botão 1
        button2.gameObject.SetActive(true); // Ativa o Botão 2
    }
}
