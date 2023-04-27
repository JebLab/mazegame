using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// These two using static statements are for the Relative, Percent & Pixel enums
using static UnityEngine.UIElements.Position;
using static UnityEngine.UIElements.LengthUnit;
using static UnityEngine.TextAnchor;

public class StartScreenUICode : MonoBehaviour
{

    // This is our factory, to make elements when we need them.
    UIElementFactory ElFacto;

    // This is how we interact with the UI the player sees
    private UIDocument uiDocument;

    // These are the buttons you see when 1st entering the start screen
    private Button StartButton;
    private Button SettingsButton;
    private Button QuitButton;

    // These are the buttons that are created when settings is clicked
    private Slider VolSlider;
    private Button BackButton;

    private Label _Bux;
    
    public Animator transition;

    private AudioSource buttSound;
    // Called playSound, but it plays when both Start and Quit are clicked
    private AudioSource playSound;

    private Animator animator;


    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        #if UNITY_EDITOR
        Debug.Log("You've entered onEnable()\n");
        #endif
        ElFacto = new UIElementFactory();

        // The UXML is already instantiated by the UIDocument component
        uiDocument = GetComponent<UIDocument>();

        StartButton = uiDocument.rootVisualElement.Q("Play") as Button;
        SettingsButton = uiDocument.rootVisualElement.Q("Set") as Button;
        QuitButton = uiDocument.rootVisualElement.Q("Quit") as Button;
        _Bux = uiDocument.rootVisualElement.Q("Boxy") as Label;
        VolSlider = null;
        BackButton = null;

        StartButton.RegisterCallback<ClickEvent>(ClickPlay);
        QuitButton.RegisterCallback<ClickEvent>(ClickQuit);
        SettingsButton.RegisterCallback<ClickEvent>(ClickSettings);

        buttSound = GameObject.Find("ButtonSfx").GetComponent<AudioSource>();
        playSound = GameObject.Find("PlaySfx").GetComponent<AudioSource>();
        animator = FindObjectOfType<Animator>();
        
        // As far as I'm aware, StartCoroutine is the only way to make a method sleep for a specified time in Unity
        StartCoroutine(LoadLevel(0));
        
        #if UNITY_EDITOR
        Debug.Log("Exiting onEnable!");
        #endif
    }

    private void OnDisable()
    {
        StartButton.UnregisterCallback<ClickEvent>(ClickPlay);
        SettingsButton.UnregisterCallback<ClickEvent>(ClickSettings);
        QuitButton.UnregisterCallback<ClickEvent>(ClickQuit);
        VolSlider.UnregisterCallback<ClickEvent>(changeVolume);
        if(BackButton != null)
            BackButton.UnregisterCallback<ClickEvent>(ClickBack);
    }

    private void ClickPlay(ClickEvent evt)
    {
        #if UNITY_EDITOR
        Debug.Log("Play was clicked!");
        #endif
        playSound.Play(0);

        // Setting the animator bool to true starts the transition animation
        animator.SetBool("IsClickedStart", true);
        Destroy(uiDocument);
        StartCoroutine(sceneTrans());
    }
    
    // We start sceneTrans as a coroutine so that we can transition but only after a second has passed
    IEnumerator sceneTrans() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName:"TransToScene");
    }

    private void ClickSettings(ClickEvent evt)
    {
        #if UNITY_EDITOR
        Debug.Log("Settings was clicked.");
        #endif
        if(VolSlider == null) {
            VolSlider = ElFacto.createNormalizedSlider("Sfx");
            #if UNITY_EDITOR
            Debug.Log("The range of the slider is " + VolSlider.range + "; it's low value is " + VolSlider.lowValue + " & high value is " + VolSlider.highValue);
            #endif
            VolSlider.RegisterCallback<ClickEvent>(changeVolume);
            VolSlider.value = AudioListener.volume;
        }
        if(BackButton == null) {
            BackButton = ElFacto.createStandardButton("back");
            #if UNITY_EDITOR
            Debug.Log("The back button has a position of " + BackButton.style.position.ToString());
            #endif
            BackButton.text = "Back";
            BackButton.style.marginTop = new StyleLength(new Length(1, Pixel));
            BackButton.style.top = new StyleLength(new Length(25, Percent));
            BackButton.RegisterCallback<ClickEvent>(ClickBack);
            BackButton.tooltip = "Click this to go back to Start & Quit";
        }

        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(_Bux);

        // Originally intended VolSliderContainer to be a box, but
        // boxes can't have text fields :(
        Label VolSliderContainer = ElFacto.createStandardLabel("Sfx Container");
        
        VolSliderContainer.style.height = new StyleLength(40);
        VolSliderContainer.style.backgroundColor = new StyleColor(new Color(0.59f, 0.35f, 0.35f, 1.0f));
        VolSliderContainer.text = " Sound Volume";
        VolSliderContainer.Add(VolSlider);

        uiDocument.rootVisualElement.Add(VolSliderContainer);
        

        #if UNITY_EDITOR
        Debug.Log("Bux is " + _Bux.ToString());
        if(SettingsButton == null) Debug.Log("I'm null!");
        else Debug.Log("I'm not null.");
        #endif
        uiDocument.rootVisualElement.Add(SettingsButton);
        uiDocument.rootVisualElement.Add(BackButton);
        #if UNITY_EDITOR
        Debug.Log("You now have " + uiDocument.rootVisualElement.childCount + " children.");
        #endif
        buttSound.Play(0);
    }

    private void ClickBack(ClickEvent evt) {
        #if UNITY_EDITOR
        Debug.Log("You clicked the back button! Congratulations!");
        #endif
        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(_Bux);
        uiDocument.rootVisualElement.Add(StartButton);
        uiDocument.rootVisualElement.Add(SettingsButton);
        uiDocument.rootVisualElement.Add(QuitButton);
        buttSound.Play(0);
    }

    private void ClickQuit(ClickEvent evt)
    {
        Debug.Log("Quit was clicked");
        playSound.Play(0);
        animator.SetBool("IsClickedStart", true);
        
        Destroy(uiDocument);

        StartCoroutine(quitFunc(1));
    }

    IEnumerator quitFunc(int myParam) {
        yield return new WaitForSeconds(myParam);
        Quit();
    }

    void changeVolume(ClickEvent evt) {
        #if UNITY_EDITOR
        Debug.Log("The volume is now " + VolSlider.value);
        #endif
        AudioListener.volume = VolSlider.value;
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
