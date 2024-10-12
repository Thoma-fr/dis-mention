using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField, Tooltip("Corps du snake")]
    private GameObject m_snakeBody;

    [SerializeField, Tooltip("Endroit de spawn du corps")]
    private Transform m_snakeSpawnBody;
    // Start is called before the first frame update
    void Start()
    {
        m_dir = transform.up;
        m_timeMovement = timeMovement;
    }

    // Update is called once per frame
    void Update()
    {
        //input
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_dir = new Vector3(-distanceMovement, 0, 0);
            this.transform.rotation = Quaternion.Euler(0,0,90);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_dir = new Vector3(distanceMovement, 0, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, -90);

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_dir = new Vector3(0, distanceMovement, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_dir = new Vector3(0, -distanceMovement, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        //mouvement du snake
        if (m_timeMovement > 0)
            m_timeMovement -= Time.deltaTime * moveSpeed;
        else
        {
            this.transform.position += m_dir;
            m_timeMovement = timeMovement;
            //verifier la collision avant de spawn un nouveau body
            Instantiate(m_snakeBody, m_snakeSpawnBody.position, this.transform.rotation);
        }
    }
}
