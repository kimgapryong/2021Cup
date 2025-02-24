using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GridController grid;

    public float speed = 10;
    private Cell current;
    private Cell next;

    private Vector3Int dir;
    private bool isMoving = false;

    bool[,] closed;
    public List<Vector3Int> cells = new List<Vector3Int>();
    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

    private void Start()
    {

        grid = GameObject.Find("@Grid").GetComponent<GridController>();
        closed = new bool[grid.xTile, grid.yTile];
        Vector3Int currentPos = grid.grid.WorldToCell(transform.position);
        current = grid.cellDic[currentPos];

        Debug.Log(current.x +"/" +current.y);
     
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            dir = Vector3Int.up;
        else if (Input.GetKeyDown(KeyCode.A))
            dir = Vector3Int.left;
        else if (Input.GetKeyDown(KeyCode.S))
            dir = Vector3Int.down;
        else if (Input.GetKeyDown(KeyCode.D))
            dir = Vector3Int.right;

        MovePlayer();
    }

    private void MovePlayer()
    {
        if(!isMoving)
        {
            Vector3Int nextPos = new Vector3Int(current.x, current.y) + dir;
            next = grid.cellDic[nextPos];

            if (next.TiieType == Define.TileTiles.Wall)
                return;
            if (closed[next.x , next.y])
                return; // 죽음


            if(!(next.TiieType == Define.TileTiles.P_Tile))
            {
                cells.Add(new Vector3Int(next.x, next.y));
                closed[next.x, next.y] = true;
            }
            isMoving = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3Int(next.x, next.y), speed * Time.deltaTime);
          
            if (Vector3.Distance(transform.position, new Vector3Int(next.x, next.y)) < 0.001f) 
            {
                if (next.TiieType == Define.TileTiles.P_Tile && cells.Count > 0)
                    ColorGird();

                current = next;
                if(!(current.TiieType == Define.TileTiles.P_Tile))
                {
                    current.TiieType = Define.TileTiles.P_Tile;
                    current.obj.GetComponent<SpriteRenderer>().sprite = grid.p_Sprite;
                    current.obj.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
                }
                


                isMoving = false;
            }
        }
    }

    private void ColorGird()
    {

       Vector3Int nextVec = GetCenterVec();
        
        Debug.Log(nextVec.x + ", " + nextVec.y);

     
        if(nextVec != Vector3Int.zero && cells.Count > 2)
            FoolBfs(nextVec);

        foreach(Vector3Int vec in cells)
        {
            if (grid.cellDic.ContainsKey(vec))
                grid.cellDic[vec].obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        cells.Clear();
        closed = null;
        closed = new bool[grid.xTile, grid.yTile];
    }

    private void FoolBfs(Vector3Int vec)
    {
        if (visited.Contains(vec)) return;  // 이미 방문한 셀이면 리턴
        visited.Add(vec); // 방문한 셀 저장

        if (!grid.cellDic.TryGetValue(vec, out Cell cell)) return;

        cell.TiieType = Define.TileTiles.P_Tile;
        cell.obj.GetComponent<SpriteRenderer>().sprite = grid.p_Sprite;

        Vector3Int[] dirs = new Vector3Int[4]
        {
        new Vector3Int(1, 0), new Vector3Int(-1, 0),
        new Vector3Int(0, 1), new Vector3Int(0, -1)
        };

        for (int i = 0; i < 4; i++)
        {
            Vector3Int nextPos = vec + dirs[i];

            if (grid.cellDic.TryGetValue(nextPos, out Cell nextCell))
            {
                if (nextCell.TiieType != Define.TileTiles.P_Tile && nextCell.TiieType != Define.TileTiles.Wall)
                    FoolBfs(nextPos);
            }
        }
    }
    private Vector3Int GetCenterVec()
    {
        int maxX = cells[0].x;
        int maxY = cells[0].y;
        int minX = cells[0].x;
        int minY = cells[0].y;
        for(int i = 1; i < cells.Count; i++)
        {
            if(maxX < Mathf.Abs(cells[i].x))
                maxX = cells[i].x;

            if(maxY < Mathf.Abs(cells[i].y))
                maxY = cells[i].y;
        }
        for (int i = 1; i < cells.Count; i++)
        {
            if (minX > Mathf.Abs(cells[i].x))
                minX = cells[i].x;

            if (minY > Mathf.Abs(cells[i].y))
                minY = cells[i].y;
        }

       if(minX == maxX ||  minY == maxY)
            return Vector3Int.zero;

        Vector3Int centerPos = Search(new Vector3Int(minX, minY), new Vector3Int(maxX, maxY));

        return centerPos;    
    }

    private Vector3Int Search(Vector3Int minVec, Vector3Int maxVec)
    {
        for(int x = maxVec.x; x >= minVec.x; x--)
        {
            for(int y = maxVec.y; y >= minVec.y; y--)
            {
                if(grid.cellDic.ContainsKey(new Vector3Int(x, y)))
                {
                    if(grid.cellDic[new Vector3Int(x, y)].TiieType == Define.TileTiles.E_Tile)
                    {
                        Debug.Log(x + "," + y);
                        return new Vector3Int(x, y);
                    }
                        
                }
            }
        }

        return Vector3Int.zero;
    }
}
