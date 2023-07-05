using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsStorage : MonoBehaviour
{
    public int goalPantsCount;
    public PantsCalc pantsCalc;

    private void Start()
    {
        goalPantsCount = FindObjectsOfType<GirlController>().Length;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            pantsCalc.StoragePants();
            
            if(goalPantsCount == pantsCalc.storagedPantsCount)
            {
                Debug.Log("ÉSÅ[Éã");
            }
        }
    }
}
