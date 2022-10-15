using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = false;

   
    void Update()
    {
        if (isActivate)
            TryAttack();

    }

    protected override IEnumerator HitCoroutine()
    {
        {
            while (isSwing) //팔을 접을때까지
            {
                if (CheckObject())
                {
                    //충돌했음
                    isSwing = false;
                    Debug.Log(hitinfo.transform.name); //충돌 했으면 이름을 가져옴
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