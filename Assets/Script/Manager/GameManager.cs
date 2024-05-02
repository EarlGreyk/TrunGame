using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //게임의 흐름을 관장합니다. 
    //필드에 존재하는 오브젝트의 활성화와 비활성화
    //플레이어의턴,적의턴 그에따른 날짜 갱신을 시행합니다.

    public PlayerCreture Player;
    //스테이지 : 해당 변수의 크기에 따라 진행도가 어려워집니다.
    //턴 : 플레이어 와 적의턴을 각각 한번씩 하면 숫자가 증가합니다. 스테이지의 크기에 따라 턴의 최대크기가 증가합니다.
    private int stage ;
    public int Stage { get { return stage; } set { stage = value; } }
    private int turn ;
    public int Turn { get { return turn; } set { turn = value;  } }
    private int Maxturn;
    private int _MaxStage;
    public int MaxStage { get { return _MaxStage; } set { _MaxStage = value; } }

    private bool _Delay = false;
    public bool Delay { get { return _Delay; } set { _Delay = value; } }





    [SerializeField]
    private GameObject _BlockParent;


    private Block[,] _BlockArray;
    public Block[,] BlockArray { get { return _BlockArray; } }

    private Block _SenterBlock;
    public Block SenterBlock { get { return _SenterBlock; } }



    //어차피 현재 카운트 값은 내부에서만 알면됨. <<블록리스크 카운트가 결국 에너미 카운터잖아. 그럼저걸 왜만듬?
    [SerializeField]
    public List<Block> _EnemyBlock = new List<Block>();
    private float EnemyDistance;
    private int MaxEnemyCount;



    [SerializeField]
    public List<Block> _ItemBlock = new List<Block>();
    private float ItemDistance;
    private int MaxItemCount;


    [SerializeField]
    public List<Block> _MatarialBlock = new List<Block>();
    private float MatarialDistance;
    private int MaxMatarialCount;


    [SerializeField]
    public List<Block> _TreeBlock = new List<Block>();
    private float TreeDistance;
    private int MaxTreeCount;
  
    private float time;

    private Queue<MonsterBuild> _ActiveMB = new Queue<MonsterBuild>();
    /// <summary>
    /// 턴관리용
    /// </summary>
    private bool _IsTurn = false;
    public bool IsTurn { get { return _IsTurn; }
        set 
        {
            _IsTurn = value; 
            if(_IsTurn == true)
            {
                ManagerTurnInit();
            }
        } 
    }

    private bool _IsGameEnd = false;

    private float _GameTime;
    public float GameTime { get { return _GameTime; } }
    private int _KillCount;
    public int KillCount { get { return _KillCount; } set { _KillCount = value; } }
    private int _PRCount;
    public int PRCount { get { return _PRCount; } set { _PRCount = value; } }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void GameStart()
    {
        enabled = true;
        StartCoroutine("InitStart");
    }

    private void Update()
    {
        if(!_IsGameEnd)
        {

            _GameTime += Time.deltaTime;
        
            if (_Delay == false)
            { 
                if(_IsTurn)
                {
                    time += Time.deltaTime;
                    if (time >= 0.5f)
                    {
                        time = 0f;
                        if(Player == null)
                        {
                            Lose();
                            return;
                        }
                        
                        if (CheckTurn() == false)
                        {
                            _IsTurn = false;
                            Player.TurnSet(true);
                    
                        }else
                        {
                            ManagerTurn();
                        }
                    }
                }else
                {
                    if (_EnemyBlock.Count <= 0)
                    {
                        Victory();
                        return;
                    }
                    if (Player.IsTurn == false)
                    {
                        _ActiveMB.Clear();
                    }


                }
            }
        }
    }

    private void InitNewStart()
    {
        //아래의 값은 난이도에 따라 조정됩니다. 이후에 조정이필요합니다.
        stage = 1;
        turn = 0;
        Maxturn = 10;
        MaxStage = GameSetManager.Instance.FieldDataBase.MaxStage;
        _IsTurn = false;
        //
        FieldStart();
    }

    /// <summary>
    /// 필드관리
    /// </summary>
    private void FieldStart()
    {
        //필드를 생성합니다.
        //필드생성순서 : Tree -> EnemyField -> Item -> Matarial 
        //필드 생성에 필요한 최소거리를 지정합니다.
        GameSetManager Gsm = GameSetManager.Instance;
        GsmDifficult(Gsm.Difficult);
        StartCoroutine(FeildDelay());

    }
    /// <summary>
    /// 필드 블록 생성입니다.
    /// </summary>
    
    private void BlockSetting()
    {
        GameSetManager Gsm = GameSetManager.Instance;
        
        _BlockParent = new GameObject("BlockParent");
        _BlockArray = new Block[Gsm.Size, Gsm.Size];
        //가로
        for (int i = 0; i < Gsm.Size ; i++)
        {
            int X = i * 2;
            //세로
            for (int k = 0; k < Gsm.Size ; k++)
            {
                int Z = k * 2;
                GameObject LastcreatObject =  Instantiate(Gsm.BlockPrefabs[(int)Gsm.fdtype],
                    _BlockParent.transform);
                LastcreatObject.transform.position = new Vector3(X, 0, Z);
                LastcreatObject.name = i.ToString() + " , " + k.ToString();
                _BlockArray[i, k] = LastcreatObject.GetComponent<Block>();

                if (i == Gsm.Size / 2 && k == Gsm.Size / 2)
                    _SenterBlock = _BlockArray[i, k];


            }
        }
    }

    /// <summary>
    /// 필드 생성, 플레이어 생성입니다.
    /// </summary>

    private void PlayerSetting()
    {
        GameObject player = Instantiate(GameSetManager.Instance.PlayerPrefabs[(int)GameSetManager.Instance.PType]);
        Player = player.GetComponent<PlayerCreture>();
        Player.CurrentBlock = _SenterBlock;
        Player.transform.position = _SenterBlock.transform.position;
        Player.gameObject.SetActive(false);
        //
    }
    /// <summary>
    /// 필드내 블록의 시야를 차단시킵니다.
    /// </summary>
    private void FieldLock()
    {
        for(int i =0; i<_BlockArray.GetLength(0); i++) 
        {
            for(int k =0; k < _BlockArray.GetLength(1);k++)
            {
                _BlockArray[i,k].Cloud.gameObject.SetActive(true);
                if (_BlockArray[i,k].EnableObject != null)
                    _BlockArray[i,k].EnableObject.SetActive(false);
            }
        }


    }
   /// <summary>
   /// 필드 생성입니다. type에 따른 필드를 생성합니다.
   /// </summary>
   /// <param name="Range"></중앙에서 떨어진 최소 거리입니다.>
   /// <param name="Distance"></해당 타입의 객체와의 거리입니다.>
   /// <param name="Maxvalue"></최대 개수입니다.>
   /// <param name="type"></타입에 따라 생성이 달라집니다.>

    private void FieldSet(float Distance,int Maxvalue , int type)
    {
         List<Block> _BlockList = new List<Block>();
        
        for(int i=0; i< _BlockArray.GetLength(0); i++)
        {
            for(int k=0; k< _BlockArray.GetLength(1);k++)
            {
                if (_BlockArray[i, k].BlockType == Block.type.None)
                {
                    _BlockList.Add(_BlockArray[i, k]);
                }
                _BlockList.Remove(_SenterBlock);

            }
        }
        if(_BlockList.Count>0)
        {

            int rance = 0;
            Block block = null;
            int Value = 0;
            while (true)
            {
                if (_BlockList.Count > 0 && Value <Maxvalue)
                {
                    rance = Random.Range(0, _BlockList.Count);
                    _BlockList[rance].ObjectSet(type);
                    block = _BlockList[rance];
                    _BlockList.Remove(block);
                    Value++;  
                }
                else
                {
                    
                    break;
                }
                for (int k = _BlockList.Count - 1; k >= 0; k--)
                {
                    Vector3 v = _BlockList[k].transform.position;
                    if (Vector3.Distance(v, block.transform.position) <= Distance+1 && _BlockList[k]!=block)
                    {
                        _BlockList.Remove(_BlockList[k]);    
                    }
                }
            }
        }

    }

    
   
    //해당 세팅은 GameSetManager으로 변경될 수 있습니다.
    private void GsmDifficult(int value)
    {
        //몬스터의 거리와 몬스터 생성숫자, 몬스터와의 최초거리입니다.
        EnemyDistance = 10 - (2 * value);
        MaxEnemyCount = 3+(1*value);
      


        //아이템의 생성 숫자입니다. 이는 난이도에 따라 조정됩니다.
        ItemDistance = 6 + (2 * value);
        MaxItemCount = 8-(1*value);



        //나무가 아닌 재료를 개수를 지정합니다. 이는 난이도에 따라 조정됩니다.
        MatarialDistance = 2 + (1 * value);
        MaxMatarialCount = 40-(4*value);
 
        //
        TreeDistance = 0;
        MaxTreeCount = 500-(80*value);


    }

    

    /////////////////////////////////////////////////////////////////
    /// <summary>
    /// 턴관리
    /// </summary>
   
    private bool CheckTurn()
    {
        bool check = false;
        //플레이어턴에서 하는것을 제외한 모든 행동을 진행후 플레이어의 턴으로 넘깁니다.
        if (_ActiveMB.Count > 0)
        {
            check = true;
        }
        return check;


    }
    private void ManagerTurnInit()
    {
        //턴이 증가함과 동시에 플레이어에서 턴 종료를 받아냅니다.
        //만약 플레이어의 턴종료 시점으로 현재턴이 다음턴이 되면 스테이지가 넘어갑니다.
        //스테이지를 넘어가면 몬스터건물과 아이템을 제외한 자원, 목재가 재생됩니다. 이떄 재생은 모두 랜덤값입니다.
        if(Turn >= Maxturn)
        {
            Stage++;
            Turn = 0;
        }
        //Ai 행동을 수행합니다.
        //행동 원리 순서
        //1. 몬스터 블록 빌딩내에 있는 CreateMonster 함수를 작동합니다.
        //스테이지에 비례하여 _EnemyBlock 의 턴을 활성화 시켜야합니다. 이는 Max스테이지에 절대값에 비례합니다.
       
        int enablecount = Stage*3 / (MaxStage / _EnemyBlock.Count);
        for(int i =0; i < enablecount; i++) 
        {
            if (_EnemyBlock[i].EnableObject != null)
            { 
                _EnemyBlock[i].EnableObject.SetActive(true);
                _ActiveMB.Enqueue(_EnemyBlock[i].MonsterBuild);
            }

        }
        //선입선출을 이용하여 활성화 시킵니다.
        if(_ActiveMB.Count>0)
            _ActiveMB.Peek().OnTurnEnter();


    }

    private void ManagerTurn()
    {
        if(_ActiveMB.Count > 0)
        {
            if (_ActiveMB.Peek().isturn == false)
            {
                _ActiveMB.Dequeue();
                if(_ActiveMB.Count >0)
                {
                    _ActiveMB.Peek().OnTurnEnter();
                }
            }
        }
        
    }
    
   


    private void MonsterBuildRe()
    {
        //거리비례 빌드정렬.
        List<Block> templist = new List<Block>();

        float distance = 0;
        float distance2 = 0;
        int number = 0;


        for (int i = _EnemyBlock.Count-1; i>=0; i--)
        {
            number = i;
            distance = Vector3.Distance(_EnemyBlock[i].transform.position,_SenterBlock.transform.position);
            for(int k = _EnemyBlock.Count - 2; k >= 0; k--)
            {
                distance2 = Vector3.Distance(_EnemyBlock[k].transform.position, _SenterBlock.transform.position);

                if(distance2 <  distance) 
                {
                    distance = distance2;
                    number = k;
                }
            }
            templist.Add(_EnemyBlock[number]);
            _EnemyBlock.Remove(_EnemyBlock[number]);
        }

        _EnemyBlock = templist;

    }

    /// <summary>
    /// 초기화
    /// </summary>
    private IEnumerator InitStart()
    {
        yield return new WaitForSeconds(0.5f);
        if (!SaveLoadManager.instance.IsLoad)
        {
            InitNewStart();
        }
        else
        {
            InitLoadStart();
        }
        yield return null;
    }


    private IEnumerator FeildDelay()
    {
        _Delay = true;
        BlockSetting();
        PlayerSetting();
        yield return new WaitForSeconds(0.01f);
        FieldSet(EnemyDistance, MaxEnemyCount, 1);
        yield return new WaitForSeconds(0.01f);
        FieldSet(MatarialDistance, MaxMatarialCount, 3);
        yield return new WaitForSeconds(0.01f);
        FieldSet(ItemDistance, MaxItemCount, 2);
        yield return new WaitForSeconds(0.01f);
        FieldSet(TreeDistance, MaxTreeCount, 10);
        yield return new WaitForSeconds(0.01f);
        MonsterBuildRe();
        FieldLock();
        UImanager.Instance.InterFaceActive();
        Player.gameObject.SetActive(true);
        _Delay = false;
        Player.TurnSet(false);
        yield return null;
    }




    /// <summary>
    /// 게임을 로드하면 작동합니다.
    /// 해당 구문은 어디론가 이동할 필요가 있습니다.
    /// </summary>
    private void InitLoadStart()
    {
        
        stage = SaveLoadManager.instance.GmData.stage;
        MaxStage = SaveLoadManager.instance.GmData.maxstage;
        turn = SaveLoadManager.instance.GmData.turn;
        Maxturn = 10 ;
        _GameTime = SaveLoadManager.instance.GmData.gameTime;
        _PRCount = SaveLoadManager.instance.GmData.prCount;
        _KillCount = SaveLoadManager.instance.GmData.killCount;


        GameSetManager Gsm = GameSetManager.Instance;
        GsmDifficult(Gsm.Difficult);
        StartCoroutine(FeildLoad());

    }
    private IEnumerator FeildLoad()
    {
        _Delay = true;
        UImanager.Instance.InterFaceActive();
        BlockSetting();
        yield return new WaitForSeconds(0.05f);
        LoadBlockStart();
        yield return new WaitForSeconds(0.01f);
        LoadPlayerStart();
        yield return new WaitForSeconds(0.01f);
        MonsterBuildRe();
        Player.gameObject.SetActive(true);
        _Delay = false;
        Player.TurnSet(true);
        SaveLoadManager.instance.IsLoad = false;
        yield return null;
    }
    private void LoadPlayerStart()
    {
        Player = LoadPlayer(SaveLoadManager.instance.PlayerData);
        LoadplayerResource(SaveLoadManager.instance.PlayerResourceData);
        UnitStateUiMnager.Instance.StateGet(Player);


    }
    private PlayerCreture LoadPlayer(PlayerSaveData playerSaveData)
    {
        PlayerCreture player = null;
        GameObject LastGameObject = Instantiate(GameSetManager.Instance.
            PlayerPrefabs[(int)GameSetManager.Instance.PType]);
        player = LastGameObject.GetComponent<PlayerCreture>();
        player.gameObject.transform.position = playerSaveData.pos;
        player.name = playerSaveData.name;
        player.AttackRange = playerSaveData.attackRange;
        player.AttackDamage = playerSaveData.attackDamage;
        player.MaxHp = playerSaveData.maxHp;
        player.Hp = playerSaveData.currentHp;
        player.MaxHand = playerSaveData.maxHand;
        player.Name = playerSaveData.name;

        int x = (int)player.transform.position.x / 2;
        int z = (int)player.transform.position.z / 2;
        player.CurrentBlock = BlockArray[x, z];

        return player;

    }
    private void  LoadplayerResource(PlayerResourceSaveData prSaveData)
    {
        PlayerResource playerPr = null;
        playerPr = Player.GetComponent<PlayerResource>();
        playerPr.Tree = prSaveData.tree;
        playerPr.Stone = prSaveData.stone;
        playerPr.Metal = prSaveData .metal;
        playerPr.MaxCardCount = prSaveData.MaxcardCount;
        foreach(var carddata in prSaveData.cardDataList)
        {
            CardDataBase Data = ScriptableObject.CreateInstance<CardDataBase>();
            Data.CreateDataBase(carddata);
            playerPr.CreateCard(Data);
        }

        
    }
    private void LoadBlockStart()
    {
        GmSaveData gmSaveData = SaveLoadManager.instance.GmData;
        //int size = GameSetManager.Instance.Size;
        foreach (var block in gmSaveData.blockList)
        {
            LoadBlock(block);

        }
       

        
    }
    private void LoadBlock(BlockSaveData blockSaveData)
    {
        Block block = null;
        GameSetManager Gsm = GameSetManager.Instance;
        ///
        int x = (int)blockSaveData.pos.x / 2;
        int z = (int)blockSaveData.pos.z / 2;
        block = BlockArray[x, z];
        block.gameObject.transform.position = blockSaveData.pos;
        block.BlockType = blockSaveData.blockType;
        block.ObjectValue = blockSaveData.objectValue;
        block.Value = blockSaveData.value;
        block.isobejct = blockSaveData.isObject;
        block.Cloud.gameObject.SetActive(blockSaveData.cloud);
        block.name = (blockSaveData.pos.x/2).ToString() + " , " + (blockSaveData.pos.z/2).ToString();

        if (blockSaveData.mbcheck ==true)
        {
           block.MonsterBuild = LoadMonsterBuild(blockSaveData.mbSaveData,block);
        }
        if (blockSaveData.itcheck ==true)
        {
            LoadItem(blockSaveData.itemSaveData,block);
        }
        if (blockSaveData.pos == SaveLoadManager.instance.GmData.senTerBlock.pos)
        {
            _SenterBlock = block;
        }
        block.ObjectLoad((int)block.BlockType);



    }
    private MonsterBuild LoadMonsterBuild(MonsterBuildSaveData monsterBuildSaveData, Block parentblock)
    {
        MonsterBuild monsterBuild = parentblock.MonsterbuildPrefabs[parentblock.ObjectValue].GetComponent<MonsterBuild>();
        monsterBuild.MaxHp = monsterBuildSaveData.maxHp;
        monsterBuild.Hp = monsterBuildSaveData.hp;
        monsterBuild.MaxMonster = monsterBuildSaveData.maxMonster;
        monsterBuild.CreateValue = monsterBuildSaveData.createValue;
        monsterBuild.Value = monsterBuildSaveData.value;
        monsterBuild.Range = monsterBuildSaveData.range;
        monsterBuild.BlockBase = parentblock;
        foreach(var monster in monsterBuildSaveData.monsterdataList)
        {
            monsterBuild.MonsterList.Add(LoadMonster(monster,monsterBuild));
        }
        monsterBuild.CurrentMonster = monsterBuild.MonsterList.Count;
        
        

        
        


        return monsterBuild;
    }
    private MonsterCreture LoadMonster(MonsterSaveData monsterSaveData,MonsterBuild parentmb)
    {
        MonsterCreture monster = null;
        GameObject LastcreatObject = Instantiate(parentmb.MonsterPrefabs);
        monster = LastcreatObject.GetComponent<MonsterCreture>();
        monster.transform.position = monsterSaveData.pos;
        monster.TargetRange = monsterSaveData.targetRange;
        monster.MaxAcionCount = monsterSaveData.maxActionCount;
        monster.MaxHp = monsterSaveData.maxHp;
        monster.Hp = monsterSaveData.hp;
        monster.ParentMonsterBuild = parentmb;
        int x = (int)monster.transform.position.x / 2;
        int z = (int)monster.transform.position.z / 2;
        monster.CurrentBlock = BlockArray[x, z];
        monster.CurrentBlock.Monster = monster;
        if (monsterSaveData.target == true)
            monster.PTarget = Player;
             


        return monster;
    }
    private void LoadItem(ItemSaveData itemSaveData,Block parentblock)
    {
        Item item = parentblock.ItemPrefabs[parentblock.ObjectValue].GetComponent<Item>();
        int number = itemSaveData.itemID;
        ItemDataBase itemdata = Resources.Load<ItemDataBase>("DataTable/Item/ItemNumber/Item_" + number);
        item.ItemName = itemdata.ItemName;
        item.ItemID = itemdata.ItemID;
        item.Acion = itemdata.Acion;
    }


   




    //////////
    ///
    //////////

    private void Lose()
    {
        _IsGameEnd = true;
        SoundManager.instance.AudioPlay("Sound/Effect/End/Lose");
        StartCoroutine(Ending());
    }

    private void Victory()
    {
        _IsGameEnd = true;
        SoundManager.instance.AudioPlay("Sound/Effect/End/Victory");
        StartCoroutine(Ending());
        return;
    }
    IEnumerator Ending()
    {
        yield return new WaitForSeconds(2.5f);
        SaveLoadManager.instance.SaveRemove();
        UImanager.Instance.ResultInterface.SetActive(true);
        yield return null;
    }


}
