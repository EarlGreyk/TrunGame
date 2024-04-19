using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateUiMnager : MonoBehaviour
{
    public static UnitStateUiMnager Instance;
    [SerializeField]
    private UnitState _PlayerUnitState;
    [SerializeField]
    private UnitState _MonsterUnitState;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void StateGet(Creture creture)
    {
        _PlayerUnitState.Set(creture);
    }
    public void StateGet(PlayerCreture creture)
    {
        _PlayerUnitState.Set(creture);
    }
    public void StateGet(MonsterCreture creture)
    {
        if (!_MonsterUnitState.gameObject.activeSelf)
            _MonsterUnitState.gameObject.SetActive(true);
        _MonsterUnitState.Set(creture);
    }


    public void StateGet(MonsterBuild monsterBuild) 
    {
        if(!_MonsterUnitState.gameObject.activeSelf) 
            _MonsterUnitState.gameObject.SetActive(true);
        
        _MonsterUnitState.Set(monsterBuild);
    }
    public void StateUiEnable()
    {
        _MonsterUnitState.gameObject.SetActive(false);
    }
}
