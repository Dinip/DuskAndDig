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
    private int diamondSpawnHeight = -25;

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
            var newTile = TileToSpawn(spawnProbability, slot.y);
            tile.tile = newTile;
            _mainTilemap.SetTile(slot, newTile);
        }
    }

    private RuleTileWithData TileToSpawn(SpawnProbability originalSpawnProbability, float y)
    {
        Debug.Log(y);
        SpawnProbability spawnProbability = new()
        {
            day = originalSpawnProbability.day,
            ironProbability = y <= diamondSpawnHeight ? originalSpawnProbability.ironProbability : originalSpawnProbability.ironProbability + (originalSpawnProbability.diamondProbability / 2),
            goldProbability = y <= diamondSpawnHeight ? originalSpawnProbability.goldProbability : originalSpawnProbability.goldProbability + (originalSpawnProbability.diamondProbability / 2),
            diamondProbability = y <= diamondSpawnHeight ? originalSpawnProbability.diamondProbability : 0
        };

        float randomValue = UnityEngine.Random.value;

        if (randomValue <= spawnProbability.ironProbability)
        {
            // Spawn iron tile
            return ores[0];
        }
        else if (randomValue <= spawnProbability.ironProbability + spawnProbability.goldProbability)
        {
            // Spawn gold tile
            return ores[1];
        }
        else if (randomValue <= spawnProbability.ironProbability + spawnProbability.goldProbability + spawnProbability.diamondProbability)
        {
            // Spawn diamond tile
            return ores[2];
        }
        else
        {
            // Spawn stone tile
            return ores[3];
        }
    }
}


[System.Serializable]
public class SpawnProbability
{
    public int day;
    public float ironProbability;
    public float goldProbability;
    public float diamondProbability;
}