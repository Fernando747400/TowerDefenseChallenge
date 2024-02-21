using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "TowerStatsSO")]
public class TowerStatsSO : ScriptableObject
{
    public int CostToPurchase;
    public int SellValue;
    public float Range;
    public float FireRate;
    public int Damage;
}
