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
    private GravityPlane _gravityPlane;

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
        StageFactory factory = new StageFactory(5, 30, 30, difficultyProfile);
        StageInfo info = factory.create();
        stageSpawner.SpawnStage(info);
        _gravityPlane = info.GravityPlane;
    }

    public Vector2 GravityFromPosition(Vector2 position)
    {
        if (_gravityPlane == null) return Vector2.zero;

        return _gravityPlane.GetGravityAtPos(position);
    }
}
