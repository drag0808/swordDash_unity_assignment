using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera_Controller : MonoBehaviour
{



    [Header("따라가는 대상 플레이어")]
    [SerializeField] Transform tf_Player = null;

    [Header("따라갈 스피드")]
    [Range(0, 1f)] [SerializeField] float chaseSpeed = 0;

    float camNormalZPos = 0;
    /*
    [Header("부스터 시 떨어질 거리")]
    [SerializeField] float camJetPosX;
    */
    float camCurrentZPos;// X값이 변할 때 변한값을 여기에 넣어 함수 하나로 처리.

    Player_Controller player_Con;

    // Start is called before the first frame update
    void Start()
    {
        player_Con = tf_Player.GetComponent<Player_Controller>();
        camNormalZPos = transform.position.z;
        camCurrentZPos = camNormalZPos;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Player_Con.IsJump)
        //    camCurrentXPos = camJetPosX;
        //else
        camCurrentZPos = camNormalZPos;

        Vector3 movePos = Vector3.Lerp(transform.position, tf_Player.position, chaseSpeed);

        // -> 앞과 뒤 사이의 Lerp계산.
        float camerPosZ = Mathf.Lerp(transform.position.z, camCurrentZPos, chaseSpeed);

        transform.position = new Vector3(movePos.x, movePos.y, camerPosZ);
    }
}
