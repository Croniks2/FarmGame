using System.Collections.Generic;

using UnityEngine;
using TMPro;

using DG.Tweening;
using EventBusSystem;


public class MoneySetter : MonoBehaviour, ICoinsChangeHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform _moneyInfoContainer;
    [SerializeField] private RectTransform _coinsImage;
    [SerializeField] private List<Coin> _coins;

    private int _preivousCount = 0;

    private void OnEnable()
    {
        EventBus.Subscribe(this);

        _text.text = "0";
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public void HandleCoinsChange(int count)
    {
        Coin freeCoin = _coins.Find(c => c.gameObject.activeSelf == false );
        Sequence sec = GetSellSequence(freeCoin, count);
        sec.Play();
    }

    private Sequence GetSellSequence(Coin coin, int count)
    {
        float previousCount = _preivousCount;
        _preivousCount = count;

        Sequence moneyReceivingSeq = DOTween.Sequence().SetRecyclable(true);

        moneyReceivingSeq
            .Insert(0.1f, _moneyInfoContainer.DOShakePosition(0.15f, 5f));

        for(int i = 0; i < count - previousCount; i++)
        {
            moneyReceivingSeq
                .InsertCallback(0.1f + i * 0.01f, () => { previousCount++; _text.text = previousCount.ToString(); }).SetRecyclable(true);
        }
        
        Sequence flyCoinSeq = DOTween.Sequence().SetRecyclable(true);
        float interval = -0.25f;
        flyCoinSeq
            .InsertCallback(interval += 0.25f, () => { coin.gameObject.SetActive(true); })
            .Insert(interval += 0.25f, coin.RectTransform.DOMove(_coinsImage.position, 1.5f))
            .OnComplete(() => 
            { 
                coin.gameObject.SetActive(false);
                moneyReceivingSeq.Play();
            });
            
        return flyCoinSeq;
    }
}