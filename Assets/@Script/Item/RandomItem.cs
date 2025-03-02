using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : ItemBase
{
    public Sprite[] sprites;
    public override void PlayerSetAbility()
    {
        
        int rand = Random.Range(0, 4);
        switch(rand)
        {
            case 0:
                sprite = sprites[0];
                PlayerSetAbility1();
                break;
            case 1:
                sprite = sprites[1];
                PlayerSetAbility2();
                break;
            case 2:
                sprite = sprites[2];
                PlayerSetAbility3();
                break;
            case 3:
                sprite = sprites[3];
                PlayerSetAbility4();
                break;
        }

        Destroy(gameObject);
    }
    public  void PlayerSetAbility1()
    {
        GetItme();
        if (player.godTime)
            return;

        player.godTime = true;
    }
    public void PlayerSetAbility2()
    {
        GetItme();
        player.speed += 3f;
        Destroy(gameObject);
    }
    public void PlayerSetAbility3()
    {
        GetItme();
        if (GameManager.Instance.Life < 5)
        {
            GameManager.Instance.Life++;
        }
    }
    public void PlayerSetAbility4()
    {
        GetItme();
        GameManager.Instance.Def++;
    }
}
