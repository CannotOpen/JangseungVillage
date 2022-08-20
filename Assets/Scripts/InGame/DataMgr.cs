using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : Singleton<DataMgr>
{
    [Header("Control Value")] 
    public int monsterNumber;
    public float setTime;
    public float successValue;
    public float goldSucValue;
    public float failValue;
    public float moveSpeedTime;
    public float comboGraceTime; //유예시간
    public int addScoreValue;
    //public int addScoreValueGold;
    public float purpleDelayGrace;

    public float failedTime;

    public float morningSpeed;
    public float afternoonSpeed;
    public float nightSpeed;

    public float redMonPer;
    public float blueMonPer;
    public float purpleMonPer;
    public float goldMonPer;

    public int goldInputNum;


    [Header("Bonus")] 
    public int bonusStandNum;
    public float bonusTime;
    public int bonusScoreValue;

    [Header("Air")] 
    public int airStandNum;
    public float airCamMoveTime;
    public float airTime;
    public float airSwipeStand;
    public float airBulletSpeedTime;
    public float airMonSpeedTime;
    
    [Header("ImageRes")]
    public Sprite[] monsterImgs;

    public Sprite[] airMonsterImgs;
}
