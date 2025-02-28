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

    public bool canWrite;

    private bool _god;
    public bool godTime
    {
        get { return _god; }
        set
        {
            Debug.Log(value);
            if (value)
                StartCoroutine(GodTime());
        }
    }

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
        QuestItem();
    }

  

    public override void ColorGird()
    {

       Vector3Int nextVec = GetCenterVec();
        Debug.Log(nextVec.x + "," + nextVec.y);

        if(nextVec != Vector3Int.zero && cells.Count > 2)
            FoolBfs(nextVec);


        foreach (Vector3Int vec in cells)
        {
            if (grid.cellDic.ContainsKey(vec))
                grid.cellDic[vec].obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        cells.Clear();
        closed = new bool[grid.xTile, grid.yTile];
        canWrite = false;
    }

    private void FoolBfs(Vector3Int vec)
    {
     
        if (visited.Contains(vec)) return;  // 이미 방문한 셀이면 리턴
        visited.Add(vec); // 방문한 셀 저장

        if (!grid.cellDic.TryGetValue(vec, out Cell cell)) return;

        if ((cell.x >= currentMax.x || cell.y >= currentMax.y || cell.x <= currentMin.x || cell.y <= currentMin.y) && canWrite)
            return;
            

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
            Debug.Log(nextPos);
            if (grid.cellDic.TryGetValue(nextPos, out Cell nextCell))
            {
                if (nextCell.TiieType != Define.TileTiles.P_Tile)
                    FoolBfs(nextPos);
            }
          
        }
    }
    private Vector3Int GetCenterVec()
{
    if (cells.Count == 0)
        return Vector3Int.zero;

    int maxX = cells[0].x, maxY = cells[0].y;
    int minX = cells[0].x, minY = cells[0].y;

    for (int i = 1; i < cells.Count; i++)
    {
        maxX = Mathf.Max(maxX, cells[i].x);
        maxY = Mathf.Max(maxY, cells[i].y);
        minX = Mathf.Min(minX, cells[i].x);
        minY = Mathf.Min(minY, cells[i].y);
    }

    if (minX == maxX || minY == maxY)
        return Vector3Int.zero;

    currentMax = new Vector3Int(maxX, maxY);
    currentMin = new Vector3Int(minX, minY);

    return Search(new Vector3Int(minX, minY), new Vector3Int(maxX, maxY));
}

    private Vector3Int Search(Vector3Int minVec, Vector3Int maxVec)
    {
        int count = 0;
        Vector3Int current;

   /*     if (grid.cellDic[new Vector3Int(maxVec.x - 1, maxVec.y - 1)].TiieType == Define.TileTiles.E_Tile ||
            grid.cellDic[new Vector3Int(maxVec.x - 1, maxVec.y - 1)].TiieType == Define.TileTiles.Wall)
        {
            current = new Vector3Int(maxVec.x - 1, maxVec.y - 1);
            if (closed[current.x + Vector3Int.up.x, current.y + Vector3Int.up.y] &&
             closed[current.x + Vector3Int.right.x, current.y + Vector3Int.right.y])
                return current;
        }

        if (grid.cellDic[new Vector3Int(minVec.x + 1, maxVec.y - 1)].TiieType == Define.TileTiles.E_Tile ||
            grid.cellDic[new Vector3Int(minVec.x + 1, maxVec.y - 1)].TiieType == Define.TileTiles.Wall)
        {
            current = new Vector3Int(minVec.x + 1, maxVec.y - 1);
            if (grid.cellDic[current].TiieType == Define.TileTiles.E_Tile
                 || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
                return current;
        }

        if (grid.cellDic[new Vector3Int(minVec.x + 1, minVec.y + 1)].TiieType == Define.TileTiles.E_Tile ||
            grid.cellDic[new Vector3Int(minVec.x + 1, minVec.y + 1)].TiieType == Define.TileTiles.Wall)
        {
            current = new Vector3Int(minVec.x + 1, minVec.y + 1);
            if (grid.cellDic[current].TiieType == Define.TileTiles.E_Tile
                 || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
                return current;
        }

        if (grid.cellDic[new Vector3Int(maxVec.x - 1, minVec.y + 1)].TiieType == Define.TileTiles.E_Tile ||
            grid.cellDic[new Vector3Int(maxVec.x - 1, minVec.y + 1)].TiieType == Define.TileTiles.Wall)
        {
            current = new Vector3Int(maxVec.x - 1, minVec.y + 1);
            if (grid.cellDic[current].TiieType == Define.TileTiles.E_Tile
                 || grid.cellDic[current].TiieType == Define.TileTiles.Wall)
                return current;
        }*/

        for(int i =0; i < cells.Count; i++)
        {
            int maxY = maxVec.y;
          
            if (cells[i].y < maxY)
                continue;
            Vector3Int curCell = cells[i];

            if (grid.cellDic[curCell + Vector3Int.down].TiieType != Define.TileTiles.P_Tile)
                return curCell + Vector3Int.down;
        }
        for (int i = 0; i < cells.Count; i++)
        {
            int minY = minVec.y;

            if (cells[i].y > minY)
                continue;

            Vector3Int curCell = cells[i];
            if (grid.cellDic[curCell + Vector3Int.up].TiieType != Define.TileTiles.P_Tile)
                return curCell + Vector3Int.up;
        }
        for (int i = 0; i < cells.Count; i++)
        {
            int maxX = maxVec.x;

            if (cells[i].x < maxX)
                continue;
            Vector3Int curCell = cells[i];
            if (grid.cellDic[curCell + Vector3Int.left].TiieType != Define.TileTiles.P_Tile)
                return curCell + Vector3Int.left;
        }
        for (int i = 0; i < cells.Count; i++)
        {
            int minX = minVec.x;

            if (cells[i].x > minX)
                continue;
            Vector3Int curCell = cells[i];
            if (grid.cellDic[curCell + Vector3Int.right].TiieType != Define.TileTiles.P_Tile)
                return curCell + Vector3Int.right;
        }


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

    private void QuestItem()
    {
        if (next == null)
            return;

        if(next.Item != null)
            next.Item.GetComponent<ItemBase>().PlayerSetAbility();
    }

    private IEnumerator GodTime()
    {
        int cur = GameManager.Instance.Life;
        GameManager.Instance.Life = 99;


        yield return new WaitForSeconds(12f);

        godTime = false;
        GameManager.Instance.Life = cur;
    }
}
