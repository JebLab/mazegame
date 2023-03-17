using System;
using UnityEngine;

public class Q3PlayerLook : MonoBehaviour
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
}
