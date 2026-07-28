using System;
using UnityEngine;

// Locks in the kick direction from the player's first mouse click.
public class BallKickDirection : MonoBehaviour
{
    public static BallKickDirection Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform ball;
    [SerializeField] private Camera aimCamera;

    [Header("Aim Plane")]
    [SerializeField] private Vector3 kickAxis = Vector3.forward; // direction the goal faces
    [SerializeField] private float   aimPlaneDistance = 10f;             // how far ahead the aim plane sits
    [Header("Runtime Info (read-only)")]
    [SerializeField] private bool isAiming;
    [SerializeField] private bool directionConfirmed;
    [SerializeField] private Vector3 originalDirection;

    public bool IsAiming => isAiming;
    public bool DirectionConfirmed => directionConfirmed;
    public Vector3 OriginalDirection => originalDirection;
    public event Action<Vector3> OnDirectionConfirmed;

    void Awake(){
        Instance = this;
        if (aimCamera == null) aimCamera = Camera.main;
        BeginAiming();
    }
    void Update(){
        if (!isAiming || directionConfirmed) return;
        if (Input.GetMouseButtonDown(0))
            TryConfirmDirection();
    }
    // Lets the player start aiming again
    public void BeginAiming(){
        isAiming           = true;
        directionConfirmed = false;
        originalDirection  = Vector3.zero;
    }
    void TryConfirmDirection(){
        if (!TryGetAimDirection(out Vector3 direction)) return;
        originalDirection   = direction;
        directionConfirmed  = true;
        isAiming            = false;
        OnDirectionConfirmed?.Invoke(originalDirection);
    }
    // Turns the current mouse position into a direction from the ball
    bool TryGetAimDirection(out Vector3 direction){
        Vector3 axis     = kickAxis.normalized;
        Plane aimPlane   = new Plane(-axis, ball.position + axis * aimPlaneDistance);
        Ray   mouseRay   = aimCamera.ScreenPointToRay(Input.mousePosition);
        if (!aimPlane.Raycast(mouseRay, out float distance)){
            direction = Vector3.zero;
            return false;
        }
        Vector3 toPoint = mouseRay.GetPoint(distance) - ball.position;
        if (toPoint.sqrMagnitude < 0.0001f){   // clicked right on the ball, ignore
            direction = Vector3.zero;
            return false;
        }
        direction = toPoint.normalized;
        return true;
    }
    // Call after a kick finishes so the next one needs a fresh BeginAiming()
    public void ResetAim(){
        isAiming           = false;
        directionConfirmed = false;
        originalDirection  = Vector3.zero;
    }
}
