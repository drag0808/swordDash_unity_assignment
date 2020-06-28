using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckPoint : MonoBehaviour
{

    [Header("파티클 시스템")]
    [SerializeField] ParticleSystem ps_check = null;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ps_check.Play();
        }
    }


}
