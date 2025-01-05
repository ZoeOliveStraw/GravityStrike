using System;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField] public float maxShields;
    [SerializeField] public float maxHealth;
    [SerializeField] private float shieldRegenRate;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentShields;

    private void Start()
    {
        currentHealth = maxHealth;
        currentShields = maxShields;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentShields = Mathf.Clamp(currentShields - damage, 0f, maxShields);
        }
        else
        {
            currentShields = Mathf.Clamp(currentShields - damage, 0f, maxShields);
        }
        
    }
}
