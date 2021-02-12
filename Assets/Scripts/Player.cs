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

  private int scoreNum = 0;

  public GameObject bird;

  public Text bestScore;

  public ParticleSystem birdParticle;
  private int lastHightScore = 0;

  public Text menuPoint;
  void Start()
  {
    sp = bird.GetComponent<SpriteRenderer>();
    egWidth = eggPrefabs.GetComponent<SpriteRenderer>().size.x;
    screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    localScaleX = transform.localScale.x;
    localScaleY = transform.localScale.y;
    localScaleZ = transform.localScale.z;
    giftObject = Instantiate(giftPrefabs) as GameObject;
    giftObject.SetActive(false);
    StartCoroutine(RandomGift());
    // birdParticle.
    rb = GetComponent<Rigidbody2D>();
    anim = bird.GetComponent<Animator>();
    rb.gravityScale = 0;
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    score.gameObject.SetActive(false);
    lastHightScore = PlayerPrefs.GetInt("Score", 0);
    bestScore.text = "BEST SCORE: " + lastHightScore.ToString();
  }


  // Update is called once per frame
  void Update()
  {

    if (isGameOver) return;

    for (int i = 0; i < Input.touchCount; ++i)
    {
      if (Input.GetTouch(i).phase == TouchPhase.Began)
      {
        if (!isGameStart)
        {
          isGameStart = true;
          rb.gravityScale = 1;
          prepareGameStart();
          StartCoroutine(RandomGift());
          anim.enabled = false;
        }
        anim.enabled = true;
        anim.SetTrigger("Fly");
        birdParticle.Play();
        rb.AddForce(Vector2.up * pushForce);
        rb.velocity = Vector2.right.normalized * speed * orientation;
      }
    }

    if (Input.GetButtonDown("Jump"))
    {
      if (!isGameStart)
      {
        isGameStart = true;
        rb.gravityScale = 1;
        prepareGameStart();
        StartCoroutine(RandomGift());
        anim.enabled = false;
      }
      anim.enabled = true;
      anim.SetTrigger("Fly");
      birdParticle.Play();
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
    score.text = "00";
    score.gameObject.SetActive(true);
    score.text = scoreNum.ToString();
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
    // birdParticle.transform.localScale = new Vector3(localScaleX, localScaleY, localScaleZ);
  }

  private void increaseScore()
  {
    if (!isGameOver)
    {
      scoreNum++;
      score.text = scoreNum.ToString();

    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "RightWall")
    {
      birdParticle.Stop();
      increaseScore();
      ChangeLocalScale();
      orientation = -orientation;
      rb.velocity = Vector2.right.normalized * speed / 2 * orientation;
      leftObstacles.GetComponent<LeftWallHandle>().MoveWall(true);
      rightObstacles.GetComponent<RightObstacles>().MoveWall(false);
    }
    else if (other.gameObject.tag == "LeftWall")
    {
      birdParticle.Stop();
      increaseScore();
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
        score.text = "";
        menu.SetActive(true);
        menu.GetComponent<Animator>().SetTrigger("ShowMenu");
        menuPoint.text = scoreNum.ToString() + " Points";
        score.gameObject.SetActive(false);
        if (scoreNum > lastHightScore)
        {
          PlayerPrefs.SetInt("Score", scoreNum);
        }


        break;
      case "LeftWall":
        increaseScore();
        ChangeLocalScale();
        orientation = -orientation;
        // rb.velocity = Vector2.right.normalized * speed / 2 * orientation;
        leftObstacles.GetComponent<LeftWallHandle>().MoveWall(false);
        rightObstacles.GetComponent<RightObstacles>().MoveWall(true);
        birdParticle.Stop();
        break;
      default: return;
    }
  }


}
