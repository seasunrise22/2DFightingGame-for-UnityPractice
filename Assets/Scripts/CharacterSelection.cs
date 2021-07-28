using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterList; // 캐릭터들을 담을 배열
    private int index;                  // 어떤 캐릭터를 표시할 것인가 결정짓기 위한 인덱스

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected", 0);

        // 이 스크립트가 부착 된 오브젝트의 자식 오브젝트들 숫자만큼의 크기를 가진 배열 생성
        characterList = new GameObject[transform.childCount];   

        // 배열에 자식 오브젝트들을 하나하나 담는다.
        for (int i = 0; i < transform.childCount; i++)          
            characterList[i] = transform.GetChild(i).gameObject;

        // 초기에 각 오브젝트들을 액티브 해제해둔다.
        foreach(GameObject go in characterList)
            go.SetActive(false);

        // 선택된 적이 있는 저장된 인덱스의 오브젝트는 켜둔다. 게임이 꺼져도 이 선택은 로컬에 저장된다.
        if (characterList[index])
            characterList[index].SetActive(true);
    }

    // 왼쪽 버튼을 눌렀을 때
    public void ToggleLeft()
    {
        characterList[index].SetActive(false); ; // 왼쪽 버튼을 누르면 현재 인덱스에 해당하는 오브젝트는 당연히 비활성화 해야함.

        index--; // 왼쪽 버튼을 누른거니까 인덱스는 하나 감소해야 함.

        // 첫 오브젝트(0)에서 왼쪽으로 이동하면 마지막 인덱스로 이동.
        if (index < 0)
            index = characterList.Length - 1;

        // 하나 감소한 인덱스에 위치한 오브젝트 활성화.
        characterList[index].SetActive(true);    
    }

    // 오른쪽 버튼을 눌렀을 때
    public void ToggleRight()
    {
        characterList[index].SetActive(false); ; // 오른쪽 버튼을 누르면 현재 인덱스에 해당하는 오브젝트는 당연히 비활성화 해야함.

        index++; // 오른쪽 버튼을 누른거니까 인덱스는 하나 감소해야 함.

        // 마지막 인덱스보다 값이 커지면 첫 인덱스로 이동해야 함.
        if (index == characterList.Length)
            index = 0;

        // 인덱스 하나 증가한 위치에 있는 오브젝트 활성화.
        characterList[index].SetActive(true);
    }

    // 선택 버튼을 눌렀을 때
    public void ConfirmButton()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
        SceneManager.LoadScene("Battle");        
    }
}
