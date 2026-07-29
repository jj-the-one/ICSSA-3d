using UnityEngine;

// Goalkeeper AI: slides side to side to track and block the ball once it's within range.
[RequireComponent(typeof(Rigidbody))]
public class GKBehavior : MonoBehaviour
{
    [Header("Tracking")]
    [SerializeField] private Vector3 kickAxis = Vector3.forward; // direction from the goal toward the kicker
    [SerializeField] private float moveRange = 3f; // how far each side of the start position it can slide

    [Header("Difficulty")]
    [SerializeField] private float baseMoveSpeed = 6f;
    [SerializeField] private float speedPerGoal = 0.5f; // added to move speed for each goal scored
    [SerializeField] private float baseTrackingRange = 8f;
    [SerializeField] private float rangePerGoal = 0.5f; // added to tracking range for each goal scored

    private Rigidbody rb;
    private Vector3 homePosition;
    private Vector3 rightAxis;
    private Transform ball;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        homePosition = transform.position;
        rightAxis = Vector3.Cross(Vector3.up, kickAxis.normalized).normalized;
    }
    void FixedUpdate(){
        if (!FindBall()) return;
        if (Vector3.Distance(ball.position, homePosition) > CurrentTrackingRange()) return;

        float offset = Mathf.Clamp(Vector3.Dot(ball.position - homePosition, rightAxis), -moveRange, moveRange);
        Vector3 targetPosition = homePosition + rightAxis * offset;
        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, CurrentMoveSpeed() * Time.fixedDeltaTime));
    }
    float CurrentMoveSpeed(){
        return baseMoveSpeed + GoalsScored() * speedPerGoal;
    }
    float CurrentTrackingRange(){
        return baseTrackingRange + GoalsScored() * rangePerGoal;
    }
    int GoalsScored(){
        return PlayerInventory.Instance != null ? PlayerInventory.Instance.Goals : 0;
    }
    bool FindBall(){
        if (ball != null) return true;
        GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
        if (ballObj == null) return false;
        ball = ballObj.transform;
        return true;
    }
}
