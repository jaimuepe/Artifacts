using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasking : MonoBehaviour
{
    public GameMask currentActiveMask;

    public void SwapMask(GameMask newMask)
    {
        if (currentActiveMask == null)
        {
            currentActiveMask = newMask;
            currentActiveMask.FadeIn();
        }
        else if (currentActiveMask != newMask)
        {
            currentActiveMask.FadeOut();
            currentActiveMask = newMask;
            currentActiveMask.FadeIn();
        }
    }
}
