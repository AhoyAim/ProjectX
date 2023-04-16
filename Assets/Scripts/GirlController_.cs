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
                // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
                if(noticeable)
                {
                    currentState = State.Notice;
                }



                // �ŏI�I�ɃX�e�[�g�̑J�ڂ��Ȃ���Ζ{���̏��������s
                if(currentState == State.Normal)
                {
                    OnNormal();
                }
                break;


            case State.Notice:
                // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
                if(isNaked)
                {
                    currentState = State.Approch;
                }
                else
                {
                    currentState = State.Runaway;
                }


                // �ŏI�I�ɃX�e�[�g�̑J�ڂ��Ȃ���Ζ{���̏��������s
                if (currentState == State.Notice)
                {
                    Debug.Log("Notice������I");
                }
                break;


            case State.Runaway:
                // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
                if(!isRunaway && !noticeable)
                {
                    currentState = State.Normal;
                }

                // �ŏI�I�ɃX�e�[�g�̑J�ڂ��Ȃ���Ζ{���̏��������s
                if(currentState == State.Runaway)
                {
                    OnRunaway();
                }
                break;


            case State.Approch:
                // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
                if(!noticeable)
                {
                    Vector3 toPlayerDirection = playerTransform.position - girlTransform.position;
                    if(Physics.SphereCast(sphereCastRoot.position, girlCollider.radius, toPlayerDirection, out RaycastHit hitinfo,20f))
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
                if(attackable)
                {
                    currentState = State.Attack;
                }


                // �ŏI�I�ɃX�e�[�g�̑J�ڂ��Ȃ���Ζ{���̏��������s
                if (currentState == State.Approch)
                {
                    OnApproch();
                }
                break;

            case State.Attack:
                // ���̃X�e�[�g�ɑJ�ڂł��Ȃ����`�F�b�N
                if (!attackable)
                {
                    currentState = State.Approch;
                }

                // �ŏI�I�ɃX�e�[�g�̑J�ڂ��Ȃ���Ζ{���̏��������s
                if (currentState == State.Attack)
                {
                    Debug.Log("Attack������");
                }
                break;

            case State.Vacuumed:
                Debug.Log("Vaccumed����Ă܂�");
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
    /// player��girl�̋����ɂ��noticeable�t���O���X�V����
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
    /// player��girl�̋����ɂ��noticeable�t���O���X�V����
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
    /// player��girl�̋����ƃA���O���ɂ��vacuumedable�t���O���X�V����
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
    /// �e��t���O���X�V����
    /// </summary>
    void UpDateFlags()
    {
        CheckNoticeable();
        CheckVacuumedable();
        CheckAttackable();

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
    /// Runaway�t���O��true�ɂ��Aagent�𓮂����R���[�`���B�R���[�`���̏I���� Runaway�t���O��false�ɂ���B
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
                if (Physics.SphereCast(sphereCastRoot.position, girlCollider.radius * 6, tempDestination - girlPosition, out RaycastHit hitinfo, 10f, LayerMask.GetMask("Character")))
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
    }

    /// <summary>
    /// Runaway�X�e�[�g����NavMeshAgent�̗����U�镑��
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
    /// Approch�X�e�[�g����NavMeshAgent�̗����U�镑��
    /// </summary>
    public void OnApproch()
    {
        navMeshAgent.speed = runSpeed;
        navMeshAgent.stoppingDistance = 1.5f;
        navMeshAgent.destination = playerTransform.position;
    }

    /// <summary>
    /// Normal�X�e�[�g����NavMeshAgent�̗����U�镑��
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
