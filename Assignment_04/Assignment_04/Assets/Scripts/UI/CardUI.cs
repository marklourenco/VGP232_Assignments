using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardIconImage;

    public void Setup(CardData card, ThemeData theme)
    {
        if (card == null || theme == null)
        {
            Debug.LogError("CardUI Setup Failed: Missing Data.");
            return;
        }

        nameText.text = LocalizationManager.instance.GetText(card.nameKey);
        descriptionText.text = LocalizationManager.instance.GetText(card.descriptionKey);
        typeText.text = card.type;
        costText.text = "$" + card.cost.ToString();
        attackText.text = "ATK: " + card.atk.ToString();
        defenseText.text = "DEF: " + card.def.ToString();
        cardImage.sprite = card.image;
        cardIconImage.sprite = card.iconImage;

        ApplyTheme(theme, card.theme);
    }

    private void ApplyTheme(ThemeData theme, Theme themeChosen)
    {
        if (themeChosen == Theme.Special)
        {
            nameText.font = theme.specialFontType;
            nameText.color = theme.specialFontColor;
            descriptionText.font = theme.specialFontType;
            descriptionText.color = theme.specialFontColor;
            typeText.font = theme.specialFontType;
            typeText.color = theme.specialFontColor;
            costText.font = theme.specialFontType;
            costText.color = theme.specialFontColor; ;
            attackText.font = theme.specialFontType;
            attackText.color = theme.specialFontColor;
            defenseText.font = theme.specialFontType;
            defenseText.color = theme.specialFontColor;
            cardImage.sprite = theme.specialButtonStyle;
        }
        else
        {

            nameText.font = theme.regularFontType;
            nameText.color = theme.regularFontColor;
            descriptionText.font = theme.regularFontType;
            descriptionText.color = theme.regularFontColor;
            typeText.font = theme.regularFontType;
            typeText.color = theme.regularFontColor;
            costText.font = theme.regularFontType;
            costText.color = theme.regularFontColor; ;
            attackText.font = theme.regularFontType;
            attackText.color = theme.regularFontColor;
            defenseText.font = theme.regularFontType;
            defenseText.color = theme.regularFontColor;
            cardImage.sprite = theme.regularButtonStyle;
        }
    }
}