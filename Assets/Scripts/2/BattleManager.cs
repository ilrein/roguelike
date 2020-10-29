using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
  // hero
  public GameObject playerCharacter;
 
  // different spawnable monsters
  public GameObject hornetPrefab;

  // deck
  public Deck deck;

  private float DetermineEnemies()
  {
    return Random.Range(0f, 3f);
  }

  private void Start()
  {
    double d = 2.5;
    float f = (float) d;

    playerCharacter.transform.position = Camera.main.ScreenToWorldPoint(
      new Vector3(
        Screen.width / 8,
        Screen.height / f,
        100
       )
    );

    double d2 = 0.9;
    float f2 = (float)d2;

    for (float i = 0; i < DetermineEnemies(); i++)
    {
      var enemy = Instantiate(hornetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
      var offset = i / 10;

      enemy.transform.position = Camera.main.ScreenToWorldPoint(
        new Vector3(
          Screen.width * (f2 - offset),
          Screen.height / f,
          100
        )
      );

      enemy.GetComponent<UnityArmatureComponent>().sortingOrder = 2;
    }
  }

  public void Reset()
  {
    var currentScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(currentScene);
  }
}
