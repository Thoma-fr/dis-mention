using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField, Tooltip("Temps entre chaque mouvement")]
    private float moveSpeed = 2f;

    [SerializeField, Tooltip("Direction du snake")]
    private Vector3 m_dir;

    [SerializeField, Tooltip("Distance du mouvement")]
    private float distanceMovement = .5f;

    [Tooltip("Interval de temps entre chaque mouvement")]
    public float timeMovement = 1f;
    private float m_timeMovement;

    [Header("Corps")]
    private SpriteRenderer m_snakeSprite;
    [SerializeField, Tooltip("Corps du snake")]
    private GameObject m_snakeBody;

    [SerializeField, Tooltip("Endroit de spawn du corps")]
    private Transform[] m_snakeSpawnBody;

    [SerializeField]
    private Sprite[] headSprites;

    [SerializeField]
    private Sprite[] bodySprites;

    [SerializeField]
    private Sprite m_headSprite;

    [SerializeField]
    private Sprite m_bodySprite;

    public enum Movement
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [SerializeField]
    private Movement m_actualMovement;

    [SerializeField]
    private Movement m_previousMovement;
    [SerializeField, Tooltip("mort du snake")]
    private bool m_isDead;

    [SerializeField, Tooltip("Vitesse en plus apres collectable")]
    private float m_collectableSpeedBoost = 1f;
    // Start is called before the first frame update
    void Start()
    {
        m_dir = transform.up;
        m_timeMovement = timeMovement;
        m_headSprite = headSprites[0];
        m_bodySprite = bodySprites[0];
        m_actualMovement = Movement.UP;
        m_previousMovement = Movement.UP;
        m_snakeSprite = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDead) return;
        //input
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_dir = new Vector3(-distanceMovement, 0, 0);
            //this.transform.rotation = Quaternion.Euler(0,0,90);
            
            switch(m_previousMovement)
            {
                case Movement.UP:
                    m_bodySprite = bodySprites[2];
                    break;

                case Movement.DOWN:
                    m_bodySprite = bodySprites[3];
                    break;
            }
            m_headSprite = headSprites[2];
            m_actualMovement = Movement.LEFT;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_dir = new Vector3(distanceMovement, 0, 0);
            //this.transform.rotation = Quaternion.Euler(0, 0, -90);

            switch (m_previousMovement)
            {
                case Movement.UP:
                    m_bodySprite = bodySprites[1];
                    break;

                case Movement.DOWN:
                    m_bodySprite = bodySprites[4];
                    break;
            }

            m_headSprite = headSprites[3];
            m_actualMovement = Movement.RIGHT;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_dir = new Vector3(0, distanceMovement, 0);
            //this.transform.rotation = Quaternion.Euler(0, 0, 0);

            switch (m_previousMovement)
            {
                case Movement.LEFT:
                    m_bodySprite = bodySprites[4];
                    break;

                case Movement.RIGHT:
                    m_bodySprite = bodySprites[3];
                    break;

                case Movement.UP:
                    m_bodySprite = bodySprites[0];
                    break;
            }

            m_headSprite = headSprites[0];
            m_actualMovement = Movement.UP;

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_dir = new Vector3(0, -distanceMovement, 0);
            //this.transform.rotation = Quaternion.Euler(0, 0, 180);

            switch (m_previousMovement)
            {
                case Movement.LEFT:
                    m_bodySprite = bodySprites[1];
                    break;

                case Movement.RIGHT:
                    m_bodySprite = bodySprites[2];
                    break;

                case Movement.DOWN:
                    m_bodySprite = bodySprites[0];
                    break;
            }
            m_headSprite = headSprites[1];
            m_actualMovement = Movement.DOWN;

        }

        //mouvement du snake
        if (m_timeMovement > 0)
            m_timeMovement -= Time.deltaTime * moveSpeed;
        else
        {
            this.transform.position += m_dir;
            m_timeMovement = timeMovement;
            //verifier la collision avant de spawn un nouveau body
            GameObject body;
            if (m_previousMovement == m_actualMovement) body = InstantiateLongBody();
            else body = Instantiate(m_snakeBody, m_snakeSpawnBody[(int)m_actualMovement].position, Quaternion.identity);
            
            body.GetComponent<SpriteRenderer>().sprite = m_bodySprite;
            m_snakeSprite.sprite = m_headSprite;
            m_previousMovement = m_actualMovement;

        }
    }

    private GameObject InstantiateLongBody()
    {
        GameObject b;
        m_bodySprite = bodySprites[0];
        switch(m_actualMovement)
        {
            case Movement.UP:
                b = Instantiate(m_snakeBody, m_snakeSpawnBody[(int)m_actualMovement].position, Quaternion.identity);
                break;
            case Movement.DOWN:
                b = Instantiate(m_snakeBody, m_snakeSpawnBody[(int)m_actualMovement].position, Quaternion.Euler(0,0,180));
                break;
            case Movement.LEFT:
                b = Instantiate(m_snakeBody, m_snakeSpawnBody[(int)m_actualMovement].position, Quaternion.Euler(0,0,90));
                break;
            case Movement.RIGHT:
                b = Instantiate(m_snakeBody, m_snakeSpawnBody[(int)m_actualMovement].position, Quaternion.Euler(0,0,-90));
                break;

            default:
                b = Instantiate(m_snakeBody, m_snakeSpawnBody[(int)m_actualMovement].position, Quaternion.identity);
            break;
        }
        return b;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collectable
        if(collision.CompareTag("Collectable"))
        {
            //incremente score etc
            if(GamaManager.instance != null) GamaManager.instance.Score += 10;
            moveSpeed += m_collectableSpeedBoost;
            collision.gameObject.SetActive(false);
        }
        //mort
        if (!collision.CompareTag("2DWall")) return;
        m_isDead = true;
        Debug.Log("GAME OVER");
        StartCoroutine(RechargeLevel());
    }

    private IEnumerator RechargeLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
