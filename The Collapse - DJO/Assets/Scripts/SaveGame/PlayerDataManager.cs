using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerDataManager : MonoBehaviour
{
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static PlayerDataManager Instance;
    public Transform playerTransform;
    public float healthPlayer;
    public float healthFamily;

    public int batteries;

    public int level;
    public void SaveGame(){
        PlayerData playerData = new PlayerData();
        playerData.position = new float[] {playerTransform.position.x, playerTransform.position.y,playerTransform.position.z};
        playerData.healthPlayer = GameManager.Instance.GetVidaJogador();
        playerData.healthFamily = GameManager.Instance.GetVidaFamilia();
        playerData.level = GameManager.Instance.GetFaseAtual();
        playerData.batteries = GameManager.Instance.GetPilhas();

        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + "/playerData.json";
        System.IO.File.WriteAllText(path,json);
    }

    public void LoadGame(){
        string path =  Application.persistentDataPath + "/playerData.json";
        if(File.Exists(path)){
            string json = System.IO.File.ReadAllText(path);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);
            FindObjectOfType<CharacterController>().enabled = false;
            Invoke("enableController",1f);
            Vector3 loadedPosition = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            playerTransform.position = loadedPosition;

            GameManager.Instance.SetVidaJogador(loadedData.healthPlayer);
            GameManager.Instance.SetVidaFamilia(loadedData.healthFamily);
            GameManager.Instance.SetFaseAtual(loadedData.level);
            GameManager.Instance.SetPilhas(loadedData.batteries);
        }else {
            Debug.LogWarning("Arquivo não encontrado.");
        }
    }

    public void enableController(){
        FindObjectOfType<CharacterController>().enabled = true;
    }
}
