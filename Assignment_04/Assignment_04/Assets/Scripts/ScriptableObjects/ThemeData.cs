using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeData", menuName = "Cards/ThemeData")]
public class ThemeData : ScriptableObject
{
    [Header("Regular Style")]
    [SerializeField] public TMP_FontAsset regularFontType;
    [SerializeField] public Color regularFontColor;
    [SerializeField] public Sprite regularButtonStyle;

    [Header("Special Style")]
    [SerializeField] public TMP_FontAsset specialFontType;
    [SerializeField] public Color specialFontColor;
    [SerializeField] public Sprite specialButtonStyle;
}