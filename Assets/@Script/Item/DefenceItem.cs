using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceItem : ItemBase
{
    public override void PlayerSetAbility()
    {
        GameManager.Instance.Def++;
        Destroy(gameObject);
    }
}
