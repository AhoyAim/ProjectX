using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsGetter : MonoBehaviour
{
    public PantsCalc pantsCalc;
    public Collider pantsGetterCollider;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            pantsGetterCollider.enabled = true;
        }
        if(Input.GetButtonUp("Fire1"))
        {
            pantsGetterCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl"))
        {
            GirlController girl = other.gameObject.GetComponent<GirlController>();
            if (!girl.isNaked)
            {
                girl.isNaked = true;
                pantsCalc.GetPants();
            }

        }
        if (other.CompareTag("Pants"))
        {
            Destroy(other.gameObject);
            pantsCalc.GetPants();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Girl"))
        {
            GirlController girl = other.gameObject.GetComponent<GirlController>();
            if (!girl.isNaked)
            {
                girl.isNaked = true;
                pantsCalc.GetPants();
            }

        }
        if (other.CompareTag("Pants"))
        {
            Destroy(other.gameObject);
            pantsCalc.GetPants();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Girl"))
        {
            GirlController girl = other.gameObject.GetComponent<GirlController>();
            if(!girl.isNaked)
            {
                girl.isNaked = true;
                pantsCalc.GetPants();
            }
            
        }
        if (other.CompareTag("Pants"))
        {
            Destroy(other.gameObject);
            pantsCalc.GetPants();
        }
    }
}
