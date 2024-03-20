using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewCamera : MonoBehaviour
{
    Vector3 offset;
    [SerializeField] Transform playerTr;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTr.position;  //  플레이어가 카메라를 보는 방향과 크기 플레이어 더하면 => 카메라 위치
    }

    // PlayerController Update에서 이동 시키고, LateUpdate 카메라를 움직여 준다.
    void LateUpdate()
    {
        transform.position = playerTr.position + offset;  // 카메라의 위치 = 플레이어가 이동한 위치 + 카메라와 플레이어가 고정되어야할 방향과 거리

        offset = transform.position - playerTr.position;  // 플레이어가 이동함에 따라 변화한 offset을 다시 갱신해준다.
    }
}
