using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsGetter : MonoBehaviour
{
    public PantsCalc pantsCalc;
    public Collider pantsGetterCollider;
    public float vaccumableDistance = 2.25f;
    public float vaccumableAngle = 90f;
    public bool vccuming = false;

    HashSet<GirlController_> vaccumedableGirlControllers = null;
    private void Start()
    {
        vaccumedableGirlControllers = Player_GirlManager.instance.vaccumedableGirlControllers;
        
    }
    private void Update()
    {
        //if(Input.GetButtonDown("Fire1"))
        //{
        //    pantsGetterCollider.enabled = true;
        //}
        //if(Input.GetButtonUp("Fire1"))
        //{
        //    pantsGetterCollider.enabled = false;
        //}

        //if(Input.GetButtonDown("Fire1"))
        //{
        //    vccuming = true;
        //}
        //if(Input.GetButton("Fire1"))
        //{
        //    foreach(var vaccumablegirlConroller in vaccumedableGirlControllers)
        //    {
        //        vaccumablegirlConroller.isNaked = true;
        //        vaccumablegirlConroller.currentState = GirlController_.State.Vacuumed;
        //    }
        //}
        //if(Input.GetButtonUp("Fire1"))
        //{
        //    vccuming = false;
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Girl"))
    //    {
    //        GirlController girl = other.gameObject.GetComponent<GirlController>();
    //        if (!girl.isNaked)
    //        {
    //            girl.isNaked = true;
    //            pantsCalc.GetPants();
    //        }

    //    }
    //    if (other.CompareTag("Pants"))
    //    {
    //        Destroy(other.gameObject);
    //        pantsCalc.GetPants();
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Girl"))
    //    {
    //        GirlController girl = other.gameObject.GetComponent<GirlController>();
    //        if (!girl.isNaked)
    //        {
    //            girl.isNaked = true;
    //            pantsCalc.GetPants();
    //        }

    //    }
    //    if (other.CompareTag("Pants"))
    //    {
    //        Destroy(other.gameObject);
    //        pantsCalc.GetPants();
    //    }
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if(other.CompareTag("Girl"))
    //    {
    //        GirlController girl = other.gameObject.GetComponent<GirlController>();
    //        if(!girl.isNaked)
    //        {
    //            girl.isNaked = true;
    //            pantsCalc.GetPants();
    //        }
            
    //    }
    //    if (other.CompareTag("Pants"))
    //    {
    //        Destroy(other.gameObject);
    //        pantsCalc.GetPants();
    //    }
    //}
}
