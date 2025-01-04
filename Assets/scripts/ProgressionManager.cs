using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public SO_DifficultyProfile difficultyProfile;
    public SO_PhysicsConstants physicsProfile;

    private int _stage = 0;
    
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
}
