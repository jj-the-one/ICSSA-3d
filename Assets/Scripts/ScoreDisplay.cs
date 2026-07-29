using UnityEngine;
using TMPro;

// Shows the player's current goal count on a UI text field.
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Start(){
        PlayerInventory.Instance.OnGoalsChanged += UpdateText;
        UpdateText(PlayerInventory.Instance.Goals);
    }
    void OnDestroy(){
        if (PlayerInventory.Instance != null) PlayerInventory.Instance.OnGoalsChanged -= UpdateText;
    }
    void UpdateText(int goals){
        scoreText.text = "Score: " + goals;
    }
}
