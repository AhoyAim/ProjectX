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

    public LayerMask layerMask;

    public Transform playerTransform;
    Transform girlTransform;
    public float awakeDistance;
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
        Debug.Log(ai.pathStatus);
        if (!girlController.isNaked)
        {
            ai.stoppingDistance = 0f;
            if (girlController.state == GirlController.STATE.ALERT)
            {
                ai.speed = runSpeed;


                Debug.Log(ai.pathStatus);

                Vector3 girlPosition = girlTransform.position;

                toPlayerDirection = (playerTransform.position - girlPosition).normalized;
                if (NavMesh.SamplePosition(-2f * toPlayerDirection + girlPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    ai.destination = hit.position;

                    Vector3 toDestinationDirection = (hit.position - girlPosition).normalized;

                    Debug.DrawRay(girlPosition, toDestinationDirection, Color.red, 0.5f);


                    if (Physics.SphereCast(girlPosition, girlCollider.radius, toDestinationDirection, out RaycastHit hitinfo, 0.5f, LayerMask.GetMask("Girl")))
                    {
                        ai.speed = 0;
                        Debug.Log("SphereCast‚ªhit");
                        ai.destination = girlPosition;
                    }


                }
                else
                {

                    girlTransform.LookAt(playerTransform);
                }


            }
            if (girlController.state == GirlController.STATE.Normal)
            {
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

        animator.SetFloat(animIDSpeed, ai.velocity.magnitude);
    }

   
    Vector3 GetSamplePointNavMesh()
    {
        NavMeshTriangulation samplePointNavMeshTriangulation = NavMesh.CalculateTriangulation();
        int index = Random.Range(0, samplePointNavMeshTriangulation.indices.Length);
        return samplePointNavMeshTriangulation.vertices[index];
    }
}
