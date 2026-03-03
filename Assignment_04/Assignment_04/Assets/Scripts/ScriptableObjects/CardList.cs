using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardList", menuName = "Cards/CardList")]
public class CardList : ScriptableObject
{
    [SerializeField] public List<string> cardResourcePath = new List<string>();

    [SerializeField] private Dictionary<string, CardData> loadedCards = new Dictionary<string, CardData>();

    public CardData GetCard(string resourcePath)
    {
        if (string.IsNullOrEmpty(resourcePath))
        {
            Debug.LogError("Card resource path is null or empty.");
            return null;
        }

        if (loadedCards.ContainsKey(resourcePath))
        {
            return loadedCards[resourcePath];
        }

        CardData card = Resources.Load<CardData>(resourcePath);

        if (card == null)
        {
            Debug.LogError($"Card not found at path: {resourcePath}");
            return null;
        }

        loadedCards.Add(resourcePath, card);
        return card;
    }
}