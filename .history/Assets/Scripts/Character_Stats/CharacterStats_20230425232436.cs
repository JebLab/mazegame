using System;
using UnityEngine;

[Serializable]
public class CharacterStats : MonoBehaviour
{
  private HybridMotor Motor;

  [Header("Health Parameters")]

  private float currentHealth;
  [SerializeField] private float maxHeatlh = 100.0f;

  [Header("Health Regen Parameters")]

  [Header("Stamina Parameters")]
  private float currentStamina;
  [SerializeField] private float maxStamina = 100.0f;
  [SerializeField] private float staminaCost = 5.0f;

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
    if (Motor.m_isSprinting == false)
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
      if (!hasRegened)
      {
        StaminaRegen = 0.5f;
        currentStamina += StaminaRegen * Time.deltaTime;
      }

    }
  }

}
