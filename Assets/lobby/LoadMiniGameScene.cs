using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMiniGameScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button _playButton;
    
    void Awake()
    {
        _playButton.onClick.AddListener(StartMiniGame);
    }
    private void StartMiniGame()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 45), 1f,RotateMode.LocalAxisAdd));
        mySequence.Join(transform.DOScale(20, 1f)).OnComplete(()=> SceneManager.LoadSceneAsync("1D"));
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
