using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTweens : MonoBehaviour
{
  public delegate void FinishIntro();
  public static event FinishIntro OnFinishIntro;

  public float moveDown= 150f;
  public float scale = 1.2f;

  public float animationScale = 1f;

  void OnEnable()
  {
    Battle.OnBattleStart += OnStart;
  }

  private void OnDisable()
  {
    Battle.OnBattleStart -= OnStart;
  }

  private void OnStart()
  {
    LeanTween
      .moveY(gameObject, moveDown, animationScale).setEase(LeanTweenType.easeInExpo)
      .setOnComplete(Scale);
  }

  void Scale()
  {
    LeanTween.scale(gameObject, new Vector3(scale, scale), animationScale * 1.5f).setOnComplete(Leave);
  }

  void Leave()
  {
    LeanTween
      .rotate(gameObject, new Vector3(0f, 0f, 30f), animationScale * 0.25f)
      .setEase(LeanTweenType.easeInCubic);

    LeanTween
      .moveX(gameObject, -5000f, animationScale * 1.5f)
      .setEase(LeanTweenType.easeInCubic)
      .setOnComplete(OnFinish);
  }

  public void OnFinish()
  {
    OnFinishIntro();
  }
}
