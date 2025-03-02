using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodItem : ItemBase
{
    public override void PlayerSetAbility()
    {
        GetItme();
        if (player.godTime)
            return;
        
        player.godTime = true;
        Destroy(gameObject);
    }

   
}
