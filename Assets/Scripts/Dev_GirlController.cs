using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_GirlController : MonoBehaviour
{
    public GirlState girlState;
    public AgentMovement ai;

    public GirlState.State test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        test = girlState.currentState;
        switch (girlState.currentState)
        {
            case GirlState.State.Normal:
                ai.OnNormal();
                break;
            case GirlState.State.Runaway:
                ai.OnRunaway();
                break;
            case GirlState.State.Approch:
                ai.OnApproch();
                break;
            default:
                break;
        }
    }
}
