using System;
using UnityEngine;

[Serializable]
public class HybridLook : MonoBehaviour
{
  [Header("Aiming")]
  [SerializeField] private Camera m_Camera;
  [SerializeField] private float m_XSensitivity = 2f;
  [SerializeField] private float m_YSensitivity = 2f;
  [SerializeField] private bool m_ClampVerticalRotation = true;
  [SerializeField] private float m_MinimumX = -90F;
  [SerializeField] private float m_MaximumX = 90F;
  [SerializeField] private bool m_Smooth = false;
  [SerializeField] private float m_SmoothTime = 5f;
  [SerializeField] private bool m_LockCursor = true;

  private Quaternion m_CharacterTargetRot;
  private Quaternion m_CameraTargetRot;
  private bool m_cursorIsLocked = true;
  public Transform m_CamTran;

  void Start()
  {
    if (!m_Camera)
    { m_Camera = Camera.main; }

    m_CamTran = m_Camera.transform;
  }
  public void Init(Transform character, Transform camera)
  {
    m_CharacterTargetRot = character.localRotation;
    m_CameraTargetRot = camera.localRotation;
  }
  public void CameraInput(Vector2 c_Input)
  {
    float yRot = c_Input.x * m_XSensitivity;
    float xRot = c_Input.y * m_YSensitivity;
    m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
    m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

    if (m_ClampVerticalRotation)
    {
      m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
    }

    if (m_Smooth)
    {
      character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
          m_SmoothTime * Time.deltaTime);
      camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
          m_SmoothTime * Time.deltaTime);
    }
    else
    {
      character.localRotation = m_CharacterTargetRot;
      camera.localRotation = m_CameraTargetRot;
    }

    UpdateCursorLock();
  }
}
}
