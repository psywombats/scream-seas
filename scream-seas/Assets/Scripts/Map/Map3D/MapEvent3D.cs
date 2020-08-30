﻿using UnityEngine;

// todo
public class MapEvent3D : MapEvent {

    public override Vector2Int OwnWorldToTile(Vector3 pos) {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x) * OrthoDir.East.Px3DX(),
            Mathf.RoundToInt(pos.z) * OrthoDir.North.Px3DZ());
    }

    public override Vector3 OwnTileToWorld(Vector2Int position) {
        // return new Vector3(position.x, Map.terrain.HeightAt(position), position.y);
        return default;
    }

    public override Vector2Int OffsetForTiles(OrthoDir dir) {
        return dir.XY3D();
    }

    public override void SetScreenPositionToMatchTilePosition() {
        if (Map != null) {
           // transform.localPosition = new Vector3(Position.x, Map.terrain.HeightAt(Position), Position.y);
        }
    }

    public override void SetTilePositionToMatchScreenPosition() {
        SetPosition(OwnWorldToTile(transform.localPosition));
        Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
        //size = new Vector2Int(
        //    Mathf.RoundToInt(sizeDelta.x),
        //    Mathf.RoundToInt(sizeDelta.y));
    }

    public override Vector3 InternalPositionToDisplayPosition(Vector3 position) {
        return position;
    }

    public override void SetDepth() {
        // our global height is identical to the height of the parent layer
        //if (Map != null) {
        //    transform.localPosition = new Vector3(
        //        gameObject.transform.localPosition.x,
        //        Map.terrain.HeightAt(position),
        //        gameObject.transform.localPosition.z);
        //}
    }

    public override Vector3 GetHandlePosition() {
        return transform.position;
    }

    public override OrthoDir DirectionTo(Vector2Int position) {
        return OrthoDirExtensions.DirectionOf3D(position - Position);
    }

    protected override void DrawGizmoSelf() {
        if (GetComponent<Map3DHandleExists>() != null) {
            return;
        }
        //Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
        //Gizmos.DrawCube(new Vector3(
        //        transform.position.x + size.x * OrthoDir.East.Px3DX() / 2.0f,
        //        transform.position.y,
        //        transform.position.z + size.y * OrthoDir.North.Px3DZ() / 2.0f),
        //    new Vector3((size.x - 0.1f), 0.002f, (size.y - 0.1f)));
        //Gizmos.color = Color.white;
        //Gizmos.DrawWireCube(new Vector3(
        //        transform.position.x + size.x * OrthoDir.East.Px3DX() / 2.0f,
        //        transform.position.y,
        //        transform.position.z + size.y * OrthoDir.North.Px3DZ() / 2.0f),
        //    new Vector3((size.x - 0.1f), 0.002f, (size.y - 0.1f)));
    }

    protected override bool UsesSnap() {
        return false;
    }
}
