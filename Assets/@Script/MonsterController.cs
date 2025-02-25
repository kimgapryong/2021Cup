using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CretureController
{
    public float moveTime = 2f;
    Vector3Int[] randVec;
    
    protected override void Start()
    {
        base.Start();

        tileTiles = Define.TileTiles.E_Tile;
        Sprite = grid.e_Sprite;

        randVec = new Vector3Int[4] {new Vector3Int(1,0), new Vector3Int(-1,0), new Vector3Int(0,1), new Vector3Int(0,-1) };
        StartCoroutine(RandomDir());


    }


    protected override void Update()
    {
        base.Update();
    }

    IEnumerator RandomDir()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveTime);
            int rand = Random.Range(0, randVec.Length);
            dir = randVec[rand];
        }
       
    }
}
