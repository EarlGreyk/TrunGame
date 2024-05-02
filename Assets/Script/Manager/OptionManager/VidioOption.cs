using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class VidioOption : MonoBehaviour
{
    private FullScreenMode screenMode;
    [SerializeField]
    public Toggle _FullScreenBtn;
    [SerializeField]
    private TMP_Dropdown _ResolutionDropdown;
    List<Resolution> _Resolutions = new List<Resolution>();

    private int _ResolutionValue;
    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        

        //�ػ󵵰� ���������� ������ �� �ֵ��� Reverse�Լ� ����
        _Resolutions.AddRange(Enumerable.Reverse(Screen.resolutions));
        _ResolutionDropdown.options.Clear();
        int optionvalue = 0;
        foreach (Resolution resolution in _Resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = resolution.width + "x " + resolution.height;
            _ResolutionDropdown.options.Add(option);
            //���� �ػ󵵿� �°� �������
            if (resolution.width == Screen.width && resolution.height == Screen.height)
                _ResolutionDropdown.value = optionvalue;

            optionvalue++;

        }
        _FullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
        _ResolutionDropdown.RefreshShownValue();
    }

    //�ػ� ��ưŬ��
    public void OptionChange(int x )
    {
        Debug.Log(x);
        _ResolutionValue = x;
    }
    //��ü ȭ�� ����
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    //���� ����
    public void VidioBtnClick()
    {
        Screen.SetResolution(_Resolutions[_ResolutionValue].width, _Resolutions[_ResolutionValue].height, screenMode);
    }
}
