using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text life;
    public Text score;
    public Text def;

    public void Init()
    {
        GameManager.Instance.ScoreAction -= SetScore;
        GameManager.Instance.ScoreAction += SetScore;

        GameManager.Instance.LifeAction -= SetLife;
        GameManager.Instance.LifeAction += SetLife;


        GameManager.Instance.DefAction -= SetDef;
        GameManager.Instance.DefAction += SetDef;
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

    private void SetDef(int sh)
    {
        Debug.Log("dlaglksdka");
        def.text = sh.ToString();
    }
}
