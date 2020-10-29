using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class CardContainer : MonoBehaviour,
  IPointerEnterHandler,
  IPointerExitHandler
{
  public Card card;
  public float moveY = 100f;

  public new TextMeshProUGUI name;
  public TextMeshProUGUI description;
  public TextMeshProUGUI cost;
  public new AudioSource audio;
  private AudioClip clip;

  private float animationTime = 0.35f;

  public Image artContainer;
  public GameObject cardOutline;

  public float fadeInDuration = 0.35f;

  private float initialY;
  private float initialRotation;
  // public int index;

  private Image outline;

  private RectTransform rectTransform;

  public bool isTargetting;

  public delegate void PlayCard(Card card, int index);
  public static event PlayCard OnPlayCard;

  public delegate void SelectTargetableCard(Card card, int index);
  public static event SelectTargetableCard OnSelectTargetableCard;

  private void OnEnable()
  {
    rectTransform = GetComponent<RectTransform>();
    outline = cardOutline.GetComponent<Image>();

    LeanTween.scale(gameObject, new Vector3(0f, 0f), 0.05f);
    initialY = transform.position.y;
  }

  private void Start()
  {
    Battle.OnCancelSelection += OnCancelSelection;

    LeanTween.scale(gameObject, new Vector3(1f, 1f), 0.35f);

    artContainer.sprite = card.artwork;
    name.text = card.name;
    description.text = card.description;
    cost.text = $"{card.manaCost}";

    AttachSound();
  }

  public void OnDisable()
  {
    Battle.OnCancelSelection -= OnCancelSelection;
  }

  // this function is binded to on click
  public void SelectCard()
  {
    if (Battle.frozen || card.manaCost > Battle.currentMana) return;

    if (card.targetable)
    {
      isTargetting = true;
      outline.color = Color.yellow;
      OnSelectTargetableCard(card, Int32.Parse(gameObject.name));
    }
    else
    {
      OnPlayCard(card, Int32.Parse(gameObject.name));
    }
  }

  public void OnCancelSelection()
  {
    outline.color = Color.white;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (Battle.frozen || transform.localScale.x > 1) return;
    ScaleUp();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    // Debug.Log("OnPointerExit");
    ScaleDown();
  }

  void ScaleUp()
  {
    LeanTween
      .moveY(gameObject, initialY + moveY, animationTime)
      .setEase(LeanTweenType.easeInExpo);
    LeanTween
      .scale(gameObject, new Vector3(1.7f, 1.7f, 1.7f), animationTime)
      .setEase(LeanTweenType.easeInExpo);

    transform.SetSiblingIndex(transform.parent.childCount);
  }

  void ScaleDown()
  {
    LeanTween
      .moveY(gameObject, initialY, animationTime)
      .setEase(LeanTweenType.easeInExpo);
    LeanTween
      .scale(gameObject, new Vector3(1f, 1f, 1f), animationTime)
      .setEase(LeanTweenType.easeInExpo);

    transform.SetSiblingIndex(Int32.Parse(gameObject.name));
  }

  void AttachSound()
  {
    clip = Resources.Load<AudioClip>($"Sounds/{card.audioClipName}");
  }
}
