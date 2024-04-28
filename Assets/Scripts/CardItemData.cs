using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardItemData", menuName = "ScriptableObject/Create CardItemData")]
public class CardItemData : ScriptableObject
{
    [SerializeField] CardType _cardType;
    [SerializeField] Sprite _cardSprite;

    public CardType CardType => _cardType;
    public Sprite CardSprite => _cardSprite;
}

