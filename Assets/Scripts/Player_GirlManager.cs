using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GirlManager : MonoBehaviour
{
    public static Player_GirlManager instance;

    public HashSet<GirlController> vaccumedableGirlControllers;
    void Awake()
    {
        instance = this;
        vaccumedableGirlControllers = new();
        
    }

    
    
    
}
