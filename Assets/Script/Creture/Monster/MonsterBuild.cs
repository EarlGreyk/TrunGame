using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MonsterBuild : MonoBehaviour, CretureHit
{
    HpSlider _HpSlider;
    // Start is called before the first frame update

    [SerializeField]
    private GameObject _MonsterPrefabs;
    public GameObject MonsterPrefabs { get { return _MonsterPrefabs; } }
    protected bool _isturn;
    public bool isturn { get { return _isturn; } set { _isturn = value; } }
    protected string _Name;
    public string Name { get { return _Name; }set { _Name = value; } }
    protected float _Hp;
    public float Hp { get { return _Hp; } set { _Hp = value; } }
    protected float _MaxHp; 
    public float MaxHp { get { return _MaxHp; } set { _MaxHp = value; } }
    protected int _MaxMonster; //최대 몬스터수
    public int MaxMonster { get { return _MaxMonster; }set { _MaxMonster = value; } }
    protected int _CurrentMonster; //보유한 몬스터 수 
    public int CurrentMonster { get { return _CurrentMonster; } set { _CurrentMonster = value; } }
    protected int _CreateValue; // 생산 가치
    public int CreateValue { get { return _CreateValue; } set { _CreateValue = value; } }
    protected int _Value; //현재 가치
    public int Value { get { return _Value; } set { _Value = value; } }
    protected float _Range;//몬스터 기본 행동 범위
    public float Range { get { return _Range; } set { _Range = value; } }

    private List<MonsterCreture> _MonsterList = new List<MonsterCreture>();
    public List<MonsterCreture> MonsterList { get { return _MonsterList; } }
    //컨트롤 하는 크리쳐 순서 넣기용
    private Queue<MonsterCreture> _ActiveQueue = new Queue<MonsterCreture>();
    //현재 컨트롤하는 크리쳐
    private MonsterCreture _creture = null;
    private Block _BlockBase;
    public Block BlockBase { get { return _BlockBase; }set { _BlockBase = value; } }
    public HpSlider HpSlider { get { return _HpSlider; } }
    protected virtual void Start()
    {
        Slider slider = Instantiate(Resources.Load<Slider>("UI/HPSlider/Hp_Bar"));
        _HpSlider = slider.GetComponent<HpSlider>();
        _HpSlider.ParentObject = gameObject;
        if (SaveLoadManager.instance.IsLoad)
            _HpSlider.SetValue(Hp,MaxHp);

    }

    private void Update()
    {
        if(_isturn == true)
        {
            if (_ActiveQueue.Count <= 0)
            {
                _isturn = false;
                return;
            }else
            {
                if (_creture == null)
                {
                    CurrentCretureSet();
                }
                else
                {
                    if (_creture.IsTurn == false)
                    {
                        _ActiveQueue.Dequeue();
                        _creture = null;

                    }
                }
            }
           
            



        }
    }

    // 
    //초기화
   
 
    //건물 생성시 지형파괴

    //몬스터 생성.
    
    private void CreateMonster()
    {
        //몬스터 생성
        GameObject Monster = Instantiate(MonsterPrefabs);
        MonsterCreture Mc = Monster.GetComponent<MonsterCreture>();
        Mc.Setparent(this);
        _MonsterList.Add(Mc);
        

        _CurrentMonster = _MonsterList.Count;


        LayerMask layer = LayerMask.GetMask("Plan");
        Collider[] colls = Physics.OverlapSphere(transform.position, 2,layer);
        Block block = null;

        if (colls.Length > 0)
        {
            int count = 0;
            while (true)
            {
                count++;
                int x = Random.Range(0, colls.Length);
                block = colls[x].GetComponent<Block>();

                if ((block.BlockType == Block.type.None || block.GetComponent<Block>().BlockType == Block.type.Break) )
                {
                    if(block.isobejct == false)
                    { 
                        Mc.transform.position = colls[x].transform.position;
                        Mc.CurrentBlock = block;
                        block.isobejct = true;
                        block.Monster = Mc;
                    
                        break;
                    }
                }
                if (count > colls.Length)
                    break;

            }
        }
        
        

    }

    private void CreateCheck()
    {
        if (_Value >= _CreateValue)
        {
            if(_MonsterList.Count < _MaxMonster)
            {
                _Value -= _CreateValue;
                CreateMonster();
            }
            
        }
        else
        {
            if (_MonsterList.Count >= _MaxMonster)
            {
                _MaxMonster++;
                MonsterTarget();
            }
            else
            {
                _Value++;
            }
        }

    }

    private void CurrentCretureSet()
    {
        _creture = _ActiveQueue.Peek();
        _creture.TurnSet(true);
    }


    public void OnTurnEnter()
    {
        isturn = true;
        for (int i = 0; i < _MonsterList.Count; i++)
        {
            _ActiveQueue.Enqueue(_MonsterList[i]);
        }
        CreateCheck();

    }
    public void OnTurnUpdate()
    {

    }

    public void Hit(float damage)
    {
        _Hp -= damage;
        _HpSlider.SetValue(_Hp, _MaxHp);
        SoundManager.instance.AudioPlay("Sound/Effect/Creture/Sound_Hit");
        UnitStateUiMnager.Instance.StateGet(this);
        if (_Hp <=0)
        {
            BlockBase.Value = 0;
            Destroy();
        }else
        {
            EffectManager.instance.HitEffect(transform.position, transform.position, transform, "Effect/Hit/Holy hit");
        }
    }

    private void OnDisable()
    {
        if (_HpSlider != null)
            _HpSlider.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (_HpSlider != null)
            _HpSlider.gameObject.SetActive(true);
    }
    private void Destroy()
    {
        UnitStateUiMnager.Instance.StateUiEnable();
        GameManager.instance.KillCount++;
        Destroy(gameObject);
    }
    private void MonsterTarget()
    {
        foreach(var _Monster in _MonsterList) 
        {
            _Monster.PTarget = GameManager.instance.Player;
        }
    }


}
