using UnityEngine;

// Curves the ball's flight based on spin (like a banana kick).
[RequireComponent(typeof(Rigidbody))]
public class BallSpin : MonoBehaviour
{
    [Header("Curve")]
    [SerializeField] private float magnusCoefficient = 0.05f; // curve force strength (keep small!)
    [SerializeField] private float initialSpinSpeed = 6f; // spin speed at full offset + power
    [SerializeField] private float spinDamping = 0.6f; // how fast the spin fades out
    [SerializeField] private float minSpeedToCurve = 0.5f; // minimum speed needed to curve

    private Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    // offset = where the ball was struck (-1..1 per axis), power = kick strength (0..1)
    public void ApplyKickSpin(Vector2 offset, Vector3 kickDirection, float power){
        Vector3 forward = kickDirection.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;
        Vector3 spinAxis = (right * offset.y) - (up * offset.x); // curves away from the strike point
        rb.angularVelocity = spinAxis * initialSpinSpeed * power;
    }
    void FixedUpdate(){
        if (rb.velocity.sqrMagnitude < minSpeedToCurve * minSpeedToCurve) return;
        rb.AddForce(magnusCoefficient * Vector3.Cross(rb.angularVelocity, rb.velocity));
        rb.angularVelocity *= Mathf.Exp(-spinDamping * Time.fixedDeltaTime); // spin fades over time
    }
}
