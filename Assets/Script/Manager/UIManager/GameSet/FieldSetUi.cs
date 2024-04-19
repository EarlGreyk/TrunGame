using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameSetManager;

public class FieldSetUi : MonoBehaviour
{



    [SerializeField]
    private Image _FieldImage;

    [SerializeField]
    private TextMeshProUGUI _Textdifficulty;
    [SerializeField]
    private TextMeshProUGUI _TextMaxStage;

    [SerializeField]
    private TextMeshProUGUI _TextName;


    public void FeildDataSet(FieldDataBase fieldData)
    {

     
        if (fieldData != null)
        {
            _FieldImage.sprite = fieldData.FieldSprite;
            _Textdifficulty.text = fieldData.Difficult.ToString();
            _TextMaxStage.text = fieldData.MaxStage.ToString();
            _TextName.text = fieldData.FieldName.ToString();
        }
    }
}
