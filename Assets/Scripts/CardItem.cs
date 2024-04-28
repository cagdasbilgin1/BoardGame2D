using UnityEngine;
using UnityEngine.UIElements;

public class CardItem
{
    VisualElement _cardBackground;
    CardItemData _itemData;
    Sprite _frontOfCard;
    Sprite _backOfCard;
    VisualElement _iconHolder;
    bool _isOpen;

    public VisualElement VisualElement => _cardBackground;
    public CardType CardType => _itemData.CardType;
    public bool IsOpen => _isOpen;

    public CardItem(VisualTreeAsset template, int sizePx, int marginPx, CardItemData itemData, Sprite backOfCard, Sprite frontOfCard)
    {
        _itemData = itemData;
        _frontOfCard = frontOfCard;
        _backOfCard = backOfCard;

        var cardItemContainer = template.Instantiate();
        _cardBackground = cardItemContainer.Q<VisualElement>("Card-Background");
        _iconHolder = cardItemContainer.Q<VisualElement>("Card-Icon-Holder");
        _cardBackground.style.width = sizePx;
        _cardBackground.style.height = sizePx;
        _cardBackground.style.backgroundImage = new StyleBackground(backOfCard);
        _cardBackground.RegisterCallback<ClickEvent>(OnCardItemClicked);

        ArrangeMargin(marginPx);

        _iconHolder.style.backgroundImage = new StyleBackground(_itemData.CardSprite);
        _iconHolder.style.display = DisplayStyle.None;

        //TestClose(); //OPEN FOR QUICK TEST
        //_iconHolder.style.display = DisplayStyle.Flex; //OPEN FOR QUICK TEST
    }

    public void UpdateCardData(CardItemData cardItemData)
    {
        _itemData = cardItemData;
        _iconHolder.style.backgroundImage = new StyleBackground(cardItemData.CardSprite);

        Close();

    }

    void ArrangeMargin(int marginPx)
    {
        _cardBackground.style.marginTop = marginPx;
        _cardBackground.style.marginBottom = marginPx;
        _cardBackground.style.marginLeft = marginPx;
        _cardBackground.style.marginRight = marginPx;
    }

    void OnCardItemClicked(ClickEvent e)
    {
        CardManager.Instance.OnCardSelected(this);
    }

    public void Open()
    {
        _isOpen = true;
        _cardBackground.style.backgroundImage = new StyleBackground(_frontOfCard);
        _iconHolder.style.display = DisplayStyle.Flex;

        //_iconHolder.style.width = 150; //OPEN FOR QUICK TEST
        //_iconHolder.style.height = 150; //OPEN FOR QUICK TEST
    }

    public void Close()
    {
        //TestClose(); //OPEN FOR QUICK TEST
        //return; //OPEN FOR QUICK TEST
        _isOpen = false;
        _cardBackground.style.backgroundImage = new StyleBackground(_backOfCard);
        _iconHolder.style.display = DisplayStyle.None;
    }

    public void TestClose() //OPEN FOR QUICK TEST
    {
        _isOpen = false;
        _cardBackground.style.backgroundImage = new StyleBackground(_backOfCard);

        _iconHolder.style.width = 80;
        _iconHolder.style.height = 80;
    }
}
