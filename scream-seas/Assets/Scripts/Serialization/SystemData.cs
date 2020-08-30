using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class SystemData {

    [JsonProperty] public int LastSaveSlot { get; set; } = -1;

    [JsonProperty] public SaveInfoData[] SaveInfo { get; private set; } = new SaveInfoData[SerializationManager.SaveSlotCount];

    [JsonProperty] public Setting<bool> SettingFullScreen { get; private set; } =               new Setting<bool>(false);
    [JsonProperty] public Setting<float> SettingMusicVolume { get; private set; } =             new Setting<float>(0.9f);
    [JsonProperty] public Setting<float> SettingSoundEffectVolume { get; private set; } =       new Setting<float>(0.9f);
}
