using SuperTiled2Unity;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TsxTile : PropertiedTile {

    private SuperTile tile;
    private Dictionary<string, string> properties;

    public override int TerrainId => tile.m_TileId;

    public TsxTile(SuperTile tile) {
        this.tile = tile;
        properties = new Dictionary<string, string>();
        foreach (var property in tile.m_CustomProperties) {
            properties.Add(property.m_Name, property.m_Value);
        }
    }

    public override bool EqualsTile(TileBase tile) {
        return this.tile.Equals(tile);
    }

    protected override string GetProperty(string propertyName) {
        string result = null;
        properties.TryGetValue(propertyName, out result);
        return result;
    }
}
