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

    // 프로그램 실행 시 불리는 함수
    public void PreInit()
    {
        cardList = new List<Card>();
        removeCardList = new List<Card>();
    }

    // 게임 시작할 때 마다 불리는 함수
    public void Init()
    {
        isStop = false;

        // 맵의 전체 크기의 x와 y를 곱했을 때 홀수가 나올 경우에는
        // 카드 개수가 홀수이기 때문에 짝수로 맞춰줌
        if (size.x * size.y % 2 != 0) ++size.y;

        int cardTypeCount = (int)(size.x * size.y * 0.5f);

        float offsetX = size.x * 200 * 0.5f;
        float offsetY = size.y * 300 * 0.5f;

        ShuffleCardSprites();

        for (int y = 0; y < size.y; ++y)
        {
            for (int x = 0; x < size.x; ++x)
            {
                // 카드 생성
                var card = Instantiate(cardObject, cardParent);

                // 카드 배치
                card.myRect.localPosition =
                    new Vector2(x * 200 - offsetX, y * -300 + offsetY);

                // 리스트에 카드 추가
                cardList.Add(card);
            }
        }

        // 복사 리스트 생성
        List<Card> tempCardList = new List<Card>(cardList);

        // 카드 초기화
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

    // 게임 끝났을 때 마다 불리는 함수
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
        if (isStop) return false; // 시간이 멈춰있을 때, 못누르게 막기
        if (card.isLookAtFront) return false; // 이미 뒤집혀진 카드일 경우에는 막기

        // 이미 선택된 카드가 없을 때
        if (curSelectCard == null)
        {
            // 현재 선택한 카드를 저장
            curSelectCard = card;

            return true;
        }

        // 이미 선택된 카드가 없을 경우
        else
        {
            // 저장해놓은 카드와 선택한 카드가 같을 경우
            if (curSelectCard.myType == card.myType)
            {
                removeCardList.Add(curSelectCard);
                removeCardList.Add(card);

                Invoke("RemoveCard", 0.5f);
            }


            // 다를 경우
            else
            {
                Invoke("RotateCard", 0.5f);
            }

            // 시간 멈추고
            isStop = true;

            // 저장된 카드 null로 초기화
            curSelectCard = null;

            return true;
        }
    }

    private void RemoveCard()
    {
        // 카드 지우기
        foreach (var card in removeCardList)
        {
            cardList.Remove(card);

            Destroy(card.gameObject);
        }

        removeCardList.Clear();

        // 게임 다시 진행
        isStop = false;

        if (cardList.Count <= 0)
            GameManager.Instance.OnGameEnd();
    }

    private void RotateCard()
    {
        // 카드 뒤집어 주기
        foreach (var card in cardList)
        {
            card.RotateCard(false);
        }


        // 게임 다시 진행
        isStop = false;
    }

    public void ShuffleCardSprites() // 카드 스프라이트 순서 섞어주는 함수
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