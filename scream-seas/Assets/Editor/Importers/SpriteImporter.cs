﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

internal sealed class SpriteImporter : AssetPostprocessor {

    public void OnPreprocessTexture() {
        string path = assetPath;
        string name = EditorUtils.NameFromPath(path);

        if (name.Contains("Placeholder")) {
            return;
        }

        if (path.Contains("Sprites") || path.Contains("UI") || path.Contains("Maps")) {
            TextureImporter importer = (TextureImporter)assetImporter;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.textureType = TextureImporterType.Sprite;
            Vector2Int textureSize = EditorUtils.GetPreprocessedImageSize(importer);
            if (path.Contains("Charas")) {
                int edgeSizeX = 16;
                if (textureSize.x == 72) edgeSizeX = 24;
                int edgeSizeY = 32;
                if (textureSize.y == 16) edgeSizeY = 16;
                int cols = textureSize.x / edgeSizeX;
                int rows = textureSize.y / edgeSizeY;
                importer.spritePixelsPerUnit = Map.PxPerTile;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritesheet = new SpriteMetaData[rows * cols];
                List<SpriteMetaData> spritesheet = new List<SpriteMetaData>();
                for (int y = 0; y < rows; y += 1) {
                    for (int x = 0; x < cols; x += 1) {
                        SpriteMetaData data = importer.spritesheet[y * cols + x];
                        data.rect = new Rect(x * edgeSizeX, (rows - y - 1) * edgeSizeY, edgeSizeX, edgeSizeY);
                        data.alignment = (int)SpriteAlignment.Custom;
                        data.border = new Vector4(0, 0, 0, 0);
                        data.name = FieldSpritesheetComponent.NameForFrame(name, x, y);
                        data.pivot = new Vector2(0.5f, 0.0f);
                        spritesheet.Add(data);
                    }
                }
                importer.spritesheet = spritesheet.ToArray();
            }
            if (path.Contains("Maps")) {
                importer.spritePixelsPerUnit = Map.PxPerTile;
            }
        }
    }
}
