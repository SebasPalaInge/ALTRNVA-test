using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BlockDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Block blockInfo;
    [Header("Variables")]
    public bool isUncovered = false;
    public bool isCompleted = false;
    [Header("Display objects")]
    public TextMeshProUGUI numberDisplay;
    public CanvasGroup coverAlpha;
    
    public void UpdateDisplay(Block _blockInfo)
    {
        blockInfo = _blockInfo;
        numberDisplay.text = blockInfo.number.ToString();
    }

    public void Uncover()
    {
        if(isUncovered || GameManager.instance.comparing) return;

        GameManager.instance.numberOfClicks++;
        LeanTween.scale(gameObject, new Vector2(1f, 1f), .1f);

        if(!GameManager.instance.isFistUncovered) //Is the first card to uncover
        {
            LeanTween.alphaCanvas(coverAlpha, 0f, .15f);
            GameManager.instance.firstBlockSelected = this;
            GameManager.instance.isFistUncovered = true;
        }
        else
        {
            LeanTween.alphaCanvas(coverAlpha, 0f, .15f);
            GameManager.instance.secondBlockSelected = this;
            GameManager.instance.StartComparison();
        }

        isUncovered = true;
    }

    public void Cover()
    {
        isUncovered = false;
        LeanTween.alphaCanvas(coverAlpha, 1f, .1f);
    }

    public void Complete()
    {
        isCompleted = true;
        LeanTween.color(GetComponent<RectTransform>(), new Color32(120, 224, 143, 255), .1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isUncovered) return;
        LeanTween.scale(gameObject, new Vector2(1.1f, 1.1f), .1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isUncovered) return;
        LeanTween.scale(gameObject, new Vector2(1f, 1f), .1f);
    }

}
