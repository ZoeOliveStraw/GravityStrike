using System;
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

    private void Awake()
    {
        _controls = new InputSystem_Actions();
    }

    private void Start()
    {
        _controls.Player.Previous.performed += context => SetGameMode(-1);
        _controls.Player.Next.performed += context => SetGameMode(1);
    }

    public void StartGame()
    {
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
