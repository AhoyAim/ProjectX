using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStateManager : MonoBehaviour
{
   
    public GirlController_[] girlController_s; 

    private void Start()
    {
        girlController_s = FindObjectsOfType<GirlController_>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.F1))
        {
            ChangeState(GirlController_.State.Normal);
        }
        if (Input.GetKey(KeyCode.F2))
        {
            ChangeState(GirlController_.State.Notice);

        }
        if (Input.GetKey(KeyCode.F3))
        {
            ChangeState(GirlController_.State.Runaway); 
        }
        if (Input.GetKey(KeyCode.F4))
        {
            ChangeState(GirlController_.State.Approch);
        }
        if (Input.GetKey(KeyCode.F5))
        {
            ChangeState(GirlController_.State.Attack); 
        }
        if (Input.GetKey(KeyCode.F6))
        {
            ChangeState(GirlController_.State.Damaged); 
        }
        if (Input.GetKey(KeyCode.F7))
        {
            ChangeState(GirlController_.State.Stan);
        }
        if (Input.GetKey(KeyCode.F8))
        {
            ChangeState(GirlController_.State.Attack); 
        }
        if (Input.GetKey(KeyCode.F9))
        {
            ChangeState(GirlController_.State.Vacuumed); 
        }
        if (Input.GetKey(KeyCode.F10))
        {
            ChangeState(GirlController_.State.HyperVacuumed);
        }
        if (Input.GetKey(KeyCode.F11))
        {
            ChangeState(GirlController_.State.BlownAway); 
        }

    }
    void ChangeState(GirlController_.State name)
    {
        foreach(var girl in girlController_s)
        {
            girl.currentState = name;
        }
    }
}
