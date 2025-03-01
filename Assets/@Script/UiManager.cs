using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject pop;

    public RadomSpwan rand;
    public Text life;
    public Text score;
    public Text def;
    public Text time;


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
        if(socrs >= 80)
        {
            if (rand.Stage == Define.Stage.Normal)
                SceneManager.LoadScene("ClearScene");
            else
                SceneManager.LoadScene("NiceClear");
        }
        else if (score != null)
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

  

    #region ����ȭ�� ui
    public void Cancle()
    {
        pop.gameObject.SetActive(false);
    }
    public void ShowPopUp()
    {
       pop.gameObject.SetActive(true);
    }
    #endregion

    float gameTime;
    private void Update()
    {
        if (time != null)
        {
            gameTime += Time.unscaledDeltaTime;
            time.text = gameTime.ToString();
        }
            
    }
}
