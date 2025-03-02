using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : ItemBase
{
    public override void PlayerSetAbility()
    {
        GetItme();
        if (GameManager.Instance.Life < 5)
        {
            GameManager.Instance.Life++;
        }
        Destroy(gameObject);
    }
}
