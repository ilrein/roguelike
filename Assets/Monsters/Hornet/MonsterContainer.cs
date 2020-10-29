using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class MonsterContainer : MonoBehaviour
{
  public Monster monster;
  public UnityArmatureComponent character;
  public HealthBar healthBar;
  public int currentHealth;

  public enum intention
  {
    ATTACK,
    DEFEND,
    ATTACK_AND_DEFEND,
    DEBUFF,
    BUFF,
    DEBUFF_AND_BUFF,
  }

  private void Start()
  {
    character = GetComponent<UnityArmatureComponent>();

    healthBar.SetMaxHealth(monster.totalHealth);
    healthBar.SetHealth(monster.totalHealth);
    currentHealth = monster.totalHealth;

    Monster.OnReceiveDmg += TakeDamage;
  }

  private void OnDisable()
  {
    Monster.OnReceiveDmg -= TakeDamage;
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    healthBar.SetHealth(currentHealth);
    character.animation.Play("Damage");
  }

  private void Update()
  {
    // play idle when nothing else is happening
    if (character.animation.isCompleted)
    {
      character.animation.Play("Idle");
    }
  }

  public void OnMouseOver()
  {
    if (Battle.currentlySelectedCard == null) return;
    var mesh = gameObject.GetComponentInChildren<MeshRenderer>();
    monster.OnHoverEnter(mesh);
  }

  public void OnMouseExit()
  {
    var mesh = gameObject.GetComponentInChildren<MeshRenderer>();
    monster.OnHoverExit(mesh);
  }

  public void OnMouseDown()
  {
    monster.onClickMonster();
  }
}
