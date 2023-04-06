using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
  [Header("Health Parameters")]
  [SerializeField] public float maxHeatlh = 100.0f;
  public float currentHealth;

  [Header("Stamina Parameters")]
  [SerializeField] public float maxStamina = 100.0f;
  [SerializeField] public float staminaCost = 5.0f;
  public float currentStamina;




  private void OnDeath()
  {
    currentHealth = 0;

    Console.WriteLine("YOU DIED");
  }


}
