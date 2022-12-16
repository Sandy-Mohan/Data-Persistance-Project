using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public InputField playerName;
    [SerializeField] public Text bestScoreText;

    public string name = "";

    public string bestPlayer;
    public int bestScore;

    public static UIManager Instance;

    public void SetBestScore(int score)
    {
        if(score > bestScore)
        {
            bestScore = score;
            bestPlayer = name;
            SaveGame();
            MainManager.MainInstance.BestScore.text ="Best Score: " + bestPlayer +" : " +bestScore;
        }
    }

    public void StartNew()
    {
        name = playerName.text;
        //Debug.Log(name);
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    // Start is called before the first frame update
    void Start()
    {
        if(bestPlayer!= null)
        {
            bestScoreText.text = $"Best Score: {bestPlayer} : {bestScore}";
            //Debug.Log($"Player Name: {bestPlayer}; Score: {bestScore}");
        }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        LoadGame();
    }

    [Serializable]
    class SaveData
    {
        public string bestPlayer;
        public int bestScore;
    }

    void SaveGame()
    {
        SaveData obj = new SaveData();
        obj.bestPlayer = bestPlayer;
        obj.bestScore = bestScore;

        string file = JsonUtility.ToJson(obj);

        //Debug.Log(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/gamefile.json", file);

    }
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/gamefile.json";
        //Debug.Log(path);

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;
            //bestScoreText.text = $"Best Score: {bestPlayer} : {bestScore}";
        }
    }
}
