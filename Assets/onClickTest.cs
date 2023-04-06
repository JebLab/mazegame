using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
// using UnityEngine.Enumerations;

// These two using static statements are for the Relative, Percent & Pixel enums
using static UnityEngine.UIElements.Position;
using static UnityEngine.UIElements.LengthUnit;
using static UnityEngine.TextAnchor;

// Big todo: We have WAY too many Debug.Log statements; clean 'em out
public class onClickTest : MonoBehaviour
{
    // These are the buttons you see when 1st entering the start screen
    // Also todo: rename these to more descriptive names
    private Button _button1;
    private Button _button2;
    private Button _Settings;

    // These are the buttons that are created when settings is clicked
    private Slider _volSlider = null;
    private Button _backButton = null;

    private Label _Bux;

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

        _Bux = uiDocument.rootVisualElement.Q("Boxy") as Label;

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
        if(_backButton != null) {
            _backButton.UnregisterCallback<ClickEvent>(ClickBack);
        }
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
        if(_volSlider == null) {
            _volSlider = new Slider();
            _volSlider.name = "Sfx";
            _volSlider.style.width = new StyleLength(new Length(50, Percent));
            _volSlider.style.position = new StyleEnum<Position>(Relative);
            _volSlider.style.left = new StyleLength(new Length(25, Percent));
            Debug.Log("VolSlider has a low value of " + _volSlider.lowValue + "and a high value of " + _volSlider.highValue);
            // _Bux.Add(_volSlider);
            Debug.Log("The range of the slider is " + _volSlider.range + "; it's low value is " + _volSlider.lowValue + " & high value is " + _volSlider.highValue);
        }
        // Must refactor everything inside this if
        // Did this when tired, it's an abomination of code
        if(_backButton == null) {
            _backButton = new Button();
            _backButton.name = "back";
            Debug.Log(_backButton);
            _backButton.style.height = new StyleLength(40);
            _backButton.style.position = new StyleEnum<Position>(Relative);
            _backButton.style.width = new StyleLength(new Length(50, Percent));
            _backButton.style.left = new StyleLength(new Length(25, Percent));
            _backButton.style.marginTop = new StyleLength(new Length(1, Pixel));
            _backButton.style.top = new StyleLength(new Length(25, Percent));
            Debug.Log("The back button has a position of " + _backButton.style.position.ToString());
            _backButton.text = "Back";
            _backButton.tooltip = "Blahahahahaha";
            Debug.Log("The tooltip is " + _Bux.text);
            _backButton.RegisterCallback<ClickEvent>(ClickBack);
        }
        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(_Bux);
        /* Box frankenstein = new Box(); */
        // Originally intended frankenstein to be a box, but
        // boxes can't have text fields :(
        Label frankenstein = new Label();
        frankenstein.name = "Sfx Container";
        frankenstein.style.height = new StyleLength(40);
        frankenstein.style.position = new StyleEnum<Position>(Relative);
        frankenstein.style.width = new StyleLength(new Length(50, Percent));
        frankenstein.style.left = new StyleLength(new Length(25, Percent));
        frankenstein.style.backgroundColor = new StyleColor(new Color(0.59f, 0.35f, 0.35f, 1.0f));
        frankenstein.tooltip = "This controls the volume of sound effects. Also, you can't click this.";
        frankenstein.style.unityTextAlign = new StyleEnum<TextAnchor>(MiddleLeft);
        frankenstein.text = " Sound Volume";
        frankenstein.Add(_volSlider);
        uiDocument.rootVisualElement.Add(frankenstein);
        Debug.Log("Bux is " + _Bux.ToString());
        uiDocument.rootVisualElement.Add(_Settings);
        uiDocument.rootVisualElement.Add(_backButton);
        // uiDocument.rootVisualElement.Add(_volSlider);
        Debug.Log("You now have " + uiDocument.rootVisualElement.childCount + " children.");
        Debug.Log("Settings is attached to panel " + _Settings.panel.ToString());
        Debug.Log("This is Settings style position: " + _Settings.style.position.ToString());
        buttSound.Play(0);
    }

    private void ClickBack(ClickEvent evt) {
        Debug.Log("You clicked the back button! Congratulations!");
        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(_Bux);
        uiDocument.rootVisualElement.Add(_button1);
        uiDocument.rootVisualElement.Add(_Settings);
        uiDocument.rootVisualElement.Add(_button2);
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
