using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData")]
public class TowerData : ScriptableObject
{
    public int id;
    public string towerName;
    public string towerDescription;
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
    public int projectileCount = 1;
    public float multiProjDelay = 0.005f;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 2f;
    public float projectileSize = 1f;
}