using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GirlManager : MonoBehaviour
{
    public static Player_GirlManager instance;

    public Transform playerTransform;

    public float noticeDistance = 10;
    public float vacuumableDistance = 3;

    public GirlState[] girlStates = null;
    public Transform[] girlsTransform = null;
    public bool[] isNotice = null;
    public bool[] vacuumable = null;

    int girlsAmount = 0;
    void Start()
    {
        instance = this;
        InitArrays();
    }

    
    void Update()
    {
        CheckNotice();
        CheckVaccumable();
    }

    void InitArrays()
    {
        girlStates = FindObjectsOfType<GirlState>();
        girlsAmount = girlStates.Length;
        for (var i = 0; i < girlsAmount; i++)
        {
            girlStates[i].SetIndex(i);
        }

        girlsTransform = new Transform[girlStates.Length];
        for (var i = 0; i < girlsAmount; i++)
        {
            girlsTransform[i] = girlStates[i].transform;
        }

        isNotice = new bool[girlStates.Length];
        for (var i = 0; i < girlsAmount; i++)
        {
            isNotice[i] = false;
        }

        vacuumable = new bool[girlStates.Length];
        for (var i = 0; i < girlsAmount; i++)
        {
            vacuumable[i] = false;
        }
    }

    void CheckNotice()
    {
        for (var i = 0; i < girlsAmount; i++)
        {
            
            if ((playerTransform.position - girlsTransform[i].position).sqrMagnitude < noticeDistance * noticeDistance)
            {
                isNotice[i] = true;
            }
            else
            {
                isNotice[i] = false;
            }
        }
    }
    void CheckVaccumable()
    {
        Vector3 playerForward = playerTransform.forward;
        for (var i = 0; i < girlsAmount; i++)
        {
            Vector3 toGirlDirection = girlsTransform[i].position - playerTransform.position;
            if ((playerTransform.position - girlsTransform[i].position).sqrMagnitude < vacuumableDistance * vacuumableDistance 
                && Vector3.Angle(playerForward, toGirlDirection) <= 45)
            {
                vacuumable[i] = true;
            }
            else
            {
                vacuumable[i] = false;
            }
        }
    }
}
