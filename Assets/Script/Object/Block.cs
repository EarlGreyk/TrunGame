using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    public enum type
    {
        None,
        EnemyBuild,
        Item,
        Material,
        Tree = 10,
        Break = 100
        //적건물

    }

    private type _BlockType;

    public type BlockType { get { return _BlockType; } set { _BlockType=value; } }
  

     


    public Block _block;

    [SerializeField]
    private List<GameObject> _MatarialPrefabs;
    [SerializeField]
    private List<GameObject> _MonsterBuildPrefabs;
    public List<GameObject> MonsterbuildPrefabs { get { return _MonsterBuildPrefabs; } }
    [SerializeField]
    private List<GameObject> _ItemPrefabs;
    public List <GameObject> ItemPrefabs { get { return _ItemPrefabs; } }
    [SerializeField]
    private List<GameObject> _TreePrefabs;

    [SerializeField]
    private GameObject _EnableObject;
    public GameObject EnableObject { get { return _EnableObject; } }
    [SerializeField]
    private GameObject _Cloud;

    public GameObject Cloud { get { return _Cloud; } }

    private MonsterCreture _Monster;
    public MonsterCreture Monster { get { return _Monster; } set { _Monster = value; } }

    private MonsterBuild _MonsterBuild;
    public MonsterBuild MonsterBuild { get { return _MonsterBuild; } set { _MonsterBuild = value; } }


    private Item _Item;
    public Item Item { get { return _Item; } set { _Item = value; } }



    private int _objectValue; //오브젝트가 할당되어야 등급벨류
    public int ObjectValue { get { return _objectValue; } set { _objectValue = value; } }
    private int _value; // 해당 오브젝트가 가지고 있는 수치.
    public int Value { get { return _value; } 
        set 
        {
            if(value == 0)
            {
                switch(_BlockType)
                {
                    case type.EnemyBuild: 
                        //GameManager.instance._EnemyBlock.Remove(this);
                        break;
                    case type.Item: GameManager.instance._ItemBlock.Remove(this);
                        break;
                    case type.Material: GameManager.instance._MatarialBlock.Remove(this);
                        break;
                    case type.Tree: GameManager.instance._TreeBlock.Remove(this);
                        break;
                    default:
                        break;
                }
                ObjectSet(0);
            }
            _value = value;
        } 
    }

    private bool _isobejct;
    public bool isobejct { get {  return _isobejct; } set { _isobejct = value; } }

    

    void Start()
    {
        if(SaveLoadManager.instance.IsLoad)
        {
            LoadInit();
        }else
        {
            NewInit();
        }
        
    }

    // Update is called once per frame

    private void NewInit()
    {
        _block = GetComponent<Block>();
        _BlockType = type.None;
        _isobejct = false;
        Value = 1;
        int x = (int)transform.position.x/2;
        int z = (int)transform.rotation.z/2;
    }
    private void LoadInit()
    {
        _block = GetComponent<Block>();
    }
    public void ObjectSet(int i)
    {
        if(_BlockType != type.None)
        {
            switch(_BlockType) 
            {
                case type.EnemyBuild: GameManager.instance._EnemyBlock.Remove(this); break;
                case type.Item : GameManager.instance._ItemBlock.Remove(this); break;
                case type.Material: GameManager.instance._MatarialBlock.Remove(this); break;
                case type.Tree: GameManager.instance._TreeBlock.Remove(this); break;

                    
            }
        }
        switch (i)
        {
            case 0 : _BlockType = type.None; break;
            case 1: _BlockType = type.EnemyBuild; break;
            case 2: _BlockType = type.Item; break;
            case 3: _BlockType = type.Material; break;
            case 10: _BlockType = type.Tree; break;
            case 100:_BlockType = type.Break; break;
                
        }
        if (i ==0)
        {
            ObjectValue = 0;
            isobejct = false;
            
        }
        else if(i == 1)
        {
            ObjectValue = Random.Range(1, _MonsterBuildPrefabs.Count);
            GameManager.instance._EnemyBlock.Add(this);
            BreakBlock();



        }
        else if(i == 2)
        {
            ObjectValue = Random.Range(1, _ItemPrefabs.Count);
            GameManager.instance._ItemBlock.Add(this);
        }
        else if(i == 3)
        {
            ObjectValue = Random.Range(1, _MatarialPrefabs.Count);
            GameManager.instance._MatarialBlock.Add(this);
        }
        else if(i == 100)
        {
            
        }
        else
        {
            ObjectValue = Random.Range(1, _TreePrefabs.Count);
            GameManager.instance._TreeBlock.Add(this);
        }
        ObjectEnableSet();
    }
    public void ObjectLoad(int i)
    {
        if (i == 0)
        {
            ObjectValue = 0;
            int x = (int)transform.position.x;
            int z = (int)transform.rotation.z;
        }
        else if (i == 1)
        {
            GameManager.instance._EnemyBlock.Add(this);
        }
        else if (i == 2)
        {
            GameManager.instance._ItemBlock.Add(this);
        }
        else if (i == 3)
        {
            GameManager.instance._MatarialBlock.Add(this);
        }
        else if (i == 100)
        {

        }
        else
        {
            GameManager.instance._TreeBlock.Add(this);
        }
        ObjectEnableSet();
    }
    //타입에 따른 외형 오브젝트 활성화 비활성화.
    private void ObjectEnableSet()
    {
        if(_EnableObject != null)
            _EnableObject.SetActive(false);

        switch(_BlockType)
        {
            case type.Material:
                _EnableObject = _MatarialPrefabs[ObjectValue];
                break;
            case type.EnemyBuild:
                _EnableObject = _MonsterBuildPrefabs[ObjectValue];
                _MonsterBuild = _EnableObject.GetComponent<MonsterBuild>();
                _MonsterBuild.BlockBase = this;
                _isobejct = true;
                break;
            case type.Item:
                _EnableObject = _ItemPrefabs[ObjectValue];
                _Item = _EnableObject.GetComponent<Item>();
                break;
            case type.Tree:
                _EnableObject = _TreePrefabs[ObjectValue];
                break;
            case type.Break:
                _EnableObject = null; //파괴된 지형이라는거 표기해주면됨
                break;
            default:
                _EnableObject = null;
                break;
        }
        if (_EnableObject != null)
        {
            
            if(SaveLoadManager.instance.IsLoad)
            {
                if (_Cloud.activeSelf == false)
                {
                    _EnableObject.SetActive(true);
                }
            }
            else
            {
                _EnableObject.SetActive(true);
            }
           
        }

    }
    private void BreakBlock()
    {
        int x = (int)transform.position.x;
        int z = (int)transform.position.z;

        //센터 = 자기자신.
        Collider[] colls = Physics.OverlapSphere(transform.position, 2);
        for (int i = 0; i < colls.Length; i++)
        {
            Block block = colls[i].GetComponent<Block>();
            if (block != null)
            {
                if (block.BlockType == Block.type.EnemyBuild)
                {
                    continue;
                }
                block.ObjectSet(100);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Monster") || other.gameObject.layer == LayerMask.NameToLayer("MonsterBuild"))
        {
            if(_EnableObject !=null)
            { 
                _EnableObject.SetActive(true);
            }
            _Cloud.gameObject.SetActive(false);
        }
    }

 















}
