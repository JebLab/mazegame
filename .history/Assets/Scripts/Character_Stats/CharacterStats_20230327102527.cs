using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
  [Header("Health Parameters")]
  [SerializeField] public float maxHeatlh = 100;
  public float currentHealth;
  public static Action<float> OnDamage;
  public static Action<float> OnTakeDamage;
  public static Action<float> OnHeal;

  [Header("Stamina Parameters")]
  [SerializeField] public bool useStamina = false;
  [SerializeField] public float maxStamina = 100;
  [SerializeField] public float timeBeforeStaminaRegen = 5;
  [SerializeField] public float staminaTimeIncr = 0.2f;



  public float currentStamina;




  private void OnEnable()
  {
    OnTakeDamage += ApplyDamage;
  }
  private void OnDisable()
  {
    OnTakeDamage -= ApplyDamage;
  }
  private void ApplyDamage(float dmg)
  {
    currentHealth -= dmg;
    OnDamage?.Invoke(currentHealth);
    if (currentHealth <= 0)
    {
      OnDeath();
    }
  }

  private void OnDeath()
  {
    currentHealth = 0;

    Console.WriteLine("YOU DIED");
  }


}
