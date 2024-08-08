using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockDisplay : MonoBehaviour
{
    public Block blockInfo;
    public TextMeshProUGUI numberDisplay;
    
    public void UpdateDisplay(Block _blockInfo)
    {
        blockInfo = _blockInfo;
        numberDisplay.text = blockInfo.number.ToString();
    }
}
