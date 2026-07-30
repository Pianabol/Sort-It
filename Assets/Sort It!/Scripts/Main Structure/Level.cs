using UnityEngine;

public class Level : MonoBehaviour
{
    [Header(" Level Settings ")]
    [Tooltip("Bu level için belirlenen geri sayım süresi (Saniye cinsinden)")]
    [SerializeField] private int levelDuration = 90;

    // TimerManager'ın bu değere dışarıdan erişebilmesi için public Property
    public int LevelDuration => levelDuration;
}