using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    public Collider attackTrigger;
    public string targetTagName;
    public UnityEvent addtionalAttackEvent;
    
    

    private bool hitFlag = false;//triggerを１度しかヒットさせないためのフラグ 要らないかも


    private void Start()
    {
        //Debug.Log(addtionalAttackEvent);
    }
    private void OnEnable()
    {
        attackTrigger.enabled = true;
    }

    private void OnDisable()
    {
        attackTrigger.enabled = false;
        hitFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!hitFlag && other.CompareTag(targetTagName))
        {
            var iDamageable = other.GetComponent<IDamageable>();
            if(iDamageable != null)
            {
                Vector3 direction = other.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                iDamageable.Damage(direction);
            }
        }
    }
}
