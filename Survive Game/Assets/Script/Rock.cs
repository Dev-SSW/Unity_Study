using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;   //���� ü��

    [SerializeField]
    private float destroyTime; //���� ���� �ð�

    [SerializeField]
    private SphereCollider col; //��ü �ݶ��̴� ��Ȱ��ȭ

    //�ʿ��� ���� ������Ʈ
    [SerializeField]
    private GameObject go_rock; //�Ϲ� ����
    [SerializeField]
    private GameObject go_debris; //���� ����
    [SerializeField]
    private GameObject go_effect_prefabs;

    [SerializeField]
    private AudioSource audiosource;
    [SerializeField]
    private AudioClip effect_sound;
    [SerializeField]
    private AudioClip effect_sound2;

    public void Mining()
    {
        audiosource.clip = effect_sound;
        audiosource.Play();
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone,destroyTime);

        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }
    private void Destruction()
    {
        audiosource.clip = effect_sound2;
        audiosource.Play();
        col.enabled = false;
        Destroy(go_rock);
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
