using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;

public class InGameMgr : Singleton<InGameMgr>
{
    [Header("InGameState")] [HideInInspector]
    public int dayState; // 0 == morning, 1 == afternoon, 2 == night;
    public Camera mainCam;
    [HideInInspector] public bool isSuc;
    [HideInInspector] public bool isAir;
    
    [Header("Value")]
     public Queue<int> monsterList = new Queue<int>();
    [HideInInspector] public int nowMonster = -1; // 0 == red, 1 == blue, 2 == purple, 3 == gold
    [HideInInspector] public bool isGamePlay;
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
    //public Button leftButton;
    //public Button rightButton;
    public GameObject leftButton;
    public GameObject rightButton;
    public ButtonEventScript leftScrpit;
    public ButtonEventScript rightScript;
    //[HideInInspector] public SpriteRenderer leftSpriteRenderer;
    //[HideInInspector] public SpriteRenderer rightSpriteRenderer;
    public GameObject leftFailedImg;
    public GameObject rightFailedImg;
    [HideInInspector] public bool isPurple;
    [HideInInspector] public bool isBtnLDown;
    [HideInInspector] public bool isBtnRDown;
    [HideInInspector] public bool isBtnLCheck;
    [HideInInspector] public bool isBtnRCheck;

    [Header("Score")] 
    public Text scoreText;
    public Text comboText;
    public Text goldCountText;
    public int combo;
    [HideInInspector] public int score;
    [HideInInspector] public float comboTimeCount ;

    [Header("Bonus")] 
    [HideInInspector] public bool isBonus;
    public int bonusCombo;
    [HideInInspector] public float bonusTimeCount;
    [HideInInspector] public int bonusGaugeIdx;
    public GameObject bonusBackground;
    public GameObject bonusUI;
    public Text bonusComboText;
    public GameObject[] bonusGauge;

    [Header("Air")] 
    [HideInInspector] public Vector2 startPos;
    [HideInInspector] public float airTimeCount;
    [HideInInspector] public bool isTouch;
    public GameObject[] outsideLocations;
    public GameObject[] insideLocations;
    [HideInInspector] public Vector3 leftBulletOriLocation;
    [HideInInspector] public Vector3 rightBulletOriLocation;
    [HideInInspector] public Vector3 airMonsterTargetLocation;
    public GameObject airMonster;
    [HideInInspector] public bool isAirMonMoveEnd;
    [HideInInspector] public AirMonScript airMonScript;

    public GameObject leftBullet;
    public GameObject rightBullet;
    public GameObject bulletTarget;


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
            if (!isAir)
            {
                if (!isBonus)
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

                    
                    
                    if (isPurple)
                    {
                        if (isBtnLDown || isBtnRDown)
                        {
                            purpleTimeCount += Time.deltaTime;
                        }
                    }
                    
                    
                }
                else //bonus
                {
                    bonusTimeCount += Time.deltaTime;

                    if (bonusTimeCount >= DataMgr.Instance.bonusTime / 5)
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
            else if (isAir)
            {
                airTimeCount += Time.deltaTime;
                
                if (Input.touchCount > 0)
                {

                    if (Input.GetTouch(0).phase == TouchPhase.Began && !isBtnLCheck && !isBtnRCheck)
                    {
                        startPos = Input.GetTouch(0).position;
                        //Debug.Log("tochIn"); //-> work
                        //isTouch = true;
                        if (isBtnLDown)
                        {
                            isBtnLCheck = true;
                        }
                        else if (isBtnRDown)
                        {
                            isBtnRCheck = true;
                        }
                    }

                    if (Input.GetTouch(0).phase == TouchPhase.Ended && (isBtnLCheck || isBtnRCheck))
                    {
                        //Debug.Log("End");
                        float swipeLength = Input.GetTouch(0).position.y - startPos.y;

                        //Debug.Log(swipeLength.ToString());
                        if (swipeLength >= DataMgr.Instance.airSwipeStand)
                        {
                            //swipe
                            Debug.Log("swipe");

                            if (isBtnLCheck)
                            {
                                Debug.Log("Left");
                                leftBullet.transform.DOMove(
                                    new Vector3(leftBullet.transform.position.x, bulletTarget.transform.position.y,
                                        leftBullet.transform.position.z), DataMgr.Instance.airBulletSpeedTime).OnComplete(
                                    () =>
                                    {
                                        ResetBullet(0);
                                    });
                                isBtnLCheck = false;
                            }
                            else if (isBtnRCheck)
                            {
                                Debug.Log("right");
                                rightBullet.transform.DOMove(
                                    new Vector3(rightBullet.transform.position.x, bulletTarget.transform.position.y,
                                        rightBullet.transform.position.z), DataMgr.Instance.airBulletSpeedTime).OnComplete(
                                    () =>
                                    {
                                        ResetBullet(1);
                                    });;
                                
                                isBtnRCheck = false;
                            }

                        }
                        else
                        {
                            isBtnLCheck = false;
                            isBtnRCheck = false;
                        }
                    }
                    
                    /*
                    if ((isBtnLDown || isBtnRDown)&& isTouch)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Ended)
                        {
                            isTouch = false;
                            Debug.Log("touchEnd");
                            float swipeLength = Input.GetTouch(0).position.y - startPos.y;

                            Debug.Log(swipeLength.ToString());
                            if (swipeLength >= DataMgr.Instance.airSwipeStand)
                            {
                                //swipe
                                Debug.Log("swipe");
                            }
                        }
                    }
                    */
                }

                if (airTimeCount >= DataMgr.Instance.airTime)
                {
                    UnsetAir();
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

        isPurple = false;
        
        nowMonster = monsterList.Dequeue();
        if (nowMonster == 2)
        {
            isPurple = true;
        }

        timeValue = DataMgr.Instance.setTime;
        timeGaugeImg = timeGauge.GetComponent<Image>();
        timeGaugeImg.fillAmount = 1;
        
        //leftButton.onClick.AddListener(()=> ButtonClickAction(0));
        //rightButton.onClick.AddListener(()=>ButtonClickAction(1));
        leftScrpit = leftButton.GetComponent<ButtonEventScript>();
        leftScrpit.buttonType = 0;
        rightScript = rightButton.GetComponent<ButtonEventScript>();
        rightScript.buttonType = 1;

        airMonScript = airMonster.GetComponent<AirMonScript>();

        leftBulletOriLocation = leftBullet.transform.position;
        rightBulletOriLocation = rightBullet.transform.position;

        combo = 0;
        score = 0;
        goldCount = 0;
        comboTimeCount = 0;
        purpleTimeCount = 0;
        bonusCombo = 0;
        bonusGaugeIdx = 0;
        airTimeCount = 0;

        isBtnLDown = false;
        isBtnRDown = false;
        isAir = false;
        isBtnLCheck = false;
        isBtnRCheck = false;
        isTouch = false;
        isAirMonMoveEnd = false;
        
        SetDayState();

        isGamePlay = true;
        isSuc = false;
        TextUpdate();
        StartCoroutine(SuccessAction());
        StartCoroutine(AirMonControll());
    }


    void ResetBullet(int bulletType)
    {
        if (bulletType == 0) //left
        {
            leftBullet.transform.position = leftBulletOriLocation;
        }
        else if (bulletType == 1)
        {
            rightBullet.transform.position = rightBulletOriLocation;
        }
    }

    void AirMonsterSet()
    {
        airMonster.SetActive(true);
        isAirMonMoveEnd = false;
        int monType = Random.Range(0, 2); // 0 == red, 1 == blue
        //set airMonster to montype
        airMonScript.SetAirMonType(monType);
        
        int rnd = Random.Range(0, 2); // 0 == outside, 1 == inside
        if (rnd == 0) // outside
        {
            int idx = Random.Range(0, 4);
            Vector3 spawnLoc = outsideLocations[idx].transform.position;

            airMonster.transform.position = spawnLoc;

            airMonsterTargetLocation = new Vector3(-spawnLoc.x, spawnLoc.y, spawnLoc.z);

            airMonster.transform.DOJump(airMonsterTargetLocation, 2,1,DataMgr.Instance.airMonSpeedTime).OnComplete(
                () =>
                {
                    isAirMonMoveEnd = true;
                });
        }
        else if (rnd == 1) //inside
        {
            int idx = Random.Range(0, 4);
            Vector3 spawnLoc = insideLocations[idx].transform.position;
            
            airMonster.transform.position = spawnLoc;
            
            airMonsterTargetLocation = new Vector3(spawnLoc.x, spawnLoc.y + 3, spawnLoc.z);

            airMonster.transform.DOMove(airMonsterTargetLocation, DataMgr.Instance.airMonSpeedTime).OnComplete(() =>
            {
                airMonster.transform.DOMove(spawnLoc, DataMgr.Instance.airMonSpeedTime).OnComplete(
                    () =>
                    {
                        isAirMonMoveEnd = true;
                    });;
            });
        }
    }

    IEnumerator AirMonControll()
    {
        while (true)
        {
            if (isAir && isAirMonMoveEnd)
            {
                Debug.Log("airMonSpawn");
                AirMonsterSet();
                
            }
            
            yield return null;
        }
        
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
            if (!isAir)
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
                        .DOMoveY(lastMonster.transform.position.y - moveValue, DataMgr.Instance.moveSpeedTime)
                        .OnComplete(
                            () => { AddNewMonster(nowMon); });


                    nowMonster = monsterList.Dequeue();
                    if (nowMonster == 2)
                    {
                        isPurple = true;
                    }
                    else
                    {
                        isPurple = false;
                    }

                    if (combo == DataMgr.Instance.bonusStandNum)
                    {
                        SetBonus();
                    }
                    else if (combo == DataMgr.Instance.airStandNum)
                    {
                        SetAir();
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
                        .DOMoveY(lastMonster.transform.position.y - moveValue, DataMgr.Instance.moveSpeedTime)
                        .OnComplete(
                            () => { AddNewMonster(nowMon); });


                    nowMonster = monsterList.Dequeue();
                    yield return tween.WaitForCompletion();
                }
            }
            else if (isAir)
            {
                
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

    void SetAir()
    {
        isAir = true;
        airTimeCount = 0;
        mainCam.transform.DOMove(new Vector3(0, 19.2f, -15), DataMgr.Instance.airCamMoveTime);
        isAirMonMoveEnd = true;
        //기타연출
    }

    void UnsetAir()
    {
        isAir = false;
        mainCam.transform.DOMove(new Vector3(0, 0, -15), DataMgr.Instance.airCamMoveTime);
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

    public void ButtonClickAction(int buttonType)
    {
        if (!isAir)
        {
            if (!isBonus)
            {
                if (!isPurple)
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
                        //Debug.Log("Wrong button");
                        timeValue -= DataMgr.Instance.failValue;
                        combo = 0;
                        StartCoroutine(FailedClick(buttonType));
                    }
                }
                else //isPurple
                {
                    if (purpleTimeCount >= DataMgr.Instance.purpleDelayGrace)
                    {
                        //Debug.Log("late purple");
                        timeValue -= DataMgr.Instance.failValue;
                        combo = 0;
                        StartCoroutine(FailedClick(buttonType));
                        purpleTimeCount = 0;
                        isBtnLDown = false;
                        isBtnRDown = false;
                    }

                    else if(((buttonType == 1&&isBtnLDown) || (buttonType == 0&&isBtnRDown)) &&purpleTimeCount < DataMgr.Instance.purpleDelayGrace)
                    {
                        timeValue += DataMgr.Instance.successValue;

                        //SuccessAction();
                        isSuc = true;
                        //isPurple = false;
                    }
                }
            } // ~!bonus

            else
            {
                isSuc = true;
            }
        }
        else if (isAir)
        {
            
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
}
