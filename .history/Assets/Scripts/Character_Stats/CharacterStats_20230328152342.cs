using System;
using UnityEngine;

[Serializable]
public class CharacterStats : MonoBehaviour
{
  private HybridMotor Motor;

  [Header("Health Parameters")]

  public float currentHealth;
  [SerializeField] public float maxHeatlh = 100.0f;

  [Header("Health Regen Parameters")]

  [Header("Stamina Parameters")]
  public float currentStamina;
  [SerializeField] public float maxStamina = 100.0f;
  [SerializeField] public float staminaCost = 5.0f;

  public bool exhausted;
  public bool hasRegened;

  [Header("Stamina Regen Parameters")]
  [Range(0, 50)][SerializeField] private float StaminaDrain;
  [Range(0, 50)][SerializeField] private float StaminaRegen;


  private void OnDeath()
  {
    currentHealth = 0;

    Console.WriteLine("YOU DIED");
  }

  private void Start()
  {
    Motor = GetComponent<HybridMotor>();
  }

  private void Update()
  {
    if (!Motor.m_isSprinting)
    {
      if (currentStamina <= maxStamina - 0.01f)
      {
        StaminaRegen = 2;
        currentStamina += StaminaRegen * Time.deltaTime;
        if (currentStamina >= maxStamina)
        {
          currentStamina = maxStamina;
          hasRegened = true;
        }
      }

    }
  }
  public void Sprinting()
  {
    if (hasRegened && Motor.m_isSprinting)
    {
      StaminaDrain = 5;
      currentStamina
    }
  }


}
