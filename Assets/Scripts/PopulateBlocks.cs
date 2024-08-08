using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateBlocks : MonoBehaviour
{
    public static PopulateBlocks instance;
    public GameObject displayBlockParent;
    public GameObject displayBlockPrefab;
    public FlexibleGridLayout gridLayout;

    private void Awake()
    {
        instance = this;
        gridLayout = displayBlockParent.GetComponent<FlexibleGridLayout>();
    }

    public void CreateBlocks(Blocks chargedBlocks)
    {
        List<int> rows = new List<int>();
        List<int> columns = new List<int>();

        //Instance prefabs
        for (int i = 0; i < chargedBlocks.blocks.Count; i++)
        {
            rows.Add(chargedBlocks.blocks[i].R);
            columns.Add(chargedBlocks.blocks[i].C);
            GameObject blockGO = Instantiate(displayBlockPrefab, displayBlockParent.transform);
        }

        rows.Sort();
        columns.Sort();
        gridLayout.rows = rows[rows.Count - 1];
        gridLayout.columns = columns[columns.Count - 1];

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

        for (int i = 0; i < chargedBlocks.blocks.Count; i++)
        {
            string comparison = chargedBlocks.blocks[i].R.ToString() + " " + chargedBlocks.blocks[i].C.ToString();

            for (int j = 0; j < chargedBlocks.blocks.Count; j++)
            {
                Transform actualChild = displayBlockParent.transform.GetChild(j);
                if (comparison == actualChild.name)
                {
                    actualChild.GetComponent<BlockDisplay>().UpdateDisplay(chargedBlocks.blocks[i]);
                    break;
                }
            }
        }
    }
}
