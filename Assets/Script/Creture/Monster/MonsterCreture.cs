using MonsterStateAction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

using Random = UnityEngine.Random;

public class MonsterCreture : Creture, CretureHit
{
    private enum State
    {
        None,
        BMove,
        PMove,
        Attack,
        Garthing
    }

    private State _curstate;
    private MonsterFSM _fsm;
    private MonsterAI _ai;
    public MonsterAI AI { get { return _ai; } }


    protected string _MonsterName;
    protected float _TargetRange;
    protected int _AcionCount;
    protected int _MaxAcionCount;
    public string MosnterName { get { return _MonsterName; } set { _MonsterName = value; } }
    public float TargetRange { get { return _TargetRange; } set { _TargetRange = value; } }
    public int AcionCount { get { return _AcionCount; } set { _AcionCount = value; } }
    public int MaxAcionCount { get { return _MaxAcionCount; } set { _MaxAcionCount = value; } }




    //�ڽ��� ���϶� �ൿ�������� üũ

    private MonsterBuild _ParentMonsterBuild;
    public MonsterBuild ParentMonsterBuild { get { return _ParentMonsterBuild; } set { _ParentMonsterBuild = value; } }
    private PlayerCreture _PTarget;
    public PlayerCreture PTarget { get { return _PTarget; } set { _PTarget = value; } }


    protected override void Start()
    {
        //������ �����͸� �޾ƿ��� �޾ƿ� �����Ϳ� ���� �ʱ�ȭ�մϴ�.
        base.Start();
        MonsterInit();
    }
    //���Ϳ� ���� �ʱ�ȭ
    private void MonsterInit()
    {
        _fsm = new MonsterFSM(new MonsterNoneState(this));
        _ai = GetComponent<MonsterAI>();
        _Animator = GetComponent<Animator>();
        _isAction = false;
        _isTurn = false;
        _curstate = State.None;

        int x = GameSetManager.Instance.Size;
        int z = GameSetManager.Instance.Size;
        _ai.topRight = Vector3Int.FloorToInt(GameManager.instance.BlockArray[x-1, z-1].transform.position);
        _ai.bottomLeft = Vector3Int.FloorToInt(GameManager.instance.BlockArray[0, 0].transform.position);



        //�ӽð��Դϴ�. �����ͷ� �����;��մϴ�
        //���͹ٴ� �Ʒ��� [����]�־�� �ϴ� ���簪�Դϴ�.

    }

    // Update is called once per frame
    private void Update()
    {
        if(_isTurn == true)
        {
            //�ൿ ���� : ���üũ -> ���� ->�̵�
            if (_isAction == true)
            {
                if (_curstate == State.None)
                {
                    if (PTarget == null)
                        TargetSet();


                    if (PTarget != null)
                    {
                        
                        Rotation(PTarget.transform.position);
                        TargetBlock = AI.TargetBlock();
                        if (CheckAttack() )
                        {
                            if (CheckAc())
                            {
                                ChangeState(State.Attack);
                                return;

                            }else
                            {
                                ChangeState(State.None);
                                return;
                            }
                        }
                        else
                        {
                            if (CheckGarthing())
                            {
                                if(CheckAc())
                                { 
                                    ChangeState(State.Garthing);
                                }else
                                {
                                    ChangeState(State.None);
                                }
                            }
                            else
                            {
                                if(CheckAc())
                                {
                                    ChangeState(State.PMove);
                                }else
                                {
                                    ChangeState(State.None);
                                }
                                
                            }
                        }

                    }else
                    {
                        if(CheckAc())
                        {
                            ChangeState(State.BMove);
                        }else
                        {
                            ChangeState(State.None);
                        }
                    }
                }

                _fsm.UpdateState();

            }
            else
            {
                ChangeState(State.None);
            }
           

        }
    }

    private void ChangeState(State nextState)
    {
        _curstate = nextState;
        switch(_curstate) 
        {
            case State.None:
                _fsm.ChangeState(new MonsterNoneState(this));
                break;
            case State.Attack:
                _fsm.ChangeState(new MonsterAttackState(this));
                break;
            case State.PMove:
                _fsm.ChangeState(new MonsterPMoveState(this));
                break;
            case State.BMove:
                _fsm.ChangeState(new MonsterBMoveState(this));
                break;
            case State.Garthing:
                _fsm.ChangeState(new MonsterGarthingState(this));
                break;

        }
    }

    //��� ����.
    private bool TargetSet()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider[] colls = Physics.OverlapSphere(transform.position, _TargetRange*2, mask);
        for (int i = 0; i < colls.Length; i++)
        {
            if (PTarget == null)
            {
                _PTarget = colls[i].gameObject.GetComponent<PlayerCreture>();
                break;
            }
        }

        if (PTarget == null)
            return false;
        else
            return true; 

    }
    //���� ���ɼ���
    private bool CheckAttack()
    {
        bool check = false;
        float Distance = Vector3.Distance(PTarget.transform.position, transform.position);
        //1�� ���� : ����� �ν��ߴ°�.
        if (Distance <= _AttackRange * 2)
        {
            //2���� ���� ���� �ν��� �ߴµ� ������� �̵��� �� �ִ°�?
            //�ڽ��� ��ġ���� ����� ��ã�⸦ �����ϰ� ������ ������ ��� true�� ��ȯ�ؾ���.
            //�ᱹ ����� �־���ϴµ�... �� �÷��̾� ���� �ٶ󺸰� �Ѵ�����. �׹������� �̵���Ű��? �����̱���.
            check = true;
        }
        return check;
    }

    


    private bool CheckGarthing()
    {
        bool check = false;

        
        if (TargetBlock.BlockType == Block.type.Material || TargetBlock.BlockType == Block.type.Tree)
        {
            check = true;
            //BTarget = block;
            return check;
        }




        return check;

    }

    private bool CheckAc()
    {

        if(_AcionCount>0)
        {
            _AcionCount--;
            return  true;
        }else
        {
            _AcionCount = 0;
            return false;            
        }
        
        
    }

    /// <summary>
    /// ///////////////////////////////////////////////
    /// �ൿ
    /// </summary>
    //�⺻�̵� [�ǹ��ֺ� ��ȸ]
    public void Setparent(MonsterBuild Pmb)
    {
        ParentMonsterBuild = Pmb;
    }



    private void OnDestroy()
    {
        GameManager.instance.KillCount++;
    }
    public void Hit(float damage)
    {
        _Hp -= damage;
        _HpSlider.SetValue(_Hp, _MaxHp);
        SoundManager.instance.AudioPlay("Sound/Effect/Creture/Sound_Hit");
        Vector3 hitv = transform.position;
        UnitStateUiMnager.Instance.StateGet(this);
        if (_Hp <= 0)
        {
            _CurrentBlock.isobejct = false;
            Destroy();
        }else
        {
            EffectManager.instance.HitEffect(hitv, hitv, transform, "Effect/Hit/Holy hit");
        }
    }
    private void Destroy()
    {
        UnitStateUiMnager.Instance.StateUiEnable();
        GameManager.instance.KillCount++;
        _ParentMonsterBuild.MonsterList.Remove(this);
        Destroy(gameObject);
    }
}
