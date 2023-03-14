using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
