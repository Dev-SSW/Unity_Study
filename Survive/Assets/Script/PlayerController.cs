using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float WalkSpeed; //°È±â
   
    
    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();       
    }

    // Update is called once per frame
    void Update()
    {
        Move();       
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 MovePosition = (_moveHorizontal + _moveVertical).normalized * WalkSpeed;
        myRigid.MovePosition(transform.position + MovePosition * Time.deltaTime);
    }


}
