using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeWithPlayerStoppedState : BridgeBaseState
{
    //the state in which player has stopped on a not full bridge

    /* When player leaves the bridge, last stack on the bridge will be marked. 
    ** Player can slide until the marked stack as they wish, but they need stacks to slide past the marked stack
    ** if player slides past the marked stack, it will be unmarked and the bridge state will be changed accordingly
    */
    public override void OnTriggerExitBridge(BridgeHandler bridge) {

        if (bridge.GetBridgeStacks().Count == 0) { // to ensure that our player did not collide with an empty bridge by accident

            bridge.SwitchState(bridge.GetBridgeNormalState());
        }

        else { // give LastBridgeStack script to the last stack of the bridge

            GameObject lastBridgeStack = bridge.GetBridgeStacks()[bridge.GetBridgeStacks().Count -1];

            lastBridgeStack.AddComponent<LastBridgeStack>();
            lastBridgeStack.tag = TagManager.LAST_DROPPED_STACK_TAG;

        }

        bridge.OnExitBridge();

    }
   
   public override bool Slide(BridgeHandler bridge) {

        return false;
   }
}
