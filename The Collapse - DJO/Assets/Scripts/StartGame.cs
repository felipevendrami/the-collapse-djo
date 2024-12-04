
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void MenuPrincipal(){
        SceneManager.LoadScene(0);
    }
    public void ReiniciarJogo(){
        SceneManager.LoadScene(1);
    }
    public void CarregarJogo(){
        SceneManager.LoadScene(2);
    }
    public void Ranking(){
        SceneManager.LoadScene(3);
    }        
    public void Sobre(){
        SceneManager.LoadScene(4);
    }         

    public void ComoJogar(){
        SceneManager.LoadScene(5);
    }       
    public void SairJogo()
    {
        Application.Quit();
    }
}