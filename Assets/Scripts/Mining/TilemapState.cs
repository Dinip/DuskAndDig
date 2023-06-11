using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/TilemapState")]
public class TilemapState : ScriptableObject
{
    private Dictionary<Vector3Int, RuleTileDataState> _tiles;

    void OnEnable()
    {
        _tiles = new Dictionary<Vector3Int, RuleTileDataState>();
    }

    public RuleTileDataState GetTile(Vector3Int pos)
    {
        if (_tiles.ContainsKey(pos))
        {
            return _tiles[pos];
        }
        return null;
    }

    public void LoadTiles(Tilemap tilemap)
    {
        _tiles.Clear();
        var allTiles = tilemap.cellBounds.allPositionsWithin;
        foreach (var pos in allTiles)
        {
            if (!tilemap.HasTile(pos)) continue;

            var tile = tilemap.GetTile(pos);
            if (tile is not RuleTileWithData) continue;

            var ruleTile = tile as RuleTileWithData;
            if (!ruleTile.filler) continue;

            var state = new RuleTileDataState
            {
                tile = ruleTile
            };

            _tiles.Add(pos, state);
        }
    }

    public bool BreakTile(Vector3Int position)
    {
        var tile = GetTile(position);
        if (tile == null) return false;

        tile.hits++;
        if (tile.hits >= 2)
        {
            _tiles.Remove(position);
            return true;
        }

        if (tile.tile.nextState != null)
        {
            tile.tile = tile.tile.nextState;
        }
        return false;
    }

    public List<Vector3Int> GetKeys()
    {
        return new List<Vector3Int>(_tiles.Keys);
    }
}