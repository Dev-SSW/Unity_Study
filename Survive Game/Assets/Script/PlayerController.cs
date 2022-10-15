using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float crouchSpeed;

    //���� ����
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;
    
    //������ üũ ����
    private Vector3 lastPos;

    //�ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY; //���̴°� Y�� ( ���� �� ����ϴ� �� )
    private float originPosY; // ������� ���ư� �� �����ϴ� ���� (ó�� ����)
    private float applyCrouchPosY; // ������ ���� �־��ִ� ���� ũ��ġ������ ������ ����
   
    //�� ���� ����
    private CapsuleCollider capsuleCollider;
   
    //�ΰ���
    [SerializeField]
    private float lookSensitivity; //ī�޶� �ΰ���
    
    //ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit; //ī�޶� 360�� ���� �ʰ� �Ѱ踦 ����
    private float currentCameraRotaionX = 0; //ó�� ī�޶� ������ �ٶ󺸰�
    
    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    [SerializeField]
    private GunController theGunController;
    private Crosshair theCrosshair;


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        //�ʱ�ȭ
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; // ī�޶� ���̴� ��ó�� �������ϱ� ������ theCamera ��� localPosition�� ��� �÷��̾� �ȿ� �ִ� ī�޶��� Y���� �����ؾ��ϱ� ������ 
        // �׳� �����ǿ� ��� ���尪�� �������� ī�޶� �������� 0�϶� ���� ���� ( �÷��̾� �ȿ� �ִ� ī�޶�� 0�϶� ĸ���� �߰� �κ� ��ġ)
        applyCrouchPosY = originPosY;
        

    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }
    void FixedUpdate()
    {
        MoveCheck();
    }
    //�ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    //�ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch); 
        /* if (isCrouch)
         *    is Crouch = false;
         * else
         *    is Crouch = true;
         *    �� ���� �ǹ�
        */
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY; //���� ī�޶� ��ġ
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY; //���� ī�޶� ��ġ
        }

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x , applyCrouchPosY , the Camera.transform.localPosition.z);
        //�����̵� �ϸ鼭 �ɾ����� ��� ��
        
    }
    //�ε巯�� �ɱ� ���� ����
    IEnumerator CrouchCoroutine() // ���� ó���� ���� ������� ��� �ڷ�ƾ, ���ÿ� ���� �ٷ� ����Ǵ°�
    {
        //�ڿ������� �ɱ⸦ ���� ���� �Լ� (���� �����̵� ����)
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY,applyCrouchPosY,0.02f); // pos y ���� applycrouchposy���� 0.3 �ӵ��� õõ�� �ɵ��� ������ Ex(1,2,1) �� ��� 1���� 2�� 1�� �����̱⶧���� �ٷ� �ɾƹ��� 
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 200 )
                break;
            yield return null; // null �� �ǹ� 1�����Ӹ��� ���� 
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f); // �����϶� 0.3f�� ������ �ϸ� 1�� 0.3���� ���� �� 0.0000000123 �̷��� 0�� ������ �ʱ� ������ 15�� �ݺ��ϰ� �ȵǸ� 0�� �������� ����
       
    }
    //���� üũ
    private void IsGround()
    {
        // 0.1f �缱���� �ణ �� �������� ��� �ؼ� ������ ���̵���
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //ĸ�� �ݶ��̴��� �ݸ�ŭ (bound extents y) Vector3.down �Ʒ� �������� Raycast �������� ���
        //���� ����ִ� ���� Ȯ���ϱ� ���� transform.down �� �ȵǴ� ������ ��ü�� ���� �ְų� �� ���������� ĸ�� �ݶ��̴��� �Ʒ� �κ����� �������� ��� ������ �ϴ� ���� ���� �� �� �����Ƿ� Vector3�� �̿��� ����
        theCrosshair.JumpingAnimation(!isGround);
    }
    //���� �õ�
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }    
    }
    //����
    private void Jump()
    {
        if (isCrouch) //���� ���¿��� ������ �������� ����
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }
    //�޸��� �õ�
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    //�޸��� ����
    private void Running()
    {
        if (isCrouch) //�ɾ� �ִٰ� �޸� �� ���� ���� ����
            Crouch();

        theGunController.CancelFineSight();
        


        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }
    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }
    //������ ����
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // ������ ����Ű�� ������ 1 ���� ����Ű�� ������ -1 �ȴ����� 0�� ���ϵ˴ϴ�. A D ����Ű�� ����
        float _moveDirZ = Input.GetAxisRaw("Vertical");  // ����

        Vector3 _moveHorizontal = transform.right * _moveDirX; //vector3 = (0,0,0)�� ���� ��
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //velocity �ӵ�; ��ֶ���� ���� ������ ���� ���ִ� �� �� (���� 1�� �������� ����ȭ�����ִ°�)
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
         }  
    }
    //���� ī�޶� ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotaionX -= _cameraRotationX;
        currentCameraRotaionX = Mathf.Clamp(currentCameraRotaionX, -cameraRotationLimit, cameraRotationLimit); // 45 -45 ���̿� �����ǰ�

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotaionX, 0f, 0f); //local ���ñ�� Rotation �� x , y , z ����
    }
    //�¿� ī�޶� ȸ��
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X"); // ���콺�� ���� �������� x ���Ʒ��� y������ Rotation�� �׵��� ���� ���־� ��Ʃ������� Ȯ���غ��� ��
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); //����Ƽ�� ���������� ȸ���� quaternion���� ���
        
    }
    
}
