using System;

using UnityEngine;

using DG.Tweening;


public class PlayerCharacterSellSystem : MonoBehaviour
{
    public event Action SellBlockEvent;
    public event Action UnloadBlockEvent;

    [SerializeField] private Transform _pointOfSale;
    [SerializeField] private PlayerBlockStack _blockStack;

    [SerializeField, Space] private float _sellJumpDuration = 2f;
    [SerializeField] private float _sellScaleDuration = 0.5f;
    [SerializeField] private float _blockSellAnimationOffset = 0.5f;
    
    public bool IsSaling {get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_blockStack.IsEmpty == false)
        {
            IsSaling = true;

            Sequence seq = DOTween.Sequence().SetRecyclable(true);

            float sellAnimationOffset = 0f - _blockSellAnimationOffset;

            foreach (var block in _blockStack.GetBlocksForSale())
            {
                sellAnimationOffset += _blockSellAnimationOffset;
                seq.Insert(sellAnimationOffset, GetSellBlockSequence(block));
            }

            seq.OnComplete(() => { IsSaling = false; });
            seq.Play();
        }
    }

    private Sequence GetSellBlockSequence(StackBlock stackBlock)
    {
        Sequence seq = DOTween.Sequence().SetRecyclable(true);

        Tween jumpTween = stackBlock.transform
            .DOJump(_pointOfSale.position, 5f, 1, _sellJumpDuration)
            .SetRecyclable(true);

        Tween scaleTween = stackBlock.transform
            .DOScale(Vector3.zero, _sellScaleDuration)
            .SetRecyclable(true);

        seq
            .Append(jumpTween)
            .Insert(_sellJumpDuration * 0.9f, scaleTween)
            .OnComplete(() => 
            {
                stackBlock.Deactivate();
                SellBlockEvent?.Invoke();
            });

        return seq;
    }
}