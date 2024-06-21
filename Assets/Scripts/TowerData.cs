using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData")]
public class TowerData : ScriptableObject
{
    public int id;
    public string towerName;
    public GameObject towerPrefab;
    public Sprite icon;
    public int goldCost = 10;
    public int maxhp = 50;
    public float range = 6f;
    public int damage = 1;
    public float fireRate = 1f;
    public bool isProjectile = false;
    public bool isArea = false;
    public GameObject projectile;
}