

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlController_ : MonoBehaviour
{
    public MeshRenderer mosaic;
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
    public float stanTime = 2f;

    private const RigidbodyConstraints a = RigidbodyConstraints.None;
    private CapsuleCollider girlCollider;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int animIDSpeed;
    private int animIDNavmeshAgent;
    private int animIDVacuumed;
    private int animIDHyperVcuuned;
    private int animIDBlowAway;
    private int animIDDamaged;
    private int animIDAttack;
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

    DamagedChecker damagedChecker;

    private void Start()
    {
        currentState = State.Normal;

        girlCollider = GetComponent<CapsuleCollider>();
        girlColliderRdius = girlCollider.radius;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animIDSpeed = Animator.StringToHash("Speed");
        animIDNavmeshAgent = Animator.StringToHash("NavMeshAgent");
        animIDVacuumed = Animator.StringToHash("Vacuumed");
        animIDHyperVcuuned = Animator.StringToHash("HyperVacuumed");
        animIDBlowAway = Animator.StringToHash("BlowAway");
        animIDDamaged = Animator.StringToHash("Damaged");
        animIDAttack = Animator.StringToHash("Attack");
        animIDNotice = Animator.StringToHash("Notice");
        animIDStan = Animator.StringToHash("Stan");
        vaccumedableDistance = pantsGetter.vaccumableDistance;
        vaccumedableAngle = pantsGetter.vacuumableAngle;
        girlTransform = transform;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

       
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

            case State.Attack:
                OnAttack();
                break;

            case State.Vacuumed:
                OnVacuumed();
                break;

            case State.HyperVacuumed:            
                OnHyperVacuumed();
                break;

            case State.BlownAway:
                OnBlownAway();
                break;

            case State.Damaged:
                OnDamaged();
                break;

            case State.Stan:
                OnStan();
                break;
            default:
                break;
        }

        CheckVaccumedAndDamaged();


    }
    /// <summary>
    /// ���݂̃X�e�[�g�ɉ������g���K�[��set���A�A�j���[�V�������Đ�����B
    /// </summary>
    //void AnimePlay()
    //{
    //    switch (currentState)
    //    {
            
    //        case State.Notice:
    //            break;
    //        case State.Attack:
    //            break;
    //        case State.Damaged:
    //            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
    //            {
    //                animator.SetTrigger(animIDDamaged);
    //                Debug.Break();
    //            }
    //            break;
    //        case State.Stan:
    //            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Stan"))
    //            {
    //                animator.SetTrigger(animIDStan);
    //            }
    //            break;
    //        case State.Vacuumed:
    //            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Vacuumed"))
    //            {
    //                animator.SetTrigger(animIDVacuumed);
    //            }
    //            break;
    //        case State.HyperVacuumed:
    //            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("HyperVacuumed"))
    //            {
    //                animator.SetTrigger(animIDHyperVcuuned);
    //            }
    //            break;
    //        case State.BlownAway:
    //            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("BlownAway"))
    //            {
    //                animator.SetTrigger(animIDBlowAway);
    //            }
    //            break;
    //        default:
    //            if (isRunaway)
    //            {
    //                animator.SetFloat(animIDSpeed, runSpeed);
    //            }
    //            animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude);
    //            break;
    //    }
        
        
    //}
   

    /// <summary>
    /// �A�j���[�V�����Đ��i�������X�e�[�g�ւ̑J�ډ\�ȂƂ���܂Ői��ł��邩�`�F�b�N����B
    /// </summary>
    /// <param name="animeStateName"></param>
    /// <returns>�J�ډ\�Ȃ�ture</returns>
    bool IsChangeableAnimeState(string animeStateName)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(animeStateName)
                && animator.GetCurrentAnimatorStateInfo(0).loop)
        {
            return true;
        }
        else
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName(animeStateName) 
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                return true;
            }
        }
        return false;

    }

    /// <summary>
    /// player��girl�̋����ɂ��noticeable�t���O���X�V����
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
    /// player��girl�̋����ɂ��noticeable�t���O���X�V����
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
    /// player��girl�̋����ƃA���O���ɂ��vacuumedable�t���O���X�V����
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
    /// ���݋z������Ă��邩�ǂ����`�F�b�N����
    /// </summary>
    bool CheckVaccumedNow()
    {
        return CheckVacuumedable() && pantsGetter.vacuuming;
    }
   
  
    /// <summary>
    /// Anystate����J�ڂł���Vacuum�X�e�[�g��Damaged�X�e�[�g�ւ̃t���O���`�F�b�N���\�Ȃ�J�ڂ���
    /// </summary>
    public void CheckVaccumedAndDamaged()
    {
        if(currentState == State.Damaged || currentState == State.Stan || currentState == State.Vacuumed || currentState == State.HyperVacuumed || currentState == State.BlownAway)
        {
            return;
        }

        if(CheckVaccumedNow())
        {
            //animator.Play("Vacuumed");
            currentState = State.Vacuumed;
           
        }
        
    }


    #region NavMeshAgent�֌W�̃��\�b�h�Q
    /// <summary>
    /// ���݂�NavMesh��̃����_���ȍ��W��Ԃ�
    /// </summary>
    /// <returns>NavMesh��̃����_���ȍ��W</returns>
    Vector3 GetSamplePointNavMesh()
    {
        NavMeshTriangulation samplePointNavMeshTriangulation = NavMesh.CalculateTriangulation();
        int index = Random.Range(0, samplePointNavMeshTriangulation.areas.Length);
        return samplePointNavMeshTriangulation.vertices[index];
    }

    /// <summary>
    /// isRunaway�t���O��true�ɂ��Aagent�𓮂����R���[�`���B�R���[�`���̏I���� isRunaway�t���O��false�ɂ���Bif(isRunaway)�̎����� StartCoroutine���A�R���[�`�����d�����Ȃ��悤���Ă��������B�܂��A�Ō�Ɏ��̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N���s���Ă��܂��B
    /// </summary>
    /// <returns></returns>
    IEnumerator Runaway()
    {
        isRunaway = true;


        bool isGetPosition = false;
        int tryGetPositionCount = 0;
        Vector3 girlPosition = girlTransform.position;

        // �o�H���擾�ł��邩�\�񃋁[�v����
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


        //�@�擾�ł��Ȃ������@�����@�Ǎۂ̉\��������
        if (!isGetPosition)
        {
            Debug.Log("hit�Ȃ�");

            //�@���͈͂��L�������_���Ȉʒu�Ōo�H���Ƃ�邩����
            Vector3 sorcePosition = new Vector3(girlTransform.position.x + Random.Range(-10f, 10f), girlTransform.position.y, girlTransform.position.z + Random.Range(-5f, 5f));
            if (NavMesh.SamplePosition(sorcePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                //�@�o�H���擾�ł����Ȃ�A�i�s������player�����Ȃ����`�F�b�N����
                Vector3 tempDestination = hit.position;
                if (Physics.SphereCast(sphereCastRoot.position, girlColliderRdius * 6, tempDestination - girlPosition, out RaycastHit hitinfo, 10f, LayerMask.GetMask("Character")))
                {
                    Debug.Log("navimesh�擾�ł�����player���������");
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
                Debug.Log("navimesh�擾�ł���");
                //yield return null;
            }

        }
        else
        {
            yield return new WaitForSeconds(Random.Range(minRunawayTime, maxRunawayTime));

        }

        isRunaway = false;

        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
        if(!CheckNoticeable())
        {
            currentState = State.Normal;
        }

    }
    #endregion

    /// <summary>
    /// NavmeshAgent��false�ɂ���Collider��isKinematic��false�ɂ���Ȃǂ���NavmeshAgent�哱�œ������̂𕨗����Z�哱�ɐ؂�ւ���
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
    /// NavmeshAgent��true�ɂ���Collider��isKinematic��true�ɂ���Ȃǂ��ĕ������Z�哱�œ������̂�NavmeshAgent�哱�ɐ؂�ւ���
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
    /// Normal�X�e�[�g����NavMeshAgent�̗����U�镑��
    /// </summary>
    public void OnNormal()
    {
        TeardownVacuumed();
        if (!animator.GetBool(animIDNavmeshAgent))
        {
            animator.SetBool(animIDNavmeshAgent, true);
        }
        navMeshAgent.speed = walkSpeed;
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude, 0.25f, Time.deltaTime);
        if ((Random.Range(0, 5000) < 20))
        {
            navMeshAgent.destination = GetSamplePointNavMesh();
        }
        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
        if (CheckNoticeable())
        {
            currentState = State.Notice;
        }
    }

    /// <summary>
    /// Notice�X�e�[�g���̗����U�镑��
    /// </summary>
    void OnNotice()
    {
        //Debug.Log("Notice����[��");// �����Ƀ��A�N�V�����̃A�j���[�V��������������
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
                currentState = State.Approch;
            }
            else
            {
                currentState = State.Runaway;
            }
        }
        

    }

    /// <summary>
    /// Runaway�X�e�[�g����NavMeshAgent�̗����U�镑��
    /// </summary>
    public void OnRunaway()
    {
        TeardownVacuumed();
        if (!animator.GetBool(animIDNavmeshAgent))
        {
            animator.SetBool(animIDNavmeshAgent, true);
        }
        navMeshAgent.speed = runSpeed;
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude, 0.25f, Time.deltaTime);
        if (!isRunaway)
        {
            StartCoroutine(Runaway());�@// Runaway()���Ɏ��̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N���������Ă܂�

        }
       
    }
    /// <summary>
    /// Approch�X�e�[�g����NavMeshAgent�̗����U�镑��
    /// </summary>
    public void OnApproch()
    {
        TeardownVacuumed();
        if (!animator.GetBool(animIDNavmeshAgent))
        {
            animator.SetBool(animIDNavmeshAgent, true);
        }
        navMeshAgent.speed = runSpeed;
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude, 0.25f, Time.deltaTime);
        navMeshAgent.stoppingDistance = 1.5f;
        navMeshAgent.destination = playerTransform.position;

        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
        if (CheckAttackable())
        {
            currentState = State.Attack;
        }
        else if(!CheckNoticeable())
        {
            Vector3 toPlayerDirection = playerTransform.position - girlTransform.position;
            if (Physics.SphereCast(sphereCastRoot.position, girlCollider.radius, toPlayerDirection, out RaycastHit hitinfo, 20f))
            {
                // noticeDistance�ȏ��player�Ɨ���Ă͂��邪�A���ڎ��F�ł��Ă���Ƃ�
                if (hitinfo.collider.CompareTag("Player"))
                {
                    currentState = State.Approch;
                }
                // noticeDistance�ȏ��player�Ɨ���Ă��āA���ڎ��F�ł��Ȃ��Ƃ�
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
    ///  Attack�X�e�[�g���̗����U�镑��
    /// </summary>
    void OnAttack()
    {
        //Debug.Log("Attack����[��");// ������Attack�A�j���[�V�����Ɋւ������������A�j���[�V�����C�x���g�ŃR���C�_�[��On���A�ڐG���肷��
        if (!animator.GetBool(animIDAttack))
        {
            //girlTransform.LookAt(playerTransform.position);
            animator.SetBool(animIDAttack, true);
        }
        girlTransform.LookAt(playerTransform.position);


        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
        if (IsChangeableAnimeState("Attack"))
        {
            currentState = State.Approch;
        }
    }

    /// <summary>
    /// Vacuumed�X�e�[�g���̗����U�镑��
    /// </summary>
    void OnVacuumed()
    {
        if(!animator.GetBool(animIDVacuumed))
        {
            animator.SetBool(animIDVacuumed, true);
        }
        //Debug.Log("Vaccumed����Ă܂�"); //Vacuumed�A�j�����Đ�����
        girlTransform.LookAt(2 * girlTransform.position - playerTransform.position);
       
        SetupVacuumed();

        //Debug.Log(!IsChangeableAnimeState("Vacuumed"));
        //Debug.Log(pantsGetter.hyperVacuuming);
        //Debug.Log(!pantsGetter.vacuuming);
        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
        if (pantsGetter.hyperVacuuming)
        {
            currentState = State.HyperVacuumed;
            return;
        }

        if (!IsChangeableAnimeState("Vacuumed"))
        {
            return;
        }
        else if (!pantsGetter.vacuuming)
        {
            TeardownVacuumed();
            currentState = State.Normal;
        }
    }
    /// <summary>
    /// HyperVacuumed�X�e�[�g���̗����U�镑��
    /// </summary>
    void OnHyperVacuumed()
    {
        if (!animator.GetBool(animIDHyperVcuuned))
        {
            animator.SetBool(animIDHyperVcuuned, true);
        }
        //Debug.Log("HyperVaccumed����Ă܂�"); //HyperVacuumed�A�j�����Đ�����
        
        SetupVacuumed();
        Vector3 diff = playerTransform.TransformPoint(Vector3.forward * 2f) - girlTransform.position;
        rb.velocity = diff * 10;
        //girlTransform.SetParent(playerTransform);
        girlTransform.LookAt(10 * playerTransform.forward + playerTransform.position);

        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
        if (pantsGetter.vacuumReleasing)
        {
            currentState = State.BlownAway;
            animator.SetBool(animIDBlowAway, true);
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

        //girlTransform.parent = null;

        //�@pantsGetter��BlowAway(Vector3 direction)���ĂԁB
        //�@OnCollisionEnter��State.Damaged�ɑJ��
    }
    public void BlowAway(Vector3 direction)
    {
        this.tag = "BlownAway";
        girlTransform.position = girlTransform.position + girlTransform.up * 0.2f;
        rb.velocity = direction;
        blowAwayDirection = direction;
    }
    /// <summary>
    /// currentState == State.BlownAway�̎��AGirl�EEnvironment�^�O�ƏՓ˂����Ƃ�currentState = State.Damaged�ɂ���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if(currentState == State.BlownAway)
        {
            if(collision.gameObject.CompareTag("Girl") || collision.gameObject.CompareTag("Environment"))
            {
                
                rb.velocity = Vector3.zero;
                this.tag = "Girl";
                currentState = State.Damaged;
                if(collision.gameObject.CompareTag("Girl"))
                {
                    var collisionGirl = collision.gameObject.GetComponent<GirlController_>();
                    collisionGirl.currentState = State.Damaged;
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
        // Debug.Log("Damaged����[��");
        girlCollider.enabled = false;
        TeardownVacuumed();
        if(!IsChangeableAnimeState("Stan"))
        {
            return;
        }
        currentState = State.Normal;
        girlCollider.enabled = true;
       
    }
    void OnStan()
    {
        //if(isEnable)
        //{
        //    isEnable = false;

        //TeardownVacuumed();

        //    isEnable = true;

        //    StartCoroutine(DelayCoroutine(() =>
        //    {
        //        currentState = State.Normal;
        //    }));
        //}

        TeardownVacuumed();

        if(!IsChangeableAnimeState("Stan"))
        {
            return;
        }
        currentState = State.Normal;
        animator.SetTrigger(animIDNavmeshAgent);
    }




    private IEnumerator DelayCoroutine(System.Action action)
    {
        yield return new WaitForSeconds(stanTime);
        action?.Invoke();
    }

}
