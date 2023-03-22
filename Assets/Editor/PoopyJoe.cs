using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class PoopyJoe : EditorWindow
{
    [MenuItem("Window/UI Toolkit/PoopyJoe")]
    public static void ShowExample()
    {
        PoopyJoe wnd = GetWindow<PoopyJoe>();
        wnd.titleContent = new GUIContent("PoopyJoe");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PoopyJoe.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
    }
}