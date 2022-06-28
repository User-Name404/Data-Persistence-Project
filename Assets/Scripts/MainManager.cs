using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;

    private int highScore;
    private string highScoreName;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        
        LoadData();

        UpdateHighScoreText();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        GameOverText.SetActive(true);
        if (m_Points > highScore)
        {
            highScore = m_Points;
            highScoreName = PlayerData.Instance.playerName;
            SaveScore();
        }
    }

    public void UpdateHighScoreText()
    {
        HighScoreText.text = $"High Score: {highScoreName}: {highScore}";
    }

    [System.Serializable]
    class SaveData
    {
        public int HighScore;
        public string Name;
    }

    private void SaveScore()
    {
        SaveData saveData = new SaveData();
        saveData.Name = highScoreName;
        saveData.HighScore = highScore;

        string json = JsonUtility.ToJson(saveData);
        
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            highScore = saveData.HighScore;
            highScoreName = saveData.Name;
        }
    }
}
