using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
interface CretureHit
{
    void Hit(float damage);
}

public class Creture : MonoBehaviour
{
    protected Animator _Animator;
    protected HpSlider _HpSlider;


    //턴인지 확인용
    protected string _Name;
    protected bool _isTurn;
    protected bool _isAction;
    protected float _Hp;
    protected float _MaxHp;
    protected float _AttackDamage;
    protected float _AttackRange;
    protected int _MaxAc; //최대 행동력
    protected Block _CurrentBlock;
    protected Block _TargetBlock;
   
    public string Name { get { return _Name; } set { _Name = value; } }
    public float Hp { get { return _Hp; } set { _Hp = value; if (_HpSlider != null) { _HpSlider.SetValue(_Hp, _MaxHp); } } }

    public float MaxHp { get { return _MaxHp; } set { _MaxHp = value; } }

    public float AttackDamage { get { return _AttackDamage; } set { _AttackDamage = value; } }

    public float AttackRange { get { return _AttackRange; } set { _AttackRange = value; } }

    public int MaxAc { get { return _MaxAc; } set { _MaxAc = value; } }

    public Animator Animator { get { return _Animator; } }
    public bool IsAction { get { return _isAction; } set { _isAction = value; } }
    public bool IsTurn { get { return _isTurn; } }

    public Block CurrentBlock { get { return _CurrentBlock; } set { _CurrentBlock = value; } }
    public Block TargetBlock { get { return _TargetBlock; }set { _TargetBlock = value; } }


    protected virtual void Start()
    {
        Slider slider =  Instantiate(Resources.Load<Slider>("UI/HPSlider/Hp_Bar"));
        _HpSlider = slider.GetComponent<HpSlider>();
        _HpSlider.ParentObject = gameObject;
        if (SaveLoadManager.instance.IsLoad)
            _HpSlider.SetValue(Hp, MaxHp);
    }

    protected virtual void TurnStart()
    {
    }

    protected virtual void TurnEnd()
    {
    }
        

    public void Rotation(Vector3 target)
    {
        if (transform.position.x < target.x)
        {
            if (transform.position.z >= target.z)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }
        else if (transform.position.x > target.x)
        {
            if (transform.position.z <= target.z)
            {
                transform.rotation = Quaternion.Euler(0, 270, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            if (transform.position.z < target.z)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public virtual void TurnSet(bool value)
    {
        _isTurn = value;
        _isAction = value;
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






}
