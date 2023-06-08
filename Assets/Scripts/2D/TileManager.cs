using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private TimeControllerObject timeState;

    [SerializeField]
    private TilemapState tilemapState;

    [SerializeField]
    private RuleTileWithData[] ores;

    [SerializeField]
    public List<SpawnProbability> spawnProbabilities;

    private Tilemap _mainTilemap;

    private void Awake()
    {
        _mainTilemap = GetComponent<Tilemap>();
        tilemapState.LoadTiles(_mainTilemap);
        ReplaceTilesWithOres();
    }

    private void ReplaceTilesWithOres()
    {
        var spawnProbability = spawnProbabilities.Find(sp => sp.day == timeState.currentDay);
        spawnProbability ??= spawnProbabilities[spawnProbabilities.Count - 1];

        var rng = new System.Random();

        var possibleSlots = new List<Vector3Int>(tilemapState.GetKeys()).OrderBy(_ => rng.Next()).ToList();

        foreach (var slot in possibleSlots)
        {
            var tile = tilemapState.GetTile(slot);
            var newTile = TileToSpawn(spawnProbability);
            tile.tile = newTile;
            _mainTilemap.SetTile(slot, newTile);
        }
    }

    private RuleTileWithData TileToSpawn(SpawnProbability spawnProbability)
    {
        // calculate the remaining percentage for the stone tile
        float remainingPercentage = 1f - spawnProbability.ironProbability - spawnProbability.goldProbability;

        float randomValue = UnityEngine.Random.value;

        if (randomValue <= remainingPercentage)
        {
            // spawn stone tile
            return ores[2];
        }
        else if (randomValue <= spawnProbability.goldProbability)
        {
            // spawn gold tile
            return ores[1];
        }
        else
        {
            // spawn gold tile
            return ores[0];
        }
    }
}


[System.Serializable]
public class SpawnProbability
{
    public int day;
    public float ironProbability;
    public float goldProbability;
}