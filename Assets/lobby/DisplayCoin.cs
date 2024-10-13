using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayCoin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshPro m_TextMeshPro;
    void Start()
    {
        m_TextMeshPro.text="X"+GamaManager.instance.Collectible.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        m_TextMeshPro.text = "X" + GamaManager.instance.Collectible.ToString();
    }
}
