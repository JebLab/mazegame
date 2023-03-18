using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void change(string newText)
    {
        text.text = newText;
    }
}
