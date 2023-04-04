using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : MonoBehaviour
{
    public Transform playerTransform;
    Transform girlTransform;

    public float awakeDistance;
    public bool isNaked;
    public enum STATE
    {
        Normal,
        ALERT,
    }
    public STATE state;

    private void Start()
    {
        state = STATE.Normal;
        girlTransform= transform;
    }
    private void Update()
    {
        if ((playerTransform.position - girlTransform.position).sqrMagnitude < awakeDistance * awakeDistance)
        {
            state = STATE.ALERT;
        }
        else
        {
            state = STATE.Normal;
        }
    }
}
