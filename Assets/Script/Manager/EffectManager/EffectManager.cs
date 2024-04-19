using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EffectManager;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private Dictionary<string,ParticleSystem> ParticleSystems = new Dictionary<string, ParticleSystem>();
    

    public enum EffectType
    {
        Hit,
        Aoe
    }



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// 대상 지정형 이펙트 생성입니다.
    /// Hit대상자가 있으며 부착해야할 대상자가 있는 이펙트에 적합합니다.
    /// </summary>
    /// <param name="pos"></생성될 좌표>
    /// <param name="Rotation"></생성되고 나서 이후 바라볼 각도>
    /// <param name="parent"></이펙트의 부모 [피격자]>
    /// <param name="effectpath"></이펙트>
    /// <param name="effectType"></타입>
  
    public void HitEffect(Vector3 pos,Vector3 Rotation, Transform parent, string effectpath,EffectType effectType = EffectType.Hit)
    {
        ParticleSystem Prefab = EffectSerch(effectpath);

        if (Prefab == null)
            return;


        var Effect = Instantiate(Prefab, pos,Quaternion.LookRotation(Rotation));

        if(parent !=null)
            Effect.transform.SetParent(parent);

        Effect.Play();
    }
    /// <summary>
    /// 지점 지정형 이펙트 생성입니다.
    /// Hit대상자가 없으며 지점에 사용되는 이펙트에 적합합니다.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="effectpath"></param>
    /// <param name="effectType"></param>
    public void AoeEffect(Vector3 pos, string effectpath, EffectType effectType = EffectType.Aoe)
    {
        ParticleSystem Prefab = EffectSerch(effectpath);

        if (Prefab == null)
            return;

        var Effect = Instantiate(Prefab,pos, Quaternion.LookRotation(Vector3.zero));


        Effect.Play();

    }


    private ParticleSystem EffectSerch(string effectpath)
    {
        ParticleSystem Prefab;
        if (ParticleSystems.ContainsKey(effectpath))
        {
            Prefab = ParticleSystems[effectpath];
        }
        else
        {
            Prefab = Resources.Load<ParticleSystem>(effectpath);
            if (Prefab != null)
            {
                ParticleSystems.Add(effectpath, Prefab);
            }
        }
        if (Prefab == null)
        {
            Debug.Log("이펙트 서치 실패");
            return null;
        }else
        {
            return Prefab;
        }
    }



    

}
