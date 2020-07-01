using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingBlock_EnterBlock : MonoBehaviour
{
    [Header("무빙블록 애니메이터")]
    [SerializeField] Animator animator;
    Rigidbody2D rigidbody = null;

    //GameObject nowPlayer = null;
    Rigidbody2D playerRigi = null;
    Player_Controller player_Controller = null;

    [Header("움직임 스피드")]
    [SerializeField] float moveSpeed = 0;// 움직임스피드
    [Header("이동 거리 X")]
    [SerializeField] float moveToX = 0;
    [Header("이동 거리 Y")]
    [SerializeField] float moveToY = 0;

    [SerializeField] bool isMove = false;
    [SerializeField] bool isEnd = false;

    Vector2 endPos = Vector2.zero;
    Vector2 originPos = Vector2.zero;
    Vector2 backPos = Vector2.zero;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        originPos = transform.position;       
        endPos.Set(originPos.x + moveToX, originPos.y + moveToY);

        //nowPlayer = GameObject.Find("Player").gameObject;
        //playerRigi = nowPlayer.GetComponent<Rigidbody2D>();
        //player_Controller = nowPlayer.GetComponent<Player_Controller>();
        isMove = false;
        isEnd = false;
    }

    private void Update()
    {
        
        //rigidbody.AddForce(new Vector2());

        if (isMove && !isEnd)
        {
            Move();
            if(EndCheck())
            {             
                MovingEnd();
                isMove = false;
                rigidbody.velocity = Vector2.zero;
            }
        }
        else
        {
            if(rigidbody.velocity.y < 0)
               rigidbody.AddForce(Vector2.up * 5f, ForceMode2D.Force);
            //rigidbody.position = originPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        backPos = rigidbody.position;

        if (collision.transform.CompareTag("Player") && !isEnd)
        {
            isMove = true;
            animator.SetTrigger("PlayerTouch");
            //Invoke("BackYPos", 0.1f);
        }
    }

    // check move End -> true
    private bool EndCheck()
    {
        if(moveToX > 0)
        {
            if(moveToY > 0)
            {
                if (rigidbody.position.x > endPos.x || rigidbody.position.y > endPos.y)
                    return true;
                else return false;
            }
            else if(moveToY < 0)
            {
                if (rigidbody.position.x > endPos.x || rigidbody.position.y < endPos.y)
                    return true;
                else return false;
            }
            else
            {
                if (rigidbody.position.x > endPos.x)
                    return true;
                else return false;
            }
        }
        else if(moveToX < 0)
        {
            if (moveToY > 0)
            {
                if (rigidbody.position.x < endPos.x || rigidbody.position.y > endPos.y)
                    return true;
                else return false;
            }
            else if (moveToY < 0)
            {
                if (rigidbody.position.x < endPos.x || rigidbody.position.y < endPos.y)
                    return true;
                else return false;
            }
            else
            {
                if (rigidbody.position.x < endPos.x)
                    return true;
                else return false;
            }
        }
        else
        {
            if (moveToY > 0)
            {
                if (rigidbody.position.y > endPos.y)
                    return true;
                else return false;
            }
            else if (moveToY < 0)
            {
                if (rigidbody.position.y < endPos.y)
                    return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
    }


    // y pos return
    private void BackYPos()
    {       
        rigidbody.position = new Vector2(rigidbody.position.x, backPos.y);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
    }

    // stay y pos
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player_Controller = collision.transform.GetComponent<Player_Controller>();
            if (!player_Controller.getCanWallJump())
            {
                //Debug.Log("위로 가는중");
                rigidbody.AddForce(Vector2.up * 33f);
            }
            else
            {
                rigidbody.AddForce(Vector2.left * player_Controller.getWhereTo() * player_Controller.getMoveSpeed() * 2000, ForceMode2D.Impulse);
            }
        }
    }

    // go to endPos
    private void Move()
    {
        //Debug.Log("가는중");

        float goToY = 0;
        float goToX = 0;
        if (moveToX != 0)
            goToX = moveToX / Mathf.Abs(moveToX);
        if (moveToY != 0)
            goToY = moveToY / Mathf.Abs(moveToY);

        rigidbody.velocity = new Vector2(goToX, goToY) * moveSpeed;
    }

    private void PlayerCheck()
    {
        Vector2 leftUnder = new Vector2(gameObject.transform.position.x - 10, gameObject.transform.position.y - 10);
        Vector2 rightUp = new Vector2(gameObject.transform.position.x + 10, gameObject.transform.position.y + 10);

        Collider2D nearPlayer = Physics2D.OverlapArea(leftUnder, rightUp);

        if (nearPlayer == null)
            return;
        if (nearPlayer.CompareTag("Player"))
        {
            
        }
    }


    private void MovingEnd()
    {
        isEnd = true;
        rigidbody.velocity = Vector2.zero;
        animator.SetTrigger("TurnOnEnd");
        //Debug.Log("turnOn End");
    }

    // play by animation
    private void Re_set()
    {
        rigidbody.position = originPos;
        animator.SetTrigger("ReSet");
        //Debug.Log("Re Set");
        isEnd = false;
        //rigidbody.velocity = Vector2.zero;
    }
}
