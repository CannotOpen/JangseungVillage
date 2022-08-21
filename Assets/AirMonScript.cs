using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMonScript : MonoBehaviour
{
    public int airMonType;
    public SpriteRenderer airMonSprite;

    void Start()
    {
        airMonSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetAirMonType(int type)
    {
        airMonSprite.sprite = DataMgr.Instance.airMonsterImgs[type];
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            InGameMgr.Instance.HitAirMonster();
            
        }
    }
}
