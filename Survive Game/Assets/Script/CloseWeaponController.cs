using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{   //미완성 클래스 = 추상 클래스
    

    // 현재 장착된 hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;
    //공격중
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitinfo; //닿았는지 확인하는 변수

    // Update is called once per frame
    
    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //코루틴 실행
                StartCoroutine(AttackCoroutine()); //마우스 클릭하자마자 isAttack이 트루가 되어서 한번 클릭하면 다시 누르는걸 막아줌 ( 여러번 공격을 막기 위해)
            }
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttack = true; //한번 공격하고
        currentCloseWeapon.anim.SetTrigger("Attack"); //상태변수 트리거 어택을 발동시킨다
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA); // 딜레이를 받고 다시 공격 할 수 있도록 false
        isSwing = true; //팔을 폈다
        //공격 활성화 시점
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;//팔을 접었다
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB); //필을 완전히 접으면 공격 할 수 있게 이렇게 하면 어택 딜레이마다 공격 가능
        isAttack = false;
    }




    //미완성 = 추상 코루틴
    protected abstract IEnumerator HitCoroutine();
    


    protected bool CheckObject() //맞았는지 확인하는 함수
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }
    //가상 함수 = 완성 함수이지만, 추가 편집이 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon _closehand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false); //기존의 총을 사라지게 만듦
        }
        currentCloseWeapon = _closehand;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
        
    }
}
