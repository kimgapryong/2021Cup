using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReTurn : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene("Stage1");
    }
    
}
