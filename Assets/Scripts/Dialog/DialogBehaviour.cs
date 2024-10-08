using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogBehaviour : MonoBehaviour
{
    public static DialogBehaviour instance;
    [Header("Variables")]
    public bool dialogOpened = false;
    public int queueNumber;
    public float textSpeed;
    private List<string> lastTextsUsed;
    [Header("Objects")]
    public CanvasGroup _dialogPanel;
    public TextMeshProUGUI generalText;
    [Header("Audio")]
    public AudioSource audioSrc;

    private bool isNewGame = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        generalText.text = " ";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (generalText.text == lastTextsUsed[queueNumber])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                generalText.text = lastTextsUsed[queueNumber];
            }
        }
    }

    public void OpenDialoguePanel(List<string> textLines, bool _isNewGame)
    {
        FadeEnter();
        dialogOpened = true;
        _dialogPanel.gameObject.SetActive(true);
        queueNumber = 0;
        isNewGame = _isNewGame;
        lastTextsUsed = textLines;
        generalText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    public void CloseDialoguePanel()
    {
        dialogOpened = false;
        FadeExit();
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in lastTextsUsed[queueNumber].ToCharArray())
        {
            generalText.text += c;
            audioSrc.pitch = Random.Range(0.6f, 0.8f);
            audioSrc.Play();
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (queueNumber < lastTextsUsed.Count - 1)
        {
            queueNumber++;
            generalText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            CloseDialoguePanel();
        }
    }

    private void FadeEnter()
    {
        _dialogPanel.gameObject.SetActive(true);
        LeanTween.alphaCanvas(_dialogPanel, 1f, 0.2f);
    }

    private void FadeExit()
    {
        LeanTween.alphaCanvas(_dialogPanel, 0f, 0.2f).setOnComplete(() =>
        {
            _dialogPanel.gameObject.SetActive(false);
            if(isNewGame) LeaderboardManager.instance.OpenLeaderboard();
        });
    }
}
