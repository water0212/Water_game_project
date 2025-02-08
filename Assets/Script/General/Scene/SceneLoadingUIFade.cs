using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneLoadingUIFade : MonoBehaviour
{
    [Header("物件")]
    public Image blackBackGround;
    public Image LoadImage;
    public IEnumerator blackFadeIn(float duration)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            blackBackGround.color = new Color(0, 0, 0, t / duration);
            yield return null;
        }
            StartCoroutine(FadeIn(LoadImage, 2f));
    }
    public IEnumerator FadeIn(Image fadeImage,float duration)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, t / duration);
            yield return null;
        }
    }
    public IEnumerator FadeOut(float duration)
    {
        for (float t = duration; t > 0; t -= Time.deltaTime)
        {
            blackBackGround.color = new Color(blackBackGround.color.r, blackBackGround.color.g, blackBackGround.color.b, t / duration);
            LoadImage.color = new Color(LoadImage.color.r, LoadImage.color.g, LoadImage.color.b, t / duration);
            yield return null;
        }
        blackBackGround.color = new Color(blackBackGround.color.r, blackBackGround.color.g, blackBackGround.color.b, 0);
        LoadImage.color = new Color(LoadImage.color.r, LoadImage.color.g, LoadImage.color.b, 0);
    }
}
