using UnityEngine;

public class CardListScreen : MonoBehaviour
{
    [SerializeField] private CardList cardList;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private ThemeData theme;
    [SerializeField] private CardDetailScreen detailScreen;

    private void OnEnable()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string path in cardList.cardResourcePath)
        {
            CardData card = cardList.GetCard(path);

            if (card == null)
            {
                continue;
            }

            GameObject obj = Instantiate(cardPrefab, contentParent);
            obj.GetComponent<CardUI>().Setup(card, theme);
            obj.GetComponent<CardClickHandler>().Initialize(card, detailScreen);
        }
    }
}
