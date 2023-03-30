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
  [SerializeField] bool useStamina = false;
  [SerializeField] public float maxStamina = 100;
  [SerializeField] private float timeBeforeStaminaRegen = 5;
  [SerializeField]


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
