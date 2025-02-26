using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CretureController
{
    public float moveTime = 2f;
    Vector3Int[] randVec;
    public Coroutine coroutine;
    public bool isTel; //텔레포트 몬스터 전용
    protected override void Start()
    {

        tileTiles = Define.TileTiles.E_Tile;

        base.Start();

        Sprite = grid.e_Sprite;

        randVec = new Vector3Int[4] {new Vector3Int(1,0), new Vector3Int(-1,0), new Vector3Int(0,1), new Vector3Int(0,-1) };
        coroutine = StartCoroutine(RandomDir());


    }


    protected override void Update()
    {
        MoveMonster();
    }

    public IEnumerator RandomDir()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveTime);
            int rand = Random.Range(0, randVec.Length);
            dir = randVec[rand];
        }
       
    }
    private void MoveMonster()
    {
        if (!isMoving && dir != Vector3Int.zero)
        {
            Vector3Int nextPos = new Vector3Int(current.x, current.y) + dir;
            next = grid.cellDic[nextPos];

            if (next.TiieType == Define.TileTiles.Wall)
                return;

            if (playerController.closed[next.x, next.y])
            {
                playerController.PlayerReTrans();
                return;
            }

            isMoving = true;
        }
        else if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3Int(next.x, next.y), speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, new Vector3Int(next.x, next.y)) < 0.001f)
            {
                if (next.TiieType == tileTiles && cells.Count > 0 && tileTiles == Define.TileTiles.P_Tile)
                    ColorGird();

                current = next;
                if (!(current.TiieType == tileTiles))
                {
                    current.TiieType = tileTiles;
                    current.obj.GetComponent<SpriteRenderer>().sprite = Sprite;
                    current.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                }



                isMoving = false;
            }
        }
    }

}
