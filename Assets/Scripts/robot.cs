using UnityEngine;
using System.IO;
using System.Collections; 
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class DialogoItem
{
    public int id;
    public List<string> linhas;
}

public class Robot : MonoBehaviour
{
    private List<DialogoItem> dialogos = new List<DialogoItem>();
    private List<int> indicesNaoUsados = new List<int>();
    private bool todasForamDitas = false;
    private string pasta = "Resources"; 

    void Start()
    {
        string filePath = Path.Combine(Application.dataPath, pasta, "falas.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            dialogos = JsonConvert.DeserializeObject<List<DialogoItem>>(json);
            if (dialogos != null && dialogos.Count > 0)
            {
                ResetIndicesNaoUsados();
            }
        }
        else
        {
            Debug.LogError($"Ficheiro de falas não encontrado ");
        }
    }

    void Update()
    {

    }

    public void EscolhaFala(bool interagir, System.Action<List<string>> aoObterFala, float tempoEspera = 4.0f)
    {
        if (interagir)
        {
            List<string> linhas = ObterFala();
            aoObterFala?.Invoke(linhas);
        }
        else
        {
            StartCoroutine(Aguardar(tempoEspera, aoObterFala));
        }
    }

    private IEnumerator Aguardar(float segundos, System.Action<List<string>> aoObterFala)
    {
        yield return new WaitForSeconds(segundos);
        List<string> linhas = ObterFala();
        aoObterFala?.Invoke(linhas);
    }

    public List<string> ObterFala()
    {
        if (dialogos == null || dialogos.Count == 0) 
            return new List<string>();

        int dialogoIndex = 0;

        if (!todasForamDitas)
        {
            int randomIndex = Random.Range(0, indicesNaoUsados.Count);
            dialogoIndex = indicesNaoUsados[randomIndex];
            indicesNaoUsados.RemoveAt(randomIndex);

            if (indicesNaoUsados.Count == 0)
            {
                todasForamDitas = true;
            }
        }
        else
        {
            dialogoIndex = Random.Range(0, dialogos.Count);
        }

        List<string> linhasAtuais = dialogos[dialogoIndex].linhas;

        return linhasAtuais;
    }

    private void ResetIndicesNaoUsados()
    {
        indicesNaoUsados.Clear();
        for (int i = 0; i < dialogos.Count; i++)
        {
            indicesNaoUsados.Add(i);
        }
    }
}