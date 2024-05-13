using System.Collections.Generic;
using UnityEngine;

namespace MonsterStateAction
{ 
    public class MonsterNoneState : MonsterBaseState
    {
        public MonsterNoneState(MonsterCreture creture) : base(creture) { }

        //상태에 처음 진입. 호출
        public override void OnstateEnter()
        {
            if (_creture.AcionCount >0)
            {
                _creture.IsAction = true;
            }else
            {
                _creture.TurnSet(false);
                _creture.AcionCount = _creture.MaxAcionCount;
            }
            
       
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

    public class MonsterBMoveState : MonsterBaseState
    {
        public MonsterBMoveState(MonsterCreture creture) : base(creture) { }
        private Block TargetBlock;
        private Vector3 vel = Vector3.zero;


        //가지고 있어야 하는것. 부모의 좌표.
        public override void OnstateEnter()
        {
            _Time = 0f;
            TargetBlock = TargetBlockSet();
            CameraManager.instance.TargetSet(_creture);
            if (TargetBlock != null)
            {
                _creture.Rotation(TargetBlock.transform.position);
                OnStateAnimation("Move");
            }
           
        }

        public override void OnstateUpdate()
        {
            if(TargetBlock != null)
            { 
                if (Vector3.Distance(_creture.transform.position, TargetBlock.transform.position) > 0.05f && _Time < _Anifloat)
                {
                    _Time += Time.deltaTime;
                    TargetMove();
                }
                else
                {
                    _creture.transform.position = TargetBlock.transform.position;               
                    _creture.IsAction = false;
                }
            }else
            {
                _creture.IsAction = false;
            }

        }
        public override void OnstateExit()
        {
            _creture.IsAction = false;
            _creture.CurrentBlock = TargetBlock;
            OnStateAnimation("Idle");
        }
        public override void OnStateAnimation(string anim)
        {
            Animator ani = _creture.Animator;
            ani.Play(anim);
            _Anifloat = ani.GetCurrentAnimatorStateInfo(0).length;
        }


        private Block TargetBlockSet()
        {
            Block target = null;
            MonsterBuild mb = _creture.ParentMonsterBuild;
            
            //자신의 부모 블록의 Range내에 존재하며 None블록이며 자기 상 하 좌우 이여야함.
            //가장편한건 조건에 부합하는 블록을 산출한뒤 랜덤으로 가져와서 이동시키기.
            //float Range = mb.Range;
            Vector3 Center = mb.transform.position;
            //첫째 자신이 이동할 수 있는 모든 블럭 산출하기. 
            List<Block> MoveBlock = new List<Block>(9);
            Block CenterBlock = null;
            LayerMask layer = LayerMask.GetMask("Plan");
            Collider[] colls = Physics.OverlapSphere(_creture.transform.position, 2, layer);
            for (int i = 0; i < colls.Length; i++)
            {
                MoveBlock.Add(colls[i].GetComponent<Block>());
            }

            //거리 비교해서 블록 지우기
            for (int i = MoveBlock.Count - 1; i >= 0; i--)
            {
                float Distance = Vector3.Distance(MoveBlock[i].transform.position, mb.transform.position);
                float Distance2 = Vector3.Distance(MoveBlock[i].transform.position, _creture.transform.position);

                if (Distance2 == 0)
                {
                    CenterBlock = MoveBlock[i];
                }
                if (Distance > mb.Range || Distance2 > 2)
                {
                    MoveBlock.Remove(MoveBlock[i]);
                }

            }
            

            if (MoveBlock.Count > 0)
            {
                int maxCount = 0;
                while (true)
                {

                    int random = Random.Range(0, MoveBlock.Count);
                    if ((MoveBlock[random].BlockType == Block.type.None || MoveBlock[random].BlockType == Block.type.Break))
                    {
                        if(MoveBlock[random].isobejct == false)
                        {
                            target = MoveBlock[random];
                            if (CenterBlock != null)
                                CenterBlock.isobejct = false;

                            target.isobejct = true;
                            CenterBlock.Monster = null;
                            target.Monster = _creture;
                            break;
                        }
                    }else
                    {
                        maxCount++;
                        if (maxCount > 30)
                        {
                            Debug.Log("오류 너무 많은 값을 찾고있습니다.");
                            break;
                        }

                    }
 

                }
            }
            return target;
        }

        private void TargetMove()
        {
            _creture.transform.position = Vector3.SmoothDamp(_creture.transform.position, TargetBlock.transform.position,ref vel,_Anifloat-_Time);



        }

    }
    public class MonsterPMoveState : MonsterBaseState
    {
        public MonsterPMoveState(MonsterCreture creture) : base(creture) { }
        private Block TargetBlock;
        private Vector3 vel = Vector3.zero;
    


        public override void OnstateEnter()
        {

            _Time = 0f;
            _creture.AI.startPos = Vector3Int.FloorToInt(_creture.transform.position);
            _creture.AI.targetPos = Vector3Int.FloorToInt(_creture.PTarget.transform.position);
            TargetBlock = _creture.TargetBlock;
            CameraManager.instance.TargetSet(_creture);

            if (TargetBlock != null)
            {
                _creture.Rotation(TargetBlock.transform.position);
                OnStateAnimation("Move");
            }

        }

        public override void OnstateUpdate()
        {
            if(TargetBlock != null)
            { 
                if (Vector3.Distance(_creture.transform.position,TargetBlock.transform.position)>0.05f && _Time < _Anifloat  )
                {
                    _Time += Time.deltaTime;
                    TargetMove();
                }
                else
                {
                    _creture.transform.position = TargetBlock.transform.position;
                    _creture.IsAction = false;
                }
            }else
            {
                _creture.IsAction = false;
            }
        }
        public override void OnstateExit()
        {
            _creture.IsAction = false;
            _creture.CurrentBlock.isobejct = false;
            _creture.CurrentBlock.Monster = null;
            _creture.CurrentBlock = TargetBlock;
            TargetBlock.isobejct = true;
            TargetBlock.Monster = _creture;

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
            _creture.transform.position = Vector3.SmoothDamp(_creture.transform.position, TargetBlock.transform.position, ref vel, _Anifloat);
        }

   

        private Block TargetBlockSet()
        {
            MonsterBuild mb = _creture.ParentMonsterBuild;
            List<Block> MoveBlock = new List<Block>();
            Block Targetblock = null;
            Block Tempblock = null;
            Block CenterBlock = null;

            LayerMask layer = LayerMask.GetMask("Plan");
            Collider[] colls = Physics.OverlapSphere(_creture.transform.position, 2, layer);
            for (int i = 0; i < colls.Length; i++)
            {
                MoveBlock.Add(colls[i].GetComponent<Block>());
            }
            CenterBlock = _creture.CurrentBlock;
            
            int angle = 0;
            float dis = Vector3.Distance(_creture.transform.position,_creture.PTarget.transform.position);
            for (int i = 0; i < 4; i++)
            {
                // 90 x+2 270 x-2 0 z+2 180 z-2
                switch (angle%360)
                {
                    case 0:
                        Targetblock = CheckTargetBlock(MoveBlock, 0, 2);
                        break;
                    case 90:
                        Targetblock = CheckTargetBlock(MoveBlock, 2, 0);
                        break;
                    case 180:
                        Targetblock = CheckTargetBlock(MoveBlock, 0, -2);
                        break;
                    case 270:
                        Targetblock = CheckTargetBlock(MoveBlock, -2, 0);
                        break;
                }
                if (Targetblock != null && Targetblock.isobejct == false )
                {
                    float dis2 = PBDistance(Targetblock.transform.position);
                    if(dis2 < dis)
                    {
                        dis = dis2;
                        Tempblock = Targetblock; 
                    }
                    
                    
                }
                angle += 90;
             
            }
            if(Tempblock !=null)
            {
                Targetblock = Tempblock;
            }
            if(Targetblock !=null)
            {
                if(!Targetblock.isobejct)
                {
                    CenterBlock.isobejct = false;
                    Targetblock.isobejct = true;
                    CenterBlock.Monster = null;
                    Targetblock.Monster = _creture;
                }else
                {
                    Targetblock = null;
                }
            }
                

            return Targetblock;



            //이동 인식가능. 경로찾기해야함.
            //x와 z의 값을 22.해서 None인곳이 연결되어 있으면 대상과 이동이 가능하다는 뜻임으로 이이동을 사용해야함.
        }

        private Block CheckTargetBlock(List<Block> MoveBlock, int x, int z)
        {
            Block block = null;
            Vector3 pos = _creture.transform.position + new Vector3(x, 0, z);

            for (int i = 0; i < MoveBlock.Count; i++)
            {
                if (MoveBlock[i].transform.position == pos && (MoveBlock[i].BlockType == Block.type.None || MoveBlock[i].BlockType == Block.type.Break))
                {
                    block = MoveBlock[i];
                    break;
                }
            }

            return block;
        }

        private float PBDistance(Vector3 Target)
        {
            float dis = 0f;
            dis = Vector3.Distance(Target, _creture.PTarget.transform.position);

            return dis;

        }
    }

    public class MonsterAttackState : MonsterBaseState
    {
        public MonsterAttackState(MonsterCreture creture) : base(creture) { }

        public override void OnstateEnter()
        {
            _Time = 0;
            CameraManager.instance.TargetSet(_creture);
            if (_creture.PTarget !=null)
            { 
                OnStateAnimation("Attack");

            }
        }
        public override void OnstateUpdate()
        {
            {
                if (_Time < _Anifloat)
                {
                    _Time += Time.deltaTime;
                }
                else
                {
                    _creture.PTarget.Hit(_creture.AttackDamage);
                    _creture.IsAction = false;
                }
            }
        }

        public override void OnstateExit()
        {
            _creture.IsAction = false;
            OnStateAnimation("Idle");
        }
        public override void OnStateAnimation(string anim)
        {
            Animator ani = _creture.Animator;
            ani.Play(anim);
            _Anifloat = ani.GetCurrentAnimatorStateInfo(0).length;
        }
    }




    public class MonsterGarthingState : MonsterBaseState
    {
        public MonsterGarthingState(MonsterCreture creture) : base(creture) { }
        private Block TargetBlock;

        public override void OnstateEnter()
        {
            _Time = 0;
            TargetBlock = Checkblock();
            CameraManager.instance.TargetSet(_creture);
            if (TargetBlock != null)
            { 
                _creture.Rotation(TargetBlock.transform.position);
                OnStateAnimation("Gathing");
            }
        }
        public override void OnstateUpdate()
        {
            if (_Time < _Anifloat)
            {
                _Time += Time.deltaTime;
            }
            else
            {
                TargetGathing();
                _creture.IsAction = false;
            }
        }

        public override void OnstateExit()
        {
            _creture.IsAction = false;
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
            TargetBlock.Value--;
        }
    }
}
