using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    [field:SerializeField] public int DimensionIndex {  get; set; }
    [field: SerializeField] public int Score { get; set; }
    [field: SerializeField] public int Collectible { get; set; }
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClipText;
    public static GamaManager instance;
    
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayTextSFX()
    {
        if(_audioSource != null && _audioClipText != null)
            _audioSource.PlayOneShot(_audioClipText);
    }
}
