using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : ItemBase
{
    public override void PlayerSetAbility()
    {
        player.speed += 3f;
        Destroy(gameObject);
    }
}
