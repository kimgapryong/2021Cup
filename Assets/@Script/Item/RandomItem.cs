using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : ItemBase
{
    public override void PlayerSetAbility()
    {
        int rand = Random.Range(0, 4);
        switch(rand)
        {
            case 0:
                PlayerSetAbility1();
                break;
            case 1:
                PlayerSetAbility2();
                break;
            case 2:
                PlayerSetAbility3();
                break;
            case 3:
                PlayerSetAbility4();
                break;
        }

        Destroy(gameObject);
    }
    public  void PlayerSetAbility1()
    {
        if (player.godTime)
            return;

        player.godTime = true;
    }
    public void PlayerSetAbility2()
    {
        player.speed += 3f;
        Destroy(gameObject);
    }
    public void PlayerSetAbility3()
    {
        if (GameManager.Instance.Life < 5)
        {
            GameManager.Instance.Life++;
        }
    }
    public void PlayerSetAbility4()
    {
        GameManager.Instance.Def++;
    }
}
