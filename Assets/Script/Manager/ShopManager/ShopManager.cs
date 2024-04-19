using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    // Start is called before the first frame update
    [SerializeField]
    private List<ShopThing> _ShopThingList;
    public List<ShopThing> ShopThingList { get { return _ShopThingList; } }


    private void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }


    private void Start()
    {
        if(SaveLoadManager.instance.IsLoad == true)
        {
            int Count = 0;
            ShopManagerSaveData shopData = SaveLoadManager.instance.ShopManagerData;
            foreach (ShopThing thing in _ShopThingList)
            {                   
                thing.ShopData.TreePrice = shopData.shopDataList[Count].treePrice;
                thing.ShopData.MetalPrice = shopData.shopDataList[Count].metalPrice;
                thing.ShopData.StonePrice = shopData.shopDataList[Count].stonePrice;
                Count++;
            }
        }
        gameObject.SetActive(false);
    }



    //프라이스 버튼은 해당 메서드를 발동시킴.
    //즉 자기 자신의 버튼을 반환해야함.
    public void Price(ShopThing ShopThing)
    {
        int Tree = GameManager.instance.Player.PR.Tree;
        int Metal = GameManager.instance.Player.PR.Metal;
        int Stone = GameManager.instance.Player.PR.Stone;
        if (ShopThing.PriceCheck(Tree,Metal,Stone))
        {
            Debug.Log("조건 만족 지불시작");
            ShopThing.PriceSet();
        }
    }
   
}
