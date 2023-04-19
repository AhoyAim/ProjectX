using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsGetter : MonoBehaviour
{
    public PantsCalc pantsCalc;
    public float vaccumableDistance = 2.25f;
    public float vaccumableAngle = 90f;
    public bool vacuuming = false;
    public bool hyperVacuuming = false;
    public bool vacuumReieasing = false;

    HashSet<GirlController_> vaccumedableGirlControllers = null;
    private void Start()
    {
        vaccumedableGirlControllers = Player_GirlManager.instance.vaccumedableGirlControllers;
        
    }
    private void Update()
    {
    }
    public void OnVacuum()
    {
        vacuuming = true;
        foreach(var girl in vaccumedableGirlControllers)
        {
            if(!girl.isNaked)
            {
                girl.isNaked = true;
                pantsCalc.GetPants();
            }
        }
    }

    public void OnHyoerVacuuming()
    {
        hyperVacuuming = true;
        // girl���������葱���铮�����������
    }

    public void OnVacuumRelease()
    {
        
    }

    public void OffVacuum()
    {
        vacuuming = false;
        hyperVacuuming |= false;
    }


}
