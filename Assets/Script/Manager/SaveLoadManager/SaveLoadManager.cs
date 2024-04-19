using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GsmSaveData
{
    public GameSetManager.FieldType fieldType;
    public GameSetManager.PlayerType playerType;
    public int Size;
    public int difficult;
    public GsmSaveData(GameSetManager gameSetManager)
    {
        fieldType = gameSetManager.fdtype;
        playerType = gameSetManager.PType;
        Size = gameSetManager.Size;
        difficult = gameSetManager.Difficult;
    }
}



[System.Serializable]
public class GmSaveData
{
    public int stage;
    public int maxstage;
    public int turn;
    public int maxturn;
    public int killCount;
    public int prCount;
    public float gameTime;

    public PlayerSaveData player;
    public List<BlockSaveData> blockList;
    public BlockSaveData senTerBlock;

    public GmSaveData(GameManager manager)
    {
        stage = manager.Stage;
        maxstage = manager.MaxStage;
        turn = manager.Turn;
        player = new PlayerSaveData(manager.Player);


        // Block 데이터를 직렬화하여 저장
        blockList = new List<BlockSaveData>();
        foreach (var block in manager.BlockArray)
        {
            blockList.Add(new BlockSaveData(block));
        }
        senTerBlock = new BlockSaveData(manager.SenterBlock);
        killCount = manager.KillCount;
        prCount = manager.PRCount;
        gameTime = manager.GameTime;
        // 다른 필요한 데이터들도 초기화

    }
}
[System.Serializable]
public class BlockSaveData
{
    public Vector3 pos;
    public Block.type blockType;
    public int objectValue;
    public int value;
    public bool isObject;
    public bool cloud;
    public bool mbcheck;
    public bool itcheck;
    public MonsterBuildSaveData mbSaveData;
    public ItemSaveData itemSaveData;

    public BlockSaveData(Block block)
    {
        pos = block.transform.position;
        blockType = block.BlockType;
        objectValue = block.ObjectValue;
        value = block.Value;
        isObject = block.isobejct;
        cloud = block.Cloud.activeSelf;
        if(block.MonsterBuild != null)
        { 
            mbSaveData = new MonsterBuildSaveData(block.MonsterBuild);
            mbcheck = true;
        }else
        {
            mbcheck = false;
        }
        if(block.Item != null)
        {
            itemSaveData = new ItemSaveData(block.Item);
            itcheck = true;
        }else
        {
            itcheck = false;
        }
       

    }
}
[System.Serializable]
public class MonsterBuildSaveData
{
    public float hp;
    public float maxHp;
    public int maxMonster;
    public int createValue;
    public int value;
    public float range;
    public List<MonsterSaveData> monsterdataList;
    public MonsterBuildSaveData(MonsterBuild monsterBuild)
    {
        hp = monsterBuild.Hp;
        maxHp = monsterBuild.MaxHp;
        maxMonster = monsterBuild.MaxMonster;
        createValue = monsterBuild.CreateValue;
        value = monsterBuild.Value;
        range = monsterBuild.Range;
        monsterdataList = new List<MonsterSaveData>();
        foreach (var monster in monsterBuild.MonsterList)
        {
            monsterdataList.Add(new MonsterSaveData(monster));
        }
    }
}
[System.Serializable]
public class MonsterSaveData
{
    public Vector3 pos;
    public float targetRange;
    public int maxActionCount;
    public float hp;
    public float maxHp;
    public bool target;
    public MonsterSaveData(MonsterCreture monsterCreture)
    {
        pos = monsterCreture.transform.position;
        targetRange = monsterCreture.TargetRange;
        maxActionCount = monsterCreture.MaxAcionCount;
        hp = monsterCreture.Hp;
        maxHp = monsterCreture.MaxHp;

        if (monsterCreture.PTarget != null)
        {
            target = true;
        }
        else
        {
            target = false;
        }


    }
}

[System.Serializable]
public class PlayerSaveData
{
    public Vector3 pos;
    public string name;
    public float attackRange;
    public float attackDamage;
    public float maxHp;
    public float currentHp;
    public int maxHand;


    public PlayerSaveData(PlayerCreture playerCreture)
    {
        pos = playerCreture.transform.position;
        name = playerCreture.Name;
        attackRange = playerCreture.AttackRange;
        attackDamage = playerCreture.AttackDamage;
        maxHp = playerCreture.MaxHp;
        currentHp = playerCreture.Hp;
        maxHand = playerCreture.MaxHand;
    }
}
[System.Serializable]
public class PlayerResourceSaveData
{
    public int tree;
    public int stone;
    public int metal;
    public int MaxcardCount;
    public List<CardDataSaveData> cardDataList;
    public PlayerResourceSaveData(PlayerResource playerResource)
    {
        tree = playerResource.Tree;
        stone = playerResource.Stone;
        metal = playerResource.Metal;
        MaxcardCount = playerResource.MaxCardCount;
        cardDataList = new List<CardDataSaveData>();
        foreach (var cardData in playerResource.CardList)
        {
            cardDataList.Add(new CardDataSaveData(cardData.CardData));
            
        }

    }
}
    

[System.Serializable]
public class CardDataSaveData
{
    public CardDataBase.ValueType cardDataType;
    public string cardname;
    public string cardspritepath;
    public int cardvalue;
    public string desc;
    public CardDataSaveData(CardDataBase cardDataBase)
    {
        cardDataType = cardDataBase.DataType;
        cardspritepath = cardDataBase.SpritePath;
        cardvalue = cardDataBase.Value;
        desc = cardDataBase.Desc;
    }
}

[System.Serializable]
public class ItemSaveData
{
    public int itemID;
    public ItemSaveData(Item item)
    {
        itemID = item.ItemID;
    }
}
[System.Serializable]
public class ShopManagerSaveData
{
    public List<ShopSaveData> shopDataList;
    public ShopManagerSaveData(ShopManager shopManager)
    {
        shopDataList = new List<ShopSaveData>();
        foreach (var shopData in shopManager.ShopThingList)
        {
            shopDataList.Add(new ShopSaveData(shopData));
        }
    }
}
[System.Serializable]
public class ShopSaveData
{
    public int treePrice;
    public int metalPrice;
    public int stonePrice;

    public ShopSaveData(ShopThing shopthing)
    {  
        treePrice = shopthing.ShopData.TreePrice;
        metalPrice = shopthing.ShopData.MetalPrice;
        stonePrice = shopthing.ShopData.StonePrice;
    }
}

[System.Serializable]
public class InventoryManagerSaveData
{
    public List<InventoryItemSaveData> InvenItemDataList;

    public InventoryManagerSaveData(InventoryManager inventoryManager)
    {
        InvenItemDataList = new List<InventoryItemSaveData>();
        foreach (var ItemData in inventoryManager.Slots)
        {
            if (ItemData.ItemSprite != null)
                InvenItemDataList.Add(new InventoryItemSaveData(ItemData));
            else
                break;
        }
    }



}
[System.Serializable]
public class InventoryItemSaveData
{
    public string itemName;
    public string itemDesc;
    public string itemSpritePath;
    public int itemValue;

    public InventoryItemSaveData(ItemSlot itemSlot)
    {
        itemName = itemSlot.ItemName;
        itemDesc = itemSlot.ItemDesc;
        itemSpritePath = itemSlot.ItemSprite.name;
        itemValue = itemSlot.ItemValue;
        
    }
}

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;


    private bool _IsLoad;
    public bool IsLoad { get { return _IsLoad; } set { _IsLoad = value; } }

    private bool _IsSave;
    public bool IsSave { get { return _IsSave; } }

    [SerializeField]
    private Button LoadButton;


    //경로
    private string _GameSetManagerPath;
    private string _GameManagerPath;
    private string _PlayerPath;
    private string _PlayerResourcePath;
    private string _ShopManagerPath;
    private string _InvenToryManagerPath;
    //Load파일.
    private GmSaveData _GmData;
    public GmSaveData GmData { get { return _GmData; } }
    private GsmSaveData _GsmData;
    public GsmSaveData GsmData { get { return _GsmData; } }
    private PlayerSaveData _PlayerData;
    public PlayerSaveData PlayerData { get { return _PlayerData; } }
    private PlayerResourceSaveData _PlayerResourceData;
    public PlayerResourceSaveData PlayerResourceData { get {  return _PlayerResourceData; } }
    private ShopManagerSaveData _ShopManagerData;
    public ShopManagerSaveData ShopManagerData { get { return _ShopManagerData; } }
    private InventoryManagerSaveData _InventorySaveData;
    public InventoryManagerSaveData InventorySaveData { get { return _InventorySaveData; } }




    // Start is called before the first frame update
    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("이미있어서 기존꺼 파괴함.");
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        

            //초기경로지정
        _GameSetManagerPath = Application.persistentDataPath + "/saveGameSetManagerData.json";
        _GameManagerPath = Application.persistentDataPath + "/saveGameManagerData.json";
        _PlayerPath = Application.persistentDataPath + "/savePlayerData.json.json";
        _PlayerResourcePath = Application.persistentDataPath + "/savePlayerResourceData.json";
        _ShopManagerPath = Application.persistentDataPath + "/saveShopManagerData.json";
        _InvenToryManagerPath = Application.persistentDataPath + "/saveInvenToryManagerData.json";
        Load();



    }
    
    public void Save()
    {
        _IsSave = true;
        SaveGameSetManager();
        SaveGameManager();
        SavePlayer();
        SavePlayerResource();
        SaveShopManager();
        SaveInvenToryManager();
        _IsSave = false;

    }
    private void SaveGameSetManager()
    {
        GsmSaveData saveData = new GsmSaveData(GameSetManager.Instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_GameSetManagerPath, json);
    }

    private void SaveGameManager()
    {
        GmSaveData saveData = new GmSaveData(GameManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_GameManagerPath, json);
    }
    private void SavePlayer()
    {
        PlayerSaveData saveData = new PlayerSaveData(GameManager.instance.Player);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_PlayerPath , json);
    }
    private void SavePlayerResource()
    {
        PlayerResourceSaveData saveData = new PlayerResourceSaveData(GameManager.instance.Player.PR);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_PlayerResourcePath, json);
    }
    private void SaveShopManager()
    {
        ShopManagerSaveData saveData = new ShopManagerSaveData(ShopManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_ShopManagerPath , json);
    }
    private void SaveInvenToryManager()
    {
        InventoryManagerSaveData saveData = new InventoryManagerSaveData(InventoryManager.Instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(_InvenToryManagerPath, json);

    }





    public void Load()
    {
        _GmData = LoadGameManager();
        if (_GmData == null)
            return;
        _GsmData = LoadGameSetManager();
        if (_GsmData == null)
            return;
        _PlayerData = LoadPlayer();
        if (_PlayerData == null)
            return;
        _PlayerResourceData = LoadPlayerResource();
        if (_PlayerResourceData == null)
            return;
        _ShopManagerData = LoadShopManager();
        if (_ShopManagerData == null)
            return;
        _InventorySaveData = LoadInventory();
        if (_InventorySaveData == null)
            return;
        

        LoadButton.interactable = true;
    }

    private GmSaveData LoadGameManager() 
    {
        if(File.Exists(_GameManagerPath))
        {
            string json = File.ReadAllText(_GameManagerPath);
            return JsonUtility.FromJson<GmSaveData>(json);
        }else
        {
            Debug.Log("not file");
            return null;
        }
    }
    private GsmSaveData LoadGameSetManager()
    {
        if(File.Exists(_GameSetManagerPath))
        {
            string json = File.ReadAllText(_GameSetManagerPath);
            return JsonUtility.FromJson<GsmSaveData>(json);
        }else
        {
            Debug.Log("not file");
            return null;
        }
    }
    private PlayerSaveData LoadPlayer()
    {
        if(File.Exists(_PlayerPath)) 
        {
            string json = File.ReadAllText(_PlayerPath);
            return JsonUtility.FromJson<PlayerSaveData>(json);
        }else
        {
            Debug.Log("not file");
            return null;
        }
    }
    private PlayerResourceSaveData LoadPlayerResource()
    {
        if(File.Exists(_PlayerResourcePath)) 
        {
            string json = File.ReadAllText (_PlayerResourcePath);
            return JsonUtility.FromJson<PlayerResourceSaveData>(json);
        }else
        {
            Debug.Log("not file");
            return null;
        }
    }
    private ShopManagerSaveData LoadShopManager()
    {
        if (File.Exists(_ShopManagerPath))
        {
            string json = File.ReadAllText(_ShopManagerPath);
            return JsonUtility.FromJson<ShopManagerSaveData>(json);
        }
        else
        {
            Debug.Log("not file");
            return null;
        }
    }
    private InventoryManagerSaveData LoadInventory() 
    {
        if (File.Exists(_InvenToryManagerPath))
        {
            string json = File.ReadAllText(_InvenToryManagerPath);
            return JsonUtility.FromJson<InventoryManagerSaveData>(json);
        }
        else
        {
            Debug.Log("not file");
            return null;
        }
    }




    public void GameLoadSetting()
    {
        //게임 설정순서
        //GameSetManager -> GameManager[게임 스테이지설정 포함] -> Player -> PlayerResource
        IsLoad = true;
        GameSetManager.Instance.PType = GsmData.playerType;
        GameSetManager.Instance.fdtype = GsmData.fieldType;
        GameSetManager.Instance.Difficult = GsmData.difficult;
        GameSetManager.Instance.Size = GsmData.Size;

    }
    public void SaveRemove()
    {
        Debug.Log("파일지우기");
        //승리 혹은 패배로 세이브 파일을 지움.
        if(File.Exists(_GameSetManagerPath))
            File.Delete(_GameSetManagerPath); Debug.Log("지우기");

        if(File.Exists(_PlayerResourcePath))
            File.Delete(_PlayerResourcePath);

        if(File.Exists(_GameManagerPath))
            File.Delete(_GameManagerPath);

        if(File.Exists(_PlayerPath))
            File.Delete(_PlayerPath);
       
        if(File.Exists(_ShopManagerPath))
            File.Delete(_ShopManagerPath);

        if(File.Exists(_InvenToryManagerPath))
            File.Delete(_InvenToryManagerPath);
         

    }
 
    

}
