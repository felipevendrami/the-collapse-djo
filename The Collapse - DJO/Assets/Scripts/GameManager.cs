using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton
    public Text textoFase; // Texto para a fase
    public Text textoPilhas; // Texto para exibir a quantidade de pilhas
    public Slider sliderVidaJogador; // Slider de vida do jogador
    public Slider sliderVidaFamilia; // Slider de vida da família
    private AudioSource audio;

    private int pilhas = 0; // Quantidade de pilhas
    private int faseAtual = 1; // Fase inicial
    private float reducaoVidaJogador = 0.5f; // Velocidade inicial de redução da vida do jogador
    private float reducaoVidaFamilia = 0.3f; // Velocidade inicial de redução da vida da família

    public AudioClip[] clips;

    private void Awake()
    {
        // Configura o singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AtualizarTextoFase();
        AtualizarTextoPilhas();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Reduz a vida do jogador e da família de acordo com a velocidade configurada
        sliderVidaJogador.value -= reducaoVidaJogador * Time.deltaTime;
        sliderVidaFamilia.value -= reducaoVidaFamilia * Time.deltaTime;

        // Verifica se a vida chegou a zero
        if (sliderVidaJogador.value <= 0 || sliderVidaFamilia.value <= 0)
        {
            GameOver();
        }
    }

    // Métodos para gerenciar as pilhas
    public void AddPilha()
    {
        if(pilhas == 0) //se é a primeira vez pegando pilha, vamos mudar a fase.
        {
            pilhas++;
            ProximaFase();
        }else{
            pilhas++;
            TocarSom(0);
        }
        AtualizarTextoPilhas();
    }

    public bool ConsumirPilha()
    {
        if (pilhas > 0)
        {
            //pilhas--;
            //AtualizarTextoPilhas();
            return true;
        }
        return false;
    }

    public int GetPilhas()
    {
        return pilhas;
    }

    private void AtualizarTextoPilhas()
    {
        textoPilhas.text = pilhas.ToString();
    }

    private void AtualizarTextoFase()
    {
        switch (faseAtual)
        {
            case 1:
                textoFase.text = "Fase 1: Encontrar pilhas para utilizar o rádio";
                reducaoVidaJogador = 0.5f;
                reducaoVidaFamilia = 0.3f;
                break;
            case 2:
                textoFase.text = "Fase 2: Furtividade";
                reducaoVidaJogador = 0.7f;
                reducaoVidaFamilia = 0.4f;
                break;
            case 3:
                textoFase.text = "Fase 3: Reativar a Antena";
                reducaoVidaJogador = 1.0f;
                reducaoVidaFamilia = 0.5f;
                break;
            default:
                textoFase.text = "Fim do Jogo";
                break;
        }
    }

    public void ProximaFase()
    {
        faseAtual++;
        if (faseAtual <= 3)
        {
            AtualizarTextoFase();
        }
        else
        {
            textoFase.text = "Parabéns! Você completou todas as fases.";
        }
        
        PlayerDataManager.Instance.SaveGame();
        TocarSom(1);//sucesso
    }

    private void GameOver()
    {
        textoFase.text = "Game Over! Tente novamente.";
        Time.timeScale = 0; // Pausa o jogo
    }

    public void TocarSom(int som){
        audio.Stop();
        audio.clip = clips[som];
        audio.time = 0;
        audio.Play(); 
    }

    public void RecarregarVidaJogador(){
        sliderVidaJogador.value += 20;
    }

    public void RecarregarVidaFamilia(){
        sliderVidaFamilia.value += 15;
    }
    public float GetVidaJogador(){
        return sliderVidaJogador.value;
    }

    public float GetVidaFamilia(){
        return sliderVidaFamilia.value;
    }

    public void SetVidaJogador(float novaVida)
    {
        sliderVidaJogador.value = Mathf.Clamp(novaVida, 0, sliderVidaJogador.maxValue);
    }

    public void SetVidaFamilia(float novaVida)
    {
        sliderVidaFamilia.value = Mathf.Clamp(novaVida, 0, sliderVidaFamilia.maxValue);
    }

    public int GetFaseAtual(){
        return faseAtual;
    }    

    public void SetFaseAtual(int fase){
        faseAtual = fase;
    }
    public void SetPilhas(int newPilhas){
        pilhas = newPilhas;
    }

    public void DiminuiVidaJogador(float dano){
        sliderVidaJogador.value -= dano;
    }
}
