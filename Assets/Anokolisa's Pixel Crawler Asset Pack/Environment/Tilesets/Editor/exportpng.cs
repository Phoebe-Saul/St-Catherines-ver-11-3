
using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportAsepritePNG
{
    [MenuItem("Tools/Export Selected Texture as PNG")]
    static void Export()
    {
        Texture2D texture =
            Selection.activeObject as Texture2D;

        if (texture == null)
        {
            Debug.LogError("Select a texture first!");
            return;
        }

        string path = EditorUtility.SaveFilePanel(
            "Export PNG",
            "",
            texture.name + ".png",
            "png"
        );

        if (string.IsNullOrEmpty(path))
            return;

        RenderTexture rt = RenderTexture.GetTemporary(
            texture.width,
            texture.height,
            0,
            RenderTextureFormat.ARGB32
        );

        Graphics.Blit(texture, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readable = new Texture2D(
            texture.width,
            texture.height,
            TextureFormat.RGBA32,
            false
        );

        readable.ReadPixels(
            new Rect(0, 0, texture.width, texture.height),
            0, 0
        );

        readable.Apply();

        File.WriteAllBytes(path, readable.EncodeToPNG());

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        Object.DestroyImmediate(readable);

        Debug.Log("PNG exported successfully: " + path);
    }
}
