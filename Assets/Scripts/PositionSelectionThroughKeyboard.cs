using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PositionSelectionThroughKeyboard : MonoBehaviour
{
    EventSystem mEventSystem;   // SetSelectedGameObject를 써서 현재 선택된 오브젝트가 뭔지 알아야 하므로...
    Vector3 changeScale;        // 선택된 버튼의 사이즈를 조작하기 위하여...

    GameObject m1pButton;
    GameObject m2pButton;
    
    bool isSelected1p;
    bool isSelected2p;
    int selectedPosition;       // 현재 선택된 포지션을 PlayerPrefs.SetInt로 넘기기 위한 변수...



    private void Start()
    {
        mEventSystem = EventSystem.current;
        m1pButton = GameObject.Find("1p");
        m2pButton = GameObject.Find("2p");
        changeScale = new Vector3(0.1f, 0.1f, 0.1f);
        isSelected1p = isSelected2p = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isSelected1p)
        {
            if (isSelected2p)
            {
                m2pButton.GetComponent<RectTransform>().localScale -= changeScale;
                isSelected2p = false;
            }

            isSelected1p = true;
            mEventSystem.SetSelectedGameObject(m1pButton);
            m1pButton.GetComponent<RectTransform>().localScale += changeScale;
            selectedPosition = 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isSelected2p)
        {
            if (isSelected1p)
            {
                m1pButton.GetComponent<RectTransform>().localScale -= changeScale;
                isSelected1p = false;
            }

            isSelected2p = true;
            mEventSystem.SetSelectedGameObject(m2pButton);
            m2pButton.GetComponent<RectTransform>().localScale += changeScale;
            selectedPosition = 2;
        }
    }

    public void SelectComplete()
    {
        SceneManager.LoadScene("Character Select");
        PlayerPrefs.SetInt("position", selectedPosition);
    }
}
