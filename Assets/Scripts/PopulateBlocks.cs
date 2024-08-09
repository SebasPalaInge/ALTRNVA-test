using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateBlocks : MonoBehaviour
{
    public static PopulateBlocks instance;
    [Header("Objects")]
    public GameObject displayBlockParent;
    public GameObject displayBlockPrefab;
    public FlexibleGridLayoutTweaked gridLayout;
    public List<BlockDisplay> blocksDisplayed;
    public List<Sprite> images;

    private void Awake()
    {
        instance = this;
        gridLayout = displayBlockParent.GetComponent<FlexibleGridLayoutTweaked>();
    }

    private void Start()
    {
        blocksDisplayed = new List<BlockDisplay>();
    }

    public void CreateBlocks(Blocks chargedBlocks)
    {
        //Instance prefabs
        List<int> rows = new List<int>();
        List<int> columns = new List<int>();

        for (int i = 0; i < chargedBlocks.blocks.Count; i++)
        {
            rows.Add(chargedBlocks.blocks[i].R);
            columns.Add(chargedBlocks.blocks[i].C);
            GameObject blockGO = Instantiate(displayBlockPrefab, displayBlockParent.transform);
        }

        //Getting max number of rows and columns and adding limits
        rows.Sort();
        columns.Sort();

        int maxRows = rows[rows.Count - 1];
        int maxColumns = columns[columns.Count - 1];
        //int maxRows = 15;
        //int maxColumns = 15;

        if (maxRows > 8) maxRows = 8;
        if (maxRows < 2) maxRows = 2;
        if (maxColumns > 8) maxColumns = 8;
        if (maxColumns < 2) maxColumns = 2;

        gridLayout.rows = maxRows;
        gridLayout.columns = maxColumns;
        gridLayout.CalculateLayoutInputHorizontal();

        int rNum = 1;
        int cNum = 1;

        //Changing names
        for (int i = 0; i < chargedBlocks.blocks.Count; i++)
        {
            displayBlockParent.transform.GetChild(i).name = rNum + " " + cNum;
            cNum++;
            if (cNum > gridLayout.columns)
            {
                rNum++;
                cNum = 1;
            }
        }

        //Updating display
        for (int i = 0; i < chargedBlocks.blocks.Count; i++)
        {
            Block compareBlock = chargedBlocks.blocks[i];
            string comparison = compareBlock.R.ToString() + " " + compareBlock.C.ToString();

            for (int j = 0; j < chargedBlocks.blocks.Count; j++)
            {
                Transform actualChild = displayBlockParent.transform.GetChild(j);
                if (comparison == actualChild.name)
                {
                    actualChild.GetComponent<BlockDisplay>().UpdateDisplay(chargedBlocks.blocks[i], SelectSprite(chargedBlocks.blocks[i].number));
                    break;
                }
            }
        }

        for (int i = 0; i < displayBlockParent.transform.childCount; i++)
        {
            blocksDisplayed.Add(displayBlockParent.transform.GetChild(i).GetComponent<BlockDisplay>());
        }

        GameManager.instance.canRunGameTime = true;
    }

    public void DestroyContent()
    {
        for (int i = 0; i < blocksDisplayed.Count; i++)
        {
            Destroy(blocksDisplayed[i].gameObject);
        }
        
        blocksDisplayed = new List<BlockDisplay>();
    }

    public bool AreBlocksCompleted()
    {
        bool areCompleted = false;
        for (int i = 0; i < blocksDisplayed.Count; i++)
        {
            if(!blocksDisplayed[i].isCompleted)
            {
                areCompleted = false;
                break;
            }
            else areCompleted = true;
            
        }
        return areCompleted;
    }

    private Sprite SelectSprite(int number)
    {
        Sprite newSprite = images[number];
        return newSprite;
    }
}
