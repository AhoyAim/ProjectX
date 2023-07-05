using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PantsGetter : MonoBehaviour
{
    public PantsCalc pantsCalc;
    public GameObject PantsObj;
    public float vaccumableDistance = 2.25f;
    public float vacuumableAngle = 90f;
    public float vacuumReleasPower = 20;
    public float vacuumFailsInterval = 2f;
    public bool vacuuming = false;
    public bool hyperVacuuming = false;
    public bool vacuumReleasing = false;
    public bool vacuumingFails = false;

    HashSet<GirlController> vaccumedableGirlControllers = null;
    private void Start()
    {
        vaccumedableGirlControllers = Player_GirlManager.instance.vaccumedableGirlControllers;
        
    }
    private void Update()
    {
    }
    private void OnTriggerStay(Collider other)
    {
        var freePants = other.GetComponent<FreePants>();
        if (vacuuming && !freePants.isDestroy)
        {
            freePants.isDestroy = true;
            freePants.transform.DOMove(transform.TransformPoint(new Vector3(0, 0.5f, 0.25f)), 0.15f);
            Destroy(freePants.gameObject, 0.2f);
            pantsCalc.GetPants();
        }
    }
    public void OnVacuum()
    {
        vacuuming = true;
        foreach(var girl in vaccumedableGirlControllers)
        {
            if(!girl.isNaked)
            {
                girl.isNaked = true;
                var pantsObj = Instantiate(PantsObj, girl.sphereCastRoot.position, girl.sphereCastRoot.rotation);
                pantsObj.transform.DOMove(transform.TransformPoint(new Vector3(0, 0.5f, 0.25f)), 0.15f);
                Destroy(pantsObj, 0.2f);

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
            if(girl.currentState == GirlController.State.Damaged)
            {
                return;
            }
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
