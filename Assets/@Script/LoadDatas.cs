using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDatas : MonoBehaviour
{
    public GameObject panel;

    public float startPosY = 325;
    public float minus = -160;

    private void Start()
    {

        //StartRankLoad();
    }
    private void StartRankLoad()
    {
        List<float> list = GameManager.Rank.ScoreList();
  
        for(int i = 0; i < Mathf.Min(GameManager.Rank.maxRankCount, list.Count); i++)
        {

            GameObject clone = Instantiate(panel, transform.Find("RankPanel"));
            clone.name = panel.name;

            clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(clone.GetComponent<RectTransform>().anchoredPosition.x, startPosY + (i * minus));


            clone.transform.Find("RankTxt").GetComponent<Text>().text = (i + 1).ToString();
            clone.transform.Find("Second").GetComponent <Text>().text = $"{list[i]}√ ";
        }
    }
}
