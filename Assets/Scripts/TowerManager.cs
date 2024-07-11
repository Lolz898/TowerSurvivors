using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public List<TowerData> allTowers;
    private Dictionary<int, TowerData> towerDictionary;
    private List<int> pickedTowerIds; // List to keep track of picked towers

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTowerDictionary();
            pickedTowerIds = new List<int>(); // Initialize the picked towers list
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

    public void PickTower(int id)
    {
        if (towerDictionary.ContainsKey(id) && !pickedTowerIds.Contains(id))
        {
            pickedTowerIds.Add(id);
            towerDictionary.Remove(id);
        }
        else
        {
            Debug.LogWarning("Tower ID " + id + " not found or already picked.");
        }
    }

    public List<TowerData> GetAvailableTowers()
    {
        List<TowerData> availableTowers = new List<TowerData>();
        foreach (TowerData tower in allTowers)
        {
            if (!pickedTowerIds.Contains(tower.id))
            {
                availableTowers.Add(tower);
            }
        }
        return availableTowers;
    }

    public int GetAvailableTowerCount()
    {
        return (allTowers.Count - pickedTowerIds.Count);
    }

    public bool IsTowerPicked(int id)
    {
        return pickedTowerIds.Contains(id);
    }
}
