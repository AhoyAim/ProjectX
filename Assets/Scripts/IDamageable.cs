using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public bool DamageJudge();
    public void DamageBehaviour(Vector3 direction);
    public void Damage(Vector3 direction);
}
