﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightObstacles : MonoBehaviour
{
  private Vector2 objectBounds;
  private float objectWidth, objectHeight = 0;

  float initialPosition;

  private List<GameObject> listObs = new List<GameObject>();

  public GameObject obsPrefabs;
  // Start is called before the first frame update

  private float speed = 10.0f;

  private bool isShowWall = false;

  private List<int> randomArray = new List<int>();

  private int numberOfObs = 0;
  void Start()
  {
    objectHeight = obsPrefabs.transform.GetComponent<SpriteRenderer>().bounds.size.y;
    objectWidth = obsPrefabs.transform.GetComponent<SpriteRenderer>().bounds.size.x;
    objectBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    transform.position = new Vector2(objectBounds.x + 1.5f * objectWidth, 0);
    initialPosition = objectBounds.y - objectHeight;
    StartCoroutine(SpawObject());
    // InvokeRepeating("MoveWall", 0, 4.0f);


  }

  // Update is called once per frame
  void Update()
  {

    if (isShowWall)
    {
      transform.position = Vector2.Lerp(transform.position,
        new Vector2(objectBounds.x, transform.position.y),
        (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
    else
    {
      transform.position = Vector2.Lerp(transform.position,
             new Vector2(objectBounds.x + 1.5f * objectWidth, transform.position.y),
             (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
  }

  public void MoveWall(bool isShowWall)
  {

    this.isShowWall = isShowWall;
    gameObject.SetActive(isShowWall);
    randomArray = GetRandomArray(numberOfObs, 5);
    for (int i = 0; i < listObs.Count; i++)
    {
      if (randomArray.Contains(i))
      {
        listObs[i].SetActive(true);
      }
      else
      {
        listObs[i].SetActive(false);
      }
    }
  }

  IEnumerator SpawObject()
  {
    GameObject firstObs = Instantiate(obsPrefabs) as GameObject;
    firstObs.transform.SetParent(gameObject.transform);
    firstObs.transform.position = new Vector2(objectBounds.x + objectWidth, initialPosition);


    numberOfObs = (int)((2 * objectBounds.y - objectHeight / 2) / objectHeight);
    for (int i = 0; i < numberOfObs - 1; i++)
    {
      GameObject obs = Instantiate(obsPrefabs) as GameObject;
      obs.transform.SetParent(gameObject.transform);
      obs.transform.position = new Vector2(objectBounds.x + objectWidth, initialPosition - objectHeight);
      initialPosition = initialPosition - objectHeight;
      listObs.Add(obs);
    }
    randomArray = GetRandomArray(numberOfObs, 4);

    yield return null;
  }


  public List<int> GetRandomArray(int range, int numberPick)
  {
    List<int> returnArray = new List<int>();
    for (int i = 0; i < numberPick; i++)
    {
      int randomNumber = Random.Range(0, range);
      returnArray.Add(randomNumber);
    }
    return returnArray;
  }

}

