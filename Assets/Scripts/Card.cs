using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Input Data
    public RectTransform myRect;
    public Image cardImage;

    public Sprite cardBackSprite;

    // Local Data
    public int myIndex;
    public int myType;
    public bool isLookAtFront;

    private Sprite cardFrontSprite;

    private CardManager myCardManager;

    public void Init(int type, Sprite sprite, CardManager cardManager)
    {
        isLookAtFront = false;

        RotateCard(false);

        cardFrontSprite = sprite;
        myType = type;
        myCardManager = cardManager;
    }

    public void RotateCard(bool isFront)
    {
        if (isFront) cardImage.sprite = cardFrontSprite;
        else cardImage.sprite = cardBackSprite;

        isLookAtFront = isFront;
    }

    public void OnPressedCard()
    {
        // 현재 카드를 누를 수 있는 상황인가?
        if (myCardManager.OnPressedCard(this))
        {
            RotateCard(true);
        }
    }
}