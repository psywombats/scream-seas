﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sprite representations of BattleUnits that exist on the field.
 */
[RequireComponent(typeof(CharaEvent))]
[DisallowMultipleComponent]
public class BattleEvent : TiledInstantiated {

    private static string InstancePath = "Prefabs/Map3D/Doll";

    public string unitKey;
    public BattleUnit unit { get; private set; }
    public BattleController controller { get; private set; }

    public static BattleEvent GetInstance(BattleController controller, BattleUnit unit) {
        BattleEvent instance = Instantiate(Resources.Load<GameObject>(InstancePath)).GetComponent<BattleEvent>();
        instance.Setup(controller, unit);
        return instance;
    }

    public void Setup(BattleController controller, BattleUnit unit) {
        this.unit = unit;
        this.controller = controller;
        SetScreenPositionToMatchTilePosition();
    }

    public override void Populate(IDictionary<string, string> properties) {
        this.unitKey = properties[MapEvent.PropertyUnit];
        GetComponent<CharaEvent>().doll.AddComponent<BillboardingSpriteComponent>();
    }

    public void OnEnable() {
        BattleController controller = GetComponent<MapEvent3D>().Parent.GetComponent<BattleController>();
        controller.AddUnitFromTiledEvent(this, unitKey);
    }

    public void SetScreenPositionToMatchTilePosition() {
        GetComponent<MapEvent>().SetLocation(unit.location);
    }
}