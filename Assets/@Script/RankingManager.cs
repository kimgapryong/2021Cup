using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingManager
{
    public int maxRankCount = 15;
    public void SaveData(float score)
    {
        List<float> scores = ScoreList();
        scores.Add(score);


        scores = scores.OrderBy(s => s).Take(maxRankCount).ToList();


        // 랭킹을 다시 저장
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetFloat($"Rank_{i}", scores[i]);
        }
        PlayerPrefs.Save(); // 데이터 저장
    }

    public List<float> ScoreList()
    {
        List<float> ints = new List<float>();

        for(int i = 0; i < maxRankCount; i++)
        {
            if (PlayerPrefs.HasKey($"Rank_{i}"))
            {
                ints.Add(PlayerPrefs.GetFloat($"Rank_{i}"));
            }
        }
        return ints;
    }
}
