using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster")]
public class Monster : ScriptableObject
{
  [Header("Info")]
  public new string name;
  public string description;

  [Header("Stats")]
  public int totalHealth;
  public int strength;

  public delegate void TargetMonster(Monster monster);
  public static event TargetMonster OnTargetMonster;

  public delegate void ReceiveDmg(int amount);
  public static event ReceiveDmg OnReceiveDmg;

  public void onClickMonster()
  {
    // Debug.Log($"clicked monster: {name}");
    OnTargetMonster(this);
  }

  public void OnHoverEnter(MeshRenderer mesh)
  {
    mesh.material.color = Color.red;
  }

  public void OnHoverExit(MeshRenderer mesh)
  {
    mesh.material.color = Color.white;
  }

  public void OnTakeDamage(int amount)
  {
    OnReceiveDmg(amount);
  }
}
