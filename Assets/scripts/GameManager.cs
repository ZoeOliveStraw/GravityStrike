using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public enum SoundEffects
    {
        MissileFire,
        MissileExplode,
        ShipThruster,
        MissileThruster
    }
    
    public static GameManager Instance;
    
    [SerializeField] private StageSpawner  stageSpawner;
    [SerializeField] public SO_PhysicsConstants physicsConstants;
    [SerializeField] public SO_DifficultyProfile difficultyProfile;
    [SerializeField] public Camera sceneCamera;
    [SerializeField] public GameplayHud hud;
    [SerializeField] public LoadingShade loadingShade;

    public GameObject player;
    private List<Well> _wells;
    private List<Transform> _enemyTransforms;
    private GravityPlane _gravityPlane;
    public int currentLives;
    public StageInfo info;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip missileFire;
    [SerializeField] private AudioClip missileExplode;
    [SerializeField] private AudioClip shipThruster;
    [SerializeField] private AudioClip missileThruster;

    private void Start()
    {
        if (ProgressionManager.Instance != null)
        {
            physicsConstants = ProgressionManager.Instance.physicsProfile;
            difficultyProfile = ProgressionManager.Instance.difficultyProfile;
        }
        Instance = this;
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        loadingShade.FadeOut(1);
        yield return new WaitForSeconds(0.5f);
        InitializeGame();
        yield return new WaitForSeconds(0.5f);
    }

    private void InitializeGame()
    {
        StageFactory factory = new StageFactory(5, 30, 30, difficultyProfile);
        info = factory.create();
        player = stageSpawner.SpawnStage(info);
        _gravityPlane = info.GravityPlane;
    }

    public Vector2 GravityFromPosition(Vector2 position)
    {
        if (_gravityPlane == null) return Vector2.zero;
        return _gravityPlane.GetGravityAtPos(position);
    }

    public void ShakeCamera(float duration, float intensity)
    {
        player.GetComponent<PlayerCamera>().ShakeCamera(duration, intensity);
    }

    public void PlaySoundEffect(SoundEffects soundEffect)
    {
        switch (soundEffect)
        {
            case SoundEffects.MissileFire:
                audioSource.PlayOneShot(missileFire);
                break;
            case SoundEffects.MissileExplode:
                audioSource.PlayOneShot(missileExplode);
                break;
            case SoundEffects.ShipThruster:
                audioSource.PlayOneShot(shipThruster);
                break;
            case SoundEffects.MissileThruster:
                audioSource.PlayOneShot(missileThruster);
                break;
        }
    }

    public void ZeroEnemiesRemaining()
    {
        
    }
}
