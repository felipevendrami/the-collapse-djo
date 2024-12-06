using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.IO;
public class ScoreDisplay : MonoBehaviour
{
    public Text scoresText; // Referência ao componente de texto na tela
    private string filePath;

    private void Start()
    {
        // Define o caminho do arquivo JSON
        filePath = Application.persistentDataPath + "/scores.json";

        // Garante que o arquivo JSON existe
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }

        // Exibe os scores ao carregar a scene
        DisplayScores();
    }

    public void DisplayScores()
    {
        // Carrega os scores do arquivo JSON
        List<ScoreEntry> scores = LoadScores();

        // Monta o texto para exibir
        string displayText = "Scores:\n";
        foreach (ScoreEntry entry in scores)
        {
            displayText += $"ID: {entry.id}, Score: {entry.score}\n";
        }

        // Atualiza o texto na UI
        if (scoresText != null)
        {
            scoresText.text = displayText;
        }
        else
        {
            Debug.LogWarning("O componente scoresText não foi atribuído!");
        }
    }

    private List<ScoreEntry> LoadScores()
    {
        // Lê o arquivo JSON
        string json = File.ReadAllText(filePath);

        // Converte o JSON para uma lista de scores
        ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);
        return scoreList.scores ?? new List<ScoreEntry>();
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public int id;
        public float score;
    }

    [System.Serializable]
    public class ScoreList
    {
        public List<ScoreEntry> scores;
    }
}
