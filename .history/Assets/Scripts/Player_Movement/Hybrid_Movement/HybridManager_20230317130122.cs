using UnityEngine;

public class HybridManager : MonoBehaviour
{
  private NewPlayerInput p_Controls;
  private NewPlayerInput.PlayerActions g_Player;

  private HybridMotor h_Motor;
  private HybridLook h_Look;


  void Awake()
  {
    p_Controls = new NewPlayerInput();
    g_Player = p_Controls.Player;

    h_Motor = GetComponent<HybridMotor>();
    h_Look = GetComponent<HybridLook>();

  }

  void Start()
  {
    h_Look.Init(m_Tran, m_CamTran);
  }
  void FixedUpdate()
  {
    var g_Movement = g_Player.Move;
    h_Motor.MoveInput(g_Movement.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    var g_Camera = g_Player.Look;

  }

  private void OnEnable()
  {
    g_Player.Enable();
  }
  private void OnDisable()
  {
    g_Player.Disable();
  }

}
