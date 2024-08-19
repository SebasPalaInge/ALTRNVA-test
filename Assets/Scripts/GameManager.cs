using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Variables")]
    public bool comparing = false;
    public bool isFistUncovered = false;
    public bool pathToResultsNull = false;
    public bool isButtonLayoutOpened = false;
    [HideInInspector] public BlockDisplay firstBlockSelected;
    [HideInInspector] public BlockDisplay secondBlockSelected;
    public AudioSource audioSrc;
    public AudioClip successSound;
    public AudioClip errorSound;
    [Header("Recount variables")]
    public int score;
    public int pairsCompleted;
    public int numberOfClicks;
    public float currentGameTime;
    private float gameTimeForReducingScore;
    private int reducedTimeScore;
    [Header("Dialogs")]
    public List<DialogTexts> dialogs;
    [Header("Objects")]
    public TextMeshProUGUI gameTimeDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI showText;
    public CanvasGroup gameCover;
    public CanvasGroup buttonLayout;
    public GameObject notReadedFile;
    public GameObject displayBlocks;

    [HideInInspector] public bool canRunGameTime = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetTextAndColor("Haz click en 'Seleccionar' para iniciar un nuevo caso.", Color.black);
        notReadedFile.SetActive(true);
        displayBlocks.SetActive(false);
        gameCover.alpha = 0f;
        gameCover.gameObject.SetActive(false);
        DialogBehaviour.instance.OpenDialoguePanel(dialogs[0].textsLines, false);
        CreateDefaultState();
    }

    private void Update()
    {
        if (canRunGameTime)
        {
            currentGameTime += Time.deltaTime;
            gameTimeForReducingScore = currentGameTime;
            int minutes = Mathf.FloorToInt(currentGameTime / 60);
            int seconds = Mathf.FloorToInt(currentGameTime % 60);
            gameTimeDisplay.text = "Tiempo transcurrido: " + string.Format("{0:00}:{1:00}", minutes, seconds);
            scoreDisplay.text = "Puntaje: " + score;

            if (gameTimeForReducingScore > 60f)
            {
                gameTimeForReducingScore = 0;
                reducedTimeScore += 5;
            }
        }

        if (score < 0) score = 0;
    }

    public void SetTextAndColor(string newText, Color newColor)
    {
        showText.text = newText;
        showText.color = newColor;
    }

    private void CreateDefaultState()
    {
        firstBlockSelected = null;
        secondBlockSelected = null;
        score = 0;
        pairsCompleted = 0;
        numberOfClicks = 0;
        currentGameTime = 0;
        gameTimeForReducingScore = 0;
        reducedTimeScore = 0;
        gameTimeDisplay.text = " ";
        scoreDisplay.text = " ";
        pathToResultsNull = false;
        isButtonLayoutOpened = false;
        buttonLayout.gameObject.SetActive(false);
        buttonLayout.alpha = 0f;
    }

    public void StartRunningGame()
    {
        notReadedFile.SetActive(false);
        displayBlocks.SetActive(true);
        canRunGameTime = true;
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
            RecalculateScore();
            RecopilateResults();
            if (!pathToResultsNull)
                DialogBehaviour.instance.OpenDialoguePanel(dialogs[1].textsLines, true);
            else
                OpenButtonLayout();
        }
    }

    public void ContinueButton()
    {
        DialogBehaviour.instance.OpenDialoguePanel(dialogs[1].textsLines, true);
    }

    private void OpenButtonLayout()
    {
        if (isButtonLayoutOpened) return;

        buttonLayout.gameObject.SetActive(true);
        LeanTween.alphaCanvas(buttonLayout, 1f, .2f);
    }

    private void RecalculateScore()
    {
        if (numberOfClicks > PopulateBlocks.instance.blocksDisplayed.Count)
        {
            int clickDiminish = (numberOfClicks - PopulateBlocks.instance.blocksDisplayed.Count) * 10;
            score -= clickDiminish;
        }
        else if (numberOfClicks.Equals(PopulateBlocks.instance.blocksDisplayed.Count))
        {
            score += 200;
        }

        score -= reducedTimeScore;
        if (score < 0) score = 0;
    }

    public void RecopilateResults()
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

    public void StartNewGame()
    {
        int minutes = Mathf.FloorToInt(currentGameTime / 60);
        int seconds = Mathf.FloorToInt(currentGameTime % 60);
        string totalTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        SetTextAndColor("Caso completado en " + totalTime + ". Haz click en 'Seleccionar' para iniciar un nuevo caso.", Color.black);
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
