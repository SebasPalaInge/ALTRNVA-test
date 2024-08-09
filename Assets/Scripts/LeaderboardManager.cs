using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    public PlayerScoredList playerList;
    public string path;

    public TMP_InputField newPlayerNameInputField;

    public GameObject inputInterface;
    public GameObject closeButton;

    public GameObject leaderBoardInterface;
    public GameObject playerInstantiatePrefab;
    public GameObject instanceParent;

    private void Awake()
    {
        instance = this;
        path = Application.persistentDataPath + "/leaderboard.json";
    }

    private void Start()
    {
        leaderBoardInterface.SetActive(false);
        PopulatePlayerList();
    }

    private void PopulatePlayerList()
    {
        playerList = new PlayerScoredList();

        if (!File.Exists(path))
        {
            string newJson = JsonUtility.ToJson(playerList, true);
            File.WriteAllText(path, newJson);
        }
        else
        {
            string file = File.ReadAllText(path);
            playerList = JsonUtility.FromJson<PlayerScoredList>(file);
        }
    }

    public void OpenLeaderboard()
    {
        InstantiateScores();
        closeButton.SetActive(false);
        inputInterface.SetActive(true);
        leaderBoardInterface.SetActive(true);
        newPlayerNameInputField.text = string.Empty;
        LeanTween.alphaCanvas(leaderBoardInterface.GetComponent<CanvasGroup>(), 1f, 0.2f);
    }

    public void CloseLeaderboard()
    {
        LeanTween.alphaCanvas(leaderBoardInterface.GetComponent<CanvasGroup>(), 0f, 0.2f).setOnComplete(() =>
        {
            leaderBoardInterface.SetActive(false);
            GameManager.instance.StartNewGame();
        });
    }

    public void InstantiateScores()
    {
        if(instanceParent.transform.childCount > 0)
        {
            foreach (Transform child in instanceParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if(playerList.list.Count > 0)
        {
            for (int i = 0; i < playerList.list.Count; i++)
            {
                GameObject obj = Instantiate(playerInstantiatePrefab, instanceParent.transform);
                string displayName = playerList.list[i].name;
                string displayScore = playerList.list[i].score.ToString();

                int minutes = Mathf.FloorToInt(playerList.list[i].time / 60);
                int seconds = Mathf.FloorToInt(playerList.list[i].time % 60);
                string displayTime = string.Format("{0:00}:{1:00}", minutes, seconds);

                obj.GetComponent<PlayerScoreDisplay>().UpdateDisplay(displayName, displayScore, displayTime);
            }
        }
    }

    public void AddPlayer()
    {
        PlayerScored newPlayer = new PlayerScored
        {
            name = newPlayerNameInputField.text,
            score = GameManager.instance.score,
            time = GameManager.instance.currentGameTime
        };

        playerList.list.Add(newPlayer);
        string newJson = JsonUtility.ToJson(playerList, true);
        File.WriteAllText(path, newJson);

        closeButton.SetActive(true);
        inputInterface.SetActive(false);

        InstantiateScores();
    }
}

[Serializable]
public class PlayerScored
{
    public string name;
    public int score;
    public float time;
}

[Serializable]
public class PlayerScoredList
{
    public List<PlayerScored> list;
}
