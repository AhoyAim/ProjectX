using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public Attack attackScript;
    public void AttackStart()
    {
        attackScript.enabled = true;
    }

    public void AttackEnd()
    {
        attackScript.enabled = false;
    }
}
