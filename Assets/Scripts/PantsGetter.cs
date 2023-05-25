using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsGetter : MonoBehaviour
{
    public PantsCalc pantsCalc;
    public float vaccumableDistance = 2.25f;
    public float vacuumableAngle = 90f;
    public float vacuumReleasPower = 20;
    public float vacuumFailsInterval = 2f;
    public bool vacuuming = false;
    public bool hyperVacuuming = false;
    public bool vacuumReleasing = false;
    public bool vacuumingFails = false;

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
                girl.mosaic.enabled = true;
                girl.UpdatePants();
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
            Debug.Log($"{girl}‚ðBlowAway{transform.forward}‚µ‚½");
            // girl.currentState = GirlController_.State.BlownAway;
        }
    }

    public void Idle()
    {
        vacuuming = false;
        hyperVacuuming = false;
        vacuumReleasing = false;
    }

    public void VacuumingFails()
    {
        vacuumingFails = true;
        Idle();
        Invoke("EndVacuumingFails", vacuumFailsInterval);
    }

    public void EndVacuumingFails()
    {
        vacuumingFails = false;
    }


}
