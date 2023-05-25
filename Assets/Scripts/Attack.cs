using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Collider attackTrigger;
    public string targetTagName;
    

    private bool hitFlag = false;//triggerを１度しかヒットさせないためのフラグ

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
