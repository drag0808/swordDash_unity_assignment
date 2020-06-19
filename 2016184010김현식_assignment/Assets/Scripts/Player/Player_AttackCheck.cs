using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player_AttackCheck : MonoBehaviour
{
    [Header("공격 오브젝트(범위)")]
    [SerializeField] GameObject go_AttackGameObject = null;

    [Header("공격 성공했는지 체크중")]
    [SerializeField] private bool isCanAttack = false;

    [Header("실공격범위")]
    [SerializeField] private float attackRange = 1.5f;


    public bool AttackCheck()
    {
        // 주변 콜리더들 싹 조사
        Collider2D[] colls = Physics2D.OverlapCircleAll(go_AttackGameObject.transform.position, attackRange);
        
        if (colls == null)
            return false;

        // 돌려서 적 & 공격가능 물체가 있으면 트루
        for(int i = 0; i < colls.Length; ++i)
        {
            Debug.Log(colls[i].gameObject.transform.position);
            if (colls[i].gameObject.layer == 9 || colls[i].gameObject.layer == 11)// Layer : Enemy 그리고 canAttackThing 아니 왜 안돼냐
            {
                return true;
            }
        }

        return false;
    }
}
