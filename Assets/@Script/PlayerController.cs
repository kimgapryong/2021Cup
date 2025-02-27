using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CretureController
{
    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
    public int playerHp = 5;
    public Vector3Int currentMax;
    public Vector3Int currentMin;
    public Dictionary<Vector3Int, Vector3Int> newVecInt = new Dictionary<Vector3Int, Vector3Int>()
    {
        {Vector3Int.up, Vector3Int.down},
        {Vector3Int.down, Vector3Int.up},
        {Vector3Int.right, Vector3Int.left},
        {Vector3Int.left, Vector3Int.right},
    };

    private bool isKey = false;
    protected override void Start()
    {
        base.Start();

        tileTiles = Define.TileTiles.P_Tile;

        speed = 10;
        Sprite = grid.p_Sprite;

     
    }
    protected override void Update()
    {
        if (!isKey)
        {
            if (Input.GetKeyDown(KeyCode.W))
                dir = Vector3Int.up;
            else if (Input.GetKeyDown(KeyCode.A))
                dir = Vector3Int.left;
            else if (Input.GetKeyDown(KeyCode.S))
                dir = Vector3Int.down;
            else if (Input.GetKeyDown(KeyCode.D))
                dir = Vector3Int.right;

        }

        base.Update();
    }

  

    public override void ColorGird()
    {

       Vector3Int nextVec = GetCenterVec();

        if(nextVec != Vector3Int.zero && cells.Count > 2)
            FoolBfs(nextVec);


        foreach (Vector3Int vec in cells)
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

            

        if(cell.TiieType == Define.TileTiles.E_Tile)
        {
            cell.TiieType = Define.TileTiles.P_Tile;
            cell.obj.GetComponent<SpriteRenderer>().sprite = grid.p_Sprite;
            cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        if(cell.monster != null)
            Destroy(cell.monster);

        Vector3Int[] dirs = new Vector3Int[8]
        {
        new Vector3Int(1, 0), new Vector3Int(-1, 0),
        new Vector3Int(0, 1), new Vector3Int(0, -1),
        new Vector3Int(1,1), new Vector3Int(-1,1),
        new Vector3Int(1,-1), new Vector3Int(-1,-1)
        };

        for (int i = 0; i < dirs.Length - 1; i++)
        {
            Vector3Int nextPos = vec + dirs[i];

            if (grid.cellDic.TryGetValue(nextPos, out Cell nextCell))
            {
                if (nextCell.TiieType != Define.TileTiles.P_Tile)
                    FoolBfs(nextPos);
                continue;
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

       if (minX == maxX ||  minY == maxY)
            return Vector3Int.zero;
        currentMax = new Vector3Int(maxX, maxY);
        currentMin = new Vector3Int(minX, minY);
        Vector3Int centerPos = Search(new Vector3Int(minX, minY), new Vector3Int(maxX, maxY));

        return centerPos;    
    }

    private Vector3Int Search(Vector3Int minVec, Vector3Int maxVec)
    {
        int count = 0;
        Vector3Int current = new Vector3Int(maxVec.x - 1, maxVec.y - 1);

       

        if(grid.cellDic.ContainsKey(current))
        {
            if(grid.cellDic[current].TiieType == Define.TileTiles.E_Tile || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
            {
                if (closed[current.x + Vector3Int.up.x, current.y + Vector3Int.up.y] &&
                    closed[current.x + Vector3Int.right.x, current.y + Vector3Int.right.y]) 
                    return current;
                
            }
            else
            {
                current = new Vector3Int(minVec.x + 1, maxVec.y - 1);
                if (closed[current.x + Vector3Int.up.x, current.y + Vector3Int.up.y] &&
                    closed[current.x + Vector3Int.left.x, current.y + Vector3Int.left.y] &&
                    grid.cellDic[current].TiieType == Define.TileTiles.E_Tile
                     || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
                    return current;

                current = new Vector3Int(minVec.x + 1, minVec.y + 1);
                if (closed[current.x + Vector3Int.down.x, current.y + Vector3Int.down.y] &&
                    closed[current.x + Vector3Int.left.x, current.y + Vector3Int.left.y] &&
                    grid.cellDic[current].TiieType == Define.TileTiles.E_Tile
                     || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
                    return current;

                current = new Vector3Int(maxVec.x - 1, minVec.y + 1);
                if (closed[current.x + Vector3Int.up.x, current.y + Vector3Int.up.y] &&
                    closed[current.x + Vector3Int.right.x, current.y + Vector3Int.right.y] &&
                    grid.cellDic[current].TiieType == Define.TileTiles.E_Tile
                     || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
                    return current;
            }

        }

        for(int x = maxVec.x; x >= minVec.x; x--)
        {
            for(int y = maxVec.y ; y >= minVec.y; y--)
            {
                if(grid.cellDic.ContainsKey(new Vector3Int(x, y)))
                {
                    if(grid.cellDic[new Vector3Int(x, y)].TiieType == Define.TileTiles.P_Tile)
                    {
                       count++;
                        if(count >= 2)
                        {
                            if (grid.cellDic[new Vector3Int(x,y + 1)].TiieType == Define.TileTiles.E_Tile
                                 || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
                                return new Vector3Int(x,y +1);
                          
                        }
                        
                    }
                        
                }
            }
            count = 0;
        }

        if (grid.cellDic[new Vector3Int(minVec.x + 1, minVec.y + 1)].TiieType == Define.TileTiles.E_Tile
             || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
            return new Vector3Int(minVec.x + 1, minVec.y + 1);

        return Vector3Int.zero;
    }

    public void PlayerReTrans(out Vector3Int dir)
    {
        if (GameManager.Instance.Life - 1 <= 0)
        {
            SceneManager.LoadScene("DeathScene");
            dir = Vector3Int.zero;
            return;
        }
        GameManager.Instance.Life--;
        isKey = true;

        dir = Vector3Int.zero;
        Time.timeScale = 0;

        if (cells.Count > 0)
        {
            StartCoroutine(RewindCells());
        }

        
    }

    private IEnumerator RewindCells()
    {
        float rewindSpeed = 25f; // 되감기 속도 조절

        for (int i = cells.Count - 1; i >= 0; i--)
        {
            Vector3Int targetPos = cells[i]; // 셀의 월드 좌표 변환
     
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, rewindSpeed * Time.unscaledDeltaTime);
                yield return null;
            }

            if (grid.cellDic.TryGetValue(cells[i], out Cell cell))
            {
                cell.TiieType = Define.TileTiles.E_Tile;
                cell.obj.GetComponent<SpriteRenderer>().sprite = grid.e_Sprite;
            }
            transform.position = targetPos;
        }
        
        transform.position += newVecInt[curDir];


        cells.Clear(); // 셀 리스트 초기화
        closed = new bool[grid.xTile, grid.yTile];

        next = null;
        Vector3Int curr = grid.grid.WorldToCell(transform.position);
        current = grid.cellDic[curr];

        curDir = Vector3Int.zero;
        isMoving = false;
        isKey = false;


        Time.timeScale = 1;
    }


}
