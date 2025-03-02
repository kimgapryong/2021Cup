using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveJson
{
   static string path = Application.dataPath + "/Resources/JsonData/playerData.json";

    public static void SaveData<T>(T datas)
    {
        string data = JsonUtility.ToJson(datas);
    }

}
