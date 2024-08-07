using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;
using TMPro;

public class FileManager : MonoBehaviour
{
    public string readedFile;
    public GameObject notReadedFile;
    public GameObject fileTest;
    public TextMeshProUGUI textImportedTest;
    public Blocks chargedBlocks;

    private void Start()
    {
        chargedBlocks = new Blocks();
        notReadedFile.SetActive(true);
        fileTest.SetActive(false);
    }

    public void ReadFile()
    {
        string[] path = StandaloneFileBrowser.OpenFilePanel("Selecciona un archivo compatible", "", "json", false);
        readedFile = File.ReadAllText(path[0]);

        if(readedFile.Contains("blocks"))
        {
            chargedBlocks = JsonUtility.FromJson<Blocks>(readedFile);
        }
        else
        {
            Debug.Log("No es un archivo compatible");
        }

        //TEST 
        textImportedTest.text = readedFile;
        notReadedFile.SetActive(false);
        fileTest.SetActive(true);
    }
}

[System.Serializable]
public class Blocks
{
    public List<Block> blocks;
}
