﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardVisualizer : MonoBehaviour
{
    public GameBoard board;

    public GameObject prefabAim;
    public GameObject prefabMiss;
    public GameObject prefabHit;

    public MissileSiloScript missileSilo;

    [Header("Can be null")]
    public RevealFog fogRemover;

    [Header("Settings")]
    public bool removeFogOnAim = false;

    [SerializeField, HideInInspector]
    private List<GameObject> placed = new List<GameObject>();

    private void OnEnable()
    {
        ResetAll();
    }

    public void ResetAll()
    {
        foreach (GameObject go in placed)
        {
            Destroy(go);
        }
        placed.Clear();
        ResetAim();
    }

    public void MoveAim(Vector2Int coordinate)
    {
        Vector3 position = board.CoordinateToWorld(coordinate, transform.position.y);

        prefabAim.SetActive(true);
        prefabAim.transform.position = position;

        if (removeFogOnAim)
            RemoveFogAt(coordinate, false);
    }

    public void ResetAim()
    {
        prefabAim.SetActive(false);
    }

    public void PlaceHitAt(Vector2Int coordinate)
    {
        missileSilo.FireMissileHit(board.CoordinateToWorld(coordinate), missile =>
        {
            PlacePrefabAt(prefabHit, coordinate);
        });
        ResetAim();
    }

    public void PlaceMissAt(Vector2Int coordinate)
    {
        missileSilo.FireMissileMiss(board.CoordinateToWorld(coordinate), missile =>
        {
            PlacePrefabAt(prefabMiss, coordinate);
        });
        ResetAim();
    }

    private void PlacePrefabAt(GameObject prefab, Vector2Int coordinate)
    {
        Vector3 position = board.CoordinateToWorld(coordinate, transform.position.y);

        GameObject clone = Instantiate(prefab, position, prefab.transform.rotation, transform);
        clone.SetActive(true);
        placed.Add(clone);

        RemoveFogAt(coordinate, true);
    }

    private void RemoveFogAt(Vector2Int coordinate, bool permanently)
    {
        if (fogRemover != null)
            fogRemover.StopTheFogAt(coordinate, permanently);
    }
}
