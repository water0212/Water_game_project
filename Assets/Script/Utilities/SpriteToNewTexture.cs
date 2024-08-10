using UnityEngine;

public class SpriteToNewTexture : MonoBehaviour
{
    public Sprite sprite; // 在 Inspector 中拖入 Sprite

    void Start()
    {
        if (sprite != null)
        {
            Texture2D newTexture = SpriteToTexture2D(sprite);
            Debug.Log("New Texture width: " + newTexture.width + ", height: " + newTexture.height);
            byte[] pngData = newTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("Assets/NewTexture.png", pngData);

        // 刷新 Unity 编辑器
        UnityEditor.AssetDatabase.Refresh();

        // 输出日志
        Debug.Log("New texture created and saved to Assets folder.");
        }
        else
        {
            Debug.LogError("Sprite is not assigned.");
        }
        
    }

    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        // 创建一个新 Texture2D，大小与 Sprite 一致
        Texture2D newTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        // 获取 Sprite 的纹理原始像素
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height);

        // 将像素设置到新的 Texture2D
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        return newTexture;
    }
}
