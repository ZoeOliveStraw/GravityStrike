using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camTarget;
    [SerializeField] private float camLerpSpeed;
    [SerializeField] private float moveVectorOffset;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        camTarget.position = new Vector3(camTarget.position.x, camTarget.position.y, cam.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCamTarget();
        LerpCamTowardsTarget();
    }

    private void CalculateCamTarget()
    {
        Vector2 offsetTarget;
        if (InputManager.AimVector.magnitude > 0.1f) offsetTarget = InputManager.AimVector;
        else offsetTarget = InputManager.MoveVector;
        Vector2 v2Pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPos = v2Pos + offsetTarget * moveVectorOffset;
        camTarget.position = new Vector3(targetPos.x, targetPos.y, camTarget.position.z);
    }

    private void LerpCamTowardsTarget()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, camTarget.position, camLerpSpeed * Time.deltaTime);
    }
}
