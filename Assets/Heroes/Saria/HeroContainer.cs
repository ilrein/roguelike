using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
using TMPro;

public class HeroContainer : MonoBehaviour
{
  public Hero hero;
  public UnityArmatureComponent character;

  public int maxHealth;
  public int currentHealth;
  
  public static int mana = 3;
  public static int block = 0;

  public HealthBar healthBar;
  private void Start()
  {
    Battle.OnHeroAttack += Attack;

    UnityArmatureComponent character = GetComponent<UnityArmatureComponent>();

    maxHealth = hero.hp;
    currentHealth = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
    healthBar.SetHealth(maxHealth);
  }

  private void OnDisable()
  {
    Battle.OnHeroAttack -= Attack;
  }

  private void Update()
  {
    // the sprite should always face the right
    transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);

    // play idle when nothing else is happening
    if (character.animation.isCompleted)
    {
      character.animation.Play("Idle");
    }
  }

  void TakeDamage(int damage)
  {
    currentHealth -= damage;
    healthBar.SetHealth(currentHealth);
  }

  public void Attack(int val, Card card)
  {
    character.animation.Play("Attack A");
  }
}
