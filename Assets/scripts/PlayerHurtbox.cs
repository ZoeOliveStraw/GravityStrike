using System;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField] public float maxShields;
    [SerializeField] public float maxHealth;
    [SerializeField] public int maxLives;
    [SerializeField] private float shieldRegenRate;
    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private float shieldRechargeDelay;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentShields;
    [HideInInspector] public int currentLives;

    private float currentShieldRechargeDelay = 0;
    [SerializeField] private float respawnInvulnerabilityTime;
    private float currentInvulnerabilityTime = 0;

    private void Start()
    {
        currentHealth = maxHealth;
        currentShields = maxShields;
        maxLives = GameManager.Instance.difficultyProfile.maxLives;
        if(ProgressionManager.Instance == null) currentLives = maxLives;
        else if(ProgressionManager.Instance.livesRemaining != 0) currentLives = ProgressionManager.Instance.livesRemaining;
        
        GameManager.Instance.hud.setLives(currentLives);
    }

    private void Update()
    {
        if (currentShields < maxShields)
        {
            if (currentShieldRechargeDelay <= 0)
            {
                currentShields += (maxShields / shieldRegenRate) * Time.deltaTime;
                GameManager.Instance.hud.RenderHealthShields();   
            }
            else
            {
                currentShieldRechargeDelay = Mathf.Clamp(currentShieldRechargeDelay - Time.deltaTime, 0, shieldRechargeDelay);
            }
        }

        if (currentInvulnerabilityTime >= 0)
        {
            currentInvulnerabilityTime = Mathf.Clamp(currentInvulnerabilityTime - Time.deltaTime, 0, respawnInvulnerabilityTime);
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentInvulnerabilityTime > 0) return;
        GameManager.Instance.ShakeCamera(0.2f,1);
        currentShieldRechargeDelay = shieldRechargeDelay;
        if (currentShields > 0)
        {
            currentShields = Mathf.Clamp(currentShields - damage, 0f, maxShields);
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
        }
        GameManager.Instance.hud.RenderHealthShields();
        if (currentHealth <= 0) {
            death();
        }
    }

    public void death() 
    {
        // explosion visual, audio, and shake
        GameManager.Instance.PlaySoundEffect(GameManager.SoundEffects.MissileExplode);
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        GameManager.Instance.ShakeCamera(0.1f, 0.05f);

        if (currentLives > 0) 
        {
            GameManager.Instance.player.transform.position = new Vector3((float) GameManager.Instance.info.LevelWidth / 2, (float) GameManager.Instance.info.LevelHeight / 2, 0);
            GameManager.Instance.player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GameManager.Instance.currentLives--; 
            currentLives--;
            GameManager.Instance.hud.setLives(currentLives);

            currentHealth = maxHealth;
            currentShields = maxShields;
            GameManager.Instance.hud.RenderHealthShields();
            currentInvulnerabilityTime = respawnInvulnerabilityTime;
        } 
        else 
        {
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Sun")) death();
    }
}
