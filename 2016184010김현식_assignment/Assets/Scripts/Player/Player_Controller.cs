using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.UIElements;
using UnityEngine;

public class Player_Controller : MoveStatus
{
    [Header("벽 매달리는 시간")]
    [SerializeField] private float maxWallCount = 3.0f;
    [SerializeField] private float wallCount = 0;

    [Header("마지막으로 닿았던 벽or땅 콜리더")]
    [SerializeField] Collision2D LastTouchCollision = null;
    [Header("마지막으로 닿았던 벽or땅 리지")]
    [SerializeField] Rigidbody2D LastTouchRigi = null;

    [Header("더블점프 파티클 시스템")]
    [SerializeField] ParticleSystem ps_DoubleJumpEfect = null;
    [Header("대시 파티클 시스템")]
    [SerializeField] ParticleSystem ps_DashEfect = null;

    [Header("점프 입력했는지 그냥 떨어졌는지 확인용")]
    [SerializeField] private int spacebarDownCheck = 0;
    [Header("벽점프 가능 체크")]
    [SerializeField] private bool canWallJump = false;

    [Header("체크포인트 벡터")]
    private Vector2 checkPoint;
    [Header("체크포인트 오브젝트")]
    private GameObject go_LastCheckPoint = null;

    [Header("대시시간 타이머")]
    [SerializeField] float dashTimer = 0; // 대시 시간 따지기
    [Header("대시시간 (컨트롤 불가 시간)")]
    [SerializeField] float dashTimerEnd = 0.25f;


    private bool isRight = true; // 오른쪽 왼쪽 보고있는 방향 알기
    private bool attackSuccess = false; // 공격 한번에 한번만 체크되게

    [Header("오른쪽 공격 프린터")]
    [SerializeField] GameObject go_AttackPrinter_R = null;
    private Animator anim_AttackR = null;
    [Header("왼쪽 공격 프린터")]
    [SerializeField] GameObject go_AttackPrinter_L = null;
    private Animator anim_AttackL = null;

    [Header("오른쪽 공격 오브젝트(범위)")]
    [SerializeField] GameObject go_AttackGameObject_R = null;
    Player_AttackCheck attackCheck_R;
    [Header("왼쪽 공격 오브젝트(범위)")]
    [SerializeField] GameObject go_AttackGameObject_L = null;
    Player_AttackCheck attackCheck_L;

    [Header("공격시간 타이머")]
    [SerializeField] float attackTimer = 0; // 공격 시간 따지기
    [Header("공격시간 (컨트롤 불가 시간)")]
    [SerializeField] float attackTimerEnd = 0.20f;
    private bool isAttack = false;

    private float hurtTimer = 0;
    [Header("피해입은 상태(행동불가상태) 지속시간")]
    [SerializeField] private float hurtTimerEnd = 0;
    [Header("피해시 튕겨져 나가는 힘x")]
    [SerializeField] private float hurtForcex = 0;
    [Header("피해시 튕겨져 나가는 힘y")]
    [SerializeField] private float hurtForcey = 0;

    //private GameObject go_AttackTriger = null;

    [Header("체크포인트 파티클시스템")]
    [SerializeField] ParticleSystem ps_CheckPoint = null;

    [Header("되살아 나는 좌표(처음은 시작좌표)")]
    [SerializeField] float[] loadXY = { 0, 0 };



    void Start()
    {
        attackCheck_R = go_AttackGameObject_R.GetComponent<Player_AttackCheck>();
        attackCheck_L = go_AttackGameObject_L.GetComponent<Player_AttackCheck>();

        jumpCount = maxJumpCount;

        isInAir = false;
        canWallJump = false;


        anim_AttackR = go_AttackPrinter_R.GetComponent<Animator>();
        anim_AttackL = go_AttackPrinter_L.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.WakeUp();// 충돌 오류 없게 한번 체크

        LandingCheck();

        if (!isHurt)
        {
            WallGrabCheck();
            jump();
            Dash();
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (!isHurt)
            isRun();

        if (isHurt)
            IsHurt();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LastTouchCollision = collision;
        GameObject touchObject = collision.gameObject;

        Debug.LogError(collision.gameObject.name);

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            LastTouchCollision = collision;
            LastTouchRigi = collision.transform.GetComponent<Rigidbody2D>();
        }

        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            isHurt = true;
            rigidbody.AddForce( new Vector2(-1 * Input.GetAxisRaw("Horizontal") * hurtForcex, hurtForcey), ForceMode2D.Impulse);
            animator.SetBool("IsHurt", true);

            if (currentHp == 0)
                reBorn();
        }

        else if(collision.gameObject.layer == LayerMask.NameToLayer("FallingChecker"))
        {
            reBorn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 체크포인트 
        if (collider.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            loadXY[0] = collider.gameObject.transform.position.x;
            loadXY[1] = collider.gameObject.transform.position.y;
            ps_CheckPoint.Play();
        }
    }

    /// <summary>
    ///     Hurt    다쳤을 때 Hurt true -> false 전환하기
    void IsHurt()
    {
        hurtTimer += Time.deltaTime;
        
        if(hurtTimer >= hurtTimerEnd * 0.8f)
        {
            animator.SetBool("IsHurt", false);
        }
        if(hurtTimer >= hurtTimerEnd)
        {
            hurtTimer = 0;
            isHurt = false;           
        }
    }




    //  달리기 ///////////////////////////////////
    void isRun()
    {
        if (Input.GetButtonUp("Horizontal"))
        { 
            rigidbody.velocity = new Vector2(rigidbody.velocity.normalized.x * 0.5f, rigidbody.velocity.y);
            return;
        }


         whereTo = Input.GetAxisRaw("Horizontal");
        rigidbody.AddForce(Vector2.right * whereTo * moveSpeed, ForceMode2D.Impulse);

        // 상황에 따라 최고속도 변경(기본 최고스피드 + 대시 사용시 + 땅이 움직일 때)
        if (rigidbody.velocity.x > maxMoveSpeed + currentDashSpeed + LastTouchRigi.velocity.x)
        {
            rigidbody.velocity = new Vector2(maxMoveSpeed, rigidbody.velocity.y);
            //rigidbody.AddForce();
        }
        else if (rigidbody.velocity.x < maxMoveSpeed*(-1) + currentDashSpeed*(-1) + LastTouchRigi.velocity.x)
        {
            rigidbody.velocity = new Vector2(maxMoveSpeed * (-1), rigidbody.velocity.y);
        }

        NewSpriteFlipX();
    }


    void NewSpriteFlipX()
    {
        float whereTo = Input.GetAxisRaw("Horizontal");
        if (whereTo >= 0.9 || whereTo <= -0.9)
        {
            if (whereTo >= 0.9)
            {
                animator.SetTrigger("IsRun");
                spriteRenderer.flipX = false; // 오른쪽
            }
            else if (whereTo <= -0.9)
            {
                animator.SetTrigger("IsRun");
                spriteRenderer.flipX = true; // 왼쪽
            }
        }
        else
        {
            animator.SetTrigger("IsIdle");
        }
    }

    //  점프  /////////////////////////////////////////////
    void jump()
    {

        if (Input.GetKeyDown(KeyCode.X) && !isDash)
        {
            spacebarDownCheck++;

            // 그냥 떨어진 상태 or 벽점프 or 더블점프 체크(공중에서 눌렀을 때)
            if (isInAir == true)
            {

                 if (jumpCount <= 0) // 공중에서 점프 없을 때 스페이스바 입력
                {
                    if (canWallJump) { }

                    else
                        return;
                }


                // 그냥 떨어진 상태
                if (spacebarDownCheck == 1)
                {
                    jumpCount--; // 두개 중 하나 줄여줌. (지상에서 점프한 상태처럼)
                }

                // 월점프 했을 때
                if (canWallJump)
                {
                    animator.SetBool("IsInAir", isInAir);
                    rigidbody.AddForce(Vector2.up * jumpPower * 1.0f, ForceMode2D.Impulse);
                    spacebarDownCheck--;
                    if (spacebarDownCheck < 0) spacebarDownCheck = 0;
                    return; // 점프카운트 안줄이고 리턴
                }
                // 월점프 아니고 점프카운트가 남았을 때 (일반 더블점프일때)
                if (jumpCount > 0) // == 1
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                    animator.SetBool("DoubleJump", true);
                    ps_DoubleJumpEfect.Play();
                }
            }
            // 공중이 아니었을 때
            else
            {
                animator.SetBool("IsInAir", isInAir);
            }

            // 처음 점프. 
            if (jumpCount > 0)
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                
            jumpCount--;
            isInAir = true;
            animator.SetBool("IsInAir", isInAir);
        }
    }
    public bool Get_IsInAir()
    {
        return isInAir;
    }

    /// <summary>
    /// //////////////////////////////////대시 처리
    /// </summary>
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (DashCount > 0) { // 대시카운트가 남아 있는 경우, 입력하고 있는 방향으로 임펄스
                float whereTo = Input.GetAxisRaw("Horizontal");
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                rigidbody.AddForce(new Vector2(whereTo * dashPower, 10), ForceMode2D.Impulse);
                DashCount -= 1;
                isDash = true;
                currentDashSpeed = dashSpeed;
                ps_DashEfect.Play();
            }
        }
        if(isDash)
        {
            dashTimer += Time.deltaTime;
            
            if(dashTimer >= dashTimerEnd)
            {
                dashTimer = 0;
                isDash = false;
                currentDashSpeed = 0;
            }
        }
    }


    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            attackSuccess = false; // 공격 체크를 위해 폴스로 만듦

            Debug.Log("공격 실행");
            isAttack = true;       
            
            if (!spriteRenderer.flipX)
            {
                go_AttackGameObject_R.SetActive(true);
                anim_AttackR.Play("Player_Attack");
                isRight = true; // 오른쪽
            }
            else
            {
                go_AttackGameObject_L.SetActive(true);
                anim_AttackL.Play("Player_Attack");
                isRight = false; // 왼쪽
            }
        }

        if (isAttack)
        {
            attackTimer += Time.deltaTime;

            //////////////////////////////////////////////////////
            ////////////////////    공격체크
            //////////////////////////////////////////////////////
            
            if (attackSuccess) { } // 이미 어택에 한번 성공했다면 그냥 넘겨주기

            else if ((isRight && attackCheck_R.AttackCheck()) ||
                (!isRight && attackCheck_L.AttackCheck())) // 왼쪽볼 때 왼쪽, 오른쪽 볼 때 오른쪽
            {
                attackSuccess = true; //어택에 성공하면 트루로

                Debug.Log("공격 성공");

                if (isInAir)
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                    rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

                    DashCount++;
                    jumpCount++;
                    if (DashCount > maxDashCount)
                        DashCount = maxDashCount;
                    if (jumpCount > maxJumpCount - 1)
                        jumpCount = maxJumpCount - 1;
                }
            }
            /////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////

            if (attackTimer >= attackTimerEnd)
            {
                if (isRight)
                {
                    go_AttackGameObject_R.SetActive(false);
                }
                else
                {
                    go_AttackGameObject_L.SetActive(false);
                }
                attackTimer = 0;
                isAttack = false;
            }
        }
    }


    //  랜딩 체크   ///////////////////////////////////////////////////
    void LandingCheck()
    {
        // 랜딩은 밑으로 떨어질 때에만 체크.
        if (rigidbody.velocity.y <= 0f)
        {
            // 밑으로 레이저를 쏴서 밑에 감지된 오브젝트의 레이어가 "Ground" 일때 실행
            RaycastHit2D rayHit2D = Physics2D.Raycast(rigidbody.position, Vector3.down, 2f, LayerMask.GetMask("Ground"));
            if (rayHit2D.collider != null && rayHit2D.distance < 0.7f)
            {
                if (isInAir)
                {
                    //마지막으로 닿은 물체(최고속도 관련된) 초기화, 재설정 -> 랜딩, 벽점.
                    //LastTouchCollision = ;
                }              
                
                ifLanding_StatChange();
            }
            else
                isInAir = true;
        }
    }

    void ifLanding_StatChange()
    {


        isInAir = false;
        canWallJump = false;
        jumpCount = maxJumpCount;
        spacebarDownCheck = 0;
        wallCount = maxWallCount;
        DashCount = maxDashCount;

        animator.SetBool("IsInAir", false);
        animator.SetBool("DoubleJump", false);
    }



    //  벽 잡기 체크 ///////////////////////////////////////
    void WallGrabCheck()
    {
        RaycastHit2D rayHit2D_L = Physics2D.Raycast(rigidbody.position, Vector3.left, 1f, LayerMask.GetMask("Ground"));
        RaycastHit2D rayHit2D_R = Physics2D.Raycast(rigidbody.position, Vector3.right, 1f, LayerMask.GetMask("Ground"));
        if ((rayHit2D_L.collider != null && rayHit2D_L.distance < 0.5f) || (rayHit2D_R.collider != null && rayHit2D_R.distance < 0.5f))
        {
            //Debug.LogError("벽에 닿음!!!!!");
            if (isInAir)
            {
                canWallJump = true;
                IsOnWall();
            }

        }
        else
            canWallJump = false;

    }

    // 벽에 붙어있을 때 월카운트 감소
    private void IsOnWall()
    {

        if (wallCount <= 0)
        {
            //Debug.Log("붙어있는시간 끝!");
            wallCount = 0;

            float whereTo = Input.GetAxisRaw("Horizontal");
            rigidbody.velocity = new Vector2(5 * -whereTo, rigidbody.velocity.y);

        }
        else
        {
            //Debug.Log("벽 붙어있는중");
            wallCount -= Time.deltaTime;
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////


    private void reBorn()
    {
        rigidbody.position = new Vector2(loadXY[0], loadXY[1]);
        currentHp = maxHp;
    }







    /////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////


    public bool getCanWallJump()
    {
        return canWallJump;
    }
    public float getWhereTo()
    {
        return whereTo;
    }
    public float getMoveSpeed()
    {
        return moveSpeed;
    }

}

