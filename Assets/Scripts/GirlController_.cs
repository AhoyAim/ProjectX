
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
    private int animIDHyperVcuuned;
    private float girlColliderRdius;
    private float vaccumedableDistance;
    private float vaccumedableAngle;
    private bool isRunaway;
    private bool isEnable;
    private Transform girlTransform;
    private Vector3 toPlayerDirection;
    private Rigidbody rb;

    private void Start()
    {
        currentState = State.Normal;

        girlCollider = GetComponent<CapsuleCollider>();
        girlColliderRdius = girlCollider.radius;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animIDSpeed = Animator.StringToHash("Speed");
        animIDHyperVcuuned = Animator.StringToHash("HyperVcuumed");
        vaccumedableDistance = pantsGetter.vaccumableDistance;
        vaccumedableAngle = pantsGetter.vaccumableAngle;
        girlTransform = transform;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        isEnable = true;

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

        if(Input.GetKey(KeyCode.Z))
        {
            rb.velocity = Vector3.forward * 100;
        }
        if (Input.GetKey(KeyCode.X))
        {
            rb.velocity = Vector3.up;
        }

        if (isRunaway)
        {
            animator.SetFloat(animIDSpeed, runSpeed);
        }
        animator.SetFloat(animIDSpeed, navMeshAgent.velocity.magnitude);
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
        if(currentState == State.Stan || currentState == State.HyperVacuumed || currentState == State.BlownAway)
        {
            return;
        }

        if(CheckVaccumedNow())
        {
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
    /// NavmeshAgent��false�ɂ���Collider��isKinematic��false�ɂ���Ȃǂ���NavmeshAgent�哱�œ������̂𕨗����Z�哱��
    /// </summary>
    public void PrepareVacuumed()
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
    /// Normal�X�e�[�g����NavMeshAgent�̗����U�镑��
    /// </summary>
    public void OnNormal()
    {
        navMeshAgent.speed = walkSpeed;
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
        Debug.Log("Notice����[��");// �����Ƀ��A�N�V�����̃A�j���[�V��������������

        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
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
    /// Runaway�X�e�[�g����NavMeshAgent�̗����U�镑��
    /// </summary>
    public void OnRunaway()
    {
        navMeshAgent.speed = runSpeed;
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
        navMeshAgent.speed = runSpeed;
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
        Debug.Log("Attack����[��");// ������Attack�A�j���[�V�����Ɋւ������������A�j���[�V�����C�x���g�ŃR���C�_�[��On���A�ڐG���肷��

        // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
        if (!CheckAttackable())
        {
            currentState = State.Approch;
        }
    }

    /// <summary>
    /// Vacuumed�X�e�[�g���̗����U�镑��
    /// </summary>
    void OnVacuumed()
    {
        Debug.Log("Vaccumed����Ă܂�"); //Vacuumed�A�j�����Đ�����
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
    /// <summary>
    /// HyperVacuumed�X�e�[�g���̗����U�镑��
    /// </summary>
    void OnHyperVacuumed()
    {
        Debug.Log("HyperVaccumed����Ă܂�"); //HyperVacuumed�A�j�����Đ�����
        girlTransform.LookAt(2 * girlTransform.position - playerTransform.position);
        PrepareVacuumed();
        animator.SetBool(animIDHyperVcuuned, true);

        girlTransform.SetParent(playerTransform);
        

        if(pantsGetter.vacuumReieasing)
        {
            currentState = State.BlownAway;
            animator.SetBool(animIDHyperVcuuned, false);
        }
    }

    void OnBlownAway()
    {
        girlTransform.parent = null;
        
        //�@pantsGetter��BlowAway(Vector3 direction)���ĂԁB
        //�@OnCollisionEnter��State.Damaged�ɑJ��
    }
    public void BlowAway(Vector3 direction)
    {
        this.tag = "BlownAway";
        girlTransform.position = girlTransform.position + girlTransform.up * 0.2f;
        rb.velocity = direction;
    }
    /// <summary>
    /// currentState == State.BlownAway�̎��AGirl�EEnvironment�^�O�ƏՓ˂����Ƃ�currentState = State.Damaged�ɂ���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
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
                    collision.gameObject.GetComponent<GirlController_>().currentState = State.Damaged;
                }
            }
        }
    }
    void OnDamaged()
    {
        Debug.Log("Damaged����[��");
        PrepareVacuumed();
        currentState = State.Stan;
    }
    void OnStan()
    {
        if(isEnable)
        {
            isEnable = false;
            StartCoroutine(DelayCoroutine(2f, () =>
            {
                if (!navMeshAgent.enabled)
                {
                    rb.isKinematic = true;
                    if(navMeshAgent.hasPath)
                    {
                        navMeshAgent.ResetPath();
                    }
                    navMeshAgent.enabled = true;
                }
                isEnable = true;
                currentState = State.Normal;
            }));
        }
       
    }




    private IEnumerator DelayCoroutine(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

}
