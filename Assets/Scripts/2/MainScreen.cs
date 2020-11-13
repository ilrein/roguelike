using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
  public GameObject text1;
  public GameObject text2;
  public GameObject character;
  public GameObject play;
  public GameObject exit;

  public AudioClip whoosh;
  public AudioClip track;

  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(ScrollInMainText());
  }

  IEnumerator ScrollInMainText()
  {
    var x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
    var y = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).y;

    AudioSource audio = GetComponent<AudioSource>();
    audio.clip = whoosh;

    yield return new WaitForSeconds(0.7f);
    audio.Play();

    LeanTween
      .moveY(text1, (float)(Screen.height - (Screen.height * 0.2)), 0.65f)
      .setEase(LeanTweenType.easeOutQuad)
      .setOnComplete(() =>
      {
        audio.clip = track;
        audio.Play();
        audio.loop = true;

        LeanTween
         .moveY(text2, (float)(Screen.height - (Screen.height * 0.21)), 1.25f)
         .setEase(LeanTweenType.easeInOutBounce)
         .setOnComplete(() =>
         {
           LeanTween
            .move(
               character,
               new Vector3((float)(x * 0.9), (float)(y * 1.2)),
               0.35f
             )
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
              LeanTween
                .moveX(play, (float)(Screen.width - (Screen.width * 0.7)), 0.35f)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() =>
                {
                  LeanTween
                    .moveX(exit, (float)(Screen.width - (Screen.width * 0.7)), 0.35f)
                    .setEase(LeanTweenType.easeOutQuad);
                });
            });
         });
      });

    yield return null;
  }

  public void OnSelectExit()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Began)
      {
        StartCoroutine(Exit());
      }
    } else
    {
      StartCoroutine(Exit());
    }
  }

  IEnumerator Exit()
  {
    var img = exit.transform.GetChild(0).GetComponent<Image>();
    img.CrossFadeAlpha(0.5f, 0.2f, false);

    var text = exit.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    text.CrossFadeAlpha(0.5f, 0.2f, false);

    yield return new WaitForSeconds(0.2f);
    Application.Quit();
  }

  public void OnSelectPlay()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Began)
      {
        StartCoroutine(BeginPlay());
      }
    }
    else
    {
      StartCoroutine(BeginPlay());
    }
  }

  IEnumerator BeginPlay()
  {
    var img = play.transform.GetChild(0).GetComponent<Image>();
    img.CrossFadeAlpha(0.5f, 0.2f, false);

    var text = play.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    text.CrossFadeAlpha(0.5f, 0.2f, false);

    yield return new WaitForSeconds(0.2f);
    SceneManager.LoadScene("Overworld");
  }
}
