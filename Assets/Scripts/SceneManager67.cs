using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class SceneManager67 : MonoBehaviour
{
    [SerializeField] private AudioSource buttonEffect;
    [SerializeField] Animator transitionAnim;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    IEnumerator SoundDelay(){
        yield return new WaitForSeconds(1.0f);
    }

    public void GameSceneOpen(){
        SceneSound();
        StartCoroutine(SoundDelay());
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base");
    }

    public void RestartGame(){
        SceneSound();
        StartCoroutine(SoundDelay());
        PlayerInventory.Instance?.ResetProgress();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void Instructions(){
        SceneSound();
        StartCoroutine(SoundDelay());
        PlayerInventory.Instance?.ResetProgress();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Instructions");
    }

    public void SceneSound(){
        buttonEffect.PlayOneShot(buttonEffect.clip);
    }
}
