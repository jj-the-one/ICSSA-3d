using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Persists across every scene and tracks the player's health and goals scored.
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] private int startingHealth = 5;

    public int Health { get; private set; }
    public int Goals { get; private set; }

    public event Action<int> OnHealthChanged;
    public event Action<int> OnGoalsChanged;
    public event Action OnHealthDepleted;

    void Awake(){
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Health = startingHealth;
    }
    public void ResetProgress(){
        Health = startingHealth;
        Goals = 0;
        OnHealthChanged?.Invoke(Health);
        OnGoalsChanged?.Invoke(Goals);
    }
    public void AddGoal(){
        Goals++;
        OnGoalsChanged?.Invoke(Goals);
    }
    public void LoseHealth(){
        Health = Mathf.Max(0, Health - 1);
        OnHealthChanged?.Invoke(Health);
        if (Health == 0){
            OnHealthDepleted?.Invoke();
            SceneManager.LoadScene("Results");
        }
    }
}
