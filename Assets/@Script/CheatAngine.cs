using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatAngine : MonoBehaviour
{
    PlayerController player;

    public void Init(PlayerController pa)
    {
        player = pa;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (player.godTime)
                return;

            player.godTime = true;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            int rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0:
                    PlayerSetAbility1();
                    break;
                case 1:
                    PlayerSetAbility2();
                    break;
                case 2:
                    PlayerSetAbility3();
                    break;
                case 3:
                    PlayerSetAbility4();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameManager.Instance.Life++;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("MyHomeZone");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("Stage1");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene("BossStage");
        }
    }

    public void PlayerSetAbility1()
    {
        if (player.godTime)
            return;

        player.godTime = true;
    }
    public void PlayerSetAbility2()
    {
        player.speed += 3f;
        Destroy(gameObject);
    }
    public void PlayerSetAbility3()
    {
        if (GameManager.Instance.Life < 5)
        {
            GameManager.Instance.Life++;
        }
    }
    public void PlayerSetAbility4()
    {
        GameManager.Instance.Def++;
    }
}

