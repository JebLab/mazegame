using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{

    public Camera playerCam;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 20, Color.red);
        Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 4);

        //if(hit.transform != null)
        //    Debug.Log(hit.transform.name);

        if (hit.transform != null && hit.transform.name == "target")
            hit.transform.GetComponent<EndGame>().inSight = true;   

    }
}
