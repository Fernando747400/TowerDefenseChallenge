using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStatsSO : ScriptableObject
{
    public GameObject Prefab;
    public int MaxHealth;
    public int DamageToPlayer;
    public int CoinsOnDeath;
    public Vector3 ModelScale;
    public float MoveSpeed;
    public float RotationSpeed;
    public float ArriveDistance;
}
