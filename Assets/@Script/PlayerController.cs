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

    private Vector3Int currentCell;  // 현재 셀 좌표
    private Vector3 targetPos;       // 목표 이동 위치
    private Vector3Int dir = Vector3Int.zero;  // 이동 방향 (초기값 없음)
    private bool isMoving = false;   // 이동 중 여부

    public Vector3Int firstCell;
    public Vector3Int lastCell;

    //그리드 리스트
    public List<Vector3Int> tiles = new List<Vector3Int>();

    void Start()
    {
        currentCell = grid.WorldToCell(transform.position);
        targetPos = grid.CellToWorld(currentCell);
    }

    void Update()
    {
        // 방향 설정 (키 한 번만 눌러도 dir 값 유지)
        if (Input.GetKeyDown(KeyCode.W)) dir = Vector3Int.up;
        else if (Input.GetKeyDown(KeyCode.A)) dir = Vector3Int.left;
        else if (Input.GetKeyDown(KeyCode.S)) dir = Vector3Int.down;
        else if (Input.GetKeyDown(KeyCode.D)) dir = Vector3Int.right;

        PlayerMove();
    }

    void PlayerMove()
    {
        if (!isMoving && dir != Vector3Int.zero) // 이동 중이 아닐 때만 새로운 목표 설정
        {
            Vector3Int nextCell = currentCell + dir;

            // 벽 충돌 체크
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

            // 목표 위치에 도달하면 새로운 셀로 이동
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;  // 정확한 위치 보정
                //타일 색칠          
                m_Tilemap.SetTile(currentCell, null);
                //colorGrid 실행
                isMoving = false;  // 다음 이동 가능하게 설정
            }
        }
    }

  
}
