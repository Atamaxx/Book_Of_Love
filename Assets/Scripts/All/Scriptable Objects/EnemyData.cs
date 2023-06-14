using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : EnemyBase
{
    public string EnemyName;
    [field: SerializeField] public float Age { get; private set; }
    [field: SerializeField] public float MinAge { get; private set; }
    [field: SerializeField] public float MaxAge { get; private set; }
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float DetectRange { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public bool BenjaminButton { get; private set; }

    public override void DoTurn()
    {
        throw new System.NotImplementedException();
    }


}
