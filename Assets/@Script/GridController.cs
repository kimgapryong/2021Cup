using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    public GameObject obj;

    private Define.TileTiles _tile;
    public Define.TileTiles TiieType
    {
        get
        {
            return _tile;
        }
        set
        {
            _tile = value;
        }
    }
}
public class GridController : MonoBehaviour
{
    public int xTile;
    public int yTile;
    public int wallCount;
    private int wallLength = 10;
    private int wallweight = 10;

    public Transform tileRoot;
    public Grid grid;

    public Sprite p_Sprite;
    public Sprite e_Sprite;
    public Sprite w_Sprite;

    public GameObject player;

    public Dictionary<Vector3Int, Cell> cellDic = new Dictionary<Vector3Int, Cell>(); 
    public void Init()
    {
        MakeTile();
        grid = gameObject.GetComponent<Grid>();

    }
    private void MakeTile()
    {
        for(int x = 0; x < xTile; x++)
        {
            for(int y = 0; y < yTile; y++)
            {
                Cell cell = GetCell(new Vector3Int(x, y));
                cell.obj = new GameObject("SpriteTile");
                cell.obj.AddComponent<SpriteRenderer>().sprite = e_Sprite;
                cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1f, 0.5f);
                cell.obj.transform.parent = tileRoot;
                cell.TiieType = Define.TileTiles.E_Tile;
                cell.x = x;
                cell.y = y;

                cell.obj.transform.position = new Vector3Int(x,y);
            }
        }

        for(int x = 1; x <= 3; x++)
        {
            for(int y = 1; y <= 3; y++)
            {
                if(cellDic.ContainsKey(new Vector3Int(x, y)))
                {
                    Cell cell = cellDic[new Vector3Int(x, y)];
                    cell.obj.GetComponent<SpriteRenderer>().sprite = p_Sprite;
                    cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    cell.TiieType = Define.TileTiles.P_Tile;
                }
            }
        }

        for(int i = 0; i < wallCount; i++)
        {
            int valueX = Random.Range(xTile / 2 - 10, xTile / 2 + 15);
            int valueY = Random.Range(xTile / 2 - 10, xTile / 2 + 15);

            Vector3Int[] dirs = new Vector3Int[4] {new Vector3Int(1,0), new Vector3Int(-1,0), new Vector3Int(0,1), new Vector3Int(0,-1) };
            int RandDir = Random.Range(0, dirs.Length);
            Vector3Int dir = dirs[RandDir];

            for (int x = 0; x < xTile; x++)
            {
                for (int y = 0; y < yTile; y++)
                {
                    if (x <= 0 || y <= 0 || x >= xTile - 1 || y >= yTile - 1)
                    {
                        if (cellDic.ContainsKey(new Vector3Int(x, y)))
                        {
                            Cell cell = cellDic[new Vector3Int(x, y)];
                            cell.obj.GetComponent<SpriteRenderer>().sprite = w_Sprite;
                            cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            cell.TiieType = Define.TileTiles.Wall;
                        }
                    }
                    if(x == valueX && y == valueY)
                    {
                        if (cellDic.ContainsKey(new Vector3Int(x, y)))
                        {
                            Cell cell = cellDic[new Vector3Int(x, y)];
                            cell.obj.GetComponent<SpriteRenderer>().sprite = w_Sprite;
                            cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            cell.TiieType = Define.TileTiles.Wall;

                            Cell next;
                            for (int z = 0; z < wallweight; z++)
                            {
                                if(cellDic.ContainsKey(new Vector3Int(cell.x, cell.y) + dir))
                                {
                                    next = cellDic[new Vector3Int(cell.x, cell.y) + dir];

                                    next.obj.GetComponent<SpriteRenderer>().sprite = w_Sprite;
                                    cell.obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                                    next.TiieType = Define.TileTiles.Wall;

                                    cell = next;
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        
    }

    private Cell GetCell(Vector3Int vec)
    {
        Cell cell = new Cell();
        cellDic.Add(vec, cell);
        return cell;
    }

}
