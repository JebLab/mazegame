using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private Button _button1;
    private Button _button2;
    private Button _Settings;


    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
   
        _button1 = uiDocument.rootVisualElement.Q("Play") as Button;
        _button2 = uiDocument.rootVisualElement.Q("Quit") as Button;
        _Settings = uiDocument.rootVisualElement.Q("Set") as Button;

        _button1.RegisterCallback<ClickEvent>(PrintClickMessage);
        _button2.RegisterCallback<ClickEvent>(PrintClickMessage);
        _Settings.RegisterCallback<ClickEvent>(PrintClickMessage);

        var _inputFields = uiDocument.rootVisualElement.Q("input-message");
        _inputFields.RegisterCallback<ChangeEvent<string>>(InputMessage);
        Debug.Log("OnEnable was called!");

    }

    private void OnDisable()
    {
        _button1.UnregisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {

        Debug.Log($"{"Play"} was clicked!");
        Debug.Log($"{"Quit"} was clicked!");
        Debug.Log($"{"Set"} was clicked!");
    }

    public static void InputMessage(ChangeEvent<string> evt)
    {
        Debug.Log($"{evt.newValue} -> {evt.target}");
    }

/*
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("This is a test");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("This is another test");
    } */
}
