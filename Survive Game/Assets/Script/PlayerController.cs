using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float crouchSpeed;

    //상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;
    
    //움직임 체크 변수
    private Vector3 lastPos;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY; //숙이는건 Y값 ( 숙일 때 사용하는 값 )
    private float originPosY; // 원래대로 돌아갈 값 저장하는 변수 (처음 높이)
    private float applyCrouchPosY; // 각각의 값을 넣어주는 변수 크러치포스랑 오리진 포스
   
    //땅 착지 여부
    private CapsuleCollider capsuleCollider;
   
    //민감도
    [SerializeField]
    private float lookSensitivity; //카메라 민감도
    
    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit; //카메라가 360도 돌지 않게 한계를 설정
    private float currentCameraRotaionX = 0; //처음 카메라가 정면을 바라보게
    
    //필요한 컴포넌트
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
        //초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; // 카메라가 숙이는 것처럼 보여야하기 때문에 theCamera 사용 localPosition의 경우 플레이어 안에 있는 카메라의 Y값을 조정해야하기 때문에 
        // 그냥 포지션에 경우 월드값을 기준으로 카메라가 움직여서 0일때 땅에 박힘 ( 플레이어 안에 있는 카메라는 0일때 캡슐의 중간 부분 위치)
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
    //앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch); 
        /* if (isCrouch)
         *    is Crouch = false;
         * else
         *    is Crouch = true;
         *    와 같은 의미
        */
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY; //숙인 카메라 위치
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY; //원래 카메라 위치
        }

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x , applyCrouchPosY , the Camera.transform.localPosition.z);
        //순간이동 하면서 앉아지는 방식 ↗
        
    }
    //부드러운 앉기 동작 실행
    IEnumerator CrouchCoroutine() // 병렬 처리를 위해 만들어진 기능 코루틴, 동시에 실행 바로 실행되는거
    {
        //자연스러운 앉기를 위해 만든 함수 (원랜 순간이동 됐음)
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY,applyCrouchPosY,0.02f); // pos y 에서 applycrouchposy까지 0.3 속도로 천천히 앉도록 움직임 Ex(1,2,1) 일 경우 1에서 2로 1씩 움직이기때문에 바로 앉아버림 
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 200 )
                break;
            yield return null; // null 의 의미 1프레임마다 실행 
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f); // 복안일때 0.3f로 느리게 하면 1을 0.3으로 나눈 값 0.0000000123 이렇게 0이 나오지 않기 때문에 15번 반복하고 안되면 0이 나오도록 설정
       
    }
    //지면 체크
    private void IsGround()
    {
        // 0.1f 사선에서 약간 더 레이저를 쏘게 해서 오차를 줄이도록
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //캡슐 콜라이더의 반만큼 (bound extents y) Vector3.down 아래 방향으로 Raycast 레이저를 쏜다
        //땅에 닿아있는 것을 확인하기 위해 transform.down 이 안되는 이유는 물체가 누워 있거나 떠 있을때에도 캡슐 콜라이더의 아래 부분으로 레이저를 쏘기 때문에 하늘 위로 날라 갈 수 있으므로 Vector3를 이용해 고정
        theCrosshair.JumpingAnimation(!isGround);
    }
    //점프 시도
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }    
    }
    //점프
    private void Jump()
    {
        if (isCrouch) //앉은 상태에서 점프시 앉은상태 해제
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }
    //달리기 시도
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
    //달리기 실행
    private void Running()
    {
        if (isCrouch) //앉아 있다가 달릴 때 앉음 상태 해제
            Crouch();

        theGunController.CancelFineSight();
        


        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }
    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }
    //움직임 실행
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // 오른쪽 방향키를 누르면 1 왼쪽 방향키를 누르면 -1 안누르면 0이 리턴됩니다. A D 방향키도 가능
        float _moveDirZ = Input.GetAxisRaw("Vertical");  // 상하

        Vector3 _moveHorizontal = transform.right * _moveDirX; //vector3 = (0,0,0)을 가진 값
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //velocity 속도; 노멀라이즈를 통해 방향을 같에 해주는 것 뿐 (합이 1이 나오도록 정규화시켜주는거)
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
    //상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotaionX -= _cameraRotationX;
        currentCameraRotaionX = Mathf.Clamp(currentCameraRotaionX, -cameraRotationLimit, cameraRotationLimit); // 45 -45 사이에 고정되게

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotaionX, 0f, 0f); //local 뭐시기는 Rotation 값 x , y , z 생각
    }
    //좌우 카메라 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X"); // 마우스는 왼쪽 오른쪽이 x 위아래가 y이지만 Rotation은 그딴거 없고 비주얼 스튜디오에서 확인해보면 됨
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); //유니티는 내부적으로 회전을 quaternion으로 사용
        
    }
    
}
