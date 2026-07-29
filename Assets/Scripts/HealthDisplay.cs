using UnityEngine;
using TMPro;

// Shows the player's current health on a UI text field.
public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    void Start(){
        PlayerInventory.Instance.OnHealthChanged += UpdateText;
        UpdateText(PlayerInventory.Instance.Health);
    }
    void OnDestroy(){
        if (PlayerInventory.Instance != null) PlayerInventory.Instance.OnHealthChanged -= UpdateText;
    }
    void UpdateText(int health){
        healthText.text = "Health: " + health;
    }
}
