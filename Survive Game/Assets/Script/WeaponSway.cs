using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    //기존 위치
    private Vector3 originPos;
    //현재 위치
    private Vector3 currentPos;
    //Sway 한계
    [SerializeField]
    private Vector3 limitPos;
    //정조준 Sway
    [SerializeField]
    private Vector3 fineSightLimitPos;
    //부드러운 움직임 정도
    [SerializeField]
    private Vector3 smoothSway;
    //필요한 컴포넌트
    [SerializeField]
    private GunController thegunController;

    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition; //이 스크립트가 붙어있는 자기 자신의 위치를 대입
    }

    // Update is called once per frame
    void Update()
    {
        TrySway();
    }

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) //상하좌우가 움직였을때 (카메라)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }
    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!thegunController.isFineSightMode)
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
                       originPos.z); // z 값은 무기마다 다르기 때문에 originpos// Set 함수는 X Y Z 따로따로 설정할 수 있음 // Lerp 부드러움 주기 // Clamp는 최소 최대를 설정하여 범위를 넘기않도록 함
            
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                       originPos.z); // z 값은 무기마다 다르기 때문에 originpos// Set 함수는 X Y Z 따로따로 설정할 수 있음 // Lerp 부드러움 주기 // Clamp는 최소 최대를 설정하여 범위를 넘기않도록 함
            
        }
        transform.localPosition = currentPos;
    }
    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
