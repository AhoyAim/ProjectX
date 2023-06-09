using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    
    void Start()
    {
        CinemachineImpulseSource im = GetComponent<CinemachineImpulseSource>();
        im.GenerateImpulse(5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
