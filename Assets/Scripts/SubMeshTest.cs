using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMeshTest : MonoBehaviour
{
    public Transform bodyTransform;
    bool flag;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Material[] newMaterials;
    Color originColor;
    void Start()
    {
        skinnedMeshRenderer = bodyTransform.GetComponent<SkinnedMeshRenderer>();
        newMaterials = skinnedMeshRenderer.materials;
        originColor = newMaterials[1].color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (flag)
            {
                Debug.Log(11111);
                flag = false;
                newMaterials = skinnedMeshRenderer.materials;
                newMaterials[2].color = Color.clear;
                skinnedMeshRenderer.materials = newMaterials;

                //skinnedMeshRenderer.materials[0].SetColor("_Color", Color.red);
            }
            else
            {
                Debug.Log(22222);
                flag = true;
                newMaterials = skinnedMeshRenderer.materials;
                newMaterials[2].color = originColor;

                skinnedMeshRenderer.materials = newMaterials;
            }
        }


    }
}
