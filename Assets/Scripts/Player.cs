using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
  private Rigidbody2D rb;

  public float speed;
  public float pushForce;
  // Start is called before the first frame update
  private float orientation = 1;
  private bool isGameOver = false;

  public GameObject leftObstacles;
  public GameObject rightObstacles;

  public GameObject menu;

  private float localScaleX, localScaleY, localScaleZ;

  private Animator anim;

  public GameObject giftPrefabs;

  public GameObject eggPrefabs;

  private GameObject giftObject;

  public Text score;
  private Vector2 screenBounds;

  private float egWidth;

  public Sprite xbird;
  public SpriteRenderer sp;

  private bool isGameStart = false;

  void Start()
  {
    sp = GetComponent<SpriteRenderer>();
    egWidth = eggPrefabs.GetComponent<SpriteRenderer>().size.x;
    screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    localScaleX = transform.localScale.x;
    localScaleY = transform.localScale.y;
    localScaleZ = transform.localScale.z;
    giftObject = Instantiate(giftPrefabs) as GameObject;
    giftObject.SetActive(false);
    StartCoroutine(RandomGift());

    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    rb.gravityScale = 0;
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    score.SetActive(false);
  }


  // Update is called once per frame
  void Update()
  {


    if (!isGameOver && (Input.GetButtonDown("Jump") || Input.touchCount > 0))
    {
      if (!isGameStart)
      {
        isGameStart = true;
        rb.gravityScale = 1;
        // anim.enabled = false;
        prepareGameStart();
        StartCoroutine(RandomGift());
      }
      anim.enabled = true;
      anim.SetTrigger("Fly");
      rb.AddForce(Vector2.up * pushForce);
      rb.velocity = Vector2.right.normalized * speed * orientation;

    }
  }

  private void prepareGameStart()
  {
    GameObject.Find("Rank").SetActive(false);
    GameObject.Find("Sound").SetActive(false);
    GameObject.Find("DogeTheSpike").SetActive(false);
    GameObject.Find("TapToJump").SetActive(false);
    GameObject.Find("Bestscore").SetActive(false);
    // GameObject.Find("Score").SetActive(true);
  }

  IEnumerator RandomGift()
  {
    if (!isGameOver && isGameStart)
    {

      giftObject.SetActive(false);
      yield return new WaitForSeconds(1.5f);
      if (giftObject != null)
      {
        giftObject.SetActive(true);
        giftObject.transform.position = new Vector2(
            Random.Range(-screenBounds.x + egWidth / 2, screenBounds.x - egWidth / 2),
            Random.Range(-screenBounds.y + egWidth, screenBounds.y - egWidth));
      }

    };

  }

  private void ChangeLocalScale()
  {
    localScaleX = -localScaleX;
    transform.localScale = new Vector3(localScaleX, localScaleY, localScaleZ);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "RightWall")
    {
      ChangeLocalScale();
      orientation = -orientation;
      rb.velocity = Vector2.right.normalized * speed / 2 * orientation;
      leftObstacles.GetComponent<LeftWallHandle>().MoveWall(true);
      rightObstacles.GetComponent<RightObstacles>().MoveWall(false);
    }
    else if (other.gameObject.tag == "LeftWall")
    {
      ChangeLocalScale();
      orientation = -orientation;
      rb.velocity = Vector2.right.normalized * speed / 2 * orientation;
      leftObstacles.GetComponent<LeftWallHandle>().MoveWall(false);
      rightObstacles.GetComponent<RightObstacles>().MoveWall(true);
    }
    else if (other.gameObject.tag == "Diamon")
    {
      StartCoroutine(RandomGift());
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    switch (other.gameObject.tag)
    {

      case "Obstacle":
        isGameOver = true;
        Destroy(giftObject);
        rb.constraints = RigidbodyConstraints2D.None;
        anim.enabled = false;
        sp.sprite = xbird;
        menu.SetActive(true);
        menu.GetComponent<Animator>().SetTrigger("ShowMenu");
        GameObject.Find("Score").SetActive(false);
        break;
      case "LeftWall":
        ChangeLocalScale();
        orientation = -orientation;
        // rb.velocity = Vector2.right.normalized * speed / 2 * orientation;
        leftObstacles.GetComponent<LeftWallHandle>().MoveWall(false);
        rightObstacles.GetComponent<RightObstacles>().MoveWall(true);
        break;
      default: return;
    }
  }


}
