using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    public float speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 수평축과 수직축의 입력값을 감지하여 저장
        float xinput = Input.GetAxis("Horizontal");
        float zinput = Input.GetAxis("Vertical");
        //실제이동 속도를 입력값과 이동 속력을 사용해 결정
        float xSpeed = xinput * speed;
        float zSpeed = zinput * speed;
        //Vector3 의 속도를 설정
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidbody.velocity = newVelocity;

       /*  if (Input.GetKey(KeyCode.UpArrow) == true)
        {
         }
       if (Input.GetKey(KeyCode.DownArrow) == true)
        {
            playerRigidbody.AddForce(0f, 0f, -speed);
       }
       if (Input.GetKey(KeyCode.RightArrow) == true)
       {
           playerRigidbody.AddForce(speed, 0f, 0f);
       }
        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
        playerRigidbody.AddForce(-speed, 0f, 0f);
        }*/
    }
        public void Die()
    { 
        gameObject.SetActive(false); // 자신의 게임 오브젝트를 비활성화
    }
    
}
