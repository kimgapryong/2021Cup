using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMonster : MonsterController
{
    public void BulletInit(Vector3Int direction)
    {
        dir = direction;
        speed = 40;
        StartCoroutine(ReversakTechnique());
        Destroy(gameObject, 6);
    }

    protected override void Update()
    {
        base.Update();
    }

    IEnumerator ReversakTechnique()
    {
        yield return new WaitForSeconds(3);
        dir = -dir;
    }
}
