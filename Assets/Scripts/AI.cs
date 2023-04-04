using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    GirlController girlController;
    NavMeshAgent ai;
    Animator animator;
    int animIDSpeed;

    public LayerMask layerMask;

    public Transform playerTransform;
    Transform girlTransform;
    public float awakeDistance;
    public float runSpeed;
    public float walkSpeed;

    Vector3 toPlayerDirection;
    
    void Start()
    {
        girlController= GetComponent<GirlController>();
        ai= GetComponent<NavMeshAgent>();
        girlTransform = transform;
        animator = GetComponent<Animator>();
        animIDSpeed = Animator.StringToHash("Speed");
    }

    
    void Update()
    {
        if(!girlController.isNaked)
        {
            ai.stoppingDistance = 0f;
            if(girlController.state == GirlController.STATE.ALERT)
            {
                ai.speed = runSpeed;
               

                Debug.Log(ai.pathStatus);


                
                toPlayerDirection = (playerTransform.position - girlTransform.position).normalized;
                if (NavMesh.SamplePosition(-2f * toPlayerDirection + girlTransform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    ai.destination = hit.position;
                   // Physics.Raycast()

                }
                else
                {
                    Debug.Log("<color red>Œo˜H‚ªhit‚µ‚Ä‚È‚¢‚æ</color>");
                    girlTransform.LookAt(playerTransform);
                }


            }
            if(girlController.state == GirlController.STATE.Normal)
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
        

        if(girlController.isNaked)
        {
            ai.speed = runSpeed;
            ai.stoppingDistance = 2f;
            ai.destination = playerTransform.position;
        }

        animator.SetFloat(animIDSpeed, ai.velocity.magnitude);
    }
  
}
