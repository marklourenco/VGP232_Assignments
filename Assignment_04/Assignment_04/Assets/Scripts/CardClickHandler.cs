using UnityEngine;

public class CardClickHandler : MonoBehaviour
{
    [SerializeField] private CardData cardData;
    [SerializeField] private CardDetailScreen detailScreen;

    public void Initialize(CardData card, CardDetailScreen detail)
    {
        cardData = card;
        detailScreen = detail;
    }

    public void OnClick()
    {
        if (cardData == null || detailScreen == null)
        {
            Debug.LogError("CardClickHandler not initialized properly.");
            return;
        }

        detailScreen.Show(cardData);
    }
}
