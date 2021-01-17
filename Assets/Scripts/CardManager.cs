using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    // Input Data
    public Vector2 size;

    public Transform cardParent;

    public List<Sprite> cardSprites;

    // Prefab
    public Card cardObject;

    // Local Data
    private bool isStop;

    private Card curSelectCard;

    private List<Card> cardList;
    private List<Card> removeCardList;

    // ���α׷� ���� �� �Ҹ��� �Լ�
    public void PreInit()
    {
        cardList = new List<Card>();
        removeCardList = new List<Card>();
    }

    // ���� ������ �� ���� �Ҹ��� �Լ�
    public void Init()
    {
        isStop = false;

        // ���� ��ü ũ���� x�� y�� ������ �� Ȧ���� ���� ��쿡��
        // ī�� ������ Ȧ���̱� ������ ¦���� ������
        if (size.x * size.y % 2 != 0) ++size.y;

        int cardTypeCount = (int)(size.x * size.y * 0.5f);

        float offsetX = size.x * 200 * 0.5f;
        float offsetY = size.y * 300 * 0.5f;

        ShuffleCardSprites();

        for (int y = 0; y < size.y; ++y)
        {
            for (int x = 0; x < size.x; ++x)
            {
                // ī�� ����
                var card = Instantiate(cardObject, cardParent);

                // ī�� ��ġ
                card.myRect.localPosition =
                    new Vector2(x * 200 - offsetX, y * -300 + offsetY);

                // ����Ʈ�� ī�� �߰�
                cardList.Add(card);
            }
        }

        // ���� ����Ʈ ����
        List<Card> tempCardList = new List<Card>(cardList);

        // ī�� �ʱ�ȭ
        for (int i = 0; i < cardTypeCount; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                int randomIndex = Random.Range(0, tempCardList.Count);
                Card card = tempCardList[randomIndex];

                card.Init(i, cardSprites[i], this);
                tempCardList.RemoveAt(randomIndex);
            }
        }

    }

    // ���� ������ �� ���� �Ҹ��� �Լ�
    public void Release()
    {
        for (int i = 0; i < cardList.Count; ++i)
            Destroy(cardList[i].gameObject);
        cardList.Clear();

        for (int i = 0; i < removeCardList.Count; ++i)
            Destroy(removeCardList[i].gameObject);
        removeCardList.Clear();
    }

    public bool OnPressedCard(Card card)
    {
        if (isStop) return false; // �ð��� �������� ��, �������� ����
        if (card.isLookAtFront) return false; // �̹� �������� ī���� ��쿡�� ����

        // �̹� ���õ� ī�尡 ���� ��
        if (curSelectCard == null)
        {
            // ���� ������ ī�带 ����
            curSelectCard = card;

            return true;
        }

        // �̹� ���õ� ī�尡 ���� ���
        else
        {
            // �����س��� ī��� ������ ī�尡 ���� ���
            if (curSelectCard.myType == card.myType)
            {
                removeCardList.Add(curSelectCard);
                removeCardList.Add(card);

                Invoke("RemoveCard", 0.5f);
            }


            // �ٸ� ���
            else
            {
                Invoke("RotateCard", 0.5f);
            }

            // �ð� ���߰�
            isStop = true;

            // ����� ī�� null�� �ʱ�ȭ
            curSelectCard = null;

            return true;
        }
    }

    private void RemoveCard()
    {
        // ī�� �����
        foreach (var card in removeCardList)
        {
            cardList.Remove(card);

            Destroy(card.gameObject);
        }

        removeCardList.Clear();

        // ���� �ٽ� ����
        isStop = false;

        if (cardList.Count <= 0)
            GameManager.Instance.OnGameEnd();
    }

    private void RotateCard()
    {
        // ī�� ������ �ֱ�
        foreach (var card in cardList)
        {
            card.RotateCard(false);
        }


        // ���� �ٽ� ����
        isStop = false;
    }

    public void ShuffleCardSprites() // ī�� ��������Ʈ ���� �����ִ� �Լ�
    {
        int random1;
        int random2;

        for (int index = 0; index < cardSprites.Count; ++index)
        {
            random1 = Random.Range(0, cardSprites.Count);
            random2 = Random.Range(0, cardSprites.Count);

            var tmp = cardSprites[random1];
            cardSprites[random1] = cardSprites[random2];
            cardSprites[random2] = tmp;
        }
    } 
}