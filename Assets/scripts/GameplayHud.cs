using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHud : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Slider healthSlider;
    private GameObject playerPrefab;
    private PlayerShoot playerShoot;
    private PlayerHurtbox playerHurtbox;

    private float maxShotForce;
    private float maxHealth;
    private float maxShield;
    
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
        // healthSlider.maxValue = playerHurtbox.maxHealth;
    }
    
    private void InitializeShieldSlider()
    {
        shieldSlider.maxValue = playerHurtbox.maxShields;
    }

    
    void Update()
    {
        if (playerShoot != null)
        {
            powerSlider.value = playerShoot.currentShotForce;
        }

        if (playerHurtbox != null)
        {
            // healthSlider.value = playerHurtbox.currentHealth;
            // shieldSlider.value = playerHurtbox.currentShields;
        }
    }
}
