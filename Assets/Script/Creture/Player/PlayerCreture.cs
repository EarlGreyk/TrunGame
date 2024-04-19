using MonsterStateAction;
using PlayerStateAction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerCreture : Creture,CretureHit
{
    // Start is called before the first frame update
    //캐릭터이며 직업을 분류해놓지는 않음.

    public enum State
    {
        None = 0,//아무런상태가아님.
        Move = 1,//이동가능
        Attack = 2,//공격
        gathering = 3,//채집
        Item =4//아이템획득

    }
    private State _curstate;

    private Ray ray; //월드좌표 반환하기 위한 ray
    private PlayerResource _PR; //플레이어가 소유하고 있는 자원을 관리하는 클래스
    protected PlayerDataBase _Pdata;
    public PlayerResource PR { get { return _PR; } }
    public PlayerDataBase Pdata { get { return _Pdata; } }

    private PlayerFSM _fsm;
    private int _MaxHand;
    public int MaxHand { get { return _MaxHand; } set { _MaxHand = value; } }

    private bool _isStop = false;
    public bool IsStop {set { _isStop = value; } }






protected override void Start()
    {
        base.Start();
        if (SaveLoadManager.instance.IsLoad == false)
        {
            NewInit();
        }else
        {
            LoadInit();
        }

        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!_isStop)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Block block = UITargetCheck(Input.mousePosition);
                if (block != null)
                {
                    if (block.isobejct == true)
                    {
                        if (block.Monster !=null)
                            UnitStateUiMnager.Instance.StateGet(block.Monster);
                        else if(block.MonsterBuild != null)
                            UnitStateUiMnager.Instance.StateGet(block.MonsterBuild);
                    }
                }
                
            }

        }
        if(_isTurn  )
        { 
            if(_isAction == true)
            {
                if(_curstate == State.None)
                {
                    if (Input.GetMouseButtonDown(1) && TargetBlock == null)
                    {

                        TargetBlock = TargetCheck(Input.mousePosition);

                        //현재 플레이어 타입에 따른 정보 실행.
                        if (TargetBlock != null)
                        {
                            //Rotation(target.transform.position);
                            if (TargetBlock.isobejct == true)
                            {
                                if (Check(State.Attack))
                                    ChangeState(State.Attack);
                                else
                                    TargetBlock = null;
                            }
                            else
                            {
                                switch (TargetBlock.BlockType)
                                {
                                    case Block.type.None:
                                        if (Check(State.Move))
                                            ChangeState(State.Move);
                                        else
                                            TargetBlock = null;
                                        break;
                                    case Block.type.Break:
                                        if (Check(State.Move))
                                            ChangeState(State.Move);
                                        else
                                            TargetBlock = null;
                                        break;
                                    case Block.type.EnemyBuild:
                                        if (Check(State.Attack))
                                            ChangeState(State.Attack);
                                        else
                                            TargetBlock = null;
                                        break;
                                    case Block.type.Material:
                                        if (Check(State.gathering))
                                            ChangeState(State.gathering);
                                        else
                                            TargetBlock = null;
                                        break;
                                    case Block.type.Tree:
                                        if (Check(State.gathering))
                                            ChangeState(State.gathering); 
                                        else
                                            TargetBlock = null;
                                        break;
                                    case Block.type.Item:
                                        if (Check(State.gathering))
                                            ChangeState(State.Item);
                                        else
                                            TargetBlock = null;
                                        break;

                                }
                            }


                        }

                    }
                }else
                {
                    if (TargetBlock != null)
                    {
                        _fsm.UpdateState();
                    }
                }


            }
            else
            {
                ChangeState(State.None);
            }
            
        }
    }


    private void NewInit()
    {
        _PR = GetComponent<PlayerResource>();
        _Animator = GetComponent<Animator>();
        _fsm = new PlayerFSM(new PlayerNoneState(this));
        TurnSet(true);
    }
    private void LoadInit()
    {
        _PR = GetComponent<PlayerResource>();
        _Animator = GetComponent<Animator>();
        _fsm = new PlayerFSM(new PlayerNoneState(this));
        TurnSet(true);
    }


    //이동

    private void ChangeState(State nextState)
    {
        _curstate = nextState;
        switch (_curstate)
        {
            case State.None:
                _fsm.ChangeState(new PlayerNoneState(this));
                break;
            case State.Attack:
                _fsm.ChangeState(new PlayerAttackState(this));
                break;
            case State.Move:
                _fsm.ChangeState(new PlayerMoveState(this));
                break;
            case State.gathering:
                _fsm.ChangeState(new PlayerGarthingState(this));
                break;
            case State.Item:
                _fsm.ChangeState(new PlayerGarthingState(this));
                break;

        }
    }
    public void SetState(State state)
    {
        ChangeState(state);
    }
    protected override void TurnStart()
    {
        base.TurnStart();
        _fsm.ChangeState(new PlayerNoneState(this));
        PR.ActiveCardLIstSet();
        CameraManager.instance.TargetSet(this);

    }

    protected override void TurnEnd()
    {
        base.TurnEnd();
        //플레이어의 동작이 끝나고 턴을 증가시킵니다.
        GameManager.instance.IsTurn = true;
        GameManager.instance.Turn++;

    }
    public override void TurnSet(bool value)
    {
        base.TurnSet(value);
        if (_isTurn == false)
        {
            TurnEnd();
        }
        else
        {
            TurnStart();
        }
    }
   

    private Block TargetCheck(Vector3 point)
    {
        ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Plan");
        Block Btarget = null;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            Btarget = hit.transform.GetComponent<Block>();
        }

        if(Btarget != null)
        {
            float Distance = Vector3.Distance(transform.position, Btarget.transform.position);


            if (Btarget.isobejct == true)
            {
                if (Distance <= _AttackRange * 2)
                {
                }
                else
                {
                    Btarget = null;
                }
            }
            else
            {
                if (Distance > 2)
                {
                    Btarget = null;
                }
            }
            if (Btarget == CurrentBlock)
                Btarget = null; 
        }
        return Btarget;

    }
    private Block UITargetCheck(Vector3 point)
    {
        ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Plan");
        Block Btarget = null;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            Btarget = hit.transform.GetComponent<Block>();
        }

        if (Btarget != null)
        {
            float Distance = Vector3.Distance(transform.position, Btarget.transform.position);


            if (TargetBlock == CurrentBlock)
                Btarget = null;
        }
        return Btarget;
    }
    private bool Check(State state)
    {
        if (PR.CardSerch(state))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void Hit(float damage)
    {
        _Hp -= damage;
        _HpSlider.SetValue(_Hp, _MaxHp);
        SoundManager.instance.AudioPlay("Sound/Effect/Creture/Sound_Hit");
        UnitStateUiMnager.Instance.StateGet(this);
        if (_Hp <= 0)
        {
            _CurrentBlock.isobejct = false;
            Destroy();
        }else
        {
            EffectManager.instance.HitEffect(transform.position, transform.position, transform, "Effect/Hit/Holy hit");
        }
    }
    private void Destroy()
    {
        EffectManager.instance.AoeEffect(transform.position, "Effect/Hit/Explosion hit");
        Destroy(gameObject);
    }






}
