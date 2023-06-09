using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class DoTweenTest : MonoBehaviour
{
    // StartPos is called before the first frame update
    void Start()
    {
        Vector3 StartPos = transform.position;
        Vector3 TargeValue = transform.position + transform.forward * 10;
        float TweenTime = 5.0f;
        DOTween.To
        (
            () => StartPos,       //����
            (x) =>
            {
                StartPos = x;
                Debug.Log(StartPos);
                transform.Translate(StartPos - transform.position);
            },  //����
            TargeValue,     //�ǂ��܂�(�ŏI�I�Ȓl)
            TweenTime		//�ǂꂭ�炢�̎���
        ).OnComplete(() => Debug.Log("������яI���܂���"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
