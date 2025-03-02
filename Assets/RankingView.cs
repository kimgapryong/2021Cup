using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingView : MonoBehaviour
{
    public List<RankPanel> rankPanels = new List<RankPanel>();
    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < 20; j++)
        {
            float rand = Random.Range(0f, 155f);
            GameManager.Rank.SaveData(rand);
        }

        int i = 0;
        foreach (var panel in rankPanels)
        {
            if(GameManager.Rank.ScoreList().Count <= i)
                panel.gameObject.SetActive(false);
            else
                panel.rankText.text = GameManager.Rank.ScoreList()[i].ToString();

            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
