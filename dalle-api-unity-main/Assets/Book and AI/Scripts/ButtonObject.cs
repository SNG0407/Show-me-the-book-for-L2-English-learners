using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public GameObject bookUI;
    public GameObject bookReaderUI;

    private bool bookOpen = false;

    private void Awake()
    {
        if (bookUI != null)
        {
            bookUI.SetActive(false);

        }
        if (bookReaderUI != null)
        {
            bookReaderUI.SetActive(false);

        }
    }
    private void OnMouseUpAsButton()
    {
        Debug.Log("Show the book");
        bookOpen =! bookOpen;
        if(bookOpen == true)
        {
            if (bookUI != null)
            {
                bookUI.SetActive(true);
                //PlayerMoveCode.instance.Is_UI_On = true;
            }
            //Time.timeScale = 0;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
        else
        {
            if (bookUI != null)
            {
                bookUI.SetActive(false);
                //PlayerMoveCode.instance.Is_UI_On = false;

            }
            //Time.timeScale = 1;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = true;
        }

    }

    public void ReadBtn()
    {
        if (bookReaderUI != null)
        {
            bookUI.SetActive(false);
            bookReaderUI.SetActive(true);
            //PlayerMoveCode.instance.Is_UI_On = true;
        }
    }
    public void CloseReaderBtn()
    {
        if (bookReaderUI != null)
        {
            bookUI.SetActive(true);
            bookReaderUI.SetActive(false);
            //PlayerMoveCode.instance.Is_UI_On = true;
        }
    }
}
