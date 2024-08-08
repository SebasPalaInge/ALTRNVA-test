using System.Collections;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Variables")]
    public bool comparing = false;
    public bool isFistUncovered = false;
    public BlockDisplay firstBlockSelected;
    public BlockDisplay secondBlockSelected;
    public AudioSource audioSrc;
    public AudioClip successSound;
    public AudioClip errorSound;
    [Header("Recount variables")]
    public int score;
    public int pairsCompleted;
    public int numberOfClicks;
    public float currentGameTime;
    [Header("Objects")]
    public TextMeshProUGUI gameTimeDisplay;
    public TextMeshProUGUI showText;
    public CanvasGroup gameCover;
    public GameObject notReadedFile;
    public GameObject displayBlocks;

    [HideInInspector] public bool canRunGameTime = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        showText.text = "No se ha cargado un archivo. Haz click en importar para empezar a jugar.";
        notReadedFile.SetActive(true);
        displayBlocks.SetActive(false);
        gameCover.alpha = 0f;
        gameCover.gameObject.SetActive(false);
        CreateDefaultState();
    }

    private void CreateDefaultState()
    {
        firstBlockSelected = new BlockDisplay();
        secondBlockSelected = new BlockDisplay();
        score = 0;
        pairsCompleted = 0;
        numberOfClicks = 0;
        currentGameTime = 0;
        gameTimeDisplay.text = " ";
    }

    private void Update()
    {
        if (canRunGameTime)
        {
            currentGameTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(currentGameTime / 60);
            int seconds = Mathf.FloorToInt(currentGameTime % 60);
            gameTimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (score < 0) score = 0;
    }

    public void StartComparison()
    {
        comparing = true;
        StartCoroutine(CompareBlocks());
    }

    private IEnumerator CompareBlocks()
    {
        if (firstBlockSelected.blockInfo.number.Equals(secondBlockSelected.blockInfo.number))
        {
            firstBlockSelected.Complete();
            secondBlockSelected.Complete();
            audioSrc.PlayOneShot(successSound);
            pairsCompleted++;
            score += 50;
        }
        else
        {
            audioSrc.PlayOneShot(errorSound);
            yield return new WaitForSeconds(1.2f);
            firstBlockSelected.Cover();
            secondBlockSelected.Cover();
            score -= 20;
        }

        yield return new WaitForSeconds(.5f);

        isFistUncovered = false;
        comparing = false;

        CheckIfGameComplete();
    }

    private void CheckIfGameComplete()
    {
        if (PopulateBlocks.instance.AreBlocksCompleted())
        {
            Debug.Log("Game completed");
            canRunGameTime = false;
            RecopilateResults();
            StartNewGame();
        }
    }

    private void RecopilateResults()
    {
        Results gameData = new Results()
        {
            total_clicks = numberOfClicks,
            total_time = Mathf.FloorToInt(currentGameTime),
            pairs = pairsCompleted,
            score = score,
        };

        ResultsObject newResultsObject = new ResultsObject()
        {
            results = gameData
        };

        string dataCollected = JsonUtility.ToJson(newResultsObject, true);
        FileManager.instance.SaveFile(dataCollected);
    }

    private void StartNewGame()
    {
        int minutes = Mathf.FloorToInt(currentGameTime / 60);
        int seconds = Mathf.FloorToInt(currentGameTime % 60);
        string totalTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        showText.text = "Partida completada en " + totalTime + ". Haz click en importar para comenzar una nueva partida.";
        CreateDefaultState();
        CoverFade();
    }

    private void CoverFade()
    {
        gameCover.gameObject.SetActive(true);
        LeanTween.alphaCanvas(gameCover, 1f, 0.2f).setOnComplete(() =>
        {
            notReadedFile.SetActive(true);
            displayBlocks.SetActive(false);
            PopulateBlocks.instance.DestroyContent();
            LeanTween.alphaCanvas(gameCover, 0f, 0.2f).setOnComplete(() => 
            {
                gameCover.gameObject.SetActive(false);
            });
        });
    }
}
