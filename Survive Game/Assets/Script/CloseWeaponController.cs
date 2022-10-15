using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{   //�̿ϼ� Ŭ���� = �߻� Ŭ����
    

    // ���� ������ hand�� Ÿ�� ����
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;
    //������
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitinfo; //��Ҵ��� Ȯ���ϴ� ����

    // Update is called once per frame
    
    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //�ڷ�ƾ ����
                StartCoroutine(AttackCoroutine()); //���콺 Ŭ�����ڸ��� isAttack�� Ʈ�簡 �Ǿ �ѹ� Ŭ���ϸ� �ٽ� �����°� ������ ( ������ ������ ���� ����)
            }
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttack = true; //�ѹ� �����ϰ�
        currentCloseWeapon.anim.SetTrigger("Attack"); //���º��� Ʈ���� ������ �ߵ���Ų��
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA); // �����̸� �ް� �ٽ� ���� �� �� �ֵ��� false
        isSwing = true; //���� ���
        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;//���� ������
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB); //���� ������ ������ ���� �� �� �ְ� �̷��� �ϸ� ���� �����̸��� ���� ����
        isAttack = false;
    }




    //�̿ϼ� = �߻� �ڷ�ƾ
    protected abstract IEnumerator HitCoroutine();
    


    protected bool CheckObject() //�¾Ҵ��� Ȯ���ϴ� �Լ�
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }
    //���� �Լ� = �ϼ� �Լ�������, �߰� ������ ������ �Լ�
    public virtual void CloseWeaponChange(CloseWeapon _closehand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false); //������ ���� ������� ����
        }
        currentCloseWeapon = _closehand;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
        
    }
}
