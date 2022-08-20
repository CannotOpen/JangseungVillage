using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScrpit : MonoBehaviour
{
    public int monsterType = -1; // 0 == led(left) , 1 == right(blue, 2 == purple, 3 == gold
    

    public void SetMonsterType(int type)
    {
        if (monsterType != type)
        {
            monsterType = type;

            if (monsterType == 0)
            {
                //set led image
                gameObject.GetComponent<SpriteRenderer>().sprite = DataMgr.Instance.monsterImgs[0];
            }
            else if(monsterType == 1)
            {
                //set blue image
               gameObject.GetComponent<SpriteRenderer>().sprite = DataMgr.Instance.monsterImgs[1];
            }
            else if (monsterType == 2)
            {
                //set purple image
                gameObject.GetComponent<SpriteRenderer>().sprite = DataMgr.Instance.monsterImgs[2];
            }
            else if (monsterType == 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = DataMgr.Instance.monsterImgs[3];
            }
        }
    }
}
