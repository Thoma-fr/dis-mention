using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclespawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _prefabs;
    [SerializeField] private GameObject _collectibleprefabs;
    public float Maxdelay { get;  set; }
    void Start()
    {
        StartCoroutine(SpawnObstacle());
        Maxdelay = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnObstacle()
    {
        int count=0;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, Maxdelay));
            if (count != 6)
                Instantiate(_prefabs, transform.position,Quaternion.identity);
            else
                Instantiate(_collectibleprefabs, transform.position, Quaternion.identity);

            count++;

        }
    }
}
