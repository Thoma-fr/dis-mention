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
    [SerializeField] private bool _aiRight,_aiLeft;
    void Start()
    {
        StartCoroutine(looseControl());
    }
    private void FixedUpdate()
    {
        Vector3 direction=Vector3.zero;
        if(Input.GetKey(KeyCode.Q)||_aiRight)
                direction = ((new Vector3(0,0, 1).normalized * _speed) * Time.deltaTime);
        else if(Input.GetKey(KeyCode.D)||_aiLeft)
                    direction = ((new Vector3(0, 0, -1).normalized * _speed) * Time.deltaTime);
        else
             direction = Vector3.zero;
        _rb.velocity = direction;
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, _minbz, _maxZ));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        _lifeCounter.removeOneLife();
    }
    IEnumerator looseControl()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            _aiRight = true;
            _aiLeft=false;
            yield return new WaitForSeconds(Random.Range(0.5f,3f));
            _aiRight = false;
            _aiLeft = true;
            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }

    }
}
