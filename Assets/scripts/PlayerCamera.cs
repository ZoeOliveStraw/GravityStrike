using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] public Camera cam;
    [SerializeField] private Transform camTarget;
    [SerializeField] private float camLerpSpeed;
    [SerializeField] private float moveVectorOffset;
    [SerializeField] private Rigidbody2D rb;

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
        cam.transform.position = Vector3.Lerp(cam.transform.position, camTarget.position, camLerpSpeed * Time.deltaTime);
    }
}
