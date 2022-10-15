using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    //���� ��ġ
    private Vector3 originPos;
    //���� ��ġ
    private Vector3 currentPos;
    //Sway �Ѱ�
    [SerializeField]
    private Vector3 limitPos;
    //������ Sway
    [SerializeField]
    private Vector3 fineSightLimitPos;
    //�ε巯�� ������ ����
    [SerializeField]
    private Vector3 smoothSway;
    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController thegunController;

    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition; //�� ��ũ��Ʈ�� �پ��ִ� �ڱ� �ڽ��� ��ġ�� ����
    }

    // Update is called once per frame
    void Update()
    {
        TrySway();
    }

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) //�����¿찡 ���������� (ī�޶�)
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
                       originPos.z); // z ���� ���⸶�� �ٸ��� ������ originpos// Set �Լ��� X Y Z ���ε��� ������ �� ���� // Lerp �ε巯�� �ֱ� // Clamp�� �ּ� �ִ븦 �����Ͽ� ������ �ѱ�ʵ��� ��
            
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                       originPos.z); // z ���� ���⸶�� �ٸ��� ������ originpos// Set �Լ��� X Y Z ���ε��� ������ �� ���� // Lerp �ε巯�� �ֱ� // Clamp�� �ּ� �ִ븦 �����Ͽ� ������ �ѱ�ʵ��� ��
            
        }
        transform.localPosition = currentPos;
    }
    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
