using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTest : MonoBehaviour
{
    public PlayableDirector testPlayableDirector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            testPlayableDirector.Play();
        }
    }
    public void CalledSignal()
    {
        Debug.Log("ƒVƒOƒiƒ‹‚ªŒÄ‚Î‚ê‚Ü‚µ‚½");
    }
}
