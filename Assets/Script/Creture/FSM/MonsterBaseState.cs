using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState
{
    protected MonsterCreture _creture;
    protected MonsterBaseState(MonsterCreture creture)
    {
        _creture = creture;
    }
    protected float _Anifloat;
    protected float _Time;

    public abstract void OnstateEnter();
    public abstract void OnstateUpdate();
    public abstract void OnstateExit();
    public abstract void OnStateAnimation(string anim);
    
}
