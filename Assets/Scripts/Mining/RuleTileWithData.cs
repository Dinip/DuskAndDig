using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/CustomRuleTile")]
public class RuleTileWithData : RuleTile
{
    public ItemObject itemObject;
    public RuleTileWithData nextState;
    public bool filler = false;
}

public class RuleTileDataState
{
    public RuleTileWithData tile;
    public int hits = 0;
}
