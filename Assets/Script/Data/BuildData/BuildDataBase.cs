using UnityEngine;

[CreateAssetMenu(fileName ="BuildDatabase",menuName = "Scriptable Object/MonsterBuild/BuildDataBase", order = int.MaxValue)]
public class BuildDataBase : ScriptableObject
{
    [SerializeField]
    private string _Name;
    [SerializeField]
    private float _MaxHp;
    [SerializeField]
    private int _MaxMonster;
    [SerializeField]
    private int _CreateValue;
    [SerializeField]
    private float _Range;

    public string Name { get { return _Name; } }
    public float MaxHp { get { return _MaxHp; } }
    public int MaxMonster { get { return _MaxMonster; } }
    public int CreateValue { get { return _CreateValue;} }
    public float Range { get { return _Range; } }

}
