using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MonsterFSM 
{
    private MonsterBaseState _curstate;
    public MonsterFSM(MonsterBaseState initstate)
    {
        _curstate = initstate;
        ChangeState(_curstate);
    }


    public void ChangeState(MonsterBaseState nextstate)
    {
        if(nextstate == _curstate) 
        {
            return;
        }

        if (_curstate != null)
            _curstate.OnstateExit();

        _curstate = nextstate;
        _curstate.OnstateEnter();
    }

    public void UpdateState()
    {
        if(_curstate != null)
            _curstate.OnstateUpdate();
    }
}
