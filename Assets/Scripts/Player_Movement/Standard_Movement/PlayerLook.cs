using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;

    private float xRotate = 0f;

    private float xSense = 30f;
    private float ySense = 30f;

    // Start is called before the first frame update
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotate -= (mouseY * Time.deltaTime) * ySense;
        xRotate = Mathf.Clamp(xRotate, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotate, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSense);
    }
}


