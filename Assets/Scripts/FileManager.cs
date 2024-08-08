using UnityEngine;
using System.IO;
using SFB;

public class FileManager : MonoBehaviour
{
    [Header("Variables")]
    public Blocks chargedBlocks;
    [Header("GameObjects")]
    public GameObject notReadedFile;
    public GameObject displayBlocks;

    private void Start()
    {
        chargedBlocks = new Blocks();
        notReadedFile.SetActive(true);
        displayBlocks.SetActive(false);
    }

    public void ReadFile()
    {
        string[] path = StandaloneFileBrowser.OpenFilePanel("Selecciona un archivo compatible", "", "json", false);
        string readedFile = File.ReadAllText(path[0]);

        if (readedFile.Contains("blocks"))
        {
            chargedBlocks = JsonUtility.FromJson<Blocks>(readedFile);
            PopulateBlocks.instance.CreateBlocks(chargedBlocks);
            notReadedFile.SetActive(false);
            displayBlocks.SetActive(true);
        }
        else
        {
            Debug.Log("No es un archivo compatible");
        }
    }
}
