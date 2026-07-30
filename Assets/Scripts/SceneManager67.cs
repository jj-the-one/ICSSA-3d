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
        SceneSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base");
    }

    public void RestartGame(){
        SceneSound();
        PlayerInventory.Instance?.ResetProgress();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void Instructions(){
        SceneSound();
        PlayerInventory.Instance?.ResetProgress();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Instructions");
    }

    public void SceneSound(){
        buttonEffect.PlayOneShot(buttonEffect.clip);
    }
}
