using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<Decoration> setActivePlacementDecoration;
    // Singleton Pattern
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("Game Manager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    private static GameManager instance;
    public List<Decoration> decorationData;
    private void Awake()
    {
        decorationData = Resources.LoadAll<Decoration>("Decorations").ToList();
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    public void SendDecorationSelect_Action(Decoration decor)
    {
        setActivePlacementDecoration?.Invoke(decor);
    }
}