using UnityEngine.Tilemaps;

public abstract class PropertiedTile {

    #region Abstract

    public abstract bool EqualsTile(TileBase tile);

    public abstract int TerrainId { get; }

    protected abstract string GetProperty(string propertyName);

    #endregion

    #region Properties

    private const string PropertyPassable = "o";
    private const string PropertyImpassable = "x";
    private const string PropertyCounter = "counter";
    private const string PropertyTransparent = "trans";

    public bool IsPassable => GetProperty(PropertyPassable) != null;
    public bool IsImpassable => GetProperty(PropertyImpassable) != null;
    public bool IsCounter => GetProperty(PropertyCounter) != null;
    public bool IsTransparent => GetProperty(PropertyTransparent) != null;

    #endregion
}
