using UnityEngine;

public class NewManager : MonoBehaviour
{
  public GameObject uiDoc;
  private PlayerInput p_Controls;
  public PlayerInput.PlayerActions g_Player;

  private NewMotor h_Motor;
  private NewLook h_Look;

  private PauseScreenUICode u_Pause;

  private bool on = false;

  void Awake()
  {
    p_Controls = new PlayerInput();
    g_Player = p_Controls.Player;

    h_Motor = GetComponent<NewMotor>();
    h_Look = GetComponent<NewLook>();

    // uiDoc =
    g_Player.Pause.performed += ctx =>
    {
      on = !on;
      uiDoc.SetActive(on);
    };
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
