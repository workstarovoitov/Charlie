using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarControl : MonoBehaviour
{

    [SerializeField] private GameObject target;			//current target
    private UnitMain uMain;
    [SerializeField] private STATTYPE statType;

    [SerializeField] private BarVisualisation barFrame;
    [SerializeField] private BarVisualisation barFill;
    private int defaultMediumHealth = 250;
    public int DefaultMediumHealth
    {
        get => defaultMediumHealth;
        set  
        {
            defaultMediumHealth = value;
            healthInFrame = defaultMediumHealth / (Screen.width / 2 / 32);
        }
    }
    private int healthInFrame = 10;
    private int curMaxStat = 0;
    private bool startBlink = false;
    public bool StartBlink
    {
        get => startBlink;
        set => startBlink = value;
    }
    // Start is called before the first frame update
    //void Start()
    //{
    //    healthInFrame = defaultMediumHealth / (Screen.width / 2 / 32);
    //    if (target == null)
    //    {
    //        return;
    //    }
    //    uMain = target.GetComponent<UnitMain>();
    //    curMaxStat = Mathf.RoundToInt(GetCurrentMaxStat() / healthInFrame);
    //}

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (startBlink)
        {
            startBlink = false;
            uMain.uStamina.LowStamina = false;
            barFrame.RunFrameBlink();
        }

        int displayedFrameBlocks = barFrame.FrameList.Count;
        int frameCurMaxBlocks = Mathf.RoundToInt(GetCurrentMaxStat() / healthInFrame); 
        int frameMaxBlocks = Mathf.RoundToInt(GetMaxStat() / healthInFrame);

        //Debug.Log(name + " frameCurMaxBlocks " + frameCurMaxBlocks);
        //Debug.Log(name + " frameMaxBlocks " + frameMaxBlocks);
        if (displayedFrameBlocks < frameMaxBlocks)
        {
            for (int i = displayedFrameBlocks; i < frameMaxBlocks; i++)
            {
                int frameType;
                if (i == 0)
                {
                    frameType = 0;
                }
                else if (i == frameMaxBlocks - 1)
                {
                    frameType = 2;
                }
                else if (i == frameCurMaxBlocks - 1)
                {
                    frameType = 3;
                }
                else
                {
                    frameType = 1;
                }
                barFrame.CreateFrame();
                barFrame.SetFrameType(barFrame.FrameList[i], frameType);
            }
        }

        if (curMaxStat != frameCurMaxBlocks )
        {
            if (curMaxStat != 0)
            {
                barFrame.SetFrameType(barFrame.FrameList[curMaxStat - 1], 1);
            }
            barFrame.SetFrameType(barFrame.FrameList[frameCurMaxBlocks - 1], 3);
            barFrame.SetFrameType(barFrame.FrameList[frameMaxBlocks - 1], 2);
            curMaxStat = frameCurMaxBlocks;
            //if (curMaxStat != frameMaxBlocks)
            //{
            //    barFrame.SetFrameType(barFrame.FrameList[curMaxStat-1], 1);
            //}

            //curMaxStat = frameCurMaxBlocks;
            //if (curMaxStat != frameMaxBlocks)
            //{
            //    barFrame.SetFrameType(barFrame.FrameList[curMaxStat - 1], 1);
            //}


        }

        int displayedFillBlocks = barFill.FrameList.Count;
        int healthBlocks = Mathf.RoundToInt(GetCurrentStat() / healthInFrame);
        if (healthBlocks < 0) healthBlocks = 0;
        if (healthBlocks == displayedFillBlocks) return;
        if (displayedFillBlocks < healthBlocks)
        {
            if (displayedFillBlocks == 1)
            {
                barFill.SetFrameType(barFill.FrameList[0], 1);
            }
            
            if (displayedFillBlocks > 1)
            {
                barFill.SetFrameType(barFill.FrameList[displayedFillBlocks - 1], 1);
            }

            for (int i = displayedFillBlocks; i < healthBlocks; i++)
            {
                int frameType;
                if (i == 0)
                {
                    frameType = 0;
                }
                else if (i == healthBlocks - 1)
                {
                    frameType = 2;
                }
                else
                {
                    frameType = 1;
                }
                barFill.CreateFrame();
                barFill.SetFrameType(barFill.FrameList[i], frameType);
            }
        }
        else
        {   
            if (healthBlocks >= 1)
            { 
                for (int i = displayedFillBlocks; i > healthBlocks; i--)
                {
                    barFill.RemoveFrame(i - 1);
                }
            
                barFill.SetFrameType(barFill.FrameList[healthBlocks - 1], 2);
            }
            else
            {
                barFill.RemoveFrame(0);
            }
        } 
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        uMain = target.GetComponent<UnitMain>();
        //curMaxStat = Mathf.RoundToInt(GetMaxStat() / healthInFrame);
    }
    public void SetTargetOff()
    {
        barFill.RemoveAllFrames();
        barFrame.RemoveAllFrames();       
        target = null;

    }

    private float GetCurrentStat()
    {
        switch (statType)
        {
            case STATTYPE.HEALTH: return uMain.uHealth.CurrentHealth;
            case STATTYPE.STAMINA: return uMain.uStamina.CurrentStamina;
        }
        return 0;
    }      
    private float GetCurrentMaxStat()
    {
        switch (statType)
        {
            case STATTYPE.HEALTH: return uMain.uHealth.CurrentMaxHealth;
            case STATTYPE.STAMINA: return uMain.uStamina.CurrentMaxStamina;
        }
        return 0;
    }       
    private float GetMaxStat()
    {
        switch (statType)
        {
            case STATTYPE.HEALTH: return uMain.uHealth.MaxHealth;
            case STATTYPE.STAMINA:
                startBlink = uMain.uStamina.LowStamina;
                return uMain.uStamina.MaxStamina;
        }
        return 0;
    }   

}

public enum STATTYPE
{
    HEALTH = 1,
    STAMINA = 2,
};