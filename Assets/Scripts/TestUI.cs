using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public PantsStorage pantsStorage;
    public PantsCalc pantsCalc;

    GUIStyle labelStyle;
    GUIStyle boxStyle;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
        
    void GuiSetup()
    {
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 48;
        boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.fontSize = 24;
  
    }
    private void OnGUI()
    {
        GuiSetup();
        GUI.Box(new Rect(10, 10, 150, 150), "Žc‚è", boxStyle);
        GUI.Label(new Rect(50, 75, 150, 150), (pantsStorage.goalPantsCount - pantsCalc.storeragedPantsCount).ToString(), labelStyle);
    }
}
