using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class Hero : ScriptableObject
{
  public UnityArmatureComponent character;
  public new string name;
  public string description;
  public int hp;

  public void Print()
  {
    Debug.Log(name + ": " + description);
  }
}
