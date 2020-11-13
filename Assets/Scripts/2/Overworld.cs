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
    for (int i = 0; i < amount; i++)
    {
      Instantiate(encounterPrefab, node1.transform);
    }
  }

  private void Refresh()
  {
    SceneManager.LoadScene("Overworld");
  }
}
