using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayHud : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Slider healthSlider;
    [SerializeField] public TextMeshProUGUI txtEnemyCount;
    [SerializeField] public TextMeshProUGUI txtLivesCount;
    private GameObject playerPrefab;
    private PlayerShoot playerShoot;
    private PlayerHurtbox playerHurtbox;

    private float maxShotForce;
    private float maxHealth;
    private float maxShield;
    private int currentEnemyCount;
    
    private IEnumerator Start()
    {
        while (GameManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        
        while (GameManager.Instance.player == null)
        {
            yield return new WaitForEndOfFrame();
        }
        setLives(GameManager.Instance.difficultyProfile.maxLives);
        playerPrefab = GameManager.Instance.player;
        InitializeShotSlider();
        InitializeHealthSlider();
        InitializeShieldSlider();
    }

    private void InitializeShotSlider()
    {
        playerShoot = playerPrefab.GetComponent<PlayerShoot>();
        maxShotForce = playerShoot.minMaxShotForce.y;
        powerSlider.maxValue = maxShotForce;
    }
    
    private void InitializeHealthSlider()
    {
        playerHurtbox = playerPrefab.GetComponent<PlayerHurtbox>();
        healthSlider.maxValue = playerHurtbox.maxHealth;
        healthSlider.value = playerHurtbox.currentHealth;
    }
    
    private void InitializeShieldSlider()
    {
        shieldSlider.maxValue = playerHurtbox.maxShields;
        shieldSlider.value = playerHurtbox.currentShields;
    }

    
    void Update()
    {
        if (playerShoot != null)
        {
            powerSlider.value = playerShoot.currentShotForce;
        }
    }

    public void RenderEnemyCount(int num)
    {
        currentEnemyCount = num;
        txtEnemyCount.text = $"Enemies: {currentEnemyCount}";
    }

    public void RenderHealthShields()
    {
        healthSlider.value = playerHurtbox.currentHealth;
        shieldSlider.value = playerHurtbox.currentShields;
    }

    public void DecrementEnemyCount()
    {
        currentEnemyCount--;
        RenderEnemyCount(currentEnemyCount);
        if(currentEnemyCount <= 0) GameManager.Instance.ZeroEnemiesRemaining();
    }
    
    public void setLives(int lives) 
    {
        txtLivesCount.text = $"EXTRA LIVES: {lives}";
    }
}
