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

  public void Init(Transform character, Transform camera)
  {
    m_CharacterTargetRot = character.localRotation;
    m_CameraTargetRot = camera.localRotation;
  }

}
