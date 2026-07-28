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

    public bool HasLaunched { get; private set; }

    void Awake(){
        if (kickDirection == null) kickDirection = BallKickDirection.Instance;
        kickButton.onClick.AddListener(OnKickButtonClicked);
    }
    void Update(){
        kickButton.interactable = !HasLaunched && kickDirection.DirectionConfirmed;
    }
    void OnKickButtonClicked(){
        if (HasLaunched || !kickDirection.DirectionConfirmed) return;
        float power = Mathf.InverseLerp(powerSlider.minValue, powerSlider.maxValue, powerSlider.value);
        ballRigidbody.velocity = kickDirection.OriginalDirection * (power * maxLaunchSpeed);
        if (ballSpin != null && BallSpinSelector.Instance != null)
            ballSpin.ApplyKickSpin(BallSpinSelector.Instance.SpinOffset, kickDirection.OriginalDirection, power);
        HasLaunched = true;
    }
    // Resets everything so the player can aim and kick again
    public void ResetForNextKick(){
        HasLaunched = false;
        powerSlider.value = powerSlider.minValue;
        kickDirection.BeginAiming();
        BallSpinSelector.Instance?.ResetOffset();
    }
}
