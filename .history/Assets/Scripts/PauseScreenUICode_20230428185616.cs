using UnityEngine;
using UnityEngine.UIElements;

// These two using static statements are for the Relative, Percent & Pixel enums

// Class written by Alex Burrell, Apr. 27
// If you're wondering why it's completely silent, that's bc I didn't add any code to make noises


//  All you should have to do is add a keybind that enables an object
//  W/ a UIDocument component (for PauseScreenUI.uxml
//  & an Assets->Create->UIToolkit->PanelSettingsAsset)
//  as well as a Script component for this file



public class PauseScreenUICode : MonoBehaviour
{
  private UIDocument uiDoc;
  private Button Resume;
  private Slider Volume;
  private Button Quit;

  private void OnEnable()
  {
#if UNITY_EDITOR
    Debug.LogWarning("You've entered OnEnable");
#endif

    uiDoc = GetComponent<UIDocument>();
    Resume = uiDoc.rootVisualElement.Q("resume") as Button;
    Volume = uiDoc.rootVisualElement.Q("volSlider") as Slider;
    Time.timeScale = 0f;
    Quit = uiDoc.rootVisualElement.Q("quit") as Button;
    Resume.RegisterCallback<ClickEvent>(ResumeGame);
    Volume.RegisterCallback<ClickEvent>(ChangeVolume);
    Volume.value = AudioListener.volume;
    Quit.RegisterCallback<ClickEvent>(QuitGame);
  }

  private void OnDisable()
  {
    Resume.UnregisterCallback<ClickEvent>(ResumeGame);
    Volume.UnregisterCallback<ClickEvent>(ChangeVolume);
    Quit.UnregisterCallback<ClickEvent>(QuitGame);
    Time.timeScale = 1f;

  }

  private void ResumeGame(ClickEvent evt)
  {
    //  ToDo by some1 else: This part
    //  Just disable the aforementioned up top object
    //   W/ a UIDocument component & Script component
    this.gameObject.SetActive(false);

  }

  private void ChangeVolume(ClickEvent evt)
  {
    AudioListener.volume = Volume.value;
  }


  private void QuitGame(ClickEvent evt)
  {
#if UNITY_STANDALONE
    Application.Quit();
#endif
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Debug.LogWarning("You've clicked quit.");
  }
}
