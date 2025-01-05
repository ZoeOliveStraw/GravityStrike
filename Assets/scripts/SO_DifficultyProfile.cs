using UnityEngine;

[CreateAssetMenu(fileName = "SO_DifficultyProfile", menuName = "Difficulty Profile")]
public class SO_DifficultyProfile : ScriptableObject
{
    [SerializeField] public string difficultyName;
    [SerializeField] public Vector2Int starCountRange;
    [SerializeField] public Vector2 starSizeRange;
    [SerializeField] public Vector2Int enemyCountRange;
    [SerializeField] public float minDistanceBetweenStarSurfaces;
    [SerializeField] public int maxLives;

    public int GetStarCount()
    {
        return Random.Range(starCountRange.x, starCountRange.y);
    }

    public float GetStarSize()
    {
        return Random.Range(starSizeRange.x, starSizeRange.y);
    }

    public int GetEnemyCount()
    {
        return Random.Range(enemyCountRange.x, enemyCountRange.y);
    }
}
