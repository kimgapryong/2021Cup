using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CretureController : MonoBehaviour
{
    private bool first = false;
    private void Awake()
    {
        Init();
    }
    public virtual bool Init()
    {
        if(!first)
        {
            first = true;
            //초기화 부분 작성
            return true;
        }

        return false;
    }
    public virtual void OnDead()
    {

    }
}
