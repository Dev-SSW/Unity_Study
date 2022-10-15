using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject gameoverText;

    public Text timeText;

    public Text recordText;

    public int Life;


    private float surviveTime;

    private bool isGameover;

    // Start is called before the first frame update
    void Start()
    {
        //생존시간과 게임오버 상태 초기화

        surviveTime = 0;

        Life = 3;

       
        isGameover = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameover)
        {
            //생존시간 갱신
            surviveTime += Time.deltaTime;
            // 갱신한 생존 시간을 timetext 컴포넌트를 이용해 표시
            timeText.text = "TIme : " + (int)surviveTime + "\r\nLife : " + Life;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        
        }
        
}
    //현재 게임을 게임오버 상태로 변경하는 메서드
    public void EndGame()
    {
        timeText.text = "Time : " + (int)surviveTime + "\r\n Life : " + 0;
        //현재 상태를 게임 오버 상태로 전환
        isGameover = true;
        // 게임오버, 레코드 텍스트 게임 오브젝트를 활성화
        gameoverText.SetActive(true);

        //besttime 키로 저장된 이전까지의 최고기록 가져오기
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        
        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);

        }
        recordText.text = "BestTIme : " + (int)bestTime;






    }
}
