using System;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField] public float maxShields;
    [SerializeField] public float maxHealth;
    [SerializeField] public int maxLives;
    [SerializeField] private float shieldRegenRate;
    [SerializeField] private GameObject explosionParticles;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentShields;
    [HideInInspector] public int currentLives;

    private void Start()
    {
        currentHealth = maxHealth;
        currentShields = maxShields;
        maxLives = GameManager.Instance.difficultyProfile.maxLives;
        currentLives = maxLives;
    }

    public void TakeDamage(float damage)
    {
        if (currentShields > 0)
        {
            currentShields = Mathf.Clamp(currentShields - damage, 0f, maxShields);
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
        }

        if (currentHealth <= 0) {
            death();
        }
        
    }

    public void death() {
        // explosion visual, audio, and shake
        GameManager.Instance.PlaySoundEffect(GameManager.SoundEffects.MissileExplode);
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        GameManager.Instance.ShakeCamera(0.2f, 0.5f);

        if (currentLives > 0) {
            GameManager.Instance.player.transform.position = new Vector3((float) GameManager.Instance.info.LevelWidth / 2, (float) GameManager.Instance.info.LevelHeight / 2, 0);
            GameManager.Instance.player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GameManager.Instance.currentLives--; 
            currentLives--;
            GameManager.Instance.hud.setLives(currentLives);

            currentHealth = maxHealth;
            currentShields = maxShields;
        } else 
        {
            // else trigger game over
        }

    }
}
