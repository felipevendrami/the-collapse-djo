using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour
{
    private PlayerDataManager playerDataManager; // Referência ao PlayerDataManager

    private void Awake()
    {
        // Tenta encontrar o PlayerDataManager na cena
        playerDataManager = FindObjectOfType<PlayerDataManager>();
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(1);
        StartCoroutine(SaveInitialGame());
    }
    public void CarregarJogo()
    {
        SceneManager.LoadScene(5); 
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }


    public void Ranking()
    {
        SceneManager.LoadScene(2);
    }

    public void Sobre()
    {
        SceneManager.LoadScene(3);
    }

    public void ComoJogar()
    {
        SceneManager.LoadScene(4);
    }

    public void SairJogo()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) 
        {
            if (PlayerDataManager.Instance != null)
            {
                PlayerDataManager.Instance.LoadGame();
            }
            else
            {
                Debug.LogWarning("PlayerDataManager não encontrado!");
            }
            SceneManager.sceneLoaded -= OnSceneLoaded; // Remove o listener após o carregamento
        }
    }

    private IEnumerator SaveInitialGame()
    {
        yield return new WaitForSeconds(0.1f); // Pequeno delay para garantir que a cena seja carregada
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.SaveGame();
        }
        else
        {
            Debug.LogWarning("PlayerDataManager não encontrado!");
        }
    }    
}
