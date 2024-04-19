using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitState : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _Name;
    [SerializeField]
    private TextMeshProUGUI _Damage;
    [SerializeField]
    private TextMeshProUGUI _HP;
    [SerializeField]
    private Slider _HPSlider;

    public void Set(Creture creture)
    {
        _Name.text = creture.Name;
        _Damage.text = creture.AttackDamage.ToString();
        _HP.text = creture.Hp.ToString() + " / " + creture.MaxHp.ToString() ;
        _HPSlider.value = creture.Hp / creture.MaxHp;
    }
  
    public void Set(MonsterBuild monsterBuild)
    {
        _Name.text = monsterBuild.name;
        _Damage.text = "0";
        _HP.text = monsterBuild.Hp.ToString() + " / " + monsterBuild.MaxHp.ToString();
        _HPSlider.value = monsterBuild.Hp / monsterBuild.MaxHp;

    }
}
