using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class onClickTest : MonoBehaviour
{
    // These are the buttons you see when 1st entering the start screen
    private Button _button1;
    private Button _button2;
    private Button _Settings;

    // These are the buttons that are created when settings is clicked
    private Button _volSlider = null;
    private Button _backButton = null;

    public Animator transition;

    private AudioSource buttSound;

    private Animator animaniacs;

    private UIDocument uiDocument;


    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        Debug.Log("<color=red>Youve entered onEnable()\n</color>");
        // The UXML is already instantiated by the UIDocument component
        uiDocument = GetComponent<UIDocument>();
   
        _button1 = uiDocument.rootVisualElement.Q("Play") as Button;

        Debug.Log("shawn");

        _button2 = uiDocument.rootVisualElement.Q("Quit") as Button;
        _Settings = uiDocument.rootVisualElement.Q("Set") as Button;

        _button1.RegisterCallback<ClickEvent>(ClickPlay);
        _button2.RegisterCallback<ClickEvent>(ClickQuit);
        _Settings.RegisterCallback<ClickEvent>(ClickSettings);

        // buttSound = FindObjectOfType<AudioSource>();
        buttSound = GameObject.Find("ButtonSfx").GetComponent<AudioSource>();
        // Debug.Log(GameObject.Find("ButtonSfx").ToString());

        animaniacs = FindObjectOfType<Animator>();

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
        animaniacs.SetBool("IsClickedStart", true);
        Destroy(uiDocument);
        StartCoroutine(sceneTrans());
    }

    IEnumerator sceneTrans() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName:"TransToScene");
    }

    private void ClickSettings(ClickEvent evt)
    {
        Debug.Log($"{"Settings"} was clicked.");
        /* if(_volSlider == null) {
            _volSlider = new Button();
        }
        if(_backButton == null) {
            _backButton = new Button();
            _backButton.name = "back";
            Debug.Log(_backButton);
            _backButton.style.height = new StyleLength(40);
            _backButton.style.width = new StyleLength(450);
            //Align frankie = Auto;
            // _backButton.style.alignSelf = new StyleEnum<Align>(frankie);
            
        } */
        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(_Settings);
        // uiDocument.rootVisualElement.Add(_backButton);
        Debug.Log("You now have " + uiDocument.rootVisualElement.childCount + " children.");
        Debug.Log("Settings is attached to panel " + _Settings.panel.ToString());
        Debug.Log("Settings has a height of " + _Settings.style.height + " and a width of " + _Settings.style.width);
        buttSound.Play(0);
    }

    private void ClickQuit(ClickEvent evt)
    {
        Debug.Log($"{"Quit"} was clicked");
        buttSound.Play(0);
        animaniacs.SetBool("IsClickedStart", true);
        

        // The below multiline commented-out code can be ignored
        // I was trying to figure out how to hide the UI elements
        // Then I realized I can destroy the UIDocument object, which is way easier

        /* var MooseKnuckle = new StyleColor(new Color(0.3f, 0.4f, 0.6f, 0.0f));
        uiDocument.rootVisualElement.Q("Play").style.backgroundColor = MooseKnuckle;
        Debug.Log(MooseKnuckle); */

        Destroy(uiDocument);

        StartCoroutine(quitFunc(1));
    }

   IEnumerator quitFunc(int myParam) {
        yield return new WaitForSeconds(myParam);
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
