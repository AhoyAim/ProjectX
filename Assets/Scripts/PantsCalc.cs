using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsCalc : MonoBehaviour
{
    public int carryPantsCount;
    public int storagedPantsCount;
    //public int CarryPantsCount { get; private set; }
    //public int StoragedPantsCount { get; private set; }
    void Start()
    {
        carryPantsCount= 0;
        storagedPantsCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //carryPantsCount = carryPantsCount;
        //storagedPantsCount= storagedPantsCount;
    }
    public void GetPants()
    {
        carryPantsCount += 1;
    }
    public void StoragePants()
    {
        storagedPantsCount += carryPantsCount;
        carryPantsCount = 0;
    }
    public int LostPants()
    {
        int lostPantsAmount = Mathf.CeilToInt(carryPantsCount / 2f);
        carryPantsCount -= lostPantsAmount;
        return lostPantsAmount;
    }
}
