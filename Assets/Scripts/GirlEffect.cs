using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlEffect : MonoBehaviour
{
    public Transform damagedEffect;
    public Transform girlHead;
    Vector3 offset = Vector3.down * 1.68f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damagedEffect.position = girlHead.position + offset;
    }
}
