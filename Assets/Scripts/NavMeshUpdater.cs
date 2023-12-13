using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface Surface2D;

    private void Start()
    {
        Surface2D = GetComponent<NavMeshSurface>();
        Surface2D.BuildNavMeshAsync();
    }

    void Update()
    {
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}
