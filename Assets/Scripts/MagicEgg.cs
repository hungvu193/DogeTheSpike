using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEgg : MonoBehaviour
{
  // Start is called before the first frame update
  private float objectWidth, objectHeight;
  public float speed = 1.0f;
  private Rigidbody2D rb;
  void Start()
  {
    // transform.Translate()
    rb = GetComponent<Rigidbody2D>();
    objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    objectHeight = GetComponent<SpriteRenderer>().bounds.size.y;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void TranslateOnStart(float toX, float toY, float toZ)
  {
    // transform.position = Vector3.Lerp(
    //     transform.position,
    //     new Vector3(toX, toY, toZ),
    //     (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    if (rb != null)
    {
      Debug.Log("Run right bitches");
      rb.velocity = Vector2.right * speed;
    }
    else
    {
      Debug.Log("RB Null");
    }


  }
}
