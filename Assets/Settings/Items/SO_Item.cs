using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable Objects/SO_Item")]
public class SO_Item : ScriptableObject
{
    public LocalizedString localizedName;
    public LocalizedString localizedDescription;
    public Sprite sprite;
}
