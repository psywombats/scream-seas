using UnityEngine;
using SuperTiled2Unity.Editor;
using SuperTiled2Unity;
using UnityEditor;
using UnityEngine.Tilemaps;

[AutoCustomTmxImporter]
public class TmxImporter : CustomTmxImporter {

    private const string DollPrefabPath = "Assets/Resources/Prefabs/Doll.prefab";
    private const string CeilingPrefabPath = "Assets/Resources/Prefabs/Ceiling.prefab";
    private const string ChestPrefabPath = "Assets/Resources/Prefabs/Chest.prefab";
    private const string LightPrefabPath = "Assets/Resources/Prefabs/Lights/";
    
    private const string TypeEncounterArea = "Encounter";

    public override void TmxAssetImported(TmxAssetImportedArgs args) {
        var map = args.ImportedSuperMap;
        var tsxMap = map.gameObject.AddComponent<TsxMap>();
        tsxMap.grid = map.gameObject.GetComponentInChildren<Grid>();
        var objectLayer = map.gameObject.GetComponentInChildren<SuperObjectLayer>();
        if (objectLayer == null) return;
        tsxMap.objectLayer = objectLayer.gameObject.AddComponent<ObjectLayer>();

        //foreach (var layer in tsxMap.layers) {
        //    layer.GetComponent<TilemapRenderer>().material = materials.BackgroundMaterial;
        //}

        foreach (var layer in tsxMap.layers) {
            var renderer = layer.GetComponent<TilemapRenderer>();
            renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/Materials/Sprite2D.mat");
            var super = layer.GetComponent<SuperCustomProperties>();
            var z = 0.0f;
            if (super.TryGetCustomProperty("z", out CustomProperty prop)) {
                z = prop.GetValueAsFloat();
            }
            layer.transform.localPosition = new Vector3(
                layer.transform.localPosition.x,
                layer.transform.localPosition.y,
                z);
        }

        foreach (Transform child in objectLayer.transform) {
            if (child.GetComponent<SuperObject>() != null) {
                var tmxObject = child.GetComponent<SuperObject>();
                child.gameObject.AddComponent<MapEvent2D>();
                var mapEvent = child.gameObject.GetComponent<MapEvent2D>();
                mapEvent.Size = new Vector2Int((int)tmxObject.m_Width / Map.PxPerTile, (int)tmxObject.m_Height / Map.PxPerTile);
                mapEvent.Properties = tmxObject.GetComponent<SuperCustomProperties>();
                mapEvent.Position = new Vector2Int((int)tmxObject.m_X / Map.PxPerTile, (int)tmxObject.m_Y / Map.PxPerTile);

                var lighting = mapEvent.GetProperty("lighting");
                if (lighting != null && lighting.Length > 0) {
                    GameObject lightPrefab;
                    if (mapEvent.GetComponent<LightingComponent>() == null) {
                        mapEvent.gameObject.AddComponent<LightingComponent>();
                        lightPrefab = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(LightPrefabPath + lighting + ".prefab"));
                    } else {
                        lightPrefab = mapEvent.GetComponent<LightingComponent>().light.gameObject;
                    }
                    var light = mapEvent.GetComponent<LightingComponent>();
                    lightPrefab.transform.SetParent(mapEvent.transform);
                    light.light = lightPrefab.GetComponent<LightPrefabComponent>();
                    lightPrefab.transform.localPosition = new Vector3(mapEvent.Size.x / 2, mapEvent.Size.y / 2);
                }

                var appearance = mapEvent.GetProperty(MapEvent.PropertyAppearance);
                if (appearance != null && appearance.Length > 0) {
                    CharaEvent chara;
                    Doll doll;
                    if (mapEvent.GetComponent<FieldSpritesheetComponent>() == null) {
                        mapEvent.gameObject.AddComponent<FieldSpritesheetComponent>();
                    }
                    if (mapEvent.GetComponent<CharaEvent>() == null) {
                        chara = mapEvent.gameObject.AddComponent<CharaEvent>();
                        var dollObject = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(DollPrefabPath));
                        doll = dollObject.GetComponent<Doll>();
                        // doll.Renderer.material = materials.ForegroundMaterial;
                        doll.transform.SetParent(mapEvent.transform);
                        chara.Doll = doll;
                        if (mapEvent.GetProperty("proximity") != null && mapEvent.GetProperty("proximity").Length > 0) {
                            if (doll.Renderer.GetComponent<AlphaProximityComponent>() == null) {
                                doll.Renderer.gameObject.AddComponent<AlphaProximityComponent>();
                            }
                            var prox = mapEvent.GetProperty("proximity").ToFloat();
                            var alphaComponent = doll.Renderer.GetComponent<AlphaProximityComponent>();
                            alphaComponent.minDist = prox;
                            alphaComponent.maxDist = prox + 0.25f;
                            alphaComponent.minAlpha = 1.0f;
                            alphaComponent.maxAlpha = 0.0f;
                            alphaComponent.anchor = mapEvent;
                        }
                    } else {
                        chara = mapEvent.GetComponent<CharaEvent>();
                        doll = chara.Doll;
                    }

                    chara.alwaysAnimates = tmxObject.m_Type == "Event";
                    doll.transform.localPosition = Vector3.zero;
                    doll.SetRenderer(tmxObject.m_Type == "Event");

                    if (IndexDatabase.Instance().FieldSprites.GetDataOrNull(appearance) != null) {
                        // it's a literal
                        chara.SetAppearanceByTag(appearance);
                    } else {
                        // this should be okay... it's a lua string
                    }

                    var facing = mapEvent.GetProperty("face");
                    if (facing != null && facing.Length > 0) {
                        chara.Facing = OrthoDirExtensions.Parse(facing);
                    }
                }
            }
        }
    }
}
