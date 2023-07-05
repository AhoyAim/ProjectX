using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsSearcher : MonoBehaviour
{
    GirlController girlController;
    private void Start()
    {
        girlController = transform.parent.GetComponent<GirlController>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(girlController.isNaked && !girlController.lockOn_freePants_flag)// �p���c�͂��ĂȂ��ăt���[�p���c���܂��ǂ��Ă��Ȃ��Ƃ�
        {
            girlController.lockOn_freePants_obj = other.GetComponent<FreePants>();
            girlController.lockOn_freePants_transform = other.transform;
            girlController.lockOn_freePants_flag = true;
        }
    }
}
