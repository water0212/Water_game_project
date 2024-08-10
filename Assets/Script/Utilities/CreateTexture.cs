using UnityEngine;

public class CreateTexture : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    void Start()
    {
        // 创建一个新的纹理
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // 填充纹理颜色（示例：设置为红色）
        Color fillColor = Color.red;
        Color[] colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = fillColor;
        }
        texture.SetPixels(colors);
        texture.Apply();

        // 将纹理保存为文件
        byte[] pngData = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes("Assets/NewTexture.png", pngData);

        // 刷新 Unity 编辑器
        UnityEditor.AssetDatabase.Refresh();

        // 输出日志
        Debug.Log("New texture created and saved to Assets folder.");
    }
}
