using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
  private CharacterController controller;
  private Vector3 pVelocity;

  private bool isGrounded;
  private bool isSprinting;
  private bool isCrouching;

  public float walkSpeed = 1.42f;
  public float sprintSpeed = 3.583f;
  public float crouchSpeed = 0.7f;
  public float gravity = -9.81f;
  public float jumpY = 0.7112f;
  public float stamina = 100f;

  // Start is called before the first frame update
  void Start()
  {
    controller = GetComponent<CharacterController>();
  }

  // Update is called once per frame
  void Update()
  {
    isGrounded = controller.isGrounded;
    if (isSprinting == true)
    {
      stamina -= 2f * Time.deltaTime;
      if (stamina == 0)
      {
        sprintSpeed -= .2f * Time.deltaTime;
        if (sprintSpeed <= walkSpeed)
        { SprintReleased(); }
      }
    }
  }
  public void ProcessMove(Vector2 input)
  {
    Vector3 moveDirection = Vector3.zero;
    moveDirection.x = input.x;
    moveDirection.z = input.y;


    if (isSprinting == true)
    {
      controller.Move(transform.TransformDirection(moveDirection) * sprintSpeed * Time.deltaTime);
    }
    else if (isCrouching == true) { controller.Move(transform.TransformDirection(moveDirection) * crouchSpeed * Time.deltaTime); }
    else
      controller.Move(transform.TransformDirection(moveDirection) * walkSpeed * Time.deltaTime);
    pVelocity.y += gravity * Time.deltaTime;
    if (isGrounded && pVelocity.y < 0)
      pVelocity.y = -9.81f;
    controller.Move(pVelocity * Time.deltaTime);
  }

  public void Jump()
  {
    if (isGrounded)
    {
      isCrouching = false;
      pVelocity.y = Mathf.Sqrt(jumpY * -0.4191f * gravity);
    }
  }
  public void SprintPressed()
  {
    isSprinting = true;
    isCrouching = false;
    controller.height = 2f;
  }
  public void SprintReleased()
  {
    isSprinting = false;
    sprintSpeed = 3.583f;

  }

  public void CrouchPressed()
  {
    isCrouching = true;
    isSprinting = false;
    controller.height = 0.2f;
  }
  public void CrouchReleased()
  {
    isCrouching = false;
    controller.height = 2f;
  }
}
