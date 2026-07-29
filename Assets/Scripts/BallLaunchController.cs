using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Launches the ball using the power slider and the confirmed kick direction/spin.
public class BallLaunchController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BallKickDirection kickDirection;
    [SerializeField] private BallSpin ballSpin;
    [SerializeField] private Rigidbody ballRigidbody;
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Button kickButton;

    [Header("Launch")]
    [SerializeField] private float maxLaunchSpeed = 20f;
    [SerializeField] private float kickDelay = 4f; // windup time before the ball actually launches, for animation

    public bool HasLaunched { get; private set; }
    public event Action OnKickStarted;   // fires immediately on button press, for animation
    public event Action OnBallLaunched;  // fires once the ball actually moves

    void Awake(){
        if (kickDirection == null) kickDirection = BallKickDirection.Instance;
        kickButton.onClick.AddListener(OnKickButtonClicked);
    }
    void Update(){
        kickButton.interactable = !HasLaunched && kickDirection.DirectionConfirmed;
    }
    void OnKickButtonClicked(){
        if (HasLaunched || !kickDirection.DirectionConfirmed) return;
        HasLaunched = true;

        // lock in the kick's values now, before the windup animation plays
        float power = Mathf.InverseLerp(powerSlider.minValue, powerSlider.maxValue, powerSlider.value);
        Vector3 direction = kickDirection.OriginalDirection;
        Vector2 spinOffset = BallSpinSelector.Instance != null ? BallSpinSelector.Instance.SpinOffset : Vector2.zero;

        OnKickStarted?.Invoke();
        StartCoroutine(LaunchAfterDelay(direction, power, spinOffset));
    }
    IEnumerator LaunchAfterDelay(Vector3 direction, float power, Vector2 spinOffset){
        yield return new WaitForSeconds(kickDelay);
        ballRigidbody.velocity = direction * (power * maxLaunchSpeed);
        if (ballSpin != null) ballSpin.ApplyKickSpin(spinOffset, direction, power);
        OnBallLaunched?.Invoke();
    }
    // Points this controller at a freshly spawned ball
    public void SetBall(Rigidbody newBallRigidbody, BallSpin newBallSpin){
        ballRigidbody = newBallRigidbody;
        ballSpin = newBallSpin;
        kickDirection = BallKickDirection.Instance;
    }
    // Resets everything so the player can aim and kick again
    public void ResetForNextKick(){
        HasLaunched = false;
        powerSlider.value = powerSlider.minValue;
        kickDirection.BeginAiming();
        BallSpinSelector.Instance?.ResetOffset();
    }
}
