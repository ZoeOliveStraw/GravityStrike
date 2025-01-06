using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Serializable]
    public struct DiffPhysPairing
    {
        public SO_DifficultyProfile _difficultyProfile;
        public SO_PhysicsConstants _physicsConstants;
    }

    [SerializeField] private TextMeshProUGUI txtDifficulty;
    
    [SerializeField] private List<DiffPhysPairing> gameModes;
    private InputSystem_Actions _controls;

    private int currentGameModeIndex = 0;
    private DiffPhysPairing currentGameMode;
    [SerializeField] private LoadingShade loadingShade;

    private void Awake()
    {
        _controls = new InputSystem_Actions();
    }

    private IEnumerator Start()
    {
        while(ProgressionManager.Instance == null) yield return null;
        _controls.Player.Previous.performed += context => SetGameMode(-1);
        _controls.Player.Next.performed += context => SetGameMode(1);
        _controls.UI.Submit.performed += context => StartGame();
        currentGameModeIndex = 1;
        SetGameModeByIndex();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine(1));
    }

    private IEnumerator StartGameCoroutine(float seconds)
    {
        loadingShade.FadeIn(seconds);
        yield return new WaitForSeconds(seconds);
        ProgressionManager.Instance.stage = 0;
        SceneManager.LoadScene("Gameplay");
    }

    public void SetGameMode(int dir)
    {
        if (dir == -1 && currentGameModeIndex == 0)
        {
            currentGameModeIndex = gameModes.Count - 1;
        }
        else if (dir == -1)
        {
            currentGameModeIndex--;
        }
        else if (dir == 1 && currentGameModeIndex == gameModes.Count - 1)
        {
            currentGameModeIndex = 0;
        }
        else
        {
            currentGameModeIndex++;
        }
        SetGameModeByIndex();
    }

    private void SetGameModeByIndex()
    {
        if (currentGameModeIndex < 0 || currentGameModeIndex >= gameModes.Count) return;
        currentGameMode = gameModes[currentGameModeIndex];
        txtDifficulty.text = gameModes[currentGameModeIndex]._difficultyProfile.difficultyName;
        ProgressionManager.Instance.difficultyProfile = currentGameMode._difficultyProfile;
        ProgressionManager.Instance.physicsProfile = currentGameMode._physicsConstants;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
