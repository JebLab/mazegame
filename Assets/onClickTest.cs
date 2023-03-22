using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class onClickTest : MonoBehaviour
{
    private Button _button1;
    private Button _button2;
    private Button _Settings;

    public Animator transition;

    private AudioSource buttSound;


    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        Debug.Log("<color=red>Youve entered onEnable()\n</color>");
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
   
        _button1 = uiDocument.rootVisualElement.Q("Play") as Button;

        Debug.Log("shawn");

        _button2 = uiDocument.rootVisualElement.Q("Quit") as Button;
        _Settings = uiDocument.rootVisualElement.Q("Set") as Button;

        _button1.RegisterCallback<ClickEvent>(ClickPlay);
        _button2.RegisterCallback<ClickEvent>(ClickQuit);
        _Settings.RegisterCallback<ClickEvent>(ClickSettings);

        buttSound = FindObjectOfType<AudioSource>();

        StartCoroutine(LoadLevel(0));
        Debug.Log("OnEnable was called!");

    }

    private void OnDisable()
    {
        _button1.UnregisterCallback<ClickEvent>(ClickPlay);
        _button2.UnregisterCallback<ClickEvent>(ClickQuit);
        _Settings.UnregisterCallback<ClickEvent>(ClickSettings);
    }

    private void ClickPlay(ClickEvent evt)
    {

        Debug.Log($"{"Play"} was clicked!");
        buttSound.Play(0);
        SceneManager.LoadScene(sceneName:"TransToScene");
    }

    private void ClickSettings(ClickEvent evt)
    {
        Debug.Log($"{"Settings"} was clicked.");
        buttSound.Play(0);
    }

    private void ClickQuit(ClickEvent evt)
    {
        Debug.Log($"{"Quit"} was clicked");
        buttSound.Play(0);
        Quit();
    }

    // Stolen straight from Stack Overflow, works in the Editor
    // Haven't tried it in a Build yet
    public void Quit() {
    #if UNITY_STANDALONE
        Application.Quit();
    #endif
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    IEnumerator LoadLevel(int levelIndex) {
        // transition.SetTrigger("StartScene");
        
        yield return new WaitForSeconds(1);
    }
}
