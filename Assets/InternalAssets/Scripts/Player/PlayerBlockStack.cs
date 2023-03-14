using System.Collections.Generic;

using UnityEngine;


public class PlayerBlockStack : MonoBehaviour
{
    [SerializeField] private List<StackBlock> _stackBlocks;

    public bool IsFull => _freeSpaceID >= _stackBlocks.Count - 1;
    public bool IsEmpty => _freeSpaceID < 0;

    private int _freeSpaceID = -1;

    private void Awake()
    {
        for (int i = 0; i < _stackBlocks.Count; i++)
        {
            _stackBlocks[i].Setup(i, this);
        }
    }

    public bool TryToAddBlock()
    {
        if(IsFull == false)
        {
            _freeSpaceID++;
            
            StackBlock stackBlock = _stackBlocks[_freeSpaceID];
            stackBlock.Activate();
            
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void ReturnBlock(int id)
    {
        _freeSpaceID--;
    }

    public IEnumerable<StackBlock> GetBlocksForSale()
    {
        for(int i = _stackBlocks.Count - 1; i >= 0; i--)
        {
            if (_stackBlocks[i].IsActive == true)
            {
                yield return _stackBlocks[i];
            }
        }
    }
}