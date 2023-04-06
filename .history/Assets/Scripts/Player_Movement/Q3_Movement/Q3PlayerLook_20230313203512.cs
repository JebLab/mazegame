using System;
using UnityEngine;

[Serializable]
public class Q3PlayerLook : MonoBehaviour
{
  [SerializeField] private float m_XSensitivity = 2f;
  [SerializeField] private float m_YSensitivity = 2f;
  [SerializeField] private bool m_ClampVerticalRotation = true;
  [SerializeField] private float m_MinimumX = -90F;
  [SerializeField] private float m_MaximumX = 90F;
  [SerializeField] private bool m_Smooth = false;
  [SerializeField] private float m_SmoothTime = 5f;
  [SerializeField] private bool m_LockCursor = true;

  [Header("Aiming")]
  [SerializeField] private Camera m_Camera;

  private Quaternion m_CharacterTargetRot;
  private Quaternion m_CameraTargetRot;
  private bool m_cursorIsLocked = true;

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




