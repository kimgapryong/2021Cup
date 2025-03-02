using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReTurn : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void NextScene()
    {
        SceneManager.LoadScene("BossStage");
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene("MyHomeZone");
    }
    public void GoStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void GoRank()
    {
        SceneManager.LoadScene("RankingScene");
    }
    
}
