using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using GGG.Tool;
using GGG.Tool.Singleton;

public class GamePoolManager : Singleton<GamePoolManager>
{
    [Serializable]
    public class PoolItem
    {
        public string itemName;
        public GameObject itemPrefab;
        public int initItemCount;

        public PoolItem(string itemName, GameObject itemPrefab, int initItemCount)
        {
            this.itemName = itemName;
            this.itemPrefab = itemPrefab;
            this.initItemCount = initItemCount;
        }
    }

    [SerializeField]private List<PoolItem> itemConfigList = new List<PoolItem>();
    private Dictionary<string, Queue<GameObject>> poolCenter = new Dictionary<string, Queue<GameObject>>();
    private GameObject PoolTransform;

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        if(itemConfigList.Count == 0) return;
        for (int i = 0; i < itemConfigList.Count; i++)
        {
            for (int j = 0; j < itemConfigList[i].initItemCount; j++)
            {
                
                if (!poolCenter.ContainsKey(itemConfigList[i].itemName))
                {
                    poolCenter.Add(itemConfigList[i].itemName,new Queue<GameObject>());
                    PoolTransform = new GameObject(itemConfigList[i].itemName+"Pool");
                    PoolTransform.transform.SetParent(transform);
                }
                GameObject item = Instantiate(itemConfigList[i].itemPrefab,PoolTransform.transform);
                item.name = itemConfigList[i].itemPrefab.name + j;
                item.SetActive(false);
                    
                poolCenter[itemConfigList[i].itemName].Enqueue(item);
            }
        }
    }


    public void CreatePool(PoolItem poolItem)
    {
        if (!poolCenter.ContainsKey(poolItem.itemName))
        {
            poolCenter.Add(poolItem.itemName,new Queue<GameObject>());
            PoolTransform = new GameObject(poolItem.itemName+"Pool");
            PoolTransform.transform.SetParent(transform);
        }

        for (int i = 0; i < poolItem.initItemCount; i++)
        {
            GameObject item = Instantiate(poolItem.itemPrefab,PoolTransform.transform);
            item.name = poolItem.itemPrefab.name+i;
            item.SetActive(false);
            poolCenter[poolItem.itemName].Enqueue(item);
        }
    }

    public void GetItem(string poolName, Vector3 position, Quaternion rotation)
    {
        if (poolCenter.ContainsKey(poolName))
        {
            GameObject item = poolCenter[poolName].Dequeue();
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.SetActive(true);
            poolCenter[poolName].Enqueue(item);
        }
        else
        {
            DevelopmentToos.WTF($"poolCenter中不存在{poolName}");
            DevelopmentToos.WTF($"请前往{gameObject.name}对象中添加PoolItem的配置");
        }
    }
    public GameObject GetItem(string poolName)
    {
        
        if (poolCenter.ContainsKey(poolName))
        {
            GameObject item = poolCenter[poolName].Dequeue();
            item.SetActive(true);
            poolCenter[poolName].Enqueue(item);
            return item;
        }
  
        DevelopmentToos.WTF($"poolCenter中不存在{poolName}");
        DevelopmentToos.WTF($"请前往{gameObject.name}对象中添加PoolItem的配置");

        return null;
    }
}
