using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    
public class UI_Popup : MonoBehaviour
{
    [SerializeField] private GameObject PopupCanvus;
    public GameObject Popup { get { return PopupCanvus; } }
    [SerializeField] private Animator PopupAnimation;


    //창 열릴때 호출. 
    public void Open()
    {
        if(PopupCanvus != null)
        {
            PopupCanvus.SetActive(true);
            SoundManager.instance.AudioPlay("Sound/Effect/Pop_Open_Close");

            if (PopupAnimation != null)
            {
                PopupAnimation.SetTrigger("Open");
            }
        }
    }

    //창 닫을때 호출
    public void Close()
    {
        if (PopupCanvus != null)
        {
            PopupCanvus.SetActive(false);
            SoundManager.instance.AudioPlay("Sound/Effect/Pop_Open_Close");

            if (PopupAnimation != null)
            {
                PopupAnimation.SetTrigger("Close");
            }
        }
    }

    public void OnButtonClicked()
    {

    }

}
