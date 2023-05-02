using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public bool inSight { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (inSight)
            lookedAt();
        else
        {
            tmp.text = string.Empty;
        }

    }

    public void lookedAt()
    {
        tmp.text = "Press 'E' to escape!";
        inSight = false;
    }
}
