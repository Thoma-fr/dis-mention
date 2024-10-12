using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PixelPool
{
    private int amountToPool = 10;

    public ObjectPool<PixelBehaviour> CreatePool(GameObject prefab, int maxCapacity)
    {
        Transform poolItemsParent = new GameObject($"Pool ({prefab.name})").transform;
        return new ObjectPool<PixelBehaviour>(() => CreatePooledItem(prefab, poolItemsParent), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, amountToPool, maxCapacity);
    }

    private PixelBehaviour CreatePooledItem(GameObject prefab, Transform parent)
    {
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.SetActive(false);

        return obj.GetComponent<PixelBehaviour>();
    }

    private void OnTakeFromPool(PixelBehaviour poolItem)
    {
        poolItem.gameObject.SetActive(true);
        poolItem.B2D.enabled = false;
    }

    private void OnReturnedToPool(PixelBehaviour poolItem)
    {
        poolItem.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(PixelBehaviour poolItem)
    {
        GameObject.Destroy(poolItem.gameObject);
    }
}
