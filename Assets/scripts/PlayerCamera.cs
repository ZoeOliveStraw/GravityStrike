using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] public Camera cam;
    [SerializeField] private Transform camTarget;
    [SerializeField] private float camLerpSpeed;
    [SerializeField] private float moveVectorOffset;
    [SerializeField] private Rigidbody2D rb;

    private float remainingCameraShakeTime;
    private float currentCameraShakeIntensity;
    private bool cameraShaking;

    private void Start()
    {
        GetCameraReference();
        camTarget.position = new Vector3(camTarget.position.x, camTarget.position.y, transform.position.z - 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(cam == null) GetCameraReference();
        CalculateCamTarget();
        LerpCamTowardsTarget();
        UpdateCamShake();
    }

    private void GetCameraReference()
    {
        cam = GameManager.Instance.sceneCamera;
    }

    private void CalculateCamTarget()
    {
        Vector2 offsetTarget;
        if (InputManager.AimVector.magnitude > 0.1f) offsetTarget = InputManager.AimVector;
        else offsetTarget = InputManager.MoveVector;
        Vector2 v2Pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPos = v2Pos + offsetTarget * moveVectorOffset;
        camTarget.position = new Vector3(targetPos.x, targetPos.y, transform.position.z - 10);
    }

    private void LerpCamTowardsTarget()
    {
        if (!cameraShaking)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, camTarget.position, camLerpSpeed * Time.deltaTime);
        }
        else
        {
            float offsetX = Random.Range(-1f, 1f) * currentCameraShakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * currentCameraShakeIntensity;
            Vector2 offset = new Vector2(offsetX, offsetY);
            cam.transform.position = Vector3.Lerp(cam.transform.position + (Vector3)offset, camTarget.position, camLerpSpeed * Time.deltaTime);
        }
    }

    public void ShakeCamera(float duration, float intensity)
    {
        remainingCameraShakeTime = duration;
        currentCameraShakeIntensity = intensity;
    }

    private void UpdateCamShake()
    {
        if(remainingCameraShakeTime > 0) remainingCameraShakeTime = 
            Mathf.Clamp(remainingCameraShakeTime -= Time.deltaTime, 0, 100);
    }
}
