using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overworld : MonoBehaviour
{
  public Image node1;
  public GameObject encounterPrefab;

  private void Start()
  {
    GenerateBlock();
  }

  private void Update()
  {
    if (Input.GetKeyDown("space"))
    {
      Refresh();
    }
  }

  private void GenerateBlock()
  {
    float amount = Mathf.Round(Random.Range(0f, 4f));

    if (amount == 0) amount = 1;

    var height = node1.GetComponent<RectTransform>().rect.height;
    var breakpoint = 0f;

    //
    // if 1 breakpoint = 50% 1/2
    // 2 breakpoint = 33% 1/3
    // 3 breakpoint 25% 1/4
    // 4 breakpoint 20% 1/5
    //

    if (amount == 1) breakpoint = height / 2;
    if (amount == 2) breakpoint = height / 3;
    if (amount == 3) breakpoint = height / 4;
    if (amount == 4) breakpoint = height / 5;

    for (int i = 1; i < amount + 1; i++)
    {
      Debug.Log(amount);

      Instantiate(
        encounterPrefab,
        new Vector3(node1.transform.position.x, breakpoint * i),
        node1.transform.rotation,
        node1.transform
      );
    }
  }

  private void Refresh()
  {
    SceneManager.LoadScene("Overworld");
  }
}
