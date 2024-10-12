using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Character25Dmove : MonoBehaviour
{
    private float _horizontal;
    [SerializeField]private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxZ,_minbz;

    [SerializeField] private LifeCounter _lifeCounter;
    // Start is called before the first frame update
    [SerializeField] private
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Q)&& transform.position.z< _maxZ)
            _rb.velocity=((new Vector3(0,0, 1).normalized * _speed) * Time.deltaTime);
        else if(Input.GetKey(KeyCode.D) && transform.position.z > _minbz)
            _rb.velocity = ((new Vector3(0, 0, -1).normalized * _speed) * Time.deltaTime);
        else
            _rb.velocity = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        _lifeCounter.removeOneLife();
    }
}
