using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class LifeCounter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> heartObjects = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void removeOneLife()
    {
        if (heartObjects.Count > 0)
        {
            Destroy(heartObjects[0]);
            heartObjects.RemoveAt(0);
        }
        else
            GameOver();
    }
    private void GameOver()
    {

    }
}
