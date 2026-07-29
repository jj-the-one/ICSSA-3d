using System.Collections;
using UnityEngine;

// Runs one kick phase at a time: spawns the ball, times out a miss, and respawns for the next attempt as long as the player still has health.
public class KickRoundManager : MonoBehaviour
{
    [Header("Ball")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private BallLaunchController launchController;

    [Header("Timing")]
    [SerializeField] private float timeToScore = 10f;

    private GameObject currentBall;
    private Coroutine timeoutRoutine;

    void Awake(){
        launchController.OnBallLaunched += HandleBallLaunched;
        GoalTrigger.OnBallScored += HandleBallScored;
    }
    void OnDestroy(){
        launchController.OnBallLaunched -= HandleBallLaunched;
        GoalTrigger.OnBallScored -= HandleBallScored;
    }
    void Start() => SpawnBall();

    void SpawnBall(){
        currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        launchController.SetBall(currentBall.GetComponent<Rigidbody>(), currentBall.GetComponent<BallSpin>());
        launchController.ResetForNextKick();
    }
    void HandleBallLaunched(){
        timeoutRoutine = StartCoroutine(TimeoutCountdown());
    }
    IEnumerator TimeoutCountdown(){
        yield return new WaitForSeconds(timeToScore);
        PlayerInventory.Instance.LoseHealth();
        EndPhase();
    }
    void HandleBallScored(){
        if (timeoutRoutine != null) StopCoroutine(timeoutRoutine);
        EndPhase();
    }
    void EndPhase(){
        if (currentBall != null) Destroy(currentBall);
        if (PlayerInventory.Instance.Health > 0) SpawnBall();
    }
}
