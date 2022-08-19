using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScrpit : MonoBehaviour
{
    public int monsterType = -1; // 0 == led(left) , 1 == right(blue)
    

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
            else
            {
                //set blue image
               gameObject.GetComponent<SpriteRenderer>().sprite = DataMgr.Instance.monsterImgs[1];
            }
        }
    }
}
