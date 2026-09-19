using UnityEngine;
using System.IO;
using System.Collections; 
using System.Collections.Generic;
using Newtonsoft.Json;

public class Robot : MonoBehaviour
{
    private List<string> falas = new List<string>();
    private int falaIndex = 0;

    private String pasta = "nome da pasta em vsi estar a cena com as falas, estou a ssumir estar dentro dos assets?"

    void Start()
    {
        string filePath = Path.Combine(Application.dataPath, pasta, "falas.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var dados = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
            if (dados != null)
            {
                foreach (var item in dados)
                {
                    foreach (var kvp in item)
                    {
                        falas.Add(kvp.Value);
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"Ficheiro de falas não encontrado");
        }
    }

    void Update()
    {

    }

    public void EscolhaFala(bool interagir, float tempoEspera = 5.0f)
    {
        if (interagir)
        {
            string fala = ObterFala();
        }
        else
        {
            StartCoroutine(aguardar(tempoEspera));
        }
    }

    private IEnumerator aguardar(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        string fala = ObterFala();
    }

    public string ObterFala()
    {
        if (falas.Count == 0) return string.Empty;

        string fala = falas[falaIndex];
        
        falaIndex++;
        if (falaIndex >= falas.Count)
        {
            falaIndex = 0;
        }

        return fala;
    }
}