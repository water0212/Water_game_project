using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName ="GameScene/SceneRefenceSO")]
public class SceneRefenceSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneRefence;
}
