using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Battle : MonoBehaviour
{
  public GameObject hero;
  public Deck deck;
  public List<Card> drawPile;
  public List<Card> discardPile;

  public List<Monster> monsters;
  public Monster currentlyTargettedMonster;
  
  public static Card currentlySelectedCard;
  public int currentlySelectedCardIndex;

  public TextMeshProUGUI cardsInDrawPile;
  public TextMeshProUGUI cardsInDiscardPile;

  public AudioSource source;

  public delegate void BattleStart();
  public static event BattleStart OnBattleStart;

  public delegate void DrawInitialHand(int InitialDrawCount);
  public static event DrawInitialHand OnDrawInitialHand;

  public delegate void CancelSelection();
  public static event CancelSelection OnCancelSelection;

  public delegate void HeroAttack(int val, Card card);
  public static event HeroAttack OnHeroAttack;

  public delegate void DiscardCard(int index, Card card);
  public static event DiscardCard OnDiscardCard;

  // START, DRAW_INITIAL_HAND, PLAY_CARD, END_TURN
  // each MONSTER_ACTION
  // NEW_TURN
  public int currentTurn = 1;
  public enum state {
    PLAYERS_TURN,
    ENEMIES_TURN,
  }

  private int InitialDrawCount = 5;
  private int drawCount = 2;

  public static bool frozen = true;

  public static int currentMana;
  public TextMeshProUGUI manaText;

  private void Start()
  {
    // intro animation
    BattleTweens.OnFinishIntro += OnFinito;

    // trigger battle
    OnBattleStart();

    // shuffle the deck and update the draw pile
    drawPile = deck.Shuffle();
    cardsInDrawPile.text = $"{deck.cards.Count}";

    // subscribe to selecting a targetable card
    CardContainer.OnSelectTargetableCard += OnSelectTargetableCard;
    Monster.OnTargetMonster += OnTargetMonster;

    // subscribe to playing a card immediately (ie aoe or block)
    CardContainer.OnPlayCard += OnPlayCardImmediately;

    // when hand is finished discarding a card
    Hand.OnFinishDiscardAnimation += SendToDiscardPile;

    // set the mana
    currentMana = HeroContainer.mana;
    manaText.text = $"{currentMana}/{HeroContainer.mana}";
  }

  private void OnDisable()
  {
    BattleTweens.OnFinishIntro -= OnFinito;
    CardContainer.OnSelectTargetableCard -= OnSelectTargetableCard;
    Monster.OnTargetMonster -= OnTargetMonster;
    Hand.OnFinishDiscardAnimation -= SendToDiscardPile;
    CardContainer.OnPlayCard -= OnPlayCardImmediately;
  }

  public void DrawCard()
  {
    drawPile.RemoveAt(0);
    cardsInDrawPile.text = $"{drawPile.Count}";
  }

  public void SendToDiscardPile(int order, Card card)
  {
    discardPile.Add(card);
    cardsInDiscardPile.text = $"{discardPile.Count}";
  }

  private void OnFinito()
  {
    OnDrawInitialHand?.Invoke(InitialDrawCount: InitialDrawCount);
  }

  private void OnSelectTargetableCard(Card card, int index)
  {
    frozen = true;
    currentlySelectedCard = card;
    currentlySelectedCardIndex = index;
  }

  private void OnTargetMonster(Monster monster)
  {
    currentlyTargettedMonster = monster;

    if (currentlySelectedCard.type == Card.EffectType.DEAL_DAMAGE)
    {
      StartCoroutine(Attack());
    }
  }

  IEnumerator Attack()
  {
    for (int i = 0; i < 3; i++)
    {
      
      if (i == 0)
      {
        OnHeroAttack(1, currentlySelectedCard);
        source.clip = Resources.Load<AudioClip>($"Sounds/{currentlySelectedCard.audioClipName}");
      }

      if (i == 1)
      {
        source.Play();
        currentlyTargettedMonster.OnTakeDamage(currentlySelectedCard.value);
      }

      if (i == 2)
      {
        OnDiscardCard(currentlySelectedCardIndex, currentlySelectedCard);
        UpdateMana(currentlySelectedCard.manaCost);
        frozen = false;
      }

      yield return new WaitForSeconds(0.55f);
    }
  }

  private void UpdateMana(int amount)
  {
    currentMana -= amount;
    manaText.text = $"{currentMana}/{HeroContainer.mana}";
  }

  private void OnPlayCardImmediately(Card card, int index)
  {
    frozen = true;

    if (card.type == Card.EffectType.BLOCK)
    {
      // do the block effect
      StartCoroutine(Block(card, index));
    }
  }

  IEnumerator Block(Card card, int index)
  {
    for (int i = 0; i < 2; i++)
    {

      if (i == 0)
      {
        source.clip = Resources.Load<AudioClip>($"Sounds/{card.audioClipName}");
      }

      if (i == 1)
      {
        source.Play();
        OnDiscardCard(index, card);
        UpdateMana(card.manaCost);
      }
    }

    frozen = false;

    yield return new WaitForSeconds(0.15f);
  }

  public void EndTurn()
  {
    Debug.Log("end turn");
  }

  private void Update()
  {
    if (Input.GetKeyDown("escape"))
    {
      currentlySelectedCard = null;
      frozen = false;
      OnCancelSelection();
    }

    if (Input.GetKeyDown("return"))
    {
      if (frozen) return;
      EndTurn();
    }
  }
}
