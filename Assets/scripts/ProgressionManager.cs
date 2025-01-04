using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public SO_DifficultyProfile difficulty;
    public SO_PhysicsConstants physics;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instance != null && Instance != this ) Destroy(this.gameObject);
        else
        {
            Instance = this;
        }
    }

    public void SetDifficulty()
    {
        
    }
}
