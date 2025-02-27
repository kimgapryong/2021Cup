using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text life;
    public Text score;

    void Start()
    {
        GameManager.Instance.ScoreAction -= SetScore;
        GameManager.Instance.ScoreAction += SetScore;

        GameManager.Instance.LifeAction -= SetLife;
        GameManager.Instance.LifeAction += SetLife;
    }

    private void SetScore(int max, int current)
    {
     
         float socrs = (float)current / max * 100;
        socrs = Mathf.Floor(socrs * 10) / 10;
        if (score != null)
            score.text = $"{socrs}%";
    }

    private void SetLife(int hp)
    {
        life.text = hp.ToString();
    }
}
