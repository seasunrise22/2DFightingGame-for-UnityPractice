using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PositionSelectionThroughKeyboard : MonoBehaviour
{
    EventSystem mEventSystem;
    GameObject m1pButton;
    GameObject m2pButton;
    Vector3 changeScale;
    bool isSelected1p;
    bool isSelected2p;

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
        }
    }

    public void SelectComplete()
    {
        SceneManager.LoadScene("Character Select");
    }
}
