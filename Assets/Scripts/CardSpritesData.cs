using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSprites", menuName = "ScriptableObject/Create CardSpritesData")]
public class CardSpritesData : ScriptableObject
{
    [SerializeField] Sprite _cardBack;
    [SerializeField] Sprite _cardFront;
    //[SerializeField] List<Sprite> _cardSprites;
    //[SerializeField] List<CardItemData> _cardItemDatas;

    //[SerializeField] List<CardItemData> _cardsItemDatas;

    public Sprite CardBack => _cardBack;
    public Sprite CardFront => _cardFront;
    //public List<Sprite> CardSprites => _cardSprites;
    //public List<CardItemData> CardItemDatas => _cardItemDatas;
}

