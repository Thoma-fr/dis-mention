using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class thirdminigamemanager : MonoBehaviour
{
    public static thirdminigamemanager instance;
    public List<obstaclespawner> _spawnerlist = new List<obstaclespawner>();
    void Awake()
    {
        if(instance == null)
            instance=this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpeedUp()
    {
        foreach (var item in _spawnerlist)
        {
            item.Maxdelay -= 0.10f;
        }
    }
}
