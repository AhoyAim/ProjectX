using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsGetter : MonoBehaviour
{
    public PantsCalc pantsCalc;
    public float vaccumableDistance = 2.25f;
    public float vacuumableAngle = 90f;
    public float vacuumReleasPower = 20;
    public bool vacuuming = false;
    public bool hyperVacuuming = false;
    public bool vacuumReleasing = false;

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

    public void OnHyperVacuuming()
    {
        hyperVacuuming = true;
    }

    public void OnVacuumRelease()
    {
        Idle();
        vacuumReleasing = true;
        foreach (var girl in vaccumedableGirlControllers)
        {
            girl.BlowAway(transform.forward * vacuumReleasPower);
            Debug.Log($"{girl}��BlowAway{transform.forward}����");
        }
    }

    public void Idle()
    {
        vacuuming = false;
        hyperVacuuming = false;
        vacuumReleasing = false;
    }


}
