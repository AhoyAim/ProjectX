using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlController_ : MonoBehaviour
{
    public Transform playerTransform;
    public Transform sphereCastRoot;
    public PantsGetter pantsGetter;
    public enum State
    {
        Normal,
        Notice,
        Runaway,
        Approch,
        Attack,
        Damaged,
        Stan,
        Vacuumed,
    }
    public State currentState;
    public bool isNaked = false;

    [Space(10)]
    public float noticeDistance = 10;
    public float atttackableDistance = 2f;
    [Range(0.1f, 2f)]
    public float minRunawayTime = 0.1f;
    [Range(2.1f, 5f)]
    public float maxRunawayTime = 2.1f;
    public float runSpeed;
    public float walkSpeed;

    private CapsuleCollider girlCollider;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int animIDSpeed;
    private float vaccumedableDistance;
    private float vaccumedableAngle;
    private bool isRunaway;
    private bool noticeable;
    private bool vacuumedable;
    private bool attackable;
    private Transform girlTransform;
    private Vector3 toPlayerDirection;
    private Rigidbody rb;

    private void Start()
    {
        currentState = State.Normal;

        girlCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animIDSpeed = Animator.StringToHash("Speed");
        vaccumedableDistance = pantsGetter.vaccumableDistance;
        vaccumedableAngle = pantsGetter.vaccumableAngle;
        girlTransform = transform;
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        UpDateFlags();
        bool vaccumedNow = vacuumedable && pantsGetter.vccuming;
       

        switch (currentState)
        {
            case State.Normal:
                // 次のステートに遷移できないかチェック
                if(noticeable)
                {
                    currentState = State.Notice;
                }



                // 最終的にステートの遷移がなければ本来の処理を実行
                if(currentState == State.Normal)
                {
                    OnNormal();
                }
                break;


            case State.Notice:
                // 次のステートに遷移できないかチェック
                if(isNaked)
                {
                    currentState = State.Approch;
                }
                else
                {
                    currentState = State.Runaway;
                }


                // 最終的にステートの遷移がなければ本来の処理を実行
                if (currentState == State.Notice)
                {
                    Debug.Log("Noticeしたよ！");
                }
                break;


            case State.Runaway:
                // 次のステートに遷移できないかチェック
                if(!isRunaway && !noticeable)
                {
                    currentState = State.Normal;
                }

                // 最終的にステートの遷移がなければ本来の処理を実行
                if(currentState == State.Runaway)
                {
                    OnRunaway();
                }
                break;


            case State.Approch:
                // 次のステートに遷移できないかチェック
                if(!noticeable)
                {
                    Vector3 toPlayerDirection = playerTransform.position - girlTransform.position;
                    if(Physics.SphereCast(sphereCastRoot.position, girlCollider.radius, toPlayerDirection, out RaycastHit hitinfo,20f))
                    {
                        // noticeDistance以上にplayerと離れてはいるが、直接視認できているとき
                        if (hitinfo.collider.CompareTag("Player"))
                        {
                            currentState = State.Approch;
                        }
                        // noticeDistance以上にplayerと離れていて、直接視認できないとき
                        else
                        {
                            currentState = State.Normal;
                        }
                    }
                    else
                    {
                        currentState = State.Normal;
                    }

                }
                if(attackable)
                {
                    currentState = State.Attack;
                }


                // 最終的にステートの遷移がなければ本来の処理を実行
                if (currentState == State.Approch)
                {
                    OnApproch();
                }
                break;

            case State.Attack:
                // 次のステートに遷移できないかチェック
                if (!attackable)
                {
                    currentState = State.Approch;
                }

                // 最終的にステートの遷移がなければ本来の処理を実行
                if (currentState == State.Attack)
                {
                    Debug.Log("Attackしたよ");
                }
                break;

            case State.Vacuumed:
                Debug.Log("Vaccumedされてます");
                if(navMeshAgent.enabled)
                {
                    navMeshAgent.ResetPath();
                }
                
                girlTransform.parent = playerTransform;

                if(!pantsGetter.vccuming)
                {
                    girlTransform.parent = null;
                    navMeshAgent.enabled = false;
                    // rb.isKinematic = false;
                    rb.AddForce(playerTransform.forward * 1000, ForceMode.Impulse);
                    currentState = State.Stan;
                }
                break;
               


            default:
                break;
        }

        if (isRunaway)
        {
            animator.SetFloat(animIDSpeed, runSpeed);
        }
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude);
    }

    /// <summary>
    /// playerとgirlの距離によりnoticeableフラグを更新する
    /// </summary>
    void CheckNoticeable()
    {
        if ((playerTransform.position - girlTransform.position).sqrMagnitude < noticeDistance * noticeDistance)
        {
            noticeable = true;
        }
        else
        {
            noticeable = false;
        }
    }
    /// <summary>
    /// playerとgirlの距離によりnoticeableフラグを更新する
    /// </summary>
    void CheckAttackable()
    {
        if ((playerTransform.position - girlTransform.position).sqrMagnitude < atttackableDistance * atttackableDistance)
        {
            attackable = true;
        }
        else
        {
            attackable = false;
        }
    }
    /// <summary>
    /// playerとgirlの距離とアングルによりvacuumedableフラグを更新する
    /// </summary>
    void CheckVacuumedable()
    {
        Vector3 toGirlVector = girlTransform.position - playerTransform.position;
        if (toGirlVector.sqrMagnitude < vaccumedableDistance * vaccumedableDistance 
            && Vector3.Angle(playerTransform.forward, toGirlVector) <= vaccumedableAngle / 2)
        {
            vacuumedable = true;
            Player_GirlManager.instance.vaccumedableGirlControllers.Add(this);
        }
        else
        {
            vacuumedable = false;
            Player_GirlManager.instance.vaccumedableGirlControllers.Remove(this);
        }
    }
    /// <summary>
    /// 各種フラグを更新する
    /// </summary>
    void UpDateFlags()
    {
        CheckNoticeable();
        CheckVacuumedable();
        CheckAttackable();

    }


    #region NavMeshAgent関係のメソッド群
    /// <summary>
    /// 現在のNavMesh上のランダムな座標を返す
    /// </summary>
    /// <returns>NavMesh上のランダムな座標</returns>
    Vector3 GetSamplePointNavMesh()
    {
        NavMeshTriangulation samplePointNavMeshTriangulation = NavMesh.CalculateTriangulation();
        int index = Random.Range(0, samplePointNavMeshTriangulation.areas.Length);
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
            if (isGetPosition)
            {
                navMeshAgent.destination = hit.position;
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
                if (Physics.SphereCast(sphereCastRoot.position, girlCollider.radius * 6, tempDestination - girlPosition, out RaycastHit hitinfo, 10f, LayerMask.GetMask("Character")))
                {
                    Debug.Log("navimesh取得できたがplayerがいる方向");
                    //yield return null
                }
                else
                {
                    navMeshAgent.destination = hit.position;
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

    /// <summary>
    /// Runawayステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnRunaway()
    {
        navMeshAgent.speed = runSpeed;
        if (!isRunaway)
        {
            StartCoroutine(Runaway());
        }

    }
    /// <summary>
    /// Approchステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnApproch()
    {
        navMeshAgent.speed = runSpeed;
        navMeshAgent.stoppingDistance = 1.5f;
        navMeshAgent.destination = playerTransform.position;
    }

    /// <summary>
    /// Normalステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnNormal()
    {
        if (isRunaway)
        {
            return;
        }
        navMeshAgent.speed = walkSpeed;
        if ((Random.Range(0, 5000) < 20))
        {
            navMeshAgent.destination = GetSamplePointNavMesh();
        }
    }
    #endregion
}
