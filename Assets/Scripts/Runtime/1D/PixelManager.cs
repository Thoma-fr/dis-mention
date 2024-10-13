using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class PixelManager : MonoBehaviour
{
    public static PixelManager Instance;

    [Header("Effect")] 
    [SerializeField] private PixelEnterExit _pixelEffect;
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private GameObject _hitCamera;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TutorialEffect _tutoEffect;

    [Header("Camera")] 
    [SerializeField] private Transform _firstCamPoint;
    [SerializeField] private Transform _secondCamPoint;

    [Header("Level")]
    [SerializeField] private List<LevelData> _levelList = new List<LevelData>();
    private int _levelId = 0;
    private Vector3 _firstPos = new Vector3(-5, 0, 0);

    [Header("Pixel")]
    private GameObject _pixelPrefab;
    private ObjectPool<PixelBehaviour> _pixelPool;

    [Header("In Game")]
    private List<PixelBehaviour> _actualPixelUse = new List<PixelBehaviour>();
    private List<int> _pixelWallPos = new List<int>();
    private int _coins = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _hitCamera.SetActive(false);
    }

    private void Start()
    {
        _pixelEffect.EnterEffect();

        _pixelPrefab = Resources.Load<GameObject>("1D/Pixel");

        PixelPool pool = new PixelPool();
        _pixelPool = pool.CreatePool(_pixelPrefab, 100);

        _tutoEffect.LaunchTutorialEffect();

        CreateLevel();
    }

    private void CreateLevel()
    {
        LevelData levelData = _levelList[_levelId];

        if(levelData.maxSize < 0)
            return;

        _firstCamPoint.position = _firstPos;
        _secondCamPoint.position = _firstPos + new Vector3(levelData.maxSize, 0, 0);

        //Terrain
        for (int i = 0; i < levelData.maxSize; i++)
        {
            PixelBehaviour pixel = _pixelPool.Get();
            pixel.transform.position = _firstPos + new Vector3(i * 1, 0, 0);
            pixel.UpdateBehaviour(PixelState.NOTHING);

            _actualPixelUse.Add(pixel);
        }

        //Pixel
        List<PixelPos> specialPixelList =  levelData.specialPixelList;
        for (int i = 0; i < specialPixelList.Count; i++)
        {
            if(specialPixelList[i].posIndex < 0)
                continue;

            PixelBehaviour pixel = _pixelPool.Get();
            pixel.transform.position = _firstPos + new Vector3(specialPixelList[i].posIndex * 1, 0, 0);
            pixel.PixelPos = specialPixelList[i].posIndex;
            pixel.MaxPixelPos = levelData.maxSize;
            pixel.UpdateBehaviour(specialPixelList[i].pixelState, GetPixelAction(specialPixelList[i].pixelState));

            if(specialPixelList[i].pixelState == PixelState.WALL)
                _pixelWallPos.Add(specialPixelList[i].posIndex);

            _actualPixelUse.Add(pixel);
        }
    }

    private Action GetPixelAction(PixelState pixelState)
    {
        switch (pixelState)
        {
            case PixelState.ENEMY:
                return ResetLevel;
            case PixelState.FINISH:
                return NextLevel;
            case PixelState.COIN:
                return GetCoin;
        }

        return null;
    }

    public bool IsThereWall(int nextPos)
    {
        return _pixelWallPos.Contains(nextPos);
    }

    private void DestroyLevel()
    {
        for (int i = 0; i < _actualPixelUse.Count; i++)
        {
            _pixelPool.Release(_actualPixelUse[i]);
        }

        _pixelWallPos.Clear();
        _actualPixelUse.Clear();
    }

    private void GetCoin()
    {
        _coins++;
        _coinText.text = _coins.ToString();
        _coinText.transform.DOShakePosition(0.5f, 5f);
    }

    private void NextLevel()
    {
        _levelId++;

        if (_levelId >= _levelList.Count)
        {
            FinishGame();
            return;
        }
            
        DestroyLevel();
        CreateLevel();
    }

    private void ResetLevel()
    {
        StartCoroutine(HitEffect());
        DestroyLevel();
        CreateLevel();
    }

    private void FinishGame()
    {
        _pixelEffect.ExitEffect(_firstPos + new Vector3(_levelList[_levelId - 1].maxSize * 1, 0, 0));
    }

    private IEnumerator HitEffect()
    {
        SwitchCam(true);
        yield return new WaitForSeconds(0.5f);
        SwitchCam(false);
    }

    public void SwitchCam(bool isHit)
    {
        _virtualCamera.SetActive(!isHit);
        _hitCamera.SetActive(isHit);
    }

    
}

public enum PixelState
{
    NOTHING,
    PLAYER,
    WALL,
    ENEMY,
    FINISH,
    COIN
}

[Serializable]
public struct PixelColor
{
    public PixelState State;
    public Color color;
}

[Serializable]
public struct PixelPos
{
    public int posIndex;
    public PixelState pixelState;
}

[Serializable]
public struct LevelData
{
    public int maxSize;
    public List<PixelPos> specialPixelList;
}