using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
  [Header("Health Parameters")]

  public float currentHealth;
  [SerializeField] public float maxHeatlh = 100.0f;

  [Header("Health Regen Parameters")]

  [Header("Stamina Parameters")]
  public float currentStamina;
  [SerializeField] public float maxStamina = 100.0f;
  [SerializeField] public float staminaCost = 5.0f;

  [Header("Stamina Regen Parameters")]
  [SerializeField] private float StaminaDrain;
  [SerializeField] private float StaminaRegen;






  private void OnDeath()
  {
    currentHealth = 0;

    Console.WriteLine("YOU DIED");
  }


}
