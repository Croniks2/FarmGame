using System;

using UnityEngine;

public class PlayerCharacterCollectSystem : MonoBehaviour
{
    public event Action NewGrassBlockReceivedEvent;

    [SerializeField] private PlayerBlockStack _blockStack;

    private void OnTriggerEnter(Collider other)
    {
        if(_blockStack.IsFull == false)
        {
            bool success = _blockStack.TryToAddBlock();

            if(success == true)
            {
                NewGrassBlockReceivedEvent?.Invoke();

                Transform otherTransform = other.transform;
                otherTransform.GetComponent<FieldBlock>().Collect();
                otherTransform.gameObject.SetActive(false);
            } 
        }
    }
}