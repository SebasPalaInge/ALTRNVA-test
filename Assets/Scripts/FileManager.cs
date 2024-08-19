using UnityEngine;
using System.IO;
using SFB;
using System.Linq;

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
        if (DialogBehaviour.instance.dialogOpened) return;

        string[] path = StandaloneFileBrowser.OpenFilePanel("Selecciona un archivo compatible", "", "json", false);

        if (path.Length > 0)
        {
            if (string.IsNullOrEmpty(path[0]))
            {
                Debug.Log("No se encontró una ruta al archivo.");
                GameManager.instance.SetTextAndColor("La ruta al archivo no se encuentra o se canceló la busqueda.", Color.red);
                return;
            }
        }
        else
        {
            Debug.Log("No existe ruta (Posible cierre de fileExplorer).");
            GameManager.instance.SetTextAndColor("La ruta al archivo no se encuentra o se canceló la busqueda.", Color.red);
            return;
        }

        string readedFile = File.ReadAllText(path[0]);

        if (readedFile.Contains("blocks"))
        {
            chargedBlocks = JsonUtility.FromJson<Blocks>(readedFile);
            if (chargedBlocks.blocks.Count % 2 != 0) //Es impar
            {
                Debug.Log("El archivo contiene una cantidad de elementos impar.");
                GameManager.instance.SetTextAndColor("El archivo contiene una cantidad de elementos impar.", Color.red);
                return;
            }
            PopulateBlocks.instance.CreateBlocks(chargedBlocks);
        }
        else
        {
            Debug.Log("No es un archivo compatible");
            GameManager.instance.SetTextAndColor("El archivo seleccionado no es compatible.", Color.red);
        }
    }

    public void SaveFile(string jsonObject)
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Selecciona donde quieres guardar los resultados", "", "results", "json");
        Debug.Log(path);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, jsonObject);
            GameManager.instance.pathToResultsNull = false;
        }
        else
        {
            GameManager.instance.pathToResultsNull = true;
        }
    }
}
