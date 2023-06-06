using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GirlManager_ : MonoBehaviour
{
    public static Player_GirlManager_ instance;

    public HashSet<GirlController_> vaccumedableGirlControllers;
    void Awake()
    {
        instance = this;
        vaccumedableGirlControllers = new();

    }
}
