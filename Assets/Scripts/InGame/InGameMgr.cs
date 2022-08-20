using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.SocialPlatforms.Impl;

public class InGameMgr : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("InGameState")] [HideInInspector]
    public int dayState; // 0 == morning, 1 == afternoon, 2 == night;

    [HideInInspector] public bool isSuc;
    
    [Header("Value")]
     public Queue<int> monsterList = new Queue<int>();
    [HideInInspector] public int nowMonster = -1; // 0 == red, 1 == blue, 2 == purple, 3 == gold
    [HideInInspector] public bool isGamePlay = false;
    [HideInInspector] public float timeValue;
    public GameObject timeGauge;
    [HideInInspector] public Image timeGaugeImg;
    public GameObject[] monsterObjects;
    //public MonsterScrpit[] monsterControlls;
    public Queue<GameObject> monsterObjectListQ = new Queue<GameObject>();
    [HideInInspector] public float speedValue;
    [HideInInspector] public int goldCount;
    [HideInInspector] public float purpleTimeCount;
    
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
    [HideInInspector] public bool isBtnDown;

    [Header("Score")] 
    public Text scoreText;
    public Text comboText;
    public Text goldCountText;
    public int combo;
    [HideInInspector] public int score;
    [HideInInspector] public float comboTimeCount = 0;

    [Header("Bonus")] 
    [HideInInspector] public bool isBonus;
    public int bonusCombo;
    [HideInInspector] public float bonusTimeCount;
    [HideInInspector] public int bonusGaugeIdx;
    public GameObject bonusBackground;
    public GameObject bonusUI;
    public Text bonusComboText;
    public GameObject[] bonusGauge;



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
            if (!isBonus)
            {
                //Test for PC
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    isBtnDown = true;
                    purpleTimeCount = 0;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isBtnDown = true;
                    purpleTimeCount = 0;
                }

                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    isBtnDown = false;
                }

                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    isBtnDown = false;
                }

                //~Test for PC

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

                if (isBtnDown)
                {
                    purpleTimeCount += Time.deltaTime;
                }
            }
            else //bonus
            {
                bonusTimeCount += Time.deltaTime;

                if (bonusTimeCount >= DataMgr.Instance.bonusTime / (float)5)
                {
                    bonusGauge[bonusGaugeIdx].SetActive(false);
                    bonusTimeCount = 0;
                    bonusGaugeIdx += 1;

                    if (bonusGaugeIdx == 5)
                    {
                        UnsetBonus();
                    }
                }
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
        goldCount = 0;
        purpleTimeCount = 0;
        bonusCombo = 0;
        bonusGaugeIdx = 0;

        isBtnDown = false;
        
        SetDayState();

        isGamePlay = true;
        isSuc = false;
        TextUpdate();
        StartCoroutine(SuccessAction());
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
        if (comboText.gameObject.activeSelf)
        {
            comboText.text = "콤보 +" + combo.ToString();
        }

        if (scoreText.gameObject.activeSelf)
        {
            scoreText.text = "점수 : " + score.ToString();
        }

        if (bonusComboText.gameObject.activeSelf)
        {
            bonusComboText.text = bonusCombo.ToString();
        }

        if (goldCountText.gameObject.activeSelf)
        {
            goldCountText.text = (DataMgr.Instance.goldInputNum - goldCount).ToString();
        }
        
    }
    
    void AddNewMonster(GameObject monster)
    {
        int rand = Random.Range(1, 101);
        int newMonster;

        if (rand <= DataMgr.Instance.goldMonPer)
        {
            newMonster = 3;
        }
        else if (rand <= DataMgr.Instance.purpleMonPer + DataMgr.Instance.goldMonPer)
        {
            newMonster = 2;
        }
        else if (rand <= DataMgr.Instance.purpleMonPer + DataMgr.Instance.goldMonPer + DataMgr.Instance.blueMonPer)
        {
            newMonster = 1;
        }
        else
        {
            newMonster = 0;
        }
        

        //Add Monster Object;
        
        
        monster.GetComponent<MonsterScrpit>().SetMonsterType(newMonster);
        
        monsterList.Enqueue(newMonster);
        monsterObjectListQ.Enqueue(monster);

        lastMonster = monster;
    }

   

    IEnumerator SuccessAction()
    {
        while (true)
        {
            if (isSuc && !isBonus)
            {
                isSuc = false;
                combo += 1;
                comboTimeCount = 0;
                score += DataMgr.Instance.addScoreValue;
                GameObject nowMon = monsterObjectListQ.Dequeue();

                //nowMon Move;
                if (nowMonster == 0)
                {
                    //nowMon move to leftLoc;
                    nowMon.transform.DOMove(leftLocation.transform.position, DataMgr.Instance.moveSpeedTime)
                        .OnComplete(() => DoCom(nowMon));
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
                    obj.SetActive(true);
                    obj.transform.DOMoveY(obj.transform.position.y - moveValue, DataMgr.Instance.moveSpeedTime);
                    obj.GetComponent<SpriteRenderer>().sortingOrder = ((int)obj.transform.position.y - 10) * (-1);
                }

                lastMonster.SetActive(true);
                lastMonster.GetComponent<SpriteRenderer>().sortingOrder =
                    ((int)lastMonster.transform.position.y - 10) * (-1);
                var tween = lastMonster.transform
                    .DOMoveY(lastMonster.transform.position.y - moveValue, DataMgr.Instance.moveSpeedTime).OnComplete(
                        () => { AddNewMonster(nowMon); });
                

                nowMonster = monsterList.Dequeue();

                if (combo == DataMgr.Instance.bonusStandNum)
                {
                    SetBonus();
                }
                yield return tween.WaitForCompletion();
            }

            else if (isBonus && isSuc)
            {
                isSuc = false;
                comboTimeCount = 0;
                bonusCombo += 1;
                score += DataMgr.Instance.addScoreValue * DataMgr.Instance.bonusScoreValue;
                GameObject nowMon = monsterObjectListQ.Dequeue();
                
                if (nowMonster == 0)
                {
                    //nowMon move to leftLoc;
                    nowMon.transform.DOMove(leftLocation.transform.position, DataMgr.Instance.moveSpeedTime)
                        .OnComplete(() => DoCom(nowMon));
                }
                else
                {
                    //nowMon move to rightLoc;
                    nowMon.transform.DOMove(rightLocation.transform.position, DataMgr.Instance.moveSpeedTime)
                        .OnComplete(() => DoCom(nowMon));

                }

                foreach (GameObject obj in monsterObjectListQ)
                {
                    //obj move to obj.y - ( moveVec ); 
                    obj.SetActive(true);
                    obj.transform.DOMoveY(obj.transform.position.y - moveValue, DataMgr.Instance.moveSpeedTime);
                    obj.GetComponent<SpriteRenderer>().sortingOrder = ((int)obj.transform.position.y - 10) * (-1);
                }
                
                lastMonster.SetActive(true);
                lastMonster.GetComponent<SpriteRenderer>().sortingOrder =
                    ((int)lastMonster.transform.position.y - 10) * (-1);
                var tween = lastMonster.transform
                    .DOMoveY(lastMonster.transform.position.y - moveValue, DataMgr.Instance.moveSpeedTime).OnComplete(
                        () => { AddNewMonster(nowMon); });
                

                nowMonster = monsterList.Dequeue();
                yield return tween.WaitForCompletion();
            }

            TextUpdate();
            yield return null;
        }
    }

    void DoCom(GameObject mon)
    {
        //Debug.Log("Complete");
        mon.SetActive(false);
        mon.transform.position = lastLocation;
        
    }

    void SetBonus()
    {
        isBonus = true;
        bonusUI.SetActive(true);
        bonusBackground.SetActive(true);
        comboText.gameObject.SetActive(false);
        bonusCombo = 0;
        bonusGaugeIdx = 0;
        foreach (var obj in bonusGauge)
        {
            obj.SetActive(true);
        }
        
        TextUpdate();
    }

    void UnsetBonus()
    {
        isBonus = false;
        bonusUI.SetActive(false);
        bonusBackground.SetActive(false);
        comboText.gameObject.SetActive(true);
    }

    void ButtonClickAction(int buttonType)
    {
        if (!isBonus)
        {
            if (!isBtnDown)
            {
                if (nowMonster == buttonType) //success
                {
                    timeValue += DataMgr.Instance.successValue;
                    //SuccessAction();
                    isSuc = true;
                }
                else if (nowMonster == 3) // gold
                {
                    comboText.gameObject.SetActive(false);
                    goldCountText.gameObject.SetActive(true);
                    goldCount += 1;
                    comboTimeCount = 0;

                    if (goldCount >= DataMgr.Instance.goldInputNum)
                    {
                        goldCount = 0;
                        isSuc = true;
                        timeValue += DataMgr.Instance.goldSucValue;
                        comboText.gameObject.SetActive(true);
                        goldCountText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Wrong button");
                    timeValue -= DataMgr.Instance.failValue;
                    combo = 0;
                    StartCoroutine(FailedClick(buttonType));
                }
            }
            else
            {
                if (nowMonster == 2) //purple
                {
                    if (purpleTimeCount >= DataMgr.Instance.purpleDelayGrace)
                    {
                        Debug.Log("late purple");
                        timeValue -= DataMgr.Instance.failValue;
                        combo = 0;
                        StartCoroutine(FailedClick(buttonType));
                    }
                    else
                    {
                        timeValue += DataMgr.Instance.successValue;

                        //SuccessAction();
                        isSuc = true;
                    }
                }
            }
        } // ~!bonus

        else
        {
            isSuc = true;
        }
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
    
    public void OnPointerDown (PointerEventData eventData)
    {
        isBtnDown = true;
        purpleTimeCount = 0;
    }
 
    public void OnPointerUp (PointerEventData eventData)
    {
        isBtnDown = false;
        purpleTimeCount = 0;

    }
}
