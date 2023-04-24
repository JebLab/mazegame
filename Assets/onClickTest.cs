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
    // Called playSound, but it plays when both Start and Quit are clicked
    private AudioSource playSound;

    private Animator animaniacs;

    private UIDocument uiDocument;


    public void Blargh() {
        UIElemFact fart = new UIElemFact();
        fart.createButton();
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        #if UNITY_EDITOR
        Debug.Log("You've entered onEnable()\n");
        #endif
        // The UXML is already instantiated by the UIDocument component
        uiDocument = GetComponent<UIDocument>();
   
        _button1 = uiDocument.rootVisualElement.Q("Play") as Button;
        _button2 = uiDocument.rootVisualElement.Q("Quit") as Button;
        _Settings = uiDocument.rootVisualElement.Q("Set") as Button;
        _Bux = uiDocument.rootVisualElement.Q("Boxy") as Label;

        _button1.RegisterCallback<ClickEvent>(ClickPlay);
        _button2.RegisterCallback<ClickEvent>(ClickQuit);
        _Settings.RegisterCallback<ClickEvent>(ClickSettings);

        buttSound = GameObject.Find("ButtonSfx").GetComponent<AudioSource>();
        playSound = GameObject.Find("PlaySfx").GetComponent<AudioSource>();
        animaniacs = FindObjectOfType<Animator>();
        
        // As far as I'm aware, StartCoroutine is the only way to make a method sleep for a specified time
        StartCoroutine(LoadLevel(0));
        
        #if UNITY_EDITOR
        Debug.Log("Exiting onEnable!");
        #endif
    }

    private void OnDisable()
    {
        _button1.UnregisterCallback<ClickEvent>(ClickPlay);
        _button2.UnregisterCallback<ClickEvent>(ClickQuit);
        _Settings.UnregisterCallback<ClickEvent>(ClickSettings);
        _volSlider.UnregisterCallback<ClickEvent>(changeVolume);
        if(_backButton != null) {
            _backButton.UnregisterCallback<ClickEvent>(ClickBack);
        }
    }

    private void ClickPlay(ClickEvent evt)
    {
        #if UNITY_EDITOR
        Debug.Log("Play was clicked!");
        #endif
        playSound.Play(0);
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
        #if UNITY_EDITOR
        Debug.Log("Settings was clicked.");
        #endif
        if(_volSlider == null) {
            _volSlider = new Slider();
            _volSlider.name = "Sfx";
            _volSlider.style.width = new StyleLength(new Length(50, Percent));
            _volSlider.style.position = new StyleEnum<Position>(Relative);
            _volSlider.style.left = new StyleLength(new Length(25, Percent));
            _volSlider.highValue = 1.0f;
            #if UNITY_EDITOR
            Debug.Log("The range of the slider is " + _volSlider.range + "; it's low value is " + _volSlider.lowValue + " & high value is " + _volSlider.highValue);
            #endif
            _volSlider.RegisterCallback<ClickEvent>(changeVolume);
            _volSlider.value = AudioListener.volume;
            Debug.Log("AudioListener has a volume of " + AudioListener.volume);
            Blargh();
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
            #if UNITY_EDITOR
            Debug.Log("The back button has a position of " + _backButton.style.position.ToString());
            #endif
            _backButton.text = "Back";
            _backButton.tooltip = "Blahahahahaha";
            _backButton.RegisterCallback<ClickEvent>(ClickBack);
        }
        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(_Bux);

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
        #if UNITY_EDITOR
        Debug.Log("Bux is " + _Bux.ToString());
        if(_Settings == null) Debug.Log("I'm null!");
        else Debug.Log("I'm not null.");
        #endif
        uiDocument.rootVisualElement.Add(_Settings);
        uiDocument.rootVisualElement.Add(_backButton);
        #if UNITY_EDITOR
        Debug.Log("You now have " + uiDocument.rootVisualElement.childCount + " children.");
        #endif
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
        Debug.Log("Quit was clicked");
        playSound.Play(0);
        animaniacs.SetBool("IsClickedStart", true);
        
        Destroy(uiDocument);

        StartCoroutine(quitFunc(1));
    }

   IEnumerator quitFunc(int myParam) {
        yield return new WaitForSeconds(myParam);
        Quit();
    }

    void changeVolume(ClickEvent evt) {
        AudioListener.volume = _volSlider.value;
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
        yield return new WaitForSeconds(1);
    }
}
