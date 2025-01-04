using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHud : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    private GameObject playerPrefab;
    private PlayerShoot playerShoot;

    private float maxShotForce;
    
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
    }

    private void InitializeShotSlider()
    {
        playerShoot = playerPrefab.GetComponent<PlayerShoot>();
        maxShotForce = playerShoot.minMaxShotForce.y;
        powerSlider.maxValue = maxShotForce;
    }

    
    void Update()
    {
        if (playerShoot != null)
        {
            powerSlider.value = playerShoot.currentShotForce;
        }
    }
}
