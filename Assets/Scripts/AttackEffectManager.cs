using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectManager : MonoBehaviour
{
    public ParticleSystem attackEffect;
    public ParticleSystem attackEffect2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AttackEffectStart()
    {
        attackEffect.Play();
        attackEffect2.Play();
    }
    public void AttackEffectEnd() 
    {
        attackEffect.Stop();
        attackEffect.Clear();
        attackEffect2.Stop();
        //attackEffect2.Clear();
    }
}
