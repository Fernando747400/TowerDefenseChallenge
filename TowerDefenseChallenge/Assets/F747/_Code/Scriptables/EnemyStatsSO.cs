using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStatsSO : ScriptableObject
{
    public GameObject Prefab;
    public float MaxHealth;
    public float DamageToPlayer;
    public float ValueOnDeath;
    public Vector3 ModelScale;
    public float MoveSpeed;
    public float RotationSpeed;
    public float ArriveDistance;
}
