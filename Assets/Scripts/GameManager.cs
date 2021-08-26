using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 일반적인 게임 제어를 위한 기능들을 모아둔 스크립트
 */
public class GameManager : MonoBehaviour
{
    private int selectedPlayerIdx;              //이전 캐릭터 셀렉트 씬에서 넘어 온 플레이어가 선택한 캐릭터 인덱스
    private int selectedEnemyIdx;               //이전 캐릭터 셀렉트 씬에서 넘어 온 적 캐릭터 인덱스
    private GameObject characterPrefabLists;    //캐릭터 프리펩이 담겨져 있는 리스트 'CharacterPrefabs' 오브젝트

    private GameObject timerText;   // 체력바 사이 카운트 숫자를 제어하기 위한 변수

    // 게임 시간 초기값. 반올림한 값으로 표시되기 때문에 99.49999.. ~ 98.5000 까지가 1초임.
    // 99f로 시작해버리면 처음 1초는 1초가 아닌 0.5초가 되어버림. ex)98.999~98.5 까지만 99로 표시. 이후 98
    private float timeLeft = 99.5f;        

    private void Awake()
    {
        characterPrefabLists = GameObject.Find("CharacterPrefabs");     //변수에 실제 캐릭터 프리펩이 담긴 리스트 오브젝트를 연결한다.

        //선택된 플레이어 캐릭터와 적 캐릭터 인덱스를 가져다가 변수에 넣는다.
        selectedPlayerIdx = PlayerPrefs.GetInt("CharacterSelected");
        selectedEnemyIdx = PlayerPrefs.GetInt("EnemySelected");

        //선택된 플레이어 캐릭터에게는 'Player' 태그를, 선택된 적 캐릭터에게는 'Opponent' 태그를 부착한다.
        //주의!! 다른 오브젝트에서 태그를 가져다쓰는 코드를 Start에 넣어뒀으므로, 아래 태그를 붙이는 행위를 반드시 Awake에 넣어둬야 다른 오브젝트에서 태그를 가져다 쓸 수 있음.
        //Start에 태그를 붙여버리면 순서상 Start == Start가 되어서 다른 오브젝트에서 태그를 못 찾는 현상이 발생함. 
        characterPrefabLists.transform.GetChild(selectedPlayerIdx).tag = "Player";
        characterPrefabLists.transform.GetChild(selectedEnemyIdx).tag = "Enemy";
    }

    private void Start()
    {
        timerText = GameObject.Find("Timer");
    }

    private void Update()
    {
        timerText.GetComponent<Text>().text = (timeLeft).ToString("0"); // "0" = 정수형태로 변환. 반올림값임. ex) 98.5 = 99로 표시
        timeLeft -= Time.deltaTime;         
        if(timeLeft < 0.5f)
        {
            timerText.GetComponent<Text>().text = "끝";
        }
    }
}
