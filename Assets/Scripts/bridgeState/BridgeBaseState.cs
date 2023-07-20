using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BridgeBaseState
{
    /* Called while player is sliding on the bridge
    ** Returns true if level is finished and player has no stacks left to slide on the bridge
    */
    public abstract bool Slide(BridgeHandler bridge);

    public abstract void OnTriggerExitBridge(BridgeHandler bridge);
    
}
