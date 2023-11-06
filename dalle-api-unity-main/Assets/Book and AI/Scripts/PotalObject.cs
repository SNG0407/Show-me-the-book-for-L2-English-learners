using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalObject : MonoBehaviour
{
    public GameObject EnterRoomUI;
    public GameObject bookReaderUI;

    private bool bookShelfOpen = false;

    private void Awake()
    {
        if (EnterRoomUI != null)
        {
            EnterRoomUI.SetActive(false);

        }
      
    }
    private void OnMouseUpAsButton()
    {
        Debug.Log("Shall we enter the room?");
        bookShelfOpen =! bookShelfOpen;
        if(bookShelfOpen == true)
        {
            if (EnterRoomUI != null)
            {
                EnterRoomUI.SetActive(true);
                //PlayerMoveCode.instance.Is_UI_On = true;
            }
            //Time.timeScale = 0;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
        else
        {
            if (EnterRoomUI != null)
            {
                EnterRoomUI.SetActive(false);
                //PlayerMoveCode.instance.Is_UI_On = false;

            }
            //Time.timeScale = 1;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = true;
        }

    }

    public void EnterBtn()
    {
        Debug.Log("Enter the room!");
         SceneManager.LoadScene("HarryPotterRoom");
        // if (bookReaderUI != null)
        // {
        //     EnterRoomUI.SetActive(false);
        //     bookReaderUI.SetActive(true);
        //     //PlayerMoveCode.instance.Is_UI_On = true;
        // }
    }
    public void CloseBtn()
    {
        if (bookReaderUI != null)
        {
            EnterRoomUI.SetActive(false);
            //PlayerMoveCode.instance.Is_UI_On = true;
        }
    }
}
