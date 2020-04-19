using SuperTiled2Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * MGNE's big map class, now in MGNE2. Converted from Tiled and now to unity maps.
 */
public abstract class Map : MonoBehaviour {

    #region Constants

    /// <summary>The number of pixels a tile takes up</summary>
    public const int PxPerTile = 16;
    /// <summary>The number of pixels that make up a tile</summary>
    public const float UnitsPerTile = 1;

    public const string ResourcePath = "Maps/";

    #endregion

    #region Properties and getters

    public Grid grid;
    public ObjectLayer objectLayer;
    public string InternalName { get; set; } = "test"; // hack for dev

    // true if the tile in question is passable at x,y
    private Dictionary<Tilemap, short[,]> passabilityMap;

    private List<Tilemap> _layers;
    public List<Tilemap> layers {
        get {
            if (_layers == null) {
                _layers = new List<Tilemap>();
                foreach (Transform child in grid.transform) {
                    if (child.GetComponent<Tilemap>()) {
                        _layers.Add(child.GetComponent<Tilemap>());
                    }
                }
            }
            return _layers;
        }
    }

    private Vector2Int _size;
    public Vector2Int size {
        get {
            if (_size.x == 0) {
                _size = InternalGetSize();
            }
            return _size;
        }
    }
    protected abstract Vector2Int InternalGetSize();

    public int Width { get => size.x; }
    public int Height { get => size.y; }

    public abstract bool CameraRestricted { get; }
    public abstract string MapName { get; }
    public abstract string BgmKey { get; }
    public abstract string EncounterKey { get; }
    public abstract string TerrainEncounterKey { get; }
    public abstract string BattleBGMKey { get; }

    #endregion

    public Vector3Int TileToTilemapCoords(Vector2Int loc) {
        return TileToTilemapCoords(loc.x, loc.y);
    }

    public Vector3Int TileToTilemapCoords(int x, int y) {
        return new Vector3Int(x, -1 * y, 0);
    }

    public abstract PropertiedTile TileAt(Tilemap layer, int x, int y);

    public short IsChipPassableAt(Tilemap layer, Vector2Int loc) {
        var groundLayer = layer.GetComponent<TilemapRenderer>().sortingOrder <= 2;
        if (passabilityMap == null) {
            passabilityMap = new Dictionary<Tilemap, short[,]>();
        }
        if (!passabilityMap.ContainsKey(layer)) {
            passabilityMap[layer] = new short[Width, Height];
            for (int x = 0; x < Width; x += 1) {
                for (int y = 0; y < Height; y += 1) {
                    PropertiedTile tile = TileAt(layer, x, y);
                    if (tile != null) {
                        if (tile.IsPassable) passabilityMap[layer][x, y] += 1;
                        if (tile.IsImpassable && groundLayer) passabilityMap[layer][x, y] -= 1;
                    }
                }
            }
        }

        return passabilityMap[layer][loc.x, loc.y];
    }

    public bool IsChipPassableAt(Vector2Int loc) {
        short total = 0;
        foreach (Tilemap layer in layers) {
            layer.GetComponent<SuperCustomProperties>().TryGetCustomProperty("CheckPassability", out CustomProperty prop);
            if (prop != null && prop.GetValueAsBool()) {
                total += IsChipPassableAt(layer, loc);
            }
        }
        return total >= 0;
    }

    public bool HasTilePropertyAt(Vector2Int loc, Func<PropertiedTile, bool> rule) {
        foreach (Tilemap layer in layers) {
            var tile = TileAt(layer, loc.x, loc.y);
            if (rule(tile)) {
                return true;
            }
        }
        return false;
    }

    // careful, this implementation is straight from MGNE, it's efficiency is questionable, to say the least
    // it does support bigger than 1*1 events though
    public List<MapEvent> GetEventsAt(Vector2Int loc) {
        List<MapEvent> events = new List<MapEvent>();
        foreach (MapEvent mapEvent in objectLayer.GetComponentsInChildren<MapEvent>()) {
            if (mapEvent.ContainsPosition(loc)) {
                events.Add(mapEvent);
            }
        }
        return events;
    }

    // returns the first event at loc that implements T
    public T GetEventAt<T>(Vector2Int loc) {
        List<MapEvent> events = GetEventsAt(loc);
        foreach (MapEvent mapEvent in events) {
            if (mapEvent.GetComponent<T>() != null) {
                return mapEvent.GetComponent<T>();
            }
        }
        return default;
    }

    // returns all events that have a component of type t
    public List<T> GetEvents<T>() {
        return new List<T>(objectLayer.GetComponentsInChildren<T>());
    }

    public Tilemap TileLayerAtIndex(int layerIndex) {
        return GetComponentsInChildren<Tilemap>()[layerIndex];
    }

    public MapEvent GetEventNamed(string eventName) {
        if (eventName == "hero") {
            return Global.Instance().Maps.Avatar.Event;
        }
        foreach (var layer in GetComponentsInChildren<ObjectLayer>()) {
            foreach (var mapEvent in layer.GetComponentsInChildren<MapEvent>()) {
                if (mapEvent.name == eventName) {
                    return mapEvent;
                }
            }
        }
        return null;
    }

    public void OnTeleportTo() {
        if (BgmKey != null) {
            Global.Instance().Audio.PlayBGM(BgmKey);
        }
    }

    public void OnTeleportAway() {

    }

    public void OnStepStarted() {
        foreach (var mapEvent in GetEvents<MapEvent>()) {
            mapEvent.OnStepStarted();
        }
    }

    public void OnStepEnded() {

    }

    // returns a list of coordinates to step to with the last one being the destination, or null
    public List<Vector2Int> FindPath(MapEvent actor, Vector2Int to, bool ignoreEvents = false) {
        return FindPath(actor, to, Width > Height ? Width : Height, ignoreEvents);
    }
    public List<Vector2Int> FindPath(MapEvent actor, Vector2Int to, int maxPathLength, bool ignoreEvents = false) {
        if (ManhattanDistance(actor.GetComponent<MapEvent>().Position, to) > maxPathLength) {
            return null;
        }
        if (!actor.CanPassAt(to) && !ignoreEvents) {
            return null;
        }

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        List<List<Vector2Int>> heads = new List<List<Vector2Int>>();
        List<Vector2Int> firstHead = new List<Vector2Int>();
        firstHead.Add(actor.GetComponent<MapEvent>().Position);
        heads.Add(firstHead);

        while (heads.Count > 0) {
            heads.Sort(delegate (List<Vector2Int> pathA, List<Vector2Int> pathB) {
                int pathACost = pathA.Count + ManhattanDistance(pathA[pathA.Count - 1], to);
                int pathBCost = pathB.Count + ManhattanDistance(pathB[pathB.Count - 1], to);
                return pathACost.CompareTo(pathBCost);
            });
            List<Vector2Int> head = heads[0];
            heads.RemoveAt(0);
            Vector2Int at = head[head.Count - 1];

            if (at == to) {
                // trim to remove the current location from the beginning
                return head.GetRange(1, head.Count - 1);
            }

            if (head.Count < maxPathLength) {
                foreach (OrthoDir dir in Enum.GetValues(typeof(OrthoDir))) {
                    Vector2Int next = head[head.Count - 1];
                    // minor perf here, this is critical code
                    switch (dir) {
                        case OrthoDir.East: next.x += 1; break;
                        case OrthoDir.North: next.y += 1; break;
                        case OrthoDir.West: next.x -= 1; break;
                        case OrthoDir.South: next.y -= 1; break;
                    }
                    if (!visited.Contains(next) &&
                            (ignoreEvents || actor.GetComponent<CharaEvent>() == null || actor.CanPassAt(next) || next == to)) {
                        List<Vector2Int> newHead = new List<Vector2Int>(head) { next };
                        heads.Add(newHead);
                        visited.Add(next);
                    }
                }
            }
        }

        return null;
    }

    public static int ManhattanDistance(Vector2Int a, Vector2Int b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
