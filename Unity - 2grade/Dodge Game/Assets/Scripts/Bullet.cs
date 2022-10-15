using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody bulletRigidbody;


    // Start is called before the first frame update
    void Start()
    {   // 게임오브젝트에서 rigidbody 컴포넌트를 찾아 bulletrigidbody 에 할당
        bulletRigidbody = GetComponent<Rigidbody>();
        //리지드 바디의 속도 = 앞쪽방향 * 이동속력
        bulletRigidbody.velocity = transform.forward * speed;
        // 3초 후 사라진다.
        Destroy(gameObject, 3f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            //   상대방 게임 오브젝트에서 Playercontroller  컴포넌트 가져오기

            PlayerController playerController = other.GetComponent<PlayerController>();
            // 상대방으로부터 플레이어컨트롤 컴포넌트를 가져오는 데 성공했다면
            if (playerController != null)
            {
                //상대방 플레이어 컨트롤러 컴포넌트의 DIe()메서드 실행
                playerController.Die();
            }
        }
    }
}
    