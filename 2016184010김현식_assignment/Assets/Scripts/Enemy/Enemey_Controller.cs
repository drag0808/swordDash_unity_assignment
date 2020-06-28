using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemey_Controller : MoveStatus
{
  
    private bool attacked = false;

    [Header("맞는 파티클 시스템")]
    [SerializeField] ParticleSystem ps_Attacked = null;
    [Header("죽는 파티클 시스템")]
    [SerializeField] ParticleSystem ps_Death = null;

    float deathTimer = 0;
    float deathTimerEnd = 0.5f;


 //   private void Start()
  //  {

  //  }


    // Update is called once per frame
    void Update()
    {
        AttackedCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)// Layer :  Attack       
        {
            attacked = true;
        }
    }

    void AttackedCheck()
    {
        if(attacked)
        {
            if(!isHurt)
            {
                currentHp--;
            }
            ps_Attacked.Play();
            attacked = false;
        }
        if (currentHp <= 0)
        {
            ps_Death.Play();
            death();
        }
    }

    void death()
    {
        deathTimer += Time.deltaTime;
        if(deathTimer >= deathTimerEnd)
            gameObject.SetActive(false);
    }

}
