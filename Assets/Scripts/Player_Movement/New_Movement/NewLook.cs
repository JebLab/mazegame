using System;
using UnityEngine;

[Serializable]
public class NewLook : MonoBehaviour {

  [Header("Aiming")]
  [SerializeField]
  public Camera m_Camera;
  [SerializeField]
  private float m_XSensitivity = 2f;
  [SerializeField]
  private float m_YSensitivity = 2f;
  [SerializeField]
  private bool m_ClampVerticalRotation = true;
  [SerializeField]
  private float m_MinimumX = -90F;
  [SerializeField]
  private float m_MaximumX = 90F;
  [SerializeField]
  private bool m_Smooth = false;
  [SerializeField]
  private float m_SmoothTime = 5f;
  [SerializeField]
  private bool m_LockCursor = true;

  private Quaternion m_CharacterTargetRot;
  private Quaternion m_CameraTargetRot;
  private bool m_cursorIsLocked = true;
  public Transform m_CamTran;

  private CharacterController c_Chara = NewMotor.m_Character;

  void Start() {
    m_CamTran = gameObject.transform.GetChild(0);

    if (!m_Camera) {
      m_Camera = Camera.main;
    }

    m_CamTran = m_Camera.transform;
  }
  public void Init(Transform character, Transform camera) {
    m_CharacterTargetRot = character.transform.localRotation;
    m_CameraTargetRot = camera.transform.localRotation;
  }
  public void CameraInput(Vector2 c_Input, Transform character,
                          Transform camera) {
    float yRot = c_Input.x * m_XSensitivity;
    float xRot = c_Input.y * m_YSensitivity;
    m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
    m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

    if (m_ClampVerticalRotation) {
      m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
    }

    if (m_Smooth) {
      character.localRotation =
          Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                           m_SmoothTime * Time.deltaTime);
      camera.localRotation =
          Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                           m_SmoothTime * Time.deltaTime);
    } else {
      character.localRotation = m_CharacterTargetRot;
      camera.localRotation = m_CameraTargetRot;
    }
    UpdateCursorLock();
  }
  public void SetCursorLock(bool value) {
    m_LockCursor = value;
    if (!m_LockCursor) { // we force unlock the cursor if the user disable the
                         // cursor locking helper
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

  public void UpdateCursorLock() {
    // if the user set "lockCursor" we check & properly lock the cursos
    if (m_LockCursor) {
      InternalLockUpdate();
    }
  }

  private void InternalLockUpdate() {
    if (Input.GetKeyUp(KeyCode.Escape)) {
      m_cursorIsLocked = false;
    } else if (Input.GetMouseButtonUp(0)) {
      m_cursorIsLocked = true;
    }

    if (m_cursorIsLocked) {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    } else if (!m_cursorIsLocked) {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

  private Quaternion ClampRotationAroundXAxis(Quaternion q) {
    q.x /= q.w;
    q.y /= q.w;
    q.z /= q.w;
    q.w = 1.0f;

    float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

    angleX = Mathf.Clamp(angleX, m_MinimumX, m_MaximumX);

    q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

    return q;
  }
}
