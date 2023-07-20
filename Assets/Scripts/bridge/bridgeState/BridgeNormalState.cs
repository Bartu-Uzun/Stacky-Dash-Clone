using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeNormalState : BridgeBaseState
{
    //Default state of a bridge

    private float deltaAmount = 0.155f;

    private bool _isAdding = false;
    

    public override void OnTriggerExitBridge(BridgeHandler bridge)
    {}

    /* Player leaves their stacks as they move on the bridge
    ** if they do not have enough stacks to pass through the bridge, they should stop and bridge's state should change accordingly
    ** if they pass through, bridge's state should change accordingly
    ** if player cannot pass and level is over, then Slide() returns true
     */
    public override bool Slide(BridgeHandler bridge) {

        List<GameObject> bridgeStacks = bridge.GetBridgeStacks();
        int capacity = bridge.GetCapacity();

        if (bridgeStacks.Count < capacity) { //if bridge is not full


            if (StackHandler.Instance.GetStackCount() > 1 && !_isAdding) { // if player has a stack, add it to bridge 


                _isAdding = true;

                bridge.StartCoroutine(AddToBridge(bridge));

            }
            else if (StackHandler.Instance.GetStackCount() <= 1 && !_isAdding) { //if player has no stack left, player stops

                RigidbodyPlayerController.Instance.BridgeStop(); //stops player

                if (bridge.GetEnteredEndPlatform()) { // if level finished and player has no more stacks left, end the gameplay

                    return true;
                }

                else {

                    bridge.SwitchState(bridge.GetBridgeWithPlayerStoppedState()); // else, switch state of the bridge

                }

            }

        }
        else { //if bridge is full

            bridge.SwitchState(bridge.GetBridgeFinishedState());
        }

        return false;

        
    }

    // add last stack of the player to the bridge
    private IEnumerator AddToBridge(BridgeHandler bridge) {


        GameObject stack = StackHandler.Instance.RemoveStack();

        stack.transform.SetParent(bridge.gameObject.transform);

        stack.tag = TagManager.DROPPED_STACK_TAG;

        if (bridge.GetBridgeStacks().Count == 0) { // if bridge does not have any stack on it
            Vector3 currentPos = stack.transform.localPosition;
            stack.transform.localPosition = new Vector3(currentPos.x, 1, 0);
        }
        else { //there are stacks in the bridge, insert new stack according to prev stack's position
            GameObject prevStack = bridge.GetBridgeStacks()[bridge.GetBridgeStacks().Count - 1];

            stack.transform.localPosition = prevStack.transform.localPosition + deltaAmount * new Vector3(-1, 0, 0);

        }
        

        bridge.GetBridgeStacks().Add(stack);


        yield return new WaitForSeconds(0.09f);

        _isAdding = false;


    }
    
}
