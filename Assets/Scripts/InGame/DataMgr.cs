using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : Singleton<DataMgr>
{
    [Header("Control Value")] 
    public int monsterNumber;
    public float setTime;
    public float successValue;
    public float failValue;
    public float moveSpeedTime;
    public float comboGraceTime; //유예시간
    public int addScoreValue;

    public float failedTime;

    public float morningSpeed;
    public float afternoonSpeed;
    public float nightSpeed;

    public Sprite[] monsterImgs;
    
}
