using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSetting
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private CharacterController cCon;

        // Start is called before the first frame update
        void Start()
        {
            cCon = GetComponent<CharacterController>();
        }

        // Update is called once per frame 컴퓨터가 좋을 수록 frame이 많이 생성되고 Update도 많이 호출 됩니다.
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

            // 현재 위치 + 속도 * 가속도 = 이동 거리
            // 이동거리만큼 (매 프레임마다 움직입니다 * Time.deltaTime)=> 프레임수와 상관없이 같은 시간에 같은 거리를 움직입니다.

            cCon.Move(moveDir * moveSpeed * Time.deltaTime); 

        }       
    }

}