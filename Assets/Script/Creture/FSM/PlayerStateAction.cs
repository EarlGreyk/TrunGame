using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

namespace PlayerStateAction
{ 
    public class PlayerNoneState : PlayerBaseState
    {
        public PlayerNoneState(PlayerCreture creture) : base(creture) { }

        //상태에 처음 진입. 호출
        public override void OnstateEnter()
        {
            if (_creture.PR.CurrentUsecard)
            {
                _creture.PR.CardUse();
            }
            _creture.IsAction = true;
            _creture.TargetBlock = null;

        }
        //상태 변경 호출.
        public override void OnstateUpdate()
        {

        }

        //매 프레임 호출
        public override void OnstateExit()
        {
       
        }

       

        public override void OnStateAnimation(string anim)
        {
        }
    }

    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerCreture creture) : base(creture) { }
        private Vector3 vel = Vector3.zero;


        //가지고 있어야 하는것. 부모의 좌표.
        public override void OnstateEnter()
        {
            _Time = 0f;
            if (_creture.TargetBlock != null)
            {
                OnStateAnimation("Move");
            }
            else
            {
                _creture.IsAction = false;
                return;
            }


            CameraManager.instance.LockCameraMove();

            Block target = _creture.TargetBlock;
            if (target.BlockType == Block.type.None || target.BlockType == Block.type.Break)
            {
                _creture.Rotation(_creture.TargetBlock.transform.position);
            }
            else
            {
                _creture.IsAction = false;

            }

        }

        public override void OnstateUpdate()
        {
            if (_creture.TargetBlock != null)
            {
                if (Vector3.Distance(_creture.transform.position, _creture.TargetBlock.transform.position) > 0.05f && _Time < _Anifloat)
                {
                    _Time += Time.deltaTime;
                    TargetMove();
                }
                else
                {
                    _creture.IsAction = false;
                }
            }
            else
            {
                _creture.IsAction = false;
            }
        }
        public override void OnstateExit()
        {
            _creture.transform.position = _creture.TargetBlock.transform.position;
            _creture.CurrentBlock.isobejct = false;
            _creture.TargetBlock.isobejct = true;
            _creture.CurrentBlock = _creture.TargetBlock;
            OnStateAnimation("Idle");
        }
        public override void OnStateAnimation(string anim)
        {
            Animator ani = _creture.Animator;
            ani.Play(anim);
            _Anifloat = 1f;

        }
        private void TargetMove()
        {
            _creture.transform.position = Vector3.SmoothDamp(_creture.transform.position, _creture.TargetBlock.transform.position, ref vel, _Anifloat - _Time);
        }





    }
   
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(PlayerCreture creture) : base(creture) { }

        public override void OnstateEnter()
        {
            _Time = 0f;
            if (_creture.TargetBlock != null)
            {
                OnStateAnimation("Attack");
                _creture.Rotation(_creture.TargetBlock.transform.position);
            }
            else
            {
                _creture.IsAction = false;
                return;
            }


            CameraManager.instance.LockCameraMove();


        }
        public override void OnstateUpdate()
        {
            if (_Time < _Anifloat)
                _Time += Time.deltaTime;
            else
                _creture.IsAction = false;
        }

        public override void OnstateExit()
        {
            if (_creture.TargetBlock.BlockType != Block.type.EnemyBuild)
                _creture.TargetBlock.Monster.Hit(_creture.AttackDamage);
            else
                _creture.TargetBlock.MonsterBuild.Hit(_creture.AttackDamage);

            OnStateAnimation("Idle");
        }
        public override void OnStateAnimation(string anim)
        {
            Animator ani = _creture.Animator;
            ani.Play(anim);
            _Anifloat = ani.GetCurrentAnimatorStateInfo(0).length;
        }
    }




    public class PlayerGarthingState : PlayerBaseState
    {
        public PlayerGarthingState(PlayerCreture creture) : base(creture) { }

        public override void OnstateEnter()
        {
            if( _creture.TargetBlock != null)
            {
                _creture.IsAction = true;
                OnStateAnimation("Gathing");
                _creture.Rotation(_creture.TargetBlock.transform.position);
            }
            else
            {
                _creture.IsAction = false;
            }
        }
        public override void OnstateUpdate()
        {
            if (_Time < _Anifloat)
                _Time += Time.deltaTime;
            else
            {
                _creture.IsAction = false;
            }
        }

        public override void OnstateExit()
        {
            TargetGathing();
            OnStateAnimation("Idle");
        }

      
        public override void OnStateAnimation(string anim)
        {
            Animator ani = _creture.Animator;
            ani.Play(anim);
            _Anifloat = ani.GetCurrentAnimatorStateInfo(0).length;
        }

        private  Block Checkblock()
        {
            Block block = null;
            List<Block> MoveBlock = new List<Block>(9);
            LayerMask layer = LayerMask.GetMask("Plan");
            Collider[] colls = Physics.OverlapSphere(_creture.transform.position + _creture.transform.forward, 0.5f, layer);
            for (int i = 0; i < colls.Length; i++)
            {
                block = colls[i].GetComponent<Block>();
                Block.type type = block.BlockType;
                if (type == Block.type.Material || type == Block.type.Tree)
                {
                    return block;
                }
            }

            return block;

        }


        private void TargetGathing()
        {
            if (_creture.TargetBlock.BlockType == Block.type.Item)
            {
                ItemGet();
                return;
            }
                
            if (_creture.TargetBlock.BlockType == Block.type.Material || _creture.TargetBlock.BlockType == Block.type.Tree)
            {
                if (_creture.TargetBlock.BlockType == Block.type.Material)
                {
                    //메테리얼의 value값에 맞는 값을 증가시키면됨.
                    int i = _creture.TargetBlock.ObjectValue;
                    switch (i)
                    {
                        case 1:
                            _creture.PR.Metal++;
                            UImanager.Instance.LogTextSet("철 자원 증가 :  " + 1);
                            break;
                        case 2:
                            _creture.PR.Stone++;
                            UImanager.Instance.LogTextSet("돌 자원 증가 :  " + 1);
                            break;
                        case 3:
                            _creture.PR.Metal++;
                            UImanager.Instance.LogTextSet("철 자원 증가 :  " + 1);
                            break;
                        default:
                            Debug.Log(i + "무슨재료임?");

                            break;
                    }
                    GameManager.instance.PRCount++;

                }
                else
                {
                    _creture.PR.Tree++;
                    UImanager.Instance.LogTextSet("나무 자원 증가 :  " + 1);
                }

                _creture.TargetBlock.Value--;
            }
        }

        private void ItemGet()
        {
            _creture.TargetBlock.Item.Get();
            _creture.TargetBlock.Value--;
        }
    }

    
}
