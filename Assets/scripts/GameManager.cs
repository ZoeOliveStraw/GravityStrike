using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public enum SoundEffects
    {
        MissileFire,
        MissileExplode,
        ShipThruster,
        Laser,
        Pause
    }
    
    public static GameManager Instance;
    
    [SerializeField] private StageSpawner  stageSpawner;
    [SerializeField] public SO_PhysicsConstants physicsConstants;
    [SerializeField] public SO_DifficultyProfile difficultyProfile;
    [SerializeField] public Camera sceneCamera;
    [SerializeField] public GameplayHud hud;
    [SerializeField] public LoadingShade loadingShade;
    [SerializeField] public LoadingShade gameOverScreen;
    [SerializeField] public GameObject pauseScreen;

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
    [SerializeField] private AudioClip laserShoot;
    [SerializeField] private AudioClip pauseClip;

    private void Start()
    {
        if (ProgressionManager.Instance != null)
        {
            physicsConstants = ProgressionManager.Instance.physicsProfile;
            difficultyProfile = ProgressionManager.Instance.difficultyProfile;
        }
        InputManager.Controls.Player.Pause.performed += ctx => TogglePause();
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
        int stage;
        if(ProgressionManager.Instance != null) stage = ProgressionManager.Instance.stage;
        else stage = 0;
        StageFactory factory = new StageFactory(5, 50, 50, difficultyProfile, stage);
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
            case SoundEffects.Laser:
                audioSource.PlayOneShot(laserShoot);
                break;
            case SoundEffects.Pause:
                audioSource.PlayOneShot(pauseClip);
                break;
        }
    }

    public void GameOver()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        gameOverScreen.SetText($"GAME OVER\nSTAGE {ProgressionManager.Instance.stage}");
        gameOverScreen.FadeIn(1);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("MainMenu");
    }

    public void ZeroEnemiesRemaining()
    {
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator NextLevelCoroutine()
    {
        player.gameObject.SetActive(false);
        ProgressionManager.Instance.stage++;
        ProgressionManager.Instance.livesRemaining = player.GetComponent<PlayerHurtbox>().currentLives;
        loadingShade.SetText($"NEXT STAGE {ProgressionManager.Instance.stage + 1}");
        loadingShade.FadeIn(1);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Gameplay");
    }

    private void TogglePause()
    {
        bool pause = pauseScreen.activeSelf;
        Time.timeScale = pause ? 1 : 0;
        pauseScreen.SetActive(!pause);
    }
}
