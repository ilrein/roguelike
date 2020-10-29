using UnityEngine;
using UnityEngine.UI;

public class Act : MonoBehaviour
{
  private int floors = 3;

  public Image monster;
  public GameObject baseFloor;
  public GameObject content;

  private void Start()
  {
    GenerateTree();
  }

  private void GenerateTree()
  {
    for (int i = 0; i < floors; i++)
    {
      Debug.Log("making floor");

      Instantiate(baseFloor, content.transform, false);
    }
  }
}
