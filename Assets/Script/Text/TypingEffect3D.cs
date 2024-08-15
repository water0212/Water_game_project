using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // 使用 TextMeshPro

public class TypingEffect3D : MonoBehaviour
{
    private string text;
    public TextMeshPro textMeshPro; // 使用 TextMeshPro
    public float typingSpeed = 0.05f; // 打字速度

    private void Start()
    {
        text = textMeshPro.text;
        
    }
    public void PlayText(){
       StartCoroutine(TypeSentence(text)); 
    }

    IEnumerator TypeSentence(string sentence)
    {
        textMeshPro.text = ""; // 清空現有文本
        foreach (char letter in sentence.ToCharArray())
        {
            textMeshPro.text += letter; // 添加字符
            yield return new WaitForSeconds(typingSpeed); // 等待
        }
    }
}