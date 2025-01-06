using UnityEngine;
using UnityEngine.Serialization;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public SO_DifficultyProfile difficultyProfile;
    public SO_PhysicsConstants physicsProfile;
    public int livesRemaining = 0;

    public int stage = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instance != null && Instance != this ) Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetDifficulty(SO_DifficultyProfile difficulty, SO_PhysicsConstants physics)
    {
        difficultyProfile = difficulty;
        physicsProfile = physics;
    }

    public void SetStartingLives()
    {
        
    }
}
