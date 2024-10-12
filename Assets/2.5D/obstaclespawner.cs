using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclespawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _prefabs;
    void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
            Instantiate(_prefabs, transform.position,Quaternion.identity);
        }
    }
}
