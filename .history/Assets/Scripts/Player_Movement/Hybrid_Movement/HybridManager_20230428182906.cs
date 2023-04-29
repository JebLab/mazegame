using UnityEngine;

public class HybridManager : MonoBehaviour
{
  public GameObject uiDoc;
  private NewPlayerInput p_Controls;
  public NewPlayerInput.PlayerActions g_Player;

  private HybridMotor h_Motor;
  private HybridLook h_Look;

  private PauseScreenUICode u_Pause;



  void Awake()
  {
    p_Controls = new NewPlayerInput();
    g_Player = p_Controls.Player;

    h_Motor = GetComponent<HybridMotor>();
    h_Look = GetComponent<HybridLook>();

    // uiDoc =
    // g_Player.Pause.performed += ctx => ;
  }

  void Start()
  {
    if (!h_Look.m_Camera)
      h_Look.m_Camera = Camera.main;
    h_Look.Init(h_Motor.m_Tran, h_Look.m_CamTran);
  }
  void FixedUpdate()
  {
    var g_Movement = g_Player.Move;
    h_Motor.MoveInput(g_Movement.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    var g_Camera = g_Player.Look;
    // h_Look.LookRotation(h_Motor.m_Tran, h_Look.m_CamTran);
    h_Look.CameraInput(g_Camera.ReadValue<Vector2>(), h_Motor.m_Tran, h_Look.m_CamTran);
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
