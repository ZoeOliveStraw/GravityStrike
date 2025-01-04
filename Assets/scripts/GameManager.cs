using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private StageSpawner  stageSpawner;
    [SerializeField] public SO_PhysicsConstants physicsConstants;
    [SerializeField] public Camera sceneCamera;
    [SerializeField] public SO_DifficultyProfile difficultyProfile;

    private List<Well> _wells;
    private List<Transform> _enemyTransforms;

    private void Start()
    {
        if(Instance != null && Instance != this) Destroy(this.gameObject);
        else
        {
            Instance = this;
        }
        InitializeGame();
    }

    private void InitializeGame()
    {
        
    }
}
