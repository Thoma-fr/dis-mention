using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PixelManager : MonoBehaviour
{
    [SerializeField] private List<LevelData> levelList = new List<LevelData>();
    private int _levelId = 0;
    
    private GameObject _pixelPrefab;
    private ObjectPool<PixelBehaviour> _pixelPool;

    private List<PixelBehaviour> _actualPixelUse = new List<PixelBehaviour>();

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

        //Terrain
        for (int i = 0; i < levelData.maxSize; i++)
        {
            PixelBehaviour pixel = _pixelPool.Get();
            pixel.transform.position = new Vector3(-5 + i * 1, 0, 0);
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
            pixel.transform.position = new Vector3(-5 + specialPixelList[i].posIndex * 1, 0, 0);
            pixel.PixelPos = specialPixelList[i].posIndex;
            pixel.UpdateBehaviour(specialPixelList[i].State, GetPixelAction(specialPixelList[i].State));

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

    private void DestroyLevel()
    {
        for (int i = 0; i < _actualPixelUse.Count; i++)
        {
            _pixelPool.Release(_actualPixelUse[i]);
        }

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
    public PixelState State;
}

[Serializable]
public struct LevelData
{
    public int maxSize;
    public List<PixelPos> specialPixelList;
}