using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    private static UImanager _instance;

    public static UImanager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("현재 UImanager가 등록되어 있지 않습니다.");
                return null;
            }else
            {
                return _instance;
            }
        }
    }



    [SerializeField]
    private GameObject _WroldCanvas;
    public GameObject Wroldcanvas { get { return _WroldCanvas; } }
    [SerializeField]
    private GameObject _PrInterface;
    [SerializeField]
    private GameObject _PlayerInterface;
    [SerializeField]
    private GameObject _ShopIterface;
    [SerializeField]
    private GameObject _ResultInterface;
    public GameObject ResultInterface { get { return _ResultInterface; } }
    [SerializeField]
    private UI_Popup _EscInterface;
    [SerializeField]
    private Button _OptionButton;
    [SerializeField] 
    private Button _SaveQuitButton;
    [SerializeField]
    private GameObject _LogInterface;
    [SerializeField]
    private TextMeshProUGUI _LogText;
    [SerializeField]
    private Scrollbar _LogScrollbar;
    private Coroutine _Coroutine;
    [SerializeField]
    private UI_Popup _InventoryInterface;
    /// <summary>
    /// 인터페이스 팝업창 관리용.
    /// </summary>
    private Stack<UI_Popup> openPopups = new Stack<UI_Popup>();

    [SerializeField]
    private GameObject _CardPop;
    public GameObject CardPop { get { return _CardPop; } }
    [SerializeField]
    private GameObject _CardFocus;
    public GameObject CardFocus { get { return _CardFocus; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    public void UIStart()
    {
        StartCoroutine(ButtonInit());
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (openPopups.Count <= 0)
            { 
                OpenPopup(_EscInterface);
                GameManager.instance.Player.IsStop = true;
            }
            else
            {
                CloseLastOpenPopup();
                if(openPopups.Count <= 0) 
                {
                    GameManager.instance.Player.IsStop = false;
                }
            }
                
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!_InventoryInterface.Popup.activeSelf)
            {
                OpenPopup(_InventoryInterface);
                GameManager.instance.Player.IsStop = true;
            }else
            {
                ClosePopup(_InventoryInterface);
                GameManager.instance.Player.IsStop = false;
            }
        }
            
        
    }


    /// <summary>
    /// Ui창 팝업관리
    /// </summary>
    /// <param name="popup"></param>
    public void CheckPopup(UI_Popup popup)
    {
        popup.OnButtonClicked();
    }

    public void OpenPopup(UI_Popup popup)
    {
        if(popup != null)
        {
            popup.Open();
            //스택 팝업에 넣기. 열려있는 모든 팝업창을 해당 목록에 넣어야함.
            openPopups.Push(popup);
            
        }
    }
   
  


    public void ClosePopup(UI_Popup popup) 
    {
        //팝업이 존재하고 현재 열려 있는 openpopus창 목록에 존재함.
        if(popup != null && openPopups.Contains(popup)) 
        {
            popup.Close();
            openPopups.Pop();
        }
    }

    private void CloseLastOpenPopup()
    {
        if (openPopups.Count>0)
        {
            //현재 가장 최근에 열었던 목록을 반환함.
            ClosePopup(openPopups.Peek());
        }
    }

    //아래의 목록은 언젠가는 처리를 해야합니다.
    //분할O
    public void InterFaceActive()
    {
        _PrInterface.SetActive(true);
        _PlayerInterface.SetActive(true);
        _ShopIterface.SetActive(true);
    }
    public void SaveStart()
    {
        SaveLoadManager.instance.Save();
    }
    public void EndQuit()
    {
        SceanLoad.Instance.LoadStartSCean();
    }
    IEnumerator ButtonInit()
    {
        yield return new WaitForSeconds(1.0f);
        UI_Popup uI_Popup = null;
        uI_Popup = OptionManager.instance.ButtonPopup.GetComponent<UI_Popup>();
        _OptionButton.onClick.AddListener(() => OpenPopup(uI_Popup));
        yield return null;
    }
    
    public void LogTextSet(string text)
    {
        _LogInterface.SetActive(true);
        _LogText.text += text + "\n";
        if(_LogScrollbar.value != 0)
            _LogScrollbar.value = 0;

        if(_Coroutine == null)
            _Coroutine = StartCoroutine(Logdelete());
        else
            StopCoroutine( _Coroutine); _Coroutine = StartCoroutine(Logdelete());



    }
   

    IEnumerator Logdelete() 
    {
        yield return new WaitForSeconds(2f);
        _LogInterface.SetActive(false);
        _Coroutine = null;
        yield return null;
    }


}
