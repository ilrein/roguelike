using System;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
  public Sprite artwork;

  // public string rarity; = "common";

  public new string name;
  public string description;

  public int manaCost;

  public bool targetable;

  public string audioClipName;

  public enum EffectType
  {
    DEAL_DAMAGE,
    BLOCK,
  }

  public EffectType type;

  public int value;
}
