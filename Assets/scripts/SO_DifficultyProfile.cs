using UnityEngine;

[CreateAssetMenu(fileName = "SO_DifficultyProfile", menuName = "Difficulty Profile")]
public class SO_DifficultyProfile : ScriptableObject
{
    [SerializeField] private Vector2Int starCountRange;
    [SerializeField] private Vector2Int starSizeRange;
    [SerializeField] private Vector2Int enemyCountRange;
}
