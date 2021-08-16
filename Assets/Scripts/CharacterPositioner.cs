using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPositioner : MonoBehaviour
{
    private int selectedPosition;           // 1p2p 포지션 선택씬에서 넘어 온 선택 된 포지션. 1P인가 2P인가.
    private int selectedCharacterIndex;     // 이전 캐릭터 선택씬에서 넘어 온 선택 된 캐릭터 인덱스.
    private int selectedEnemyIndex;         // 이전 캐릭터 선택씬에서 넘어 온 선택 된 적 인덱스.
    private GameObject[] characterPrefabs;  // 히트박스랑 애니메이션 씌워진 실제 게임에 쓸 캐릭터 프리팹을 넣을 배열.
    private Vector3 leftCharacterPosition;  // 1P 캐릭터 위치.
    private Vector3 RightCharacterPosition; // 2P 캐릭터 위치.
    private GameObject player;              // 플레이어가 선택한 캐릭터 오브젝트를 담을 변수.
    private GameObject enemy;               // 적 오브젝트를 담을 변수(플레이어가 선택한 캐릭터 오브젝트 이외).

    private void Awake()
    {
        // 이전씬에서 선택된 포지션과 캐릭터 인덱스를 받아서 변수에 저장한다.
        selectedPosition = PlayerPrefs.GetInt("position");
        selectedCharacterIndex = PlayerPrefs.GetInt("CharacterSelected");
        selectedEnemyIndex = PlayerPrefs.GetInt("EnemySelected");

        // 1p2p 캐릭터가 배치될 위치를 미리 변수로 지정해둠.
        leftCharacterPosition = new Vector3(-5f, -3f, 0f);   // 1P
        RightCharacterPosition = new Vector3(5f, -3f, 0f);   // 2P
    }

    private void Start()
    {
        characterPrefabs = new GameObject[transform.childCount];    // 캐릭터 프리팹을 담아둔 이 오브젝트의 자식 오브젝트의 숫자만큼의 배열 생성.

        // 생성된 배열의 방에 캐릭터를 하나하나 넣는다.
        for(int i=0; i<transform.childCount; i++)
            characterPrefabs[i] = transform.GetChild(i).gameObject;

        // 선택된 캐릭터를 깔끔하게 조작하기 위해 player 변수에 선택된 캐릭터의 프리팹을 할당.
        player = characterPrefabs[selectedCharacterIndex];
        enemy = characterPrefabs[selectedEnemyIndex];        

        // 일단 캐릭터들은 전부 비활성화해서 안 보이도록 해둔다.
        foreach (GameObject characters in characterPrefabs)
            characters.SetActive(false);

        // 그리고 선택된 캐릭터와 적 캐릭터만 활성화시킨다.
        player.SetActive(true);
        enemy.SetActive(true);

        // 선택된 포지션이 1P인가 2P인가에 따라 다른 포지션으로 선택된 캐릭터를 둔다.
        switch (selectedPosition)
        {         
            // 1P일경우
            case 1:
                player.transform.Translate(leftCharacterPosition);
                enemy.transform.Translate(RightCharacterPosition);                
                break;
            // 2P일경우
            case 2:
                player.transform.Translate(RightCharacterPosition);
                enemy.transform.Translate(leftCharacterPosition);
                break;
        }
    }    
}
