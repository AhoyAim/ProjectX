using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Dev_AI : MonoBehaviour
{
    GirlController girlController;
    CapsuleCollider girlCollider;
    NavMeshAgent ai;
    Animator animator;
    int animIDSpeed;
    bool isRunaway;

    public LayerMask layerMask;

    public Transform playerTransform;
    Transform girlTransform;
    public float awakeDistance;
    [Range(0.1f,2f)]
    public float minRunawayTime = 0.1f;
    [Range(2.1f,5f)]
    public float maxRunawayTime = 2.1f;
    public float runSpeed;
    public float walkSpeed;
    public float toSearchAgentConfsilictOffset = 0.2f;

    Vector3 toPlayerDirection;

    void Start()
    {
        girlController = GetComponent<GirlController>();
        ai = GetComponent<NavMeshAgent>();
        girlTransform = transform;
        animator = GetComponent<Animator>();
        animIDSpeed = Animator.StringToHash("Speed");
        girlCollider = GetComponent<CapsuleCollider>();
    }


    void Update()
    {
        
        if (!girlController.isNaked)
        {
            ai.stoppingDistance = 0f;
            if(girlController.state == GirlController.STATE.ALERT && !isRunaway)
            {
                ai.speed = runSpeed;
                StartCoroutine(Runaway());
            }
            
            if (girlController.state == GirlController.STATE.Normal)
            {
                if(isRunaway)
                {
                    return;
                }
                ai.speed = walkSpeed;


                if (Random.Range(0, 5000) < 20)
                {
                    Vector3 sorcePosition = new Vector3(girlTransform.position.x + Random.Range(-5f, 5f), girlTransform.position.y, girlTransform.position.z + Random.Range(-5f, 5f));
                    if (NavMesh.SamplePosition(sorcePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        ai.destination = hit.position;
                    }

                }

            }
        }


        if (girlController.isNaked)
        {
            ai.speed = runSpeed;
            ai.stoppingDistance = 2f;
            ai.destination = playerTransform.position;
        }

        if(isRunaway)
        {
            animator.SetFloat(animIDSpeed, runSpeed);
        }
        animator.SetFloat(animIDSpeed, ai.velocity.magnitude);
    }

   
    Vector3 GetSamplePointNavMesh()
    {
        NavMeshTriangulation samplePointNavMeshTriangulation = NavMesh.CalculateTriangulation();
        int index = Random.Range(0, samplePointNavMeshTriangulation.indices.Length);
        return samplePointNavMeshTriangulation.vertices[index];
    }
    /// <summary>
    /// Runawayフラグをtrueにし、agentを動かすコルーチン。コルーチンの終わりで Runawayフラグをfalseにする。
    /// </summary>
    /// <returns></returns>
    IEnumerator Runaway()
    {
        isRunaway = true;
        

        bool isGetPosition = false;
        int tryGetPositionCount = 0;
        Vector3 girlPosition = girlTransform.position;

        // 経路が取得できるか十回ループする
        do
        {
            float r = Random.Range(-80, 80);
            
            toPlayerDirection = (playerTransform.position - girlPosition).normalized;
            Vector3 direction = Quaternion.Euler(0, r, 0) * toPlayerDirection;

            isGetPosition = NavMesh.SamplePosition(-20f * direction + girlPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas);
            if(isGetPosition)
            {
                ai.destination = hit.position;
            }

            tryGetPositionCount++;

        } while (!isGetPosition && tryGetPositionCount <= 9);

        
        //　取得できなかった　＝＞　壁際の可能性が高い
        if (!isGetPosition)
        {
            Debug.Log("hitなし");

            //　より範囲を広げランダムな位置で経路がとれるか試す
            Vector3 sorcePosition = new Vector3(girlTransform.position.x + Random.Range(-10f, 10f), girlTransform.position.y, girlTransform.position.z + Random.Range(-5f, 5f));
            if (NavMesh.SamplePosition(sorcePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                //　経路が取得できたなら、進行方向にplayerがいないかチェックする
                Vector3 tempDestination = hit.position;
                if (Physics.SphereCast(girlPosition, girlCollider.radius * 6, tempDestination - girlPosition, out RaycastHit hitinfo, 10f, LayerMask.GetMask("Character")))
                {
                    Debug.Log("navimesh取得できたがplayerがいる方向");
                    //yield return null
                }
                else
                {
                    ai.destination = hit.position;
                    yield return new WaitForSeconds(Random.Range(minRunawayTime, maxRunawayTime) / 3);
                }

            }
            else
            {
                Debug.Log("navimesh取得できず");
                //yield return null;
            }

        }
        else
        {
            yield return new WaitForSeconds(Random.Range(minRunawayTime, maxRunawayTime));

        }



        


        isRunaway = false;


    }
}
