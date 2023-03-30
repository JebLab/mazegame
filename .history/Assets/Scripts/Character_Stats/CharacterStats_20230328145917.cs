using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
  [Header("Health Parameters")]
  [SerializeField] public float maxHeatlh = 100;
  public float currentHealth;

  [Header("Stamina Parameters")]
  [SerializeField] public bool useStamina = false;
  [SerializeField] public float maxStamina = 100;
  public float currentStamina;




  private void OnDeath()
  {
    currentHealth = 0;

    Console.WriteLine("YOU DIED");
  }


}
