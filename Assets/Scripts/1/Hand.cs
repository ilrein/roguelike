using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
  public Battle battle;
  public List<Card> currentCardsInHand;

  public GameObject baseCard;

  public Transform discardPilePosition;

  public delegate void FinishDiscardAnimation(int index, Card card);
  public static event FinishDiscardAnimation OnFinishDiscardAnimation;

  void OnEnable()
  {
    Battle.OnDrawInitialHand += OnDrawInitialHand;
    Battle.OnDiscardCard += OnDiscardCard;
  }

  private void OnDisable()
  {
    Battle.OnDiscardCard -= OnDiscardCard;
  }

  void OnDrawInitialHand(int amount)
  {
    StartCoroutine("DrawCard", amount);
  }

  IEnumerator DrawCard(int amount)
  {
    Battle.frozen = true;

    for (int i = 0; i < amount; i++)
    {
      Card card = battle.drawPile[i];
      currentCardsInHand.Add(card);

      GameObject tmpCard = Instantiate(baseCard, transform);
      CardContainer container = tmpCard.GetComponent<CardContainer>();
      tmpCard.SetActive(true);
      tmpCard.name = $"{currentCardsInHand.Count}";
      
      LeanTween
        .moveX(tmpCard, baseCard.transform.position.x + (80f * i), 0.45f)
        .setEaseInCubic();
      
      container.card = card;
      battle.DrawCard();
      yield return new WaitForSeconds(0.55f);
    }

    Battle.frozen = false;
  }

  public void OnDiscardCard(int order, Card card)
  {
    StartCoroutine(DiscardCard(order, card));
  }

  IEnumerator DiscardCard(int order, Card card)
  {
    int spot;
    if (card.targetable)
    {
      spot = order;
    } else
    {
      spot = transform.childCount - 1;
    }

    var cardToDiscard = transform.GetChild(spot).gameObject;

    LeanTween
      .moveX(cardToDiscard, discardPilePosition.position.x, 0.45f)
      .setEaseInCubic()
      .setOnComplete(() =>
      {
        LeanTween
          .scale(cardToDiscard, new Vector3(0.1f, 0.1f), 0.15f)
          .setEaseInCubic()
          .setOnComplete(() => {
            Destroy(cardToDiscard);
            OnFinishDiscardAnimation(order, card);
            StartCoroutine(Reorganize(order));
          });
      });

    yield return null;
  }

  IEnumerator Reorganize(int index)
  {
    for (int i = 1; i < currentCardsInHand.Count; i++)
    {
      if (i >= index) // all the cards to the right of the card just discarded
      {
        var tmpCard = transform.GetChild(i + 1).gameObject;
        tmpCard.name = $"{i}";

        LeanTween
          .moveX(tmpCard, baseCard.transform.position.x + (80f * (i - 1)), 0.45f)
          .setEaseInCubic();
      }
    }

    currentCardsInHand.RemoveAt(index - 1);
    yield return null;
  }
}
