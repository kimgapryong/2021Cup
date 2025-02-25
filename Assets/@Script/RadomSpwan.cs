using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadomSpwan : MonoBehaviour
{
    public GridController gridController;
    public GameObject monParticel;

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
            Vector3 pos = new Vector3(Random.Range(4, 48), Random.Range(4, 48));
            GameObject pa = Instantiate(monParticel);
            pa.transform.position = pos;
            ParticleSystem ps = pa.GetComponent<ParticleSystem>();
            float time = ps.main.duration;
            yield return new WaitForSeconds(time);
            Destroy(pa);

            int randMon = Random.Range(0, monster.Length);
            currentCount++;
            GameObject sM = Instantiate(monster[randMon]);
            sM.transform.position = pos;
        }
    }

}
