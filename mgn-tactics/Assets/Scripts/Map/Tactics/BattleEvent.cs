using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sprite representations of Units that exist on the field. This class should be as dumb as
 * possible and just respond to animation and movement requests.
 */
[RequireComponent(typeof(CharaEvent))]
[DisallowMultipleComponent]
public class BattleEvent : MonoBehaviour {

    [HideInInspector]
    public Unit unitData;
    public BattleUnit unit { get; private set; }
    public BattleController controller { get; private set; }

    private TacticsTerrainMesh _terrain;
    public TacticsTerrainMesh terrain {
        get {
            if (_terrain == null) _terrain = GetComponent<MapEvent>().parent.terrain;
            return _terrain;
        }
    }

    public Vector2Int position { get { return GetComponent<MapEvent>().position; } }

    public void Setup(BattleController controller, BattleUnit unit) {
        this.unit = unit;
        this.controller = controller;
    }

    public void PopulateWithUnitData(Unit unitData) {
        this.unitData = unitData;
        if (unitData != null) {
            GetComponent<CharaEvent>().spritesheet = unitData.appearance;
            gameObject.name = unitData.unitName;
        }
    }

    public bool CanCrossTileGradient(Vector2Int from, Vector2Int to) {
        float fromHeight = terrain.HeightAt(from);
        float toHeight = GetComponent<MapEvent>().parent.terrain.HeightAt(to);
        if (fromHeight < toHeight) {
            return toHeight - fromHeight <= unit.unit.GetMaxAscent();
        } else {
            return fromHeight - toHeight <= unit.unit.GetMaxDescent();
        }
    }
}
