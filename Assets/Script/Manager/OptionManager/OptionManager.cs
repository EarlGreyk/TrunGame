using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    static public OptionManager instance;
    // Start is called before the first frame update
    [SerializeField]
    private UI_Popup _ButtonPopup;
    public UI_Popup ButtonPopup {get { return _ButtonPopup; }}


    [SerializeField]
    private Button _Button;
    public Button Button { get { return _Button; }}

    private void Awake()
    {
        if (instance != null)
        {
            instance.ButtonSet(this.Button);
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void ButtonSet(Button button)
    {
        _Button = button;
        _Button.onClick.AddListener(_ButtonPopup.Open);
    }
   
   

}
