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
        [SerializeField] ThirdCamController thirdCam;        // 3인칭 카메라 컨트롤러가 있는 게임 오브젝트를 연결시켜줘야 한다.

        [SerializeField] float smoothRotation = 5f;               // 카메라의 자연스러운 회전을 위한 가중치
        Quaternion targetRotation;                           // 키보드 입력을 하지 않았을 때 카메라 방향으로 회전하기 위하여 회전 각도를 저장하는 변수

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
            float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)); // 키보드로 상하좌우 키 한개만 입력을 하면 0보다 큰 값을 moveAmount에 저장한다.
                                                                                           // 현재 위치 + 속도 * 가속도 = 이동 거리
                                                                                           // 이동거리만큼 (매 프레임마다 움직입니다 * Time.deltaTime)=> 프레임수와 상관없이 같은 시간에 같은 거리를 움직입니다.

            Vector3 moveMent = thirdCam.camLookRotation * moveDir; // moveDir 0일 때 moveMent가 0이 된다.

            if (moveAmount > 0)
            {
               targetRotation = Quaternion.LookRotation(moveMent);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothRotation * Time.deltaTime);
          

            cCon.Move(moveMent * moveSpeed * Time.deltaTime); 
        }       
    }

}