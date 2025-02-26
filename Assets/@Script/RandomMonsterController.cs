using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomMonsterController : MonsterController
{
    public float telTime = 5;


    protected override void Start()
    {
        base.Start();
        speed = 2.5f;
        moveTime = 1;

        float randScale = Random.Range(0.7f,1.7f);
        transform.localScale = new Vector3(randScale, randScale);

        StartCoroutine(Telephoto());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    IEnumerator Telephoto()
    {
        while (true)
        {
            yield return new WaitForSeconds(telTime);

            if(coroutine != null)
                StopCoroutine(coroutine);


            dir = Vector3Int.zero;
            isTel = true;

            Vector3Int currentPos = Tel();
            transform.position = currentPos;

            next = null;
            current = grid.cellDic[new Vector3Int(currentPos.x, currentPos.y)];

            coroutine = StartCoroutine(RandomDir());

            isMoving = false;
            isTel = false;

        }
        
    }

    private Vector3Int Tel()
    {
        int x = Random.Range(1, grid.xTile - 1);
        int y = Random.Range(1, grid.yTile - 1); 
        
        if(grid.cellDic.ContainsKey(new Vector3Int(x, y)))
        {
            if (grid.cellDic[new Vector3Int(x, y)].TiieType == Define.TileTiles.P_Tile || grid.cellDic[new Vector3Int(x,y)].TiieType == Define.TileTiles.Wall)
                return Tel();
            else 
                return new Vector3Int(x, y);
        }
        return new Vector3Int(0, 0);
    }
}
