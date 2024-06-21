using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public List<TowerData> allTowers;
    private Dictionary<int, TowerData> towerDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTowerDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeTowerDictionary()
    {
        towerDictionary = new Dictionary<int, TowerData>();
        foreach (TowerData tower in allTowers)
        {
            if (!towerDictionary.ContainsKey(tower.id))
            {
                towerDictionary.Add(tower.id, tower);
            }
            else
            {
                Debug.LogWarning("Duplicate tower ID found: " + tower.id);
            }
        }
    }

    public TowerData GetTowerById(int id)
    {
        towerDictionary.TryGetValue(id, out TowerData tower);
        return tower;
    }
}
