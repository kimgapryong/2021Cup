using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMonsterController : MonsterController
{

    Vector3Int[] moves = new Vector3Int[8] {new Vector3Int(1,0), new Vector3Int(-1,0), 
        new Vector3Int(0,-1), new Vector3Int(0,1), 
        new Vector3Int(1,1), new Vector3Int(1,-1), new Vector3Int(-1,1), new Vector3Int(-1,-1) };
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveTime = 5f;
        speed = 1f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (!isMoving && dir != Vector3Int.zero)
        {
            Vector3Int nextPos = new Vector3Int(current.x, current.y) + dir;
            next = grid.cellDic[nextPos];

            if (next.TiieType == Define.TileTiles.Wall)
                return;


            if (playerController.closed[next.x, next.y])
            {
                if (GameManager.Instance.Def > 0)
                {

                    GameManager.Instance.Def--;
                    next.TiieType = tileTiles;
                    next.obj.GetComponent<SpriteRenderer>().sprite = Sprite;
                    next.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    playerController.canWrite = true;

                    Destroy(gameObject);
                    return;
                }

                playerController.PlayerReTrans(out dir);
                transform.position = grid.grid.CellToWorld(new Vector3Int(current.x, current.y));
                return;
            }

            if (tileTiles == Define.TileTiles.E_Tile)
            {
                current.monster = null;
                next.monster = gameObject;
            }

            isMoving = true;
        }
        else if (isMoving && !isTel)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3Int(next.x, next.y), speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, new Vector3Int(next.x, next.y)) < 0.001f)
            {

                current = next;
                if (!(current.TiieType == tileTiles))
                {
                    current.TiieType = Define.TileTiles.Ee_Tile;
                    current.obj.GetComponent<SpriteRenderer>().sprite = Sprite;
                    current.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    for (int i = 0; i < moves.Length - 1; i++)
                    {
                        if (grid.cellDic.TryGetValue(new Vector3Int(current.x, current.y) + moves[i], out Cell cell))
                        {
                            if(cell.TiieType != Define.TileTiles.Wall && cell.TiieType != Define.TileTiles.E_Tile)
                            {
                                cell.TiieType = Define.TileTiles.Ee_Tile;  
                                cell.obj.GetComponent<SpriteRenderer>().sprite = Sprite;
                                cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                            }
                        }
                    }
                }

                isMoving = false;
            }
        }
    }
}
