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
        if (type == 0) //red
        {
            airMonSprite.sprite = DataMgr.Instance.airMonsterImgs[0];
        }
        else if (type == 1) //blue
        {
            airMonSprite.sprite = DataMgr.Instance.airMonsterImgs[1];
        }
    }
}
