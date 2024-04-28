using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    static CardManager _instance;
    CardManager() { }
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardManager();
            }
            return _instance;
        }
    }

    [SerializeField] PlayerManager playerManager;
    [SerializeField] UIDocument uiDocument;
    [SerializeField] CardSpritesData cardSpritesData;
    [SerializeField] VisualTreeAsset cardItemTemplate;
    [SerializeField] List<CardItemData> cardItemDatas;

    VisualElement gameplayPanel;
    List<CardItem> _currentCardItems;
    CardItem _selectedCard;
    bool _isClickEnabled = true;
    int _currentSize;
    int _openedCardCount;
    int _totalCardCount => _currentSize * _currentSize;


    public event Action OnAllCardsOpened;
    public event Action OnRoundStarted;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        var root = uiDocument.rootVisualElement;
        gameplayPanel = root.Q<VisualElement>("Game-Play-Area");
    }

    void ArrangeGamePanel(int size, out int cardItemSizePx, out int cardItemMarginPx)
    {
        var MaxSquareWidth = Screen.width;
        var MaxSquareHeight = Screen.height * .55f; // game panel has 55 percent height
        var gamePanelSizeSquarePx = (int)Mathf.Min(MaxSquareWidth, MaxSquareHeight);
        gameplayPanel.style.width = gamePanelSizeSquarePx;
        gameplayPanel.style.height = gamePanelSizeSquarePx;
        cardItemSizePx = (int)(gamePanelSizeSquarePx * 0.9 / size);
        var pixelSpacePerCard = (gamePanelSizeSquarePx - (size * cardItemSizePx)) / size;
        cardItemMarginPx = pixelSpacePerCard / 2;
    }

    List<CardItemData> DoubleItems(List<CardItemData> items){
        return items.Concat(items).ToList();
    }

    public void CreateCards(int size)
    {
        if(_currentCardItems != null && _currentCardItems.Count == size * size) // Cards already created go pooling
        {
            playerManager.ResetPlayers();
            GoNextRound();
            return;
        }

        if(_currentCardItems != null)
        {
            foreach(var item in _currentCardItems)
            {
                _openedCardCount = 0;
                playerManager.ResetPlayers();
                gameplayPanel.Remove(item.VisualElement);
            }
        }

        UpdateRound();
        ClearSelectedCard();
        OnRoundStarted?.Invoke();

        _currentCardItems = new();
        _currentSize = size;
        var selectedCardItems = DoubleItems(SelectUniqueItems(cardItemDatas, _totalCardCount / 2));
        var shuffledSelectedCardItems = selectedCardItems.OrderBy(x => Random.value).ToList();

        int cardItemSizePx;
        int cardItemMargin;
        ArrangeGamePanel(size, out cardItemSizePx, out cardItemMargin);

        foreach (var itemData in shuffledSelectedCardItems)
        {
            var cardItem = new CardItem(cardItemTemplate, cardItemSizePx, cardItemMargin, itemData, cardSpritesData.CardBack, cardSpritesData.CardFront);
            gameplayPanel.Add(cardItem.VisualElement);

            _currentCardItems.Add(cardItem);
        }
    }

    //using pool system
    public void GoNextRound()
    {
        UpdateRound();
        ClearSelectedCard();
        OnRoundStarted?.Invoke();

        _openedCardCount = 0;

        var selectedCardItemsThisRound = DoubleItems(SelectUniqueItems(cardItemDatas, _totalCardCount / 2));
        var shuffledSelectedCardItemsThisRound = selectedCardItemsThisRound.OrderBy(x => Random.value).ToList();

        for (var i = 0; i< shuffledSelectedCardItemsThisRound.Count; i++)
        {
            _currentCardItems[i].UpdateCardData(shuffledSelectedCardItemsThisRound[i]);
        }
    }

    void UpdateRound()
    {
        playerManager.PrepareForRound();
    }

    public void OnCardSelected(CardItem cardItem)
    {
        if (!_isClickEnabled || cardItem.IsOpen) return;

        cardItem.Open();

        if(_selectedCard == null)
        {
            _selectedCard = cardItem;
        }
        else
        {
            CheckMatch(_selectedCard, cardItem);

            _selectedCard = null;
        }
    }

    void CheckMatch(CardItem cardItem1, CardItem cardItem2)
    {
        if(cardItem1.CardType == cardItem2.CardType)
        {
            _openedCardCount += 2;

            DisableClickForSecond(1f);

            if (playerManager.IsPlayer1Turn())
            {
                playerManager.IncreasePlayer1Point(1);
            }
            else
            {
                playerManager.IncreasePlayer2Point(1);
            }

            if (AreAllCardsOpen())
            {
                OnAllCardsOpened?.Invoke();
            }
        }
        else
        {
            //no match
            StartCoroutine(CloseCardItemsWithDelay(cardItem1, cardItem2, 1f));
        }
    }

    IEnumerator DisableClickForSecond(float seconds)
    {
        DisableAllCardsClickable();
        yield return new WaitForSeconds(seconds);
        EnableAllCardsClickable();
    }

    IEnumerator CloseCardItemsWithDelay(CardItem cardItem1, CardItem cardItem2, float delay)
    {
        _isClickEnabled = false;
        yield return new WaitForSeconds(delay);
        cardItem1.Close();
        cardItem2.Close();
        EnableAllCardsClickable();
        playerManager.NextPlayersTurn();
    }

    bool AreAllCardsOpen()
    {
        return _openedCardCount == _totalCardCount;
    }

    private List<CardItemData> SelectUniqueItems(List<CardItemData> items, int count)
    {
        List<CardItemData> shuffledItems = items.OrderBy(x => Random.value).ToList();
        return shuffledItems.Take(count).ToList();
    }

    public void DisableAllCardsClickable()
    {
        _isClickEnabled = false;
    }

    public void EnableAllCardsClickable()
    {
        _isClickEnabled = true;
    }

    public bool IsClickEnabled()
    {
        return _isClickEnabled;
    }

    void ClearSelectedCard()
    {
        _selectedCard = null;
    }
}
