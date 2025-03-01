using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadomSpwan : MonoBehaviour
{
    public Define.Stage Stage;
    public GridController gridController;
    public GameObject monParticel;
    public GameObject boss;
    public GameObject bossPrefab;
    public CheatAngine cheat;

    public GameObject player;
    public GameObject[] monster;
    public GameObject[] items;

    public float spwanTime = 5f;
    private int currentCount = 0;
    public int monMaxCount = 10;


    public float ItemTime = 0.1f;
    public int currentItem = 0;
    public int itemMaxCount = 14;

    public float PlayerSpeed;
 
    // Start is called before the first frame update

    #region 아이템 개수
    public int lifeCount = 2;
    
    public int defenceCount = 3;
    
    public int godCount = 1;
    
    public int randomCount = 5;
    
    public int speedCount = 3;
    
    #endregion
    void Start()
    {
        Debug.Log("init new");
        gridController.Init();

        GameObject pla = Instantiate(player);
        pla.transform.Find("Player").transform.position = new Vector2(2, 2);
        pla.transform.Find("Player").GetComponent<PlayerController>().rewindSpeed = PlayerSpeed;

        cheat.Init(pla.transform.Find("Player").GetComponent<PlayerController>());

        Stage = Define.Stage.Normal;
        if(bossPrefab != null)
        {
            GameObject bossMonster = Instantiate(bossPrefab);
            bossMonster.transform.position = new Vector3Int(24, 24);
            pla.transform.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 16;
            Stage = Define.Stage.Boss;
        }

        StartCoroutine(RandomSpwan());
        StartCoroutine(ItemSpwan());
    }

    IEnumerator RandomSpwan()
    {
        while (currentCount <= monMaxCount)
        {
            if (monster.Length <= 0)
                yield break;

            yield return new WaitForSeconds(spwanTime);

            Vector3Int pos = GetVec();

            GameObject bossmon = Instantiate(boss);
            bossmon.transform.position = pos;

            GameObject pa = Instantiate(monParticel);
            pa.transform.position = pos;

            ParticleSystem ps = pa.GetComponent<ParticleSystem>();
            float time = ps.main.duration;
            yield return new WaitForSeconds(time);
            Destroy(pa);
            Destroy(bossmon);

            int randMon = Random.Range(1, 101);
            int rand;

            if (randMon <= 50)
                rand = 0;
            else if(randMon <= 80)
                rand = 2;
            else
                rand = 1;

            currentCount++;
            GameObject sM = Instantiate(monster[rand]);
            sM.transform.position = pos;
        }
    } // 몬스터 랜덤 생성

    IEnumerator ItemSpwan()
    {
        while(currentItem <= itemMaxCount)
        {
            yield return new WaitForSeconds(ItemTime);

            Vector3Int pos = GetItem();
               
            int rand = Random.Range(0, items.Length);

            switch (rand)
            {
                case 0:
                    if(lifeCount > 0)
                    {
                        lifeCount--;
                        GameObject clone = Instantiate(items[rand]);
                        clone.transform.position = pos;
                        gridController.cellDic[pos].Item = clone;
                        currentItem++;
                    }
                    break;
                case 1:
                    if (defenceCount > 0)
                    {
                        defenceCount--;
                        GameObject clone = Instantiate(items[rand]);
                        clone.transform.position = pos;
                        gridController.cellDic[pos].Item = clone;
                        currentItem++;
                    }
                    break;
                case 2:
                    if (godCount > 0)
                    {
                        godCount--;
                        GameObject clone = Instantiate(items[rand]);
                        clone.transform.position = pos;
                        gridController.cellDic[pos].Item = clone;
                        currentItem++;
                    }
                    break;
                case 3:
                    if (randomCount > 0)
                    {
                        randomCount--;
                        GameObject clone = Instantiate(items[rand]);
                        clone.transform.position = pos;
                        gridController.cellDic[pos].Item = clone;
                        currentItem++;
                    }
                    break;
                case 4:
                    if (speedCount > 0)
                    {
                        speedCount--;
                        GameObject clone = Instantiate(items[rand]);
                        clone.transform.position = pos;
                        gridController.cellDic[pos].Item = clone;
                        currentItem++;
                    }
                    break;
            }
            
        }
    }

    
    private Vector3Int GetVec()
    {
        Vector3Int pos = new Vector3Int(Random.Range(4, 48), Random.Range(4, 48));
        
        if(gridController.cellDic.TryGetValue(pos, out Cell cell))
        {
            if (cell.TiieType == Define.TileTiles.Wall)
                return GetVec();
            else
                return pos;
        }

        return new Vector3Int(5,5);
    }

    private Vector3Int GetItem()
    {
        Vector3Int pos = new Vector3Int(Random.Range(4, 48), Random.Range(4, 48));

        if (gridController.cellDic.TryGetValue(pos, out Cell cell))
        {
            if (cell.TiieType == Define.TileTiles.Wall || cell.Item != null)
                return GetItem();
            else
                return pos;
        }

        return new Vector3Int(5, 5);
    }
}
