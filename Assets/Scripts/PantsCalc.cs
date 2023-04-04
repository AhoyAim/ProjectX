using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsCalc : MonoBehaviour
{
    public int carryPantsCount;
    public int storeragedPantsCount;
    public int CarryPantsCount { get; private set; }
    public int StoragedPantsCount { get; private set; }
    void Start()
    {
        CarryPantsCount= 0;
        StoragedPantsCount= 0;
    }

    // Update is called once per frame
    void Update()
    {
        carryPantsCount = CarryPantsCount;
        storeragedPantsCount= StoragedPantsCount;
    }
    public void GetPants()
    {
        CarryPantsCount += 1;
    }
    public void StoragePants()
    {
        StoragedPantsCount += CarryPantsCount;
        CarryPantsCount= 0;
    }
}
