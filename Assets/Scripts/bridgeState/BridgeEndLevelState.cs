using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeEndLevelState : BridgeBaseState
{

    public override bool Slide(BridgeHandler bridge) 
    {

        return false;
    }

    public override void OnTriggerExitBridge(BridgeHandler bridge)
    {}
}