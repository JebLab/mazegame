using UnityEngine;

private class HybridLook : MonoBehaviour
{
  [Header("Aiming")]
  [SerializeField] private Camera m_Camera;

  private Transform m_CamTran;

  void Start()
  {
    if (!m_Camera)
      m_Camera = Camera.main;

    m_CamTran = m_Camera.transform;
  }
}
