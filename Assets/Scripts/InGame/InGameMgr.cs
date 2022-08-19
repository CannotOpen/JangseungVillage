using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.SocialPlatforms.Impl;

public class InGameMgr : MonoBehaviour
{
    [Header("InGameState")] [HideInInspector]
    public int dayState; // 0 == morning, 1 == afternoon, 2 == night;
    
    [Header("Value")]
     public Queue<int> monsterList = new Queue<int>();
    [HideInInspector] public int nowMonster = -1; // 0 == left, 1 == right
    [HideInInspector] public bool isGamePlay = false;
    [HideInInspector] public float timeValue;
    public GameObject timeGauge;
    [HideInInspector] public Image timeGaugeImg;
    public GameObject[] monsterObjects;
    //public MonsterScrpit[] monsterControlls;
    public Queue<GameObject> monsterObjectListQ = new Queue<GameObject>();
    [HideInInspector] public float speedValue;
    
    [Header("Location")] 
    public GameObject leftLocation;
    public GameObject rightLocation;
    public GameObject resetLocation;
    [HideInInspector] public GameObject lastMonster;
    [HideInInspector]public Vector3 lastLocation;
    public float moveValue;

    [Header("InterUI")] 
    public Button leftButton;
    public Button rightButton;
    public GameObject leftFailedImg;
    public GameObject rightFailedImg;

    [Header("Score")] 
    public Text scoreText;
    public Text comboText;
    public int combo;
    public int score;
    [HideInInspector] public float comboTimeCount = 0;
    
    

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGamePlay)
        {
            timeValue -= Time.deltaTime * speedValue;
            timeGaugeImg.fillAmount = timeValue / DataMgr.Instance.setTime;

            comboTimeCount += Time.deltaTime;
            if (timeValue <= 0)
            {
                //GameOver
                Debug.Log("game over");
                isGamePlay = false;
            }

            if (comboTimeCount > DataMgr.Instance.comboGraceTime)
            {
                
                combo = 0;
                TextUpdate();
            }
        }
    }

    void SetUp()
    {
        for (int i = 0; i < DataMgr.Instance.monsterNumber; i++)
        {
            monsterObjects[i].SetActive(true);
            
            AddNewMonster(monsterObjects[i]);

            /*
            if (i == (DataMgr.Instance.monsterNumber - 1))
            {
                lastLocation = monsterObjects[i].transform.position;
            }
            */
        }
        
        AddNewMonster(monsterObjects[DataMgr.Instance.monsterNumber]);
        lastLocation = monsterObjects[DataMgr.Instance.monsterNumber].transform.position;

        nowMonster = monsterList.Dequeue();

        timeValue = DataMgr.Instance.setTime;
        timeGaugeImg = timeGauge.GetComponent<Image>();
        timeGaugeImg.fillAmount = 1;
        
        leftButton.onClick.AddListener(()=> ButtonClickAction(0));
        rightButton.onClick.AddListener(()=>ButtonClickAction(1));

        combo = 0;
        score = 0;
        
        SetDayState();

        isGamePlay = true;
    }

    void SetDayState()
    {
        switch (dayState)
        {
            case 0:
                speedValue = DataMgr.Instance.morningSpeed;
                break;
            case 1:
                speedValue = DataMgr.Instance.afternoonSpeed;
                break;
            case 2:
                speedValue = DataMgr.Instance.nightSpeed;
                break;
            default:
                break;
        }
    }

    void TextUpdate()
    {
        comboText.text = "콤보 +" + combo.ToString();
        scoreText.text = "점수 : " + score.ToString();
    }
    
    void AddNewMonster(GameObject monster)
    {
        int newMonster = Random.Range(0, 2);

        //Add Monster Object;
        
        monster.GetComponent<MonsterScrpit>().SetMonsterType(newMonster);
        
        monsterList.Enqueue(newMonster);
        monsterObjectListQ.Enqueue(monster);

        lastMonster = monster;
    }
    
    void SuccessAction()
    {
        combo += 1;
        comboTimeCount = 0;
        score += DataMgr.Instance.addScoreValue;
        GameObject nowMon = monsterObjectListQ.Dequeue();
        
        //nowMon Move;
        if (nowMonster == 0)
        {
            //nowMon move to leftLoc;
            nowMon.transform.DOMove(leftLocation.transform.position, DataMgr.Instance.moveSpeedTime).OnComplete(() => DoCom(nowMon));
        }
        else
        {
            //nowMon move to rightLoc;
            nowMon.transform.DOMove(rightLocation.transform.position, DataMgr.Instance.moveSpeedTime)
                .OnComplete(() => DoCom(nowMon));

        }
        
        //onComplete -> SetActive(false)

        
        //Another monster Move to front;
        foreach (GameObject obj in monsterObjectListQ)
        {
            //obj move to obj.y - ( moveVec ); 
             obj.transform.DOMoveY( obj.transform.position.y - moveValue , DataMgr.Instance.moveSpeedTime);
             obj.GetComponent<SpriteRenderer>().sortingOrder = ((int)obj.transform.position.y - 10) * (-1);
        }
        
        lastMonster.SetActive(true);
        lastMonster.GetComponent<SpriteRenderer>().sortingOrder = ((int)lastMonster.transform.position.y - 10) * (-1);
        lastMonster.transform.DOMoveY( lastMonster.transform.position.y - moveValue , DataMgr.Instance.moveSpeedTime);
        

        //nowMon move to last;
        //nowMon.transform.DOMove(lastLocation, DataMgr.Instance.moveSpeedTime);
        //nowMon.transform.DOMove(new Vector3(lastMoster.transform.position.x, lastMoster.transform.position.y + moveValue, lastMoster.transform.position.z), DataMgr.Instance.moveSpeedTime);
        
        
        nowMonster = monsterList.Dequeue();
        
    }

    void DoCom(GameObject mon)
    {
        //Debug.Log("Complete");
        mon.SetActive(false);
        mon.transform.position = lastLocation;
        AddNewMonster(mon);
    }

    void ButtonClickAction(int buttonType)
    {
        if (nowMonster == buttonType) //success
        {
            timeValue += DataMgr.Instance.successValue;
            
            SuccessAction();
        }
        else //fail
        {
            timeValue -= DataMgr.Instance.failValue;
            combo = 0;
            StartCoroutine(FailedClick(buttonType));
        }
        
        TextUpdate();
    }

    IEnumerator FailedClick(int buttonType)
    {
        if (buttonType == 0)
        {
            leftFailedImg.SetActive(true);
            yield return new WaitForSeconds(DataMgr.Instance.failedTime);
            leftFailedImg.SetActive(false);
        }
        else
        {
            rightFailedImg.SetActive(true);
            yield return new WaitForSeconds(DataMgr.Instance.failedTime);
            rightFailedImg.SetActive(false);
        }

        
        

    }
}
