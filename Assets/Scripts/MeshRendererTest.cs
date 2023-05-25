using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererTest : MonoBehaviour
{
    public Material skinmaterial1;
    public Material skinmaterial2;
    public Transform bodyTransform;


    bool flag;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Material[] newMaterials;
    void Start()
    {
        skinnedMeshRenderer = bodyTransform.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.materials[0] = skinmaterial1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (flag)
            {
                Debug.Log(11111);
                flag = false;
                newMaterials = skinnedMeshRenderer.materials;
                newMaterials[0] = skinmaterial2;
                skinnedMeshRenderer.materials = newMaterials;

                //skinnedMeshRenderer.materials[0].SetColor("_Color", Color.red);
            }
            else
            {
                Debug.Log(22222);
                flag = true;
                newMaterials = skinnedMeshRenderer.materials;
                newMaterials[0] = skinmaterial1;

                skinnedMeshRenderer.materials = newMaterials;
            }
        }
    }

}
