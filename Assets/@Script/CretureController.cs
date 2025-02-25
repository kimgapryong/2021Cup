using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CretureController : MonoBehaviour
{
    public Define.TileTiles tileTiles { get; set; }
    public GridController grid;
    public bool isMoving = false;
    public Cell current;
    public Cell next;

    public bool[,] closed;

    public float speed;

    public Vector3Int dir = Vector3Int.zero;
    public List<Vector3Int> cells = new List<Vector3Int>();

    public Sprite Sprite { get; set; }
    protected virtual void Start()
    {
        grid = GameObject.Find("@Grid").GetComponent<GridController>();
        closed = new bool[grid.xTile, grid.yTile];
        Vector3Int currentPos = grid.grid.WorldToCell(transform.position);
        current = grid.cellDic[currentPos];

    }

    // Update is called once per frame
    protected virtual void Update()
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

            if (closed[next.x, next.y])
                return; // Á×À½


            if (!(next.TiieType == tileTiles) && tileTiles == Define.TileTiles.P_Tile)
            {
                cells.Add(new Vector3Int(next.x, next.y));
                closed[next.x, next.y] = true;
            }
            isMoving = true;
        }
        else if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3Int(next.x, next.y), speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, new Vector3Int(next.x, next.y)) < 0.001f)
            {
                if (next.TiieType == tileTiles && cells.Count > 0)
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

    public virtual void ColorGird()
    {

    }
}
