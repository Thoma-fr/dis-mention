using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PixelManager : MonoBehaviour
{
    public static PixelManager Instance;

    [Header("Camera")] 
    [SerializeField] private Transform _firstCamPoint;
    [SerializeField] private Transform _secondCamPoint;

    [Header("Level")]
    [SerializeField] private List<LevelData> levelList = new List<LevelData>();
    private int _levelId = 0;
    private Vector3 _firstPos = new Vector3(-5, 0, 0);

    [Header("Pixel")]
    private GameObject _pixelPrefab;
    private ObjectPool<PixelBehaviour> _pixelPool;

    [Header("In Game")]
    private List<PixelBehaviour> _actualPixelUse = new List<PixelBehaviour>();
    private List<int> _pixelWallPos = new List<int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _pixelPrefab = Resources.Load<GameObject>("1D/Pixel");

        PixelPool pool = new PixelPool();
        _pixelPool = pool.CreatePool(_pixelPrefab, 100);

        CreateLevel();
    }

    private void CreateLevel()
    {
        LevelData levelData = levelList[_levelId];

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

    private void NextLevel()
    {
        DestroyLevel();
        _levelId++;

        if(_levelId >= levelList.Count)
            FinishGame();
        else
            CreateLevel();
    }

    private void ResetLevel()
    {
        DestroyLevel();
        CreateLevel();
    }

    private void FinishGame()
    {
        //TODO
    }
}

public enum PixelState
{
    NOTHING,
    PLAYER,
    WALL,
    ENEMY,
    FINISH
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