using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterList; // 캐릭터들을 담을 배열

    private void Start()
    {
        // 이 스크립트가 부착 된 오브젝트의 자식 오브젝트들 숫자만큼의 크기를 가진 배열 생성
        characterList = new GameObject[transform.childCount];   

        // 배열에 자식 오브젝트들을 하나하나 담는다.
        for (int i = 0; i < transform.childCount; i++)          
            characterList[i] = transform.GetChild(i).gameObject;

        // 초기에 각 오브젝트들을 액티브 해제해둔다.
        foreach(GameObject go in characterList)
            go.SetActive(false);

        // 최초 맨 처음 인덱스의 오브젝트는 켜둔다.
        if (characterList[0])
            characterList[0].SetActive(true);
    }
}
