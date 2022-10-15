using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    //Ȱ��ȭ ����
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }
    void Update()
    {
        if (isActivate)
            TryAttack();

    }

    protected override IEnumerator HitCoroutine()
    {
        {
            while (isSwing) //���� ����������
            {
                if (CheckObject())
                {
                    //�浹����
                    isSwing = false;
                    Debug.Log(hitinfo.transform.name); //�浹 ������ �̸��� ������
                }
                yield return null;

            }
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closehand)
    {
        base.CloseWeaponChange(_closehand);
        isActivate = true;
    }
}