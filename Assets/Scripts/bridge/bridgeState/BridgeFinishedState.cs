using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeFinishedState : BridgeBaseState
{
    /* if bridge is full, this state is called
    ** this state does act like a normal path
    */
    public override bool Slide(BridgeHandler bridge) 
    {
        return false;
    }

    public override void OnTriggerExitBridge(BridgeHandler bridge)
    {}
}
