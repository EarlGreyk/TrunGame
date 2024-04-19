using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _Result;
    [SerializeField]
    TextMeshProUGUI _Time;
    [SerializeField]
    TextMeshProUGUI _Kill;
    [SerializeField]
    TextMeshProUGUI _PR;


    void Start()
    {
        
        float Time = GameManager.instance.GameTime;
        int hour = 0;
        int minute = 0;
        int second = 0;
        bool Victory = false;
        if (GameManager.instance.Player != null)
            Victory = true;

        hour = (int)Time / 3600;
        Time = Time % 3600;
        minute = (int)Time / 60;
        Time = (Time % 60);
        second = (int)Time;
        if (Victory)
            _Result.text = "½Â¸®";
        else
            _Result.text = "ÆÐ¹è";
        _Time.text = hour.ToString() +" : " +minute.ToString()+ " : " + second.ToString();

        _Kill.text = GameManager.instance.KillCount.ToString();

        _PR.text = GameManager.instance.PRCount.ToString();
    }

  
}
