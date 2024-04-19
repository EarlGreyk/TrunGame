using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerFSM 
{
    private PlayerBaseState _curstate;
    public PlayerFSM(PlayerBaseState initstate)
    {
        _curstate = initstate;
        ChangeState(_curstate);
    }


    public void ChangeState(PlayerBaseState nextstate)
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
