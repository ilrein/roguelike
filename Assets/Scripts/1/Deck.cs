using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
  public List<Card> cards;

  public List<Card> Shuffle()
  {
    return cards.OrderBy(i => Guid.NewGuid()).ToList();
  }
}
