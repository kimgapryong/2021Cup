using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T compoent = obj.GetComponent<T>();
        if (compoent == null)
            compoent = obj.AddComponent<T>();
        return compoent;
    }
}
