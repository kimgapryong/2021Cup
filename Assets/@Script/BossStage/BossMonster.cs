using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonsterController
{
    public Define.MonsterState state;
    public GameObject attackBall;

    Vector3Int[] moves = new Vector3Int[8] {new Vector3Int(1,0), new Vector3Int(-1,0),
        new Vector3Int(0,-1), new Vector3Int(0,1),
        new Vector3Int(1,1), new Vector3Int(1,-1), new Vector3Int(-1,1), new Vector3Int(-1,-1) };

    protected override void Start()
    {
        state = Define.MonsterState.Moving;

        base.Start();

        moveTime = 5;
        speed = 2f;

        randVec = new Vector3Int[8] { new Vector3Int(1, 0), new Vector3Int(-1, 0), new Vector3Int(0, 1), new Vector3Int(0, -1),
        new Vector3Int(1, 1), new Vector3Int(-1, 1), new Vector3Int(-1, -1), new Vector3Int(1, -1)};

        if(coroutine != null )
            StopCoroutine(coroutine);

        StartCoroutine(RandomDir());
        StartCoroutine(WriteState());
    }

    protected override void Update()
    {
        switch(state)
        {
            case Define.MonsterState.Moving:
                MovePlayer();
                break;
            case Define.MonsterState.Attack:
                AtkPlayer();
                break;
        }
    }

    private void AtkPlayer()
    {
       for(int i = 0; i < moves.Length; i++)
        {
            GameObject obj = Instantiate(attackBall);
            obj.transform.position = grid.grid.WorldToCell(transform.position);
            obj.GetComponent<BulletMonster>().BulletInit(moves[i]);
        }
       state = Define.MonsterState.Moving;
    }

    private void MovePlayer()
    {
        if (!isMoving && dir != Vector3Int.zero)
        {
            Vector3Int curr;
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < moves.Length; j++)
                {
                    curr = new Vector3Int(current.x, current.y) + (moves[j] * i);
                    if (grid.cellDic.TryGetValue(curr, out Cell cell))
                    {
                        if (cell.TiieType == Define.TileTiles.Wall)
                            continue;

                        if (cell.TiieType != Define.TileTiles.E_Tile)
                        {
                            cell.TiieType = Define.TileTiles.E_Tile;
                            cell.obj.GetComponent<SpriteRenderer>().sprite = Sprite;
                            cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        }

                        if (playerController.closed[cell.x, cell.y])
                        {
                            playerController.PlayerReTrans(out dir);
                            transform.position = grid.grid.CellToWorld(new Vector3Int(current.x, current.y));
                            return;
                        }
                    }
                }
            }
            if (grid.cellDic.ContainsKey(new Vector3Int(current.x, current.y) + dir))
                next = grid.cellDic[new Vector3Int(current.x, current.y) + dir];

            isMoving = true;
        }
        else if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3Int(next.x, next.y), speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, new Vector3Int(next.x, next.y)) < 0.001f)
            {
                current = next;
                isMoving = false;
            }
        }
    }

    public override IEnumerator RandomDir()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveTime);
            int rand = Random.Range(0, randVec.Length);
            dir = randVec[rand];
        }
    }

    private IEnumerator WriteState()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            state = Define.MonsterState.Attack;
        }
    }
}
