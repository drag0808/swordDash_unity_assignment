using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStatus : MonoBehaviour
{
    protected int l_DEFAULT = 0;
    protected int l_TRANSPARENT_FX = 1;
    protected int l_IGNORE_RAYCAST = 2;
    protected int l_WATER = 4;
    protected int l_UI = 5;
    protected int l_GROUND = 8;

    new protected Rigidbody2D rigidbody = null;

    protected SpriteRenderer spriteRenderer = null;

    protected Animator animator = null;

    [Header("최대 체력")]
    [SerializeField] protected int maxHp = 0;
    [Header("현재 체력")]
    [SerializeField] protected int currentHp = 0;

    [Header("무적 타이머")]
    [SerializeField] float nonAttackedTimer = 0;
    [Header("무적시간")]
    [SerializeField] float nonAttackedTimeEnd = 0.6f;

    [Header("이동 속도")]
    [SerializeField] protected float moveSpeed = 0;
    [Header("최대 속도")]
    [SerializeField] protected float maxMoveSpeed = 0;

    protected float whereTo = 0;

    [Header("점프 파워")]
    [SerializeField] protected float jumpPower = 0;
    [Header("최대 점프 횟수")]
    [SerializeField] protected int maxJumpCount = 0;
    [Header("남은 점프 횟수")]
    [SerializeField] protected int jumpCount = 0;

    [Header("대시 파워")]
    [SerializeField] protected float dashPower = 0;
    [Header("대시 스피드")]
    [SerializeField] protected float dashSpeed = 0;
    protected float currentDashSpeed = 0;
    [Header("최대 대시 횟수")]
    [SerializeField] protected int maxDashCount = 0;
    [Header("남은 대시 횟수")]
    [SerializeField] protected int DashCount = 0;
    


    protected bool isInAir = false;

    protected bool isDash = false;

    protected bool isHurt = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentHp = maxHp;
    }

}
