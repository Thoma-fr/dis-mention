using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    // Start is called before the first frame update
    [field:SerializeField] public int DimensionIndex {  get; set; }
    public static GamaManager instance;
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
