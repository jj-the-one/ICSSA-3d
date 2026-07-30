using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class SceneManager67 : MonoBehaviour
{
    [SerializeField] private AudioSource buttonEffect;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void GameSceneOpen(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base");
    }

    public void RestartGame(){
        PlayerInventory.Instance?.ResetProgress();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void Instructions(){
        PlayerInventory.Instance?.ResetProgress();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Instructions");
    }

    private void SceneSound(){
        buttonEffect.PlayOneShot(buttonEffect.clip);
    }
}
