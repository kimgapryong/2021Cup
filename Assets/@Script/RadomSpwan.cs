using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadomSpwan : MonoBehaviour
{
    public GridController gridController;
    public GameObject monParticel;
    public GameObject boss;

    public GameObject player;
    public GameObject[] monster;

    public float spwanTime = 5;
    private int currentCount = 0;
    public int monMaxCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        gridController.Init();

        GameObject pla = Instantiate(player);
        pla.transform.Find("Player").transform.position = new Vector2(2, 2);

        StartCoroutine(RandomSpwan());
    }

    IEnumerator RandomSpwan()
    {
        while (currentCount <= monMaxCount)
        {
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
}
