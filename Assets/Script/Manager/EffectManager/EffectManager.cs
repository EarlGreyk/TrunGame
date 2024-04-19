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
    /// ��� ������ ����Ʈ �����Դϴ�.
    /// Hit����ڰ� ������ �����ؾ��� ����ڰ� �ִ� ����Ʈ�� �����մϴ�.
    /// </summary>
    /// <param name="pos"></������ ��ǥ>
    /// <param name="Rotation"></�����ǰ� ���� ���� �ٶ� ����>
    /// <param name="parent"></����Ʈ�� �θ� [�ǰ���]>
    /// <param name="effectpath"></����Ʈ>
    /// <param name="effectType"></Ÿ��>
  
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
    /// ���� ������ ����Ʈ �����Դϴ�.
    /// Hit����ڰ� ������ ������ ���Ǵ� ����Ʈ�� �����մϴ�.
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
            Debug.Log("����Ʈ ��ġ ����");
            return null;
        }else
        {
            return Prefab;
        }
    }



    

}
