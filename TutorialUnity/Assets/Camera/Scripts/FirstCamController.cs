using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCamController : MonoBehaviour
{
    [SerializeField] private Transform viewPort;

    // 카메라 회전 제어를 위한 변수
    [SerializeField] private float mouseSensitvity = 1f;
    [SerializeField] private int limitAngle = 60;            // 마우스의 상하 회전을 제한하는 각도
    private float verticalRot;                               // 오일러 회전하기 전 수치를 저장해두기 위한 값
    private Vector2 mouseInput;

    [SerializeField] private bool inverseLook; // true이면 마우스 상하 반전, false이면 정상


    // Start is called before the first frame update
    void Start()
    {
        // 마우스 커서를 제한 하는 파트
        Cursor.visible = false;                        // 메뉴, 옵션 버튼을 클릭 시 마우스 버튼이 보이게한다.
        Cursor.lockState = CursorLockMode.Locked;      // 마우스가 게임 창 밖으로 못나가게 해준다.
    }

    // Update is called once per frame
    void Update()
    {
        float inverseValue = inverseLook ? -1 : 1;   // inverseValue - inverseLook Bool값에 따라 마우스 회전을 변경할 수 있게 된다.

        float rotationX = Input.GetAxisRaw("Mouse X");
        float rotationY = Input.GetAxisRaw("Mouse Y");

        mouseInput = new Vector2(rotationX, rotationY) * mouseSensitvity;

        // 좌우 회전
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + mouseInput.x ,
            transform.rotation.eulerAngles.z);                       // Quaternion.Euler 함수 ( 매개변수로 x,y,z에 각 축의 0 ~360 회전 수치를 입력하면) 그 수치만큼 회전한 쿼터니언 각도를 반환한다.

        // 상하 회전

        verticalRot -= mouseInput.y;
        verticalRot = Mathf.Clamp(verticalRot, -limitAngle, limitAngle);             // 첫번째 인자로 들어간 값이 최소 최대 값을 넘어서지 않게 해준다.
             
        viewPort.rotation = Quaternion.Euler(verticalRot,
            viewPort.rotation.eulerAngles.y,
            viewPort.rotation.eulerAngles.z);
    }
}
