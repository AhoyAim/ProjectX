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
        HyperVacuumed,
        BlownAway,
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
        CheckVaccumedAndDamaged();



        switch (currentState)
        {
            case State.Normal:
                OnNormal();
                break;

            case State.Notice:
                OnNotice();
                break;

            case State.Runaway:
                OnRunaway();
                break;

            case State.Approch:
                OnApproch();
                break;

            case State.Attack:
                OnAttack();
                break;

            case State.Vacuumed:
                OnVacuumed();
                break;

            case State.HyperVacuumed:
                if((!pantsGetter.vacuuming))
                {
                    currentState = State.BlownAway;
                    this.tag = "吹き飛ばされているやつ用のタグ";
                }
               

                if(currentState == State.BlownAway)
                {
                    navMeshAgent.enabled = false;
                    rb.isKinematic = false;
                    girlCollider.isTrigger = false;
                }
                break;

            case State.BlownAway:

                // OnBlownAway() girlタグかEnvironmentタグに当たると停止する。
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
    bool CheckNoticeable()
    {
        if ((playerTransform.position - girlTransform.position).sqrMagnitude < noticeDistance * noticeDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// playerとgirlの距離によりnoticeableフラグを更新する
    /// </summary>
    bool CheckAttackable()
    {
        if ((playerTransform.position - girlTransform.position).sqrMagnitude < atttackableDistance * atttackableDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// playerとgirlの距離とアングルによりvacuumedableフラグを更新する
    /// </summary>
    bool CheckVacuumedable()
    {
        Vector3 toGirlVector = girlTransform.position - playerTransform.position;
        if (toGirlVector.sqrMagnitude < vaccumedableDistance * vaccumedableDistance 
            && Vector3.Angle(playerTransform.forward, toGirlVector) <= vaccumedableAngle / 2)
        {
            Player_GirlManager.instance.vaccumedableGirlControllers.Add(this);
            return true;
        }
        else
        {
            Player_GirlManager.instance.vaccumedableGirlControllers.Remove(this);
            return false;
        }
    }
    /// <summary>
    /// 現在吸引されているかどうかチェックする
    /// </summary>
    bool CheckVaccumedNow()
    {
        return CheckVacuumedable() && pantsGetter.vacuuming;
    }
   
  
    /// <summary>
    /// Anystateから遷移できるVacuumステートとDamagedステートへのフラグをチェックし可能なら遷移する
    /// </summary>
    public void CheckVaccumedAndDamaged()
    {
        if(currentState == State.Stan)
        {
            return;
        }

        if(CheckVaccumedNow())
        {
            currentState = State.Vacuumed;
        }
        
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
    /// isRunawayフラグをtrueにし、agentを動かすコルーチン。コルーチンの終わりで isRunawayフラグをfalseにする。if(isRunaway)の時だけ StartCoroutineし、コルーチンが重複しないようしてください。また、最後に次のステートに遷移できないかチェックも行っています。
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

        // 次のステートに遷移できないかチェック
        if(!CheckNoticeable())
        {
            currentState = State.Normal;
        }

    }
    #endregion

    /// <summary>
    /// VcuumedステートやVacuumReleaseステートに
    /// </summary>
    public void PrepareVacuumed()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
            girlCollider.isTrigger = false;
        }
    }
    /// <summary>
    /// Normalステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnNormal()
    {
        navMeshAgent.speed = walkSpeed;
        if ((Random.Range(0, 5000) < 20))
        {
            navMeshAgent.destination = GetSamplePointNavMesh();
        }
        // 次のステートに遷移できないかチェック
        if (CheckNoticeable())
        {
            currentState = State.Notice;
        }
    }

    /// <summary>
    /// Noticeステート時の立ち振る舞い
    /// </summary>
    void OnNotice()
    {
        Debug.Log("Noticeだよーん");// ここにリアクションのアニメーションを実装する

        // 次のステートに遷移できないかチェック
        if (isNaked)
        {
            currentState = State.Approch;
        }
        else
        {
            currentState = State.Runaway;
        }

    }

    /// <summary>
    /// Runawayステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnRunaway()
    {
        navMeshAgent.speed = runSpeed;
        if (!isRunaway)
        {
            StartCoroutine(Runaway());　// Runaway()内に次のステートに遷移できないかチェックを実装してます

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

        // 次のステートに遷移できないかチェック
        if (CheckAttackable())
        {
            currentState = State.Attack;
        }
        else if(!CheckNoticeable())
        {
            Vector3 toPlayerDirection = playerTransform.position - girlTransform.position;
            if (Physics.SphereCast(sphereCastRoot.position, girlCollider.radius, toPlayerDirection, out RaycastHit hitinfo, 20f))
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
    }
    /// <summary>
    ///  Attackステート時の立ち振る舞い
    /// </summary>
    void OnAttack()
    {
        Debug.Log("Attackだよーん");// ここにAttackアニメーションに関する実装をするアニメーションイベントでコライダーをOnし、接触判定する

        // 次のステートに遷移できないかチェック
        if (!CheckAttackable())
        {
            currentState = State.Approch;
        }
    }

    /// <summary>
    /// Vacuumedステート時の立ち振る舞い
    /// </summary>
    void OnVacuumed()
    {
        Debug.Log("Vaccumedされてます"); //Vacuumedアニメを再生する
        PrepareVacuumed();

        if (pantsGetter.hyperVacuuming)
        {
            currentState = State.HyperVacuumed;
        }
        else if (!pantsGetter.vacuuming)
        {
            currentState = State.Stan;
        }
    }

    void OnHyperVacuumed()
    {
        Debug.Log("HyperVaccumedされてます"); //HyperVacuumedアニメを再生する
        PrepareVacuumed();

        girlTransform.parent = playerTransform;

        if(pantsGetter.vacuumReieasing)
        {
            currentState = State.BlownAway;
        }
    }

    void OnBlownAway()
    {
        this.tag = "BlownAway";

        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(currentState == State.BlownAway)
        {
            if(collision.gameObject.CompareTag("Girl") || collision.gameObject.CompareTag("Environment"))
            {
                rb.velocity = Vector3.zero;
                collision.gameObject.GetComponent<GirlController_>().currentState = State.Damaged;
            }
        }
    }




}
