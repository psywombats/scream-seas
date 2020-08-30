using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The generic stuff that used to be attached to the player party, such as location, gp, etc.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class GameData {

    private const int InventoryCapacity = 10;

    [JsonProperty] public int GP { get; private set; }
    [JsonProperty] public string LocationName { get; private set; }
    [JsonProperty] public string CurrentBGMKey { get; private set; }

    [JsonProperty] public string MapPath { get; set; }
    [JsonProperty] public Vector2Int MapLocation { get; set; }

    [JsonProperty] public Dictionary<string, int> Variables { get; private set; }
    [JsonProperty] public Dictionary<string, bool> Switches { get; private set; }

    // meta info
    [JsonProperty] public int SaveVersion { get; set; }
    [JsonProperty] public long SavedAt { get; set; }

    public int SwitchLastUpdatedFrame { get; set; } = 0;

    public void AddGP(int gp) { GP += gp; }
    public void DeductGP(int gp) { GP -= gp; }

    public GameData() {
        Variables = new Dictionary<string, int>();
        Switches = new Dictionary<string, bool>();
        SwitchLastUpdatedFrame = Time.frameCount;
    }

    public void OnTeleportTo(Map map) {
        if (map.MapName != null && map.MapName.Length > 0) LocationName = map.MapName;
        CurrentBGMKey = map.BgmKey;
    }

    public bool GetSwitch(string switchName) {
        if (!Switches.ContainsKey(switchName)) {
            return false;
        }
        return Switches[switchName];
    }

    public void SetSwitch(string switchName, bool value) {
        Switches[switchName] = value;
        SwitchLastUpdatedFrame = Time.frameCount;
    }

    public int GetVariable(string variableName) {
        if (!Variables.ContainsKey(variableName)) {
            Variables[variableName] = 0;
        }
        return Variables[variableName];
    }

    public void IncrementVariable(string variableName) {
        Variables[variableName] = GetVariable(variableName) + 1;
    }

    public void DecrementVariable(string variableName) {
        Variables[variableName] = GetVariable(variableName) - 1;
    }

    public void SetVariable(string variableName, int value) {
        Variables[variableName] = value;
    }
}
