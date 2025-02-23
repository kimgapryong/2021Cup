using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : CretureController
{
    [SerializeField] private float speed = 5f;
    [SerializeField] public Grid grid;
    [SerializeField] public Tilemap p_Tilemap;
    [SerializeField] public Tilemap m_Tilemap;
    [SerializeField] public Tilemap w_Tilemap;
    [SerializeField] private ColorGrid colorGrid;

    private Vector3Int currentCell;  // ���� �� ��ǥ
    private Vector3 targetPos;       // ��ǥ �̵� ��ġ
    private Vector3Int dir = Vector3Int.zero;  // �̵� ���� (�ʱⰪ ����)
    private bool isMoving = false;   // �̵� �� ����

    public Vector3Int firstCell;
    public Vector3Int lastCell;

    //�׸��� ����Ʈ
    public List<Vector3Int> tiles = new List<Vector3Int>();

    void Start()
    {
        currentCell = grid.WorldToCell(transform.position);
        targetPos = grid.CellToWorld(currentCell);
    }

    void Update()
    {
        // ���� ���� (Ű �� ���� ������ dir �� ����)
        if (Input.GetKeyDown(KeyCode.W)) dir = Vector3Int.up;
        else if (Input.GetKeyDown(KeyCode.A)) dir = Vector3Int.left;
        else if (Input.GetKeyDown(KeyCode.S)) dir = Vector3Int.down;
        else if (Input.GetKeyDown(KeyCode.D)) dir = Vector3Int.right;

        PlayerMove();
    }

    void PlayerMove()
    {
        if (!isMoving && dir != Vector3Int.zero) // �̵� ���� �ƴ� ���� ���ο� ��ǥ ����
        {
            Vector3Int nextCell = currentCell + dir;

            // �� �浹 üũ
            if (w_Tilemap.GetTile(nextCell) == null)
            {
                if(m_Tilemap.GetTile(nextCell) != null)
                    firstCell = nextCell;
                if(p_Tilemap.GetTile(nextCell) && firstCell != Vector3Int.zero)
                    lastCell = currentCell;


                targetPos = grid.CellToWorld(nextCell);
                currentCell = nextCell;
                isMoving = true;
            }
        }

        if (isMoving)
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ϸ� ���ο� ���� �̵�
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;  // ��Ȯ�� ��ġ ����
                //Ÿ�� ��ĥ          
                m_Tilemap.SetTile(currentCell, null);
                //colorGrid ����
                isMoving = false;  // ���� �̵� �����ϰ� ����
            }
        }
    }

  
}
