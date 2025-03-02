using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public PlayerController player;
    public Sprite sprite;
    public GameObject obj;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        Init();
    }
    public virtual void Init()
    {

    }
    public virtual void GetItme()
    {
        GameObject item = Instantiate(obj, Camera.main.transform);
        Destroy(item, item.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        item.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public abstract void PlayerSetAbility();
}
