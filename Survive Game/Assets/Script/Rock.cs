using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;   //바위 체력

    [SerializeField]
    private float destroyTime; //파편 제거 시간

    [SerializeField]
    private SphereCollider col; //구체 콜라이더 비활성화

    //필요한 게임 오브젝트
    [SerializeField]
    private GameObject go_rock; //일반 바위
    [SerializeField]
    private GameObject go_debris; //깨진 바위
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
