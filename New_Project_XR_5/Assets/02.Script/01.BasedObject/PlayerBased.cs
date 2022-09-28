using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackInfo
{
    public float fFirstDelay;//선딜
    public float fBackDelay;//후딜
    public float fAttackTime;//선딜 후 공격 판정이 얼마나 남을지
    public float fDamage;
};

[System.Serializable]
public struct PlayerStats
{
    public float fGravityScale;
    [Space(10f)]
    public float fMoveSpeed;
    public float fJumpForce;
    public float fComboTime;

    [Space(10f)]
    public int iHP;
    public float fStamina;
    [Space(10f)]
    [Header("피격 후 경직 시간")]
    public float fStunTime;
    [Header("피격 후 넉백되는 힘")]
    public float fHitForce;
};

[System.Serializable]
public struct DashInfo
{
    public float fDashTime;
    [Header("대쉬 거리")]
    public float fDashDistance;
    public float fDashCool;

    [HideInInspector]
    public float fLastDistance;//smoothdamp 계산에 필요. 건드리는거 아님
    [HideInInspector]
    public bool bCanDash;//대쉬 가능한지
}

public class PlayerBased : MonoBehaviour
{
    public STATE mState;
    public ACTION mAction;

    [HideInInspector]
    public List<AttackInfo> CLightAttack;
    [HideInInspector]
    public List<AttackInfo> CHeavyAttack;

    public AttackInfo mAttack;

    [SerializeField]
    Transform LAttackCollider;

    [SerializeField]
    Transform HAttackCollider;

    [HideInInspector]
    public int iAttackCnt;
    //[HideInInspector]
    //public bool bLookDir;//true 우측, false 좌측

    [HideInInspector]
    public int iLookDir;//1 우측, -1 좌측

    [Space(15f)]
    public PlayerStats mStats;
    public DashInfo mDash;

    public SpineBased CSpine;

    [HideInInspector]
    public Rigidbody2D CRigid;


    Coroutine ComboCoroutine;
    List<ACTION> StunAction;

    //
    float fDashDestination;

    void SettingStunState()
    {
        //StunState.Add(STATE.HIT);
        //StunState.Add(STATE.DEATH);
        //StunState.Add(STATE.ATTACK);
    }

    public virtual void Init()
    {
        CRigid = GetComponent<Rigidbody2D>();
        //bLookDir = true;
        iLookDir = 1;
        iAttackCnt = 0;

        mDash.bCanDash = true;
        CRigid.gravityScale = mStats.fGravityScale;
        SettingStunState();
    }

    private void Start()//나중에 지움. 씬매니저로 옮겨서 로딩할때 init함수들 한번에 처리
    {
        Init();
    }




    private void Update()
    {
        if (Input.anyKey)
        {
            if (!mAction.Equals(ACTION.ATTACK) && !mAction.Equals(ACTION.HIT) && !mAction.Equals(ACTION.DEATH) && !mAction.Equals(ACTION.DASH))
            {
                if (Manager.Key.Inst.GetAction(INPUT.DOWN))
                {

                }

                if (Manager.Key.Inst.GetActionDown(INPUT.JUMP) && mState.Equals(STATE.STAND))
                {
                    DoJump();
                }

                if (Manager.Key.Inst.GetActionDown(INPUT.ATTACK_LIGHT))
                {
                    Attack();
                }

                if (Manager.Key.Inst.GetActionDown(INPUT.DASH))
                {
                    DoDash();
                }
            }
        }
        else
        {
            if (!mAction.Equals(ACTION.ATTACK) && !mAction.Equals(ACTION.HIT) && !mAction.Equals(ACTION.DEATH) && !mAction.Equals(ACTION.DASH))
                DoIdle();
        }
    }

    private void FixedUpdate()
    {
        if (Input.anyKey)
        {
            //if(!CheckStunState())
            if (!mAction.Equals(ACTION.HIT) && !mAction.Equals(ACTION.DEATH) && !mAction.Equals(ACTION.DASH))
            {
                if (Manager.Key.Inst.GetAction(INPUT.LEFT))
                {
                    if (!mAction.Equals(ACTION.ATTACK))
                    {
                        SetAction(ACTION.WALK);
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }

                    //bLookDir = false;
                    iLookDir = -1;
                    DoWalk();
                }
                else if (Manager.Key.Inst.GetAction(INPUT.RIGHT))
                {
                    if (!mAction.Equals(ACTION.ATTACK))
                    {
                        SetAction(ACTION.WALK);
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    //bLookDir = true;
                    iLookDir = 1;
                    DoWalk();
                }
            }
        }

        if(mAction.Equals(ACTION.DASH))
        {

            if (transform.position.x > fDashDestination - 0.1f &&
            transform.position.x < fDashDestination + 0.1f) //범위내
            {
                DoIdle();
            }
            else
            {
                float x = Mathf.SmoothDamp(transform.position.x, fDashDestination,
                ref mDash.fLastDistance, mDash.fDashTime);

                transform.position = new Vector2(x, transform.position.y);
            }
        }
    }

    protected virtual bool CheckStunState()
    {
        foreach (var _action in StunAction)
        {
            if (_action == mAction)
                return true;
        }
        return false;
    }


    protected virtual void SetState(STATE _state)
    {
        mState = _state;
    }

    protected virtual void SetAction(ACTION _action)
    {
        mAction = _action;
    }

    protected virtual void DoIdle()
    {
        if (mState.Equals(STATE.STAND))
            SetAction(ACTION.IDLE);
        else
            SetAction(ACTION.JUMP);
    }

    protected virtual void DoWalk()
    {
        //float MoveX = Input.GetAxis("Horizontal") * Stats.fMoveSpeed * Time.deltaTime;
        float MoveX = mStats.fMoveSpeed * Time.deltaTime;

        //if (bLookDir)
        //{
        //    //if(!mAction.Equals(ACTION.ATTACK))
        //        transform.position = new Vector2(transform.position.x + MoveX, transform.position.y);
        //}
        //else
        //{
        //    //if (!mAction.Equals(ACTION.ATTACK))
        //        transform.position = new Vector2(transform.position.x - MoveX, transform.position.y);
        //}
        transform.position = new Vector2(transform.position.x + MoveX * iLookDir, transform.position.y);


        //if (bLookDir)
        //{
        //    CRigid.velocity = new Vector2(CRigid.velocity.x + MoveX, CRigid.velocity.y);
        //}
        //else
        //    CRigid.velocity = new Vector2(CRigid.velocity.x - MoveX, CRigid.velocity.y);

    }

    public virtual void DoJump()
    {
        SetAction(ACTION.JUMP);

        CRigid.AddForce(Vector2.up * mStats.fJumpForce, ForceMode2D.Impulse);
    }

    public virtual void DoDash()
    {
        if (mDash.bCanDash)
        {
            SetAction(ACTION.DASH);
            StartCoroutine(DashCoroutine());
            CRigid.gravityScale = 0;
            CRigid.velocity = Vector2.zero;

            fDashDestination = transform.position.x + mDash.fDashDistance * iLookDir;
        }
    }

    public virtual void HitDamage(Vector2 vec2)
    {
        if (!mAction.Equals(ACTION.HIT) && !mAction.Equals(ACTION.DEATH))
        {
            mStats.iHP--;

            if (vec2.x > transform.position.x)
            {
                CRigid.AddForce(Vector2.left * mStats.fHitForce, ForceMode2D.Impulse);
            }
            else
            {
                CRigid.AddForce(Vector2.right * mStats.fHitForce, ForceMode2D.Impulse);
            }

            if (mStats.iHP > 0)
            {
                StartCoroutine(HitTime_Coroutine());
            }
            else if (mStats.iHP == 0)
            {
                //death
            }
        }
    }
    IEnumerator HitTime_Coroutine()
    {
        SetAction(ACTION.HIT);

        GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(mStats.fStunTime);

        GetComponent<SpriteRenderer>().color = Color.white;
        DoIdle();
    }



    public virtual void Attack()
    {
        StartCoroutine(AttackDelay_Coroutine());

        //if (iAttackCnt < 4)
        //{
        //    //if (ComboCoroutine != null)
        //    //    StopCoroutine(ComboCoroutine);

        //    //ComboCoroutine = StartCoroutine(ComboTimeCoroutine());
        //    //StartCoroutine(AttackDelay_Coroutine(iAttackCnt));


        //    //iAttackCnt++;
        //}
        //else
        //{
        //    iAttackCnt = 0;
        //    Attack();
        //}
    }

    IEnumerator ComboTimeCoroutine()
    {
        yield return new WaitForSeconds(mStats.fComboTime);
        iAttackCnt = 0;
    }

    IEnumerator AttackHitTime_Coroutine(int _cnt)
    {
        //공격처리 여기서

        //이펙트 출력
        LAttackCollider.GetChild(_cnt).gameObject.SetActive(true);
        yield return new WaitForSeconds(CLightAttack[_cnt].fAttackTime);
        LAttackCollider.GetChild(_cnt).gameObject.SetActive(false);
    }

    IEnumerator AttackDelay_Coroutine(int _cnt)
    {
        yield return new WaitForSeconds(CLightAttack[_cnt].fFirstDelay);

        StartCoroutine(AttackHitTime_Coroutine(_cnt));
        yield return new WaitForSeconds(CLightAttack[_cnt].fBackDelay);
    }
    IEnumerator AttackDelay_Coroutine()
    {
        SetAction(ACTION.ATTACK);

        yield return new WaitForSeconds(mAttack.fFirstDelay);

        StartCoroutine(AttackHitTime_Coroutine());
        yield return new WaitForSeconds(mAttack.fBackDelay);

        if (!mState.Equals(STATE.STAND))
            SetAction(ACTION.JUMP);
        else
            DoIdle();
    }
    IEnumerator AttackHitTime_Coroutine()
    {
        //공격처리 여기서

        //이펙트 출력
        LAttackCollider.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(mAttack.fAttackTime);
        LAttackCollider.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator DashCoroutine()
    {
        StartCoroutine(DashCoolTime_Coroutine());
        yield return new WaitForSeconds(mDash.fDashTime + 0.05f);
        CRigid.gravityScale = mStats.fGravityScale;
    }
    IEnumerator DashCoolTime_Coroutine()
    {
        mDash.bCanDash = false;
        yield return new WaitForSeconds(mDash.fDashCool);

        mDash.bCanDash = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Floor"))
        {
            Vector2 dir = collision.GetContact(0).normal;

            if (dir.y == 1)
            {
                SetState(STATE.STAND);
                SetAction(ACTION.IDLE);
            }

            if (dir.x > 0.9 || dir.x < -0.9)
                DoIdle();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Floor"))
        {
            SetState(STATE.FLOAT);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("BossAttack"))//임시 태그
        {
            HitDamage(collision.transform.position);
        }
    }
}