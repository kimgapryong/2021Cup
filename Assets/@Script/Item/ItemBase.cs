using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public PlayerController player;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public abstract void PlayerSetAbility();
}
