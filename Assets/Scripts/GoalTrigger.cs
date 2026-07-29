using System;
using System.Collections;
using UnityEngine;

// Fires when the ball enters the goal: scores a point, destroys the ball, and scatters a burst of explosion sprites that disperse and disappear.
public class GoalTrigger : MonoBehaviour
{
    public static event Action OnBallScored;

    [Header("Explosion")]
    [SerializeField] private GameObject[] explosionPrefabs;
    [SerializeField] private int pieceCount = 8;
    [SerializeField] private float pieceSpeed = 3f;
    [SerializeField] private float pieceLifetime = 1.5f;

    void OnTriggerEnter(Collider other){
        if (!other.CompareTag("Ball")) return;
        PlayerInventory.Instance.AddGoal();
        SpawnExplosion(other.transform.position);
        Destroy(other.gameObject);
        OnBallScored?.Invoke();
    }
    void SpawnExplosion(Vector3 position){
        if (explosionPrefabs == null || explosionPrefabs.Length == 0) return;
        for (int i = 0; i < pieceCount; i++){
            GameObject prefab = explosionPrefabs[UnityEngine.Random.Range(0, explosionPrefabs.Length)];
            GameObject piece = Instantiate(prefab, position, Quaternion.identity);
            StartCoroutine(Disperse(piece, UnityEngine.Random.onUnitSphere));
        }
    }
    IEnumerator Disperse(GameObject piece, Vector3 direction){
        float elapsed = 0f;
        while (elapsed < pieceLifetime){
            piece.transform.position += direction * pieceSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(piece);
    }
}
