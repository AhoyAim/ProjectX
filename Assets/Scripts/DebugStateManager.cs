using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStateManager : MonoBehaviour
{
   
    public GirlController[] girlControllers; 

    private void Start()
    {
        girlControllers = FindObjectsOfType<GirlController>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.F1))
        {
            ChangeState(GirlController.State.Normal);
        }
        if (Input.GetKey(KeyCode.F2))
        {
            ChangeState(GirlController.State.Notice);

        }
        if (Input.GetKey(KeyCode.F3))
        {
            ChangeState(GirlController.State.Runaway); 
        }
        if (Input.GetKey(KeyCode.F4))
        {
            ChangeState(GirlController.State.Approch);
        }
        if (Input.GetKey(KeyCode.F5))
        {
            ChangeState(GirlController.State.Attack); 
        }
        if (Input.GetKey(KeyCode.F6))
        {
            ChangeState(GirlController.State.Damaged); 
        }
        if (Input.GetKey(KeyCode.F7))
        {
            //ChangeState(GirlController_.State.Stan);
        }
        if (Input.GetKey(KeyCode.F8))
        {
            ChangeState(GirlController.State.Attack); 
        }
        if (Input.GetKey(KeyCode.F9))
        {
            ChangeState(GirlController.State.Vacuumed); 
        }
        if (Input.GetKey(KeyCode.F10))
        {
            ChangeState(GirlController.State.HyperVacuumed);
        }
        if (Input.GetKey(KeyCode.F11))
        {
            ChangeState(GirlController.State.BlownAway); 
        }

    }
    void ChangeState(GirlController.State name)
    {
        foreach(var girl in girlControllers)
        {
            girl.currentState = name;
        }
    }
}
