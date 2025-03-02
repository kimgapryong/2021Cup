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

    bool isSceneLoading = false;
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
        if(socrs >= 80 && !isSceneLoading)
        {
            isSceneLoading = true; 
            if (rand.Stage == Define.Stage.Normal)
                SceneManager.LoadScene("ClearScene");
            else if (rand.Stage == Define.Stage.Boss)
            {
                GameManager.Rank.SaveData(GameManager.Instance.GameTime);
                SceneManager.LoadScene("NiceClear");
            }
                
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

  

    #region 시작화면 ui
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
            GameManager.Instance.GameTime = gameTime;
            time.text = gameTime.ToString();
        }
            
    }
}
