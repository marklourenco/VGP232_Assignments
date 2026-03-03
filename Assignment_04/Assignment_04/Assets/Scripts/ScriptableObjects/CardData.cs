using UnityEngine;

public enum Theme
{
    Regular,
    Special
}

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    [Header("LocKeys")]
    [SerializeField] public string nameKey;
    [SerializeField] public string descriptionKey;

    [Header("Stats")]
    [SerializeField] public string type;
    [SerializeField] public int cost;
    [SerializeField] public int atk;
    [SerializeField] public int def;

    [Header("Image")]
    [SerializeField] public Sprite image;
    [SerializeField] public Sprite iconImage;

    [Header("Theme")]
    [SerializeField] public Theme theme;
}