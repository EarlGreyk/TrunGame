using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameSetManager;

public class GameSetManager : MonoBehaviour
{
    //������ ���� ������ ����մϴ�.
    //���̵��� ���� ���� ������ ���⼭ �����մϴ�.
    //���� ���� ������ �Ѿ�� �ش� ������ ���� �Ѱ��ݴϴ�.
    //�ʵ带 �����ϰ� �÷��̾ �����մϴ�.
    //�� ����� �ʵ���� / �÷��̾� ���� ����� ���� �Ҽ��� �ֽ��ϴ�.
    public static GameSetManager Instance;
    public enum FieldType
    {
        Forest = 1, //��
        Desert  = 2//�縷
    }
    private FieldType _fieldType;
    public FieldType fdtype {get { return _fieldType; } set { _fieldType = value; } }
   
    public enum PlayerType
    {
        Knight = 1, 
        Mage = 2, 
        Archer = 3

            

    }
    private PlayerType _PlayerType;
    public PlayerType PType { get { return _PlayerType; } set { _PlayerType = value; } }


    [SerializeField]
    private List<GameObject> _BlockPrefabs;
    [SerializeField]
    private List<GameObject> _PlayerPrefabs;

    public List<GameObject> BlockPrefabs { get { return _BlockPrefabs; } }
    public List<GameObject> PlayerPrefabs {  get { return _PlayerPrefabs; } }


    
    
    //���̵�����
    private int _Difficult;
    public int Difficult { get { return _Difficult; } set { _Difficult = value; } }


    private int _Size;
    public int  Size {  get { return _Size; } set { _Size = value; } }

    [SerializeField]
    private PlayerSetUi _SetPlayerUi;
    [SerializeField]
    private FieldSetUi _SetFieldUi;

    private FieldDataBase _FieldDataBase;
    public FieldDataBase FieldDataBase { get { return _FieldDataBase; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //�ش� �޴����� ��� ������ε� �ı��� ��������.
        /////
        ///�ش� �Ʒ� ������ �÷��̾ ���� ���� �� �� �ֵ��� �����ϸ��, ���̵��� ���� �����ϰ� �ϸ��.
        //���� Ÿ�Լ���
        _fieldType = FieldType.Forest;
        _PlayerType = PlayerType.Knight;        

        FieldDataSet();
        //���� ������ �´� ���� ����.
        _SetPlayerUi.PlayerDataSet(_PlayerType);
        _SetFieldUi.FeildDataSet(_FieldDataBase);

    }

    public void NextPlayer()
    {
        _PlayerType++;
        int Count = System.Enum.GetValues(typeof(PlayerType)).Length;
        if((int)_PlayerType > Count )
        {
            _PlayerType = Enum.GetValues(typeof(PlayerType)).Cast<PlayerType>().First();
        }
        PlayerSetUi();

    }
    
    public void BeforePlayer()
    {
        _PlayerType--;

        if ((int)_PlayerType < 1)
        {
            _PlayerType = _PlayerType = Enum.GetValues(typeof(PlayerType)).Cast<PlayerType>().Last();
        }
        PlayerSetUi();


    }
    public void NextField()
    {
        _fieldType++;
        int Count = System.Enum.GetValues(typeof(FieldType)).Length;
        if ((int)_fieldType > Count)
        {
            _fieldType = Enum.GetValues(typeof(FieldType)).Cast<FieldType>().First();
        }
        FieldDataSet();
    }
    public void BeforeField()
    {
        _fieldType--;
        if ((int)_fieldType < 1)
        {
            _fieldType  = Enum.GetValues(typeof(FieldType)).Cast<FieldType>().Last();
        }
        
        FieldDataSet();
    
    }

    private void PlayerSetUi()
    {

        _SetPlayerUi.PlayerDataSet(_PlayerType);
    }
    
    public void FieldDataSet()
    {
        switch (_fieldType)
        {
            case FieldType.Forest:
                _FieldDataBase = Resources.Load<FieldDataBase>("DataTable/Field/Forest");
                break;
            case FieldType.Desert:
                _FieldDataBase = Resources.Load<FieldDataBase>("DataTable/Field/Desert");
                break;
            default:
                break;


        }
        _Difficult = _FieldDataBase.Difficult;
        _Size = _FieldDataBase.FieldSize;
        _SetFieldUi.FeildDataSet(_FieldDataBase);
    }

    
  
    
   
}
