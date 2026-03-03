using UnityEngine;

public class CardDetailScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardUI cardUI;
    [SerializeField] private ThemeData theme;

    public void Show(CardData card)
    {
        if (card == null)
        {
            Debug.LogError("CardDetailScreen.Show(CardData data) called with null card.");
            return;
        }

        gameObject.SetActive(true);
        cardUI.Setup(card, theme);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
