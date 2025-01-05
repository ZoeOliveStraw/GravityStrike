using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShoot : MonoBehaviour
{
    //Shot information
    [SerializeField] public Vector2 minMaxShotForce;
    [SerializeField] private GameObject missilePrefab;
    
    //Aiming reticle informmation
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float aimReticleLength;
    
    private Vector2 _aimVector;
    public float currentShotForce = 0;

    void Update()
    {
        _aimVector = InputManager.AimVector;
        if (InputManager.Controls.Player.Attack.IsPressed())
        {
            currentShotForce = Mathf.Clamp(currentShotForce += minMaxShotForce.y * Time.deltaTime, 0, minMaxShotForce.y);
        }
        else
        {
            currentShotForce =  Mathf.Clamp(currentShotForce -= minMaxShotForce.y * Time.deltaTime, 0, minMaxShotForce.y);
        }
        if(InputManager.Controls.Player.Attack.WasReleasedThisFrame()) FireReleased();
        RenderAimReticle();
    }

    private void RenderAimReticle()
    {
        if (_aimVector.magnitude <= 0.1f)
        {
            lineRenderer.enabled = false;
            return;
        }
        
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (Vector2) transform.position + _aimVector * aimReticleLength);
    }

    private void FireReleased()
    {
        if (currentShotForce > minMaxShotForce.x)
        {
            float shotForce = currentShotForce;
            Vector2 launchPosition = (Vector2) transform.position + _aimVector.normalized;
            Missile missile = Instantiate(missilePrefab, launchPosition, Quaternion.identity).GetComponent<Missile>();
            missile.FireZeMissile(shotForce * _aimVector);
            GameManager.Instance.PlaySoundEffect(GameManager.SoundEffects.MissileFire);
        }
    }
}
