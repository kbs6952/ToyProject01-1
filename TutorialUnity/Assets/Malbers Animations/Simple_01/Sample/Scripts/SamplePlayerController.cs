using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;               // 플레이어의 이동 속도

    private Rigidbody rigidbody;                            // 플레이어 물리 구현을 위한 컴포넌트
   
    [SerializeField] private GameObject powerIndicator;     // 파워업 상태를 확인시켜 주기 위한 게임 오브젝트

    public bool IsPowerUp = false;

    [SerializeField] private float powerUpDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;

        rigidbody.AddForce(direction * moveSpeed);

        if (IsPowerUp)
        {
            Invoke("PowerUpTimeOver", powerUpDuration);   // 7f : 파워업이 지속되기를 원하는 시간 -> 변수화 시켜서 데이터를 변경시키거나 조합할 수 있습니다.
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            Debug.Log($"{other.gameObject.name}");
            Destroy(other.gameObject);               // 오브젝트를 먹었으므로 해당 오브젝트를 파괴한다.
            IsPowerUp = true;                        // 오브젝트를 먹었을 때 기능을 구현
            powerIndicator.SetActive(true);          // 오브젝트가 활성화 되는 코드
        }
    }

    void PowerUpTimeOver()
    {
        // 플레이어의 파워업 상태가 끝났음을 표시할 수 있는 코드 1줄을 작성해보세요. 
        IsPowerUp = false;
        powerIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    Debug.Log("적과 충돌하였음!");
        //    // 적과 충돌했을 때 적이 밖으로 더 잘 날라가게 해주는 기능을 추가해본다.

        //    Vector3 powerVector = (transform.position - collision.transform.position).normalized;    // 충돌(플레이어)가 Enemy 방향을 구한다 ( normalized를 통해 힘의 크기를 뺀 방향만 구할 수 있다.)
        //    Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();               // Enemy가 갖고 있는 Rigidbody를 참조해서 Enemy의 물리 효과를 구현할 수 있다.
        //    enemyRigidbody.AddForce(powerVector * pushPower, ForceMode.Impulse);                     // EnemyRigidbody. AddForce 함수를 이용해서 Enemy가 충돌할 때 더 크게 날라가도록 변경하였다.

        //}
    }

}
