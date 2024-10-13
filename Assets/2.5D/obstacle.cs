using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(300, 500);
        Destroy(gameObject,30);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = ((new Vector3(-1, 0, 0).normalized * _speed) * Time.deltaTime);
    }
}
