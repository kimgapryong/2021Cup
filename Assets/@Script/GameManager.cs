using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public Action<int, int> ScoreAction;
    public Action<int> LifeAction;

    [SerializeField]
    private GridController grid;

    public int maxScore;

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            ScoreAction?.Invoke(maxScore, value);
        }
    }

    private int _life;
    public int Life
    {
        get
        {
            return _life;
        }
        set
        {
            _life = value;
            LifeAction?.Invoke(value);
        }
    }
    
    private void Awake()
    {
        Init();
        maxScore = (grid.xTile * grid.yTile * 80) / 100;
        Life = 5;
    }

    private void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Manager");
            if (go != null)
            {
                DestroyImmediate(go);
                go = new GameObject("@Manager");
                go.AddComponent<GameManager>();
            }
            _instance = go.GetComponent<GameManager>();
            DontDestroyOnLoad(go);
        }
    }

}
