using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreDataManager : MonoBehaviour
{
    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/scores.json";

        // Garante que o arquivo JSON existe com uma estrutura válida
        if (!File.Exists(filePath))
        {
            // Cria um objeto inicial com uma lista vazia
            ScoreList initialScoreList = new ScoreList { scores = new List<ScoreEntry>() };
            string initialJson = JsonUtility.ToJson(initialScoreList, true);
            File.WriteAllText(filePath, initialJson);
        }
    }

    public void SaveScore(float healthPlayer, float healthFamily)
    {
        float score = healthPlayer + healthFamily;

        // Carrega os scores existentes
        List<ScoreEntry> scores = LoadScores();
        int nextId = scores.Count > 0 ? scores[scores.Count - 1].id + 1 : 1;

        // Cria uma nova entrada de score
        ScoreEntry newEntry = new ScoreEntry
        {
            id = nextId,
            score = score
        };

        // Adiciona a nova entrada à lista e salva no arquivo
        scores.Add(newEntry);
        string json = JsonUtility.ToJson(new ScoreList { scores = scores }, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"Score salvo: ID={newEntry.id}, Score={newEntry.score}");
    }

    public List<ScoreEntry> LoadScores()
    {
        string json = File.ReadAllText(filePath);
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
