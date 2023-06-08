using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningSystem : MonoBehaviour
{
    [SerializeField]
    private TilemapState tilemapState;

    [SerializeField]
    private TileBase highlightTile;

    [SerializeField]
    private Tilemap mainTilemap;

    [SerializeField]
    private Tilemap tempTilemap;

    [SerializeField]
    private GameObject prefab;

    private Vector3Int _playerPos;

    private Vector3Int _highlightedTilePos;

    private bool _highlighted;

    private void Update()
    {
        _playerPos = mainTilemap.WorldToCell(transform.position);

        //if (itemObject != null)
        //{
        //    HighlightTile(itemObject);
        //}
        HighlightTile();
        if (Input.GetMouseButtonDown(0) && _highlighted)
        {
            var mouseGridPos = GetMousePosOnGrid();
            if (InRange(mouseGridPos, _playerPos, new Vector3Int(3, 3, 0)))
            {
                Destroy(mouseGridPos);
            }
        }
    }

    private Vector3Int GetMousePosOnGrid()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = mainTilemap.WorldToCell(mousePos);
        gridPos.z = 0;
        return gridPos;
    }

    private void HighlightTile()
    {
        var mouseGridPos = GetMousePosOnGrid();
        if (_highlightedTilePos != mouseGridPos)
        {
            tempTilemap.SetTile(_highlightedTilePos, null);

            if(!InRange(mouseGridPos, _playerPos, new Vector3Int(4, 4, 0)))
            {
                _highlightedTilePos = new Vector3Int(999, 999, 999);
                _highlighted = false;
                return;
            }

            TileBase tile = mainTilemap.GetTile(mouseGridPos);

            if (!tile || tile is not RuleTileWithData || (tile as RuleTileWithData).filler)
            {
                _highlightedTilePos = new Vector3Int(999,999,999);
                _highlighted = false;
                return;
            }

            tempTilemap.SetTile(mouseGridPos, highlightTile);
            _highlightedTilePos = mouseGridPos;

            _highlighted = true;
        }
    }

    private bool InRange(Vector3Int a, Vector3Int b, Vector3Int range)
    {
        return Mathf.Abs(a.x - b.x) <= range.x && Mathf.Abs(a.y - b.y) <= range.y;
    }

    //private bool CheckCondition(RuleTileWithData tile, ItemObject currentItem)
    //{
    //    if(currentItem.type = )
    //}

    private void Destroy(Vector3Int position)
    {
        RuleTileWithData tile = mainTilemap.GetTile<RuleTileWithData>(position);
        bool broke = tilemapState.BreakTile(position);
        if (broke)
        {
            mainTilemap.SetTile(position, null);
            tempTilemap.SetTile(position, null);
            _highlighted = false;

            var pos = mainTilemap.GetCellCenterWorld(position);
            var item = Instantiate(prefab, pos, Quaternion.identity);
            item.GetComponent<Loot>().Initialize(tile.itemObject);
        } else
        {
            mainTilemap.SetTile(position, tile.nextState);
        }
    }
}
