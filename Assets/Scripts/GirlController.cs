

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class GirlController : MonoBehaviour, IDamageable
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
        ApprochToFreePants,
        Attack,
        GetFreePants,
        Damaged,
        Stan,
        Vacuumed,
        HyperVacuumed,
        HyperVacuumedCancel,
        BlownAway,
        KnockBack,
    }
    public State currentState;
    public bool isNaked = false;

    [Space(10)]
    public float noticeDistance = 10;
    public float atttackableDistance = 2f;
    public float approchDistance = 1f;
    [Range(0.1f, 2f)]
    public float minRunawayTime = 0.1f;
    [Range(2.1f, 5f)]
    public float maxRunawayTime = 2.1f;
    public float runSpeed;
    public float walkSpeed;
    public float stanTime = 2f;
    [Space(10)]
    public Transform bodyTransform;
    [Space(10)]
    public FreePants lockOn_freePants_obj;
    public Transform lockOn_freePants_transform;
    public bool lockOn_freePants_flag = false;// trueのときフリーパンツを追える状態。



    private const RigidbodyConstraints a = RigidbodyConstraints.None;
    private CapsuleCollider girlCollider;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int animIDSpeed;
    private int animIDIsNaked;
    private int animIDNavmeshAgent;
    private int animIDVacuumed;
    private int animIDHyperVcuuned;
    private int animIDHyperVcuunedCancel;
    private int animIDBlowAway;
    private int animIDDamaged;
    private int animIDAttack;
    private int animIDGetFreePants;
    private int animIDNotice;
    private int animIDStan;
    private float girlColliderRdius;
    private float vaccumedableDistance;
    private float vaccumedableAngle;
    private bool isRunaway;
    private Transform girlTransform;
    private Vector3 toPlayerDirection;
    private Vector3 blowAwayDirection;
    private Rigidbody rb;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Color originTopsColor;

    DamagedChecker damagedChecker;

    private void Start()
    {
        currentState = State.Normal;

        girlCollider = GetComponent<CapsuleCollider>();
        girlColliderRdius = girlCollider.radius;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animIDSpeed = Animator.StringToHash("Speed");
        animIDIsNaked = Animator.StringToHash("IsNaked");
        animIDNavmeshAgent = Animator.StringToHash("NavMeshAgent");
        animIDVacuumed = Animator.StringToHash("Vacuumed");
        animIDHyperVcuuned = Animator.StringToHash("HyperVacuumed");
        animIDHyperVcuunedCancel = Animator.StringToHash("HyperVacuumedCancel");
        animIDBlowAway = Animator.StringToHash("BlowAway");
        animIDDamaged = Animator.StringToHash("Damaged");
        animIDAttack = Animator.StringToHash("Attack");
        animIDGetFreePants = Animator.StringToHash("GetFreePants");
        animIDNotice = Animator.StringToHash("Notice");
        animIDStan = Animator.StringToHash("Stan");
        vaccumedableDistance = pantsGetter.vaccumableDistance;
        vaccumedableAngle = pantsGetter.vacuumableAngle;
        girlTransform = transform;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        skinnedMeshRenderer = bodyTransform.GetComponent<SkinnedMeshRenderer>();
        originTopsColor = skinnedMeshRenderer.materials[2].color;

    }

    private void Update()
    {


        //AnimePlay();

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

            case State.ApprochToFreePants:
                OnApprochToFreePants();
                break;

            case State.Attack:
                OnAttack();
                break;

            case State.GetFreePants:
                OnGetFreePants();
                break;

            case State.Vacuumed:
                OnVacuumed();
                break;

            case State.HyperVacuumed:
                OnHyperVacuumed();
                break;

            case State.HyperVacuumedCancel:
                OnHyperVacuumedCancel();
                break;

            case State.BlownAway:
                OnBlownAway();
                break;

            case State.Damaged:
                OnDamaged();
                break;

            case State.KnockBack:
                OnKnockBack();
                break;
            //case State.Stan:
            //    OnStan();
            //break;
            default:
                break;
        }

        CheckVaccumed();


    }

    /// <summary>
    /// currentStateを引数のstateへ切り替え、アニメーターにも対応したパラメーターを与える
    /// </summary>
    /// <param name="state">ここに入力したステートへ切り替わる</param>
    public void ChangeState(State state)
    {
        currentState = state;

        switch (currentState)
        {
            case State.Normal:
                animator.SetBool(animIDNavmeshAgent, true);
                TeardownVacuumed();
                break;
            case State.Notice:
                animator.SetBool(animIDNotice, true);
                break;
            case State.Runaway:
                animator.SetBool(animIDNavmeshAgent, true);
                break;
            case State.Approch:
                animator.SetBool(animIDNavmeshAgent, true);
                break;
            case State.ApprochToFreePants:
                animator.SetBool(animIDNavmeshAgent, true);
                break;
            case State.Attack:
                animator.SetBool(animIDAttack, true);
                break;
            case State.GetFreePants:
                animator.SetBool(animIDGetFreePants, true);
                break;
            case State.Damaged:
                animator.SetBool(animIDDamaged, true);
                break;
            //case State.Stan:
            //    break;
            case State.Vacuumed:
                animator.SetBool(animIDVacuumed, true);
                break;
            case State.HyperVacuumed:
                animator.SetBool(animIDHyperVcuuned, true);
                break;
            case State.HyperVacuumedCancel:
                animator.SetBool(animIDHyperVcuunedCancel, true);
                break;
            case State.BlownAway:
                animator.SetBool(animIDBlowAway, true);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// アニメーション再生進捗が次ステートへの遷移可能なところまで進んでいるかチェックする。
    /// </summary>
    /// <param name="animeStateName"></param>
    /// <returns>遷移可能ならture</returns>
    bool IsChangeableAnimeState(string animeStateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animeStateName)
                && animator.GetCurrentAnimatorStateInfo(0).loop)
        {
            return true;
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animeStateName)
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                return true;
            }
        }
        return false;

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
    /// Freepansとgirlの距離により入手可能か判断する。他のGirlが先にFreePantsをゲットしてしまったときlockOn_freePants_flag = falseする
    /// </summary>
    /// <returns></returns>
    bool CheckFreePantsGettable()
    {
        if(lockOn_freePants_obj == null)// 他のGirlが先にFreePantsに到達し、Destroy()したとき
        {
            lockOn_freePants_flag = false;
            return false;
        }
        else if(lockOn_freePants_obj.isDestroy)// 他のGirlが先にFreePantsに到達し、Destroy()したとき
        {
            lockOn_freePants_flag = false;
            return false;
        }
        else if ((lockOn_freePants_transform.position - girlTransform.position).sqrMagnitude < atttackableDistance * atttackableDistance)
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
    /// Anystateから遷移できるVacuumステートへのフラグをチェックし可能なら遷移する
    /// </summary>
    public void CheckVaccumed()
    {
        if (/*currentState == State.Damaged ||*/ currentState == State.Stan || currentState == State.Vacuumed || currentState == State.HyperVacuumed || currentState == State.BlownAway)
        {
            return;
        }

        if (CheckVaccumedNow())
        {
            if (currentState != State.Damaged)
            {
                ChangeState(State.Vacuumed);
            }
        }

    }

    public void UpdatePants()
    {
        if (isNaked)
        {
            Material[] newMaterials = skinnedMeshRenderer.materials;
            newMaterials[2].color = Color.clear;
            skinnedMeshRenderer.materials = newMaterials;
        }
        else
        {
            Material[] newMaterials = skinnedMeshRenderer.materials;
            newMaterials[2].color = originTopsColor;
            skinnedMeshRenderer.materials = newMaterials;
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
                if (Physics.SphereCast(sphereCastRoot.position, girlColliderRdius * 6, tempDestination - girlPosition, out RaycastHit hitinfo, 10f, LayerMask.GetMask("Character")))
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
        if (!CheckNoticeable())
        {
            ChangeState(State.Normal);
        }

    }
    #endregion

    /// <summary>
    /// NavmeshAgentをfalseにしてColliderのisKinematicをfalseにするなどしてNavmeshAgent主導で動かすのを物理演算主導に切り替える
    /// </summary>
    public void SetupVacuumed()
    {
        if (navMeshAgent.enabled)
        {
            if (navMeshAgent.hasPath)
            {
                navMeshAgent.ResetPath();
            }

            navMeshAgent.enabled = false;
            girlCollider.enabled = true;
            rb.isKinematic = false;
        }
    }
    /// <summary>
    /// NavmeshAgentをtrueにしてColliderのisKinematicをtrueにするなどして物理演算主導で動かすのをNavmeshAgent主導に切り替える
    /// </summary>
    public void TeardownVacuumed()
    {
        if (!navMeshAgent.enabled)
        {
            rb.isKinematic = true;
            if (navMeshAgent.hasPath)
            {
                navMeshAgent.ResetPath();
            }
            navMeshAgent.enabled = true;
        }
    }
    /// <summary>
    /// Normalステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnNormal()
    {
        TeardownVacuumed();
        if (!animator.GetBool(animIDNavmeshAgent))
        {
            animator.SetBool(animIDNavmeshAgent, true);
        }
        navMeshAgent.speed = walkSpeed;
        animator.SetFloat(animIDIsNaked, isNaked ? 1:0);
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude, 0.25f, Time.deltaTime);
        if ((Random.Range(0, 5000) < 20))
        {
            navMeshAgent.destination = GetSamplePointNavMesh();
        }
        // 次のステートに遷移できないかチェック
        if(lockOn_freePants_flag)
        {
            ChangeState(State.ApprochToFreePants);
        }
        else if (CheckNoticeable())
        {
            girlTransform.LookAt(playerTransform.position);
            ChangeState(State.Notice);
        }
    }

    /// <summary>
    /// Noticeステート時の立ち振る舞い
    /// </summary>
    void OnNotice()
    {
        TeardownVacuumed();
        if (navMeshAgent.enabled)
        {
            if (navMeshAgent.hasPath)
            {
                navMeshAgent.ResetPath();
            }
        }
        if (!animator.GetBool(animIDNotice))
        {
            girlTransform.LookAt(playerTransform.position);
            animator.SetBool(animIDNotice, true);
        }
        if (IsChangeableAnimeState("Notice"))
        {
            if (isNaked)
            {
                ChangeState(State.Approch);
            }
            else
            {
                ChangeState(State.Runaway);
            }
        }


    }

    /// <summary>
    /// Runawayステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnRunaway()
    {
        TeardownVacuumed();
        if (!animator.GetBool(animIDNavmeshAgent))
        {
            animator.SetBool(animIDNavmeshAgent, true);
        }
        navMeshAgent.speed = runSpeed;
        animator.SetFloat(animIDIsNaked, 0);
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude, 0.25f, Time.deltaTime);
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
        TeardownVacuumed();
        if (!animator.GetBool(animIDNavmeshAgent))
        {
            animator.SetBool(animIDNavmeshAgent, true);
        }
        navMeshAgent.speed = runSpeed;
        animator.SetFloat(animIDIsNaked, 1);
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude, 0.25f, Time.deltaTime);
        navMeshAgent.stoppingDistance = approchDistance;
        navMeshAgent.destination = playerTransform.position;

        // 次のステートに遷移できないかチェック
        if (lockOn_freePants_flag)
        {
            ChangeState(State.ApprochToFreePants);
        }
        else if (CheckAttackable())
        {
            ChangeState(State.Attack);
        }
        else if (!CheckNoticeable())
        {
            Vector3 toPlayerDirection = playerTransform.position - girlTransform.position;
            if (Physics.SphereCast(sphereCastRoot.position, girlCollider.radius, toPlayerDirection, out RaycastHit hitinfo, 20f))
            {
                // noticeDistance以上にplayerと離れてはいるが、直接視認できているとき
                if (hitinfo.collider.CompareTag("Player"))
                {
                    ChangeState(State.Approch);
                }
                // noticeDistance以上にplayerと離れていて、直接視認できないとき
                else
                {
                    ChangeState(State.Normal);
                }
            }
            else
            {
                ChangeState(State.Normal);
            }
        }
    }
    /// <summary>
    /// ApprochToFreePantsステート時のNavMeshAgentの立ち振る舞い
    /// </summary>
    public void OnApprochToFreePants()
    {
        TeardownVacuumed();
        if (!animator.GetBool(animIDNavmeshAgent))
        {
            animator.SetBool(animIDNavmeshAgent, true);
        }
        navMeshAgent.speed = runSpeed;
        animator.SetFloat(animIDIsNaked, 1);
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude, 0.25f, Time.deltaTime);
        navMeshAgent.stoppingDistance = approchDistance;
        if(lockOn_freePants_obj != null && !lockOn_freePants_obj.isDestroy)
        {
            navMeshAgent.destination = lockOn_freePants_transform.position;
        }
        

        // 次のステートに遷移できないかチェック
        if (CheckFreePantsGettable())
        {
            ChangeState(State.GetFreePants);
        }
        else
        {
            if(!lockOn_freePants_flag)
            {
                ChangeState(State.Approch);
            }
        }
        
    }
    /// <summary>
    ///  Attackステート時の立ち振る舞い
    /// </summary>
    void OnAttack()
    {
        //Debug.Log("Attackだよーん");// ここにAttackアニメーションに関する実装をするアニメーションイベントでコライダーをOnし、接触判定する
        if (!animator.GetBool(animIDAttack))
        {
            animator.SetBool(animIDAttack, true);
        }
        toPlayerDirection = playerTransform.position - girlTransform.position;
        toPlayerDirection.y = 0;
        girlTransform.rotation = Quaternion.LookRotation(toPlayerDirection);
        //girlTransform.LookAt(playerTransform.position);


        // 次のステートに遷移できないかチェック
        if (IsChangeableAnimeState("Attack"))
        {
            ChangeState(State.Approch);
        }
    }
    void OnGetFreePants()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
        {
            currentState = State.Normal;
        }
    }
    /// <summary>
    /// Vacuumedステート時の立ち振る舞い
    /// </summary>
    void OnVacuumed()
    {
        if (!animator.GetBool(animIDVacuumed))
        {
            animator.SetBool(animIDVacuumed, true);
        }
        //Debug.Log("Vaccumedされてます"); //Vacuumedアニメを再生する
        girlTransform.LookAt(2 * girlTransform.position - playerTransform.position);

        if (navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = true;
        }
        //SetupVacuumed();

        // 次のステートに遷移できないかチェック
        if (pantsGetter.hyperVacuuming)
        {
            navMeshAgent.isStopped = false;
            ChangeState(State.HyperVacuumed);
            return;
        }

        if (!IsChangeableAnimeState("Vacuumed"))
        {
            return;
        }
        else if (!pantsGetter.vacuuming)
        {
            navMeshAgent.isStopped = false;
            //TeardownVacuumed();
            ChangeState(State.Approch);
        }
    }
    /// <summary>
    /// HyperVacuumedステート時の立ち振る舞い
    /// </summary>
    void OnHyperVacuumed()
    {
        if (!animator.GetBool(animIDHyperVcuuned))
        {
            animator.SetBool(animIDHyperVcuuned, true);
        }
        //Debug.Log("HyperVaccumedされてます"); //HyperVacuumedアニメを再生する

        SetupVacuumed();
        Vector3 diff = playerTransform.TransformPoint(Vector3.forward * 2f) - girlTransform.position;
        rb.velocity = diff * 10;
        //girlTransform.SetParent(playerTransform);
        girlTransform.LookAt(10 * playerTransform.forward + playerTransform.position);

        // 次のステートに遷移できないかチェック
        if (pantsGetter.vacuumingFails)
        {
            //バキュームドキャンセルステートヘ
            ChangeState(State.HyperVacuumedCancel);
            rb.velocity = Vector3.zero;
        }
        else if (pantsGetter.vacuumReleasing)
        {
            ChangeState(State.BlownAway);
            //BlowAway(playerTransform.forward*20);
            Debug.Log(this.name);
            //Debug.Break();
        }
    }

    void OnBlownAway()
    {
        if (!animator.GetBool(animIDBlowAway))
        {
            animator.SetBool(animIDBlowAway, true);
        }

        Player_GirlManager.instance.vaccumedableGirlControllers.Remove(this);
        rb.velocity = blowAwayDirection;

        //　pantsGetterがBlowAway(Vector3 direction)を呼ぶ。
        //　OnCollisionEnterでState.Damagedに遷移
    }
    public void BlowAway(Vector3 direction)
    {
        this.tag = "BlownAway";
        girlTransform.position = girlTransform.position + girlTransform.up * 0.2f;
        rb.velocity = direction;
        blowAwayDirection = direction;
    }
    /// <summary>
    /// currentState == State.BlownAwayの時、Girl・Environmentタグと衝突したときcurrentState = State.Damagedにする
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (currentState == State.BlownAway)
        {
            if (collision.gameObject.CompareTag("Girl") || collision.gameObject.CompareTag("Environment"))
            {

                rb.velocity = Vector3.zero;
                this.tag = "Girl";
                ChangeState(State.Damaged);
                if (collision.gameObject.CompareTag("Girl"))
                {
                    var collisionGirl = collision.gameObject.GetComponent<GirlController>();
                    collisionGirl.ChangeState(State.Damaged);
                    if (collisionGirl.navMeshAgent.enabled)
                    {
                        if (collisionGirl.navMeshAgent.hasPath)
                        {
                            collisionGirl.navMeshAgent.ResetPath();
                        }
                    }
                }
            }
        }
    }
    void OnDamaged()
    {
        if (!animator.GetBool(animIDDamaged))
        {
            animator.SetBool(animIDDamaged, true);
        }
        // Debug.Log("Damagedだよーん");
        girlCollider.enabled = false;
        TeardownVacuumed();
        if (!IsChangeableAnimeState("Stan"))
        {
            return;
        }
        //TeardownVacuumed();
        ChangeState(State.Normal);
        girlCollider.enabled = true;

    }

    void OnKnockBack()
    {
        //Damageメソッドにより物理挙動中。特にすることなし。
    }
    void OnStan()
    {
        TeardownVacuumed();

        if (!IsChangeableAnimeState("Stan"))
        {
            return;
        }
        ChangeState(State.Normal);
        animator.SetTrigger(animIDNavmeshAgent);
    }

    void OnHyperVacuumedCancel()
    {
        TeardownVacuumed();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
        {
            currentState = State.Normal;
        }
    }




    private IEnumerator DelayCoroutine(System.Action action)
    {
        yield return new WaitForSeconds(stanTime);
        action?.Invoke();
    }

    public bool DamageJudge()
    {
        return currentState != State.Damaged && currentState != State.KnockBack;
    }

    IEnumerator DamageCoroutine(Vector3 direction)
    {
        girlTransform.LookAt(transform.position - direction);
        SetupVacuumed();
        ChangeState(State.KnockBack);
        gameObject.layer = LayerMask.NameToLayer("IgnoreGirl");
        animator.SetBool(animIDDamaged, true);
        rb.velocity = direction * 10;
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("Girl");
        Debug.Log("Girl吹き飛び終わりました");
        ChangeState(State.Damaged);
    }

    public void DamageBehaviour(Vector3 direction)
    {
        StartCoroutine(DamageCoroutine(direction));
    }

    public void Damage(Vector3 direction)
    {
        if (DamageJudge())
        {
            DamageBehaviour(direction);
        }
    }

    public void GetFreePants()
    {
        if(lockOn_freePants_obj == null)
        {
            return;
        }
        if(!lockOn_freePants_obj.isDestroy)
        {
            lockOn_freePants_flag = false;
            lockOn_freePants_obj.isDestroy = true;
            Destroy(lockOn_freePants_obj.gameObject);
            animator.SetFloat(animIDIsNaked, 0);

            isNaked = false;
            UpdatePants();
        }
        
    }
}
