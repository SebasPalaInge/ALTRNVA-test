using UnityEngine;
using System.IO;
using SFB;

public class FileManager : MonoBehaviour
{
    public static FileManager instance;
    [Header("Variables")]
    public Blocks chargedBlocks;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        chargedBlocks = new Blocks();
    }

    public void ReadFile()
    {
        string[] path = StandaloneFileBrowser.OpenFilePanel("Selecciona un archivo compatible", "", "json", false);
        string readedFile = File.ReadAllText(path[0]);

        if (readedFile.Contains("blocks"))
        {
            chargedBlocks = JsonUtility.FromJson<Blocks>(readedFile);
            PopulateBlocks.instance.CreateBlocks(chargedBlocks);
            GameManager.instance.notReadedFile.SetActive(false);
            GameManager.instance.displayBlocks.SetActive(true);
        }
        else
        {
            Debug.Log("No es un archivo compatible");
        }
    }

    public void SaveFile(string jsonObject)
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Selecciona donde quieres guardar los resultados", "", "results", "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, jsonObject);
        }
    }
}
