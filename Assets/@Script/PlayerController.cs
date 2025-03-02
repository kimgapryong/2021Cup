using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CretureController
{
    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

    public int playerHp = 5;
    public Vector3Int currentMax;
    public Vector3Int currentMin;
    public float rewindSpeed;

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
        if (grid.cellDic[new Vector3Int(next.x, next.y) + dir].TiieType != Define.TileTiles.P_Tile)
            return;

        List<Vector3Int> nextVec = GetCenterVec();

        if(nextVec.Count >= 1 && cells.Count > 2)
            FoolBfs(nextVec);


        foreach (Vector3Int vec in cells)
        {
            if (grid.cellDic.ContainsKey(vec))
                grid.cellDic[vec].obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        cells.Clear();
        visited.Clear();
        closed = new bool[grid.xTile, grid.yTile];
        canWrite = false;
    }

    private void FoolBfs(List<Vector3Int> vec)
    {
        Vector3Int[] dirs = new Vector3Int[8]
        {
        new Vector3Int(1, 0), new Vector3Int(-1, 0),
        new Vector3Int(0, 1), new Vector3Int(0, -1),
        new Vector3Int(1, 1), new Vector3Int(-1, 1),
        new Vector3Int(1, -1), new Vector3Int(-1, -1)
        };

        Queue<Vector3Int> queue = new Queue<Vector3Int>(vec);

        while (queue.Count > 0)
        {
            Vector3Int cur = queue.Dequeue();
            Debug.Log(cur);
            if (visited.Contains(cur))
                continue; 

            visited.Add(cur);

            if (!grid.cellDic.TryGetValue(cur, out Cell cell))
                continue;

            if(cell.TiieType == Define.TileTiles.P_Tile)
                continue;
            if(cell.TiieType == Define.TileTiles.Ee_Tile)
            {
                cell.TiieType = Define.TileTiles.P_Tile;
                cell.obj.GetComponent<SpriteRenderer>().sprite = grid.p_Sprite;
                cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                continue;
            }

            if(cell.x <= 0 || cell.y <= 0 || cell.x > grid.xTile - 2 ||  cell.y > grid.yTile - 2)
                continue;

            if ((cell.x >= currentMax.x || cell.y >= currentMax.y ||
                 cell.x <= currentMin.x || cell.y <= currentMin.y) && canWrite)
                continue;

            if (cell.TiieType == Define.TileTiles.E_Tile)
            {
                cell.TiieType = Define.TileTiles.P_Tile;
                cell.obj.GetComponent<SpriteRenderer>().sprite = grid.p_Sprite;
                cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }


            if (cell.monster != null)
            {
                Destroy(cell.monster);
                cell.monster = null;
            }


            foreach (Vector3Int dir in dirs)
            {
                Vector3Int nextPos = cur + dir;
                if (grid.cellDic.TryGetValue(nextPos, out Cell nextCell))
                {
                    if (!visited.Contains(nextPos) && nextCell.TiieType != Define.TileTiles.P_Tile)
                        queue.Enqueue(nextPos);
                }
            }
        }
    }

    private List<Vector3Int> GetCenterVec()
    {
        List<Vector3Int> zero = new List<Vector3Int>();

        int maxX = cells[0].x, maxY = cells[0].y;
        int minX = cells[0].x, minY = cells[0].y;

        for (int i = 1; i < cells.Count; i++)
        {
            maxX = Mathf.Max(maxX, cells[i].x);
            maxY = Mathf.Max(maxY, cells[i].y);
            minX = Mathf.Min(minX, cells[i].x);
            minY = Mathf.Min(minY, cells[i].y);
        }

        if (maxX == minX || maxY == minY)
        {
            zero.Add(Vector3Int.zero);
            return zero;
        }
        currentMax = new Vector3Int(maxX, maxY);
        currentMin = new Vector3Int(minX, minY);

    return Search(new Vector3Int(minX, minY), new Vector3Int(maxX, maxY));
}

    private List<Vector3Int> Search(Vector3Int minVec, Vector3Int maxVec)
    {
        //int count = 0;
        List<Vector3Int> current = new List<Vector3Int>();


        for(int i =0; i < cells.Count; i++)
        {
            int maxY = maxVec.y;
          
            if (cells[i].y < maxY)
                continue;
            
            Vector3Int curCell = cells[i];


            if (!CanColor(curCell, Vector3Int.down))
                continue;

            if (grid.cellDic[curCell + Vector3Int.down].TiieType != Define.TileTiles.P_Tile)
                current.Add(curCell + Vector3Int.down);
        }
        for (int i = 0; i < cells.Count; i++)
        {
            int minY = minVec.y;

            if (cells[i].y > minY)
                continue;

            Vector3Int curCell = cells[i];

            if (!CanColor(curCell, Vector3Int.up))
                continue;

            if (grid.cellDic[curCell + Vector3Int.up].TiieType != Define.TileTiles.P_Tile)
                current.Add(curCell + Vector3Int.up);
        }
        for (int i = 0; i < cells.Count; i++)
        {
            int maxX = maxVec.x;

            if (cells[i].x < maxX)
                continue;

            Vector3Int curCell = cells[i];

            if (!CanColor(curCell, Vector3Int.left))
                continue;

            if (grid.cellDic[curCell + Vector3Int.left].TiieType != Define.TileTiles.P_Tile)
                current.Add(curCell + Vector3Int.left);
        }
        for (int i = 0; i < cells.Count; i++)
        {
            int minX = minVec.x;

            if (cells[i].x > minX)
                continue;

            Vector3Int curCell = cells[i];

            if (!CanColor(curCell, Vector3Int.right))
                continue;

            if (grid.cellDic[curCell + Vector3Int.right].TiieType != Define.TileTiles.P_Tile)
                current.Add(curCell + Vector3Int.right);
        }

        if(current.Count <= 0)
            current.Add(Vector3Int.zero);

        return current;
    }
    private bool CanColor(Vector3Int cur, Vector3Int dir)
    {
        int cun = 0;

        if (dir == Vector3Int.up)
            cun = currentMax.y;
        else if (dir == Vector3Int.down)
            cun = currentMax.y;
        else if (dir == Vector3Int.left)
            cun = currentMax.x;
        else if (dir == Vector3Int.right)
            cun = currentMax.x;

        Vector3Int current = cur;
        for (int i =0; i < cun ; i++)
        {
            current += dir;
            if(grid.cellDic.ContainsKey(current))
                if (grid.cellDic[current].TiieType == Define.TileTiles.P_Tile)
                    return true;
        }

        return false;
    }
    public void PlayerReTrans(out Vector3Int dir)
    {
        if (GameManager.Instance.Life - 1 <= 0)
        {
            StopAllCoroutines(); 
            Time.timeScale = 1;  
            SceneManager.LoadScene("DeathScene");
            dir = Vector3Int.zero;
            return;
        }

        GameManager.Instance.Life--;
        isKey = true;

        dir = Vector3Int.zero;
      

        if (cells.Count > 0)
        {
            StartCoroutine(RewindCells());
        }

        Time.timeScale = 0;
    }

    private IEnumerator RewindCells()
    {

        for (int i = cells.Count - 1; i >= 0; i--)
        {
            Vector3Int targetPos = cells[i]; // ºø¿« ø˘µÂ ¡¬«• ∫Ø»Ø
     
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


        cells.Clear(); // ºø ∏ÆΩ∫∆Æ √ ±‚»≠
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
