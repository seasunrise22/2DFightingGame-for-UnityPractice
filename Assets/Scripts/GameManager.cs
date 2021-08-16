using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 일반적인 게임 제어를 위한 기능들을 모아둔 스크립트
 */
public class GameManager : MonoBehaviour
{
    private int selectedPlayerIdx;              //이전 캐릭터 셀렉트 씬에서 넘어 온 플레이어가 선택한 캐릭터 인덱스
    private int selectedEnemyIdx;               //이전 캐릭터 셀렉트 씬에서 넘어 온 적 캐릭터 인덱스
    private GameObject characterPrefabLists;    //캐릭터 프리펩이 담겨져 있는 리스트 'CharacterPrefabs' 오브젝트

    private void Awake()
    {
        characterPrefabLists = GameObject.Find("CharacterPrefabs");     //변수에 실제 캐릭터 프리펩이 담긴 리스트 오브젝트를 연결한다.

        //선택된 플레이어 캐릭터와 적 캐릭터 인덱스를 가져다가 변수에 넣는다.
        selectedPlayerIdx = PlayerPrefs.GetInt("CharacterSelected");
        selectedEnemyIdx = PlayerPrefs.GetInt("EnemySelected");
    }

    private void Start()
    {
        //선택된 플레이어 캐릭터에게는 'Player' 태그를, 선택된 적 캐릭터에게는 'Opponent' 태그를 부착한다.
        characterPrefabLists.transform.GetChild(selectedPlayerIdx).tag = "Player";
        characterPrefabLists.transform.GetChild(selectedEnemyIdx).tag = "Opponent";
    }
}
