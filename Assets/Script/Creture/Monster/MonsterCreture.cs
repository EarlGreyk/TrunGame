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




    //자신의 턴일때 행동가능한지 체크

    private MonsterBuild _ParentMonsterBuild;
    public MonsterBuild ParentMonsterBuild { get { return _ParentMonsterBuild; } set { _ParentMonsterBuild = value; } }
    private PlayerCreture _PTarget;
    public PlayerCreture PTarget { get { return _PTarget; } set { _PTarget = value; } }


    protected override void Start()
    {
        //몬스터의 데이터를 받아오고 받아온 데이터에 따라 초기화합니다.
        base.Start();
        MonsterInit();
    }
    //몬스터에 따른 초기화
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



        //임시값입니다. 데이터로 가져와야합니다
        //몬스터바다 아래는 [각자]있어야 하는 존재값입니다.

    }

    // Update is called once per frame
    private void Update()
    {
        if(_isTurn == true)
        {
            //행동 순서 : 대상체크 -> 공격 ->이동
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

    //대상 설정.
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
    //공격 가능설정
    private bool CheckAttack()
    {
        bool check = false;
        float Distance = Vector3.Distance(PTarget.transform.position, transform.position);
        //1차 조건 : 대상을 인식했는가.
        if (Distance <= _AttackRange * 2)
        {
            //2번쨰 조건 대상과 인식을 했는데 대상한테 이동할 수 있는가?
            //자신의 위치에서 위모든 길찾기를 실행하고 연결이 있으면 즉시 true로 전환해야함.
            //결국 블록을 넣어야하는데... 걍 플레이어 한테 바라보게 한다음에. 그방향으로 이동시키면? 날먹이긴함.
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
    /// 행동
    /// </summary>
    //기본이동 [건물주변 배회]
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
