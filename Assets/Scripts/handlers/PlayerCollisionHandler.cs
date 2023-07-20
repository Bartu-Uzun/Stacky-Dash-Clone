using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{

    private bool flag = false;
    private void OnTriggerEnter(Collider other) {

        if (!GameManager.Instance.GetLevelFinished()) {

            if (other.CompareTag(TagManager.STACK_TAG)) {

                StackHandler.Instance.AddStack(other.gameObject);

                ScoreHandler.Instance.IncrementScore();

            }

            else if (other.CompareTag(TagManager.BRIDGE_TAG)) {

                StartCoroutine(EnterBridge(other, false)); // player is not on end_platform
                
            }
            else if (other.CompareTag(TagManager.END_BRIDGE_TAG)) {

                StartCoroutine(EnterBridge(other, true)); //player is on end_platform

            }
            else if (other.CompareTag(TagManager.X1_TAG)) {

                GameManager.Instance.SetTimes(1);
            }
            else if (other.CompareTag(TagManager.X2_TAG)) {

                GameManager.Instance.SetTimes(2);
            }
            else if (other.CompareTag(TagManager.X3_TAG)) {

                GameManager.Instance.SetTimes(3);
            }
            else if (other.CompareTag(TagManager.X4_TAG)) {

                GameManager.Instance.SetTimes(4);
            }
            else if (other.CompareTag(TagManager.FINISHLINE_TAG)) {

                //wait for 2 seconds before restarting maybe? maybe a chest, and then an animation would be cool
                GameManager.Instance.LevelFinished(true);


                //Debug.Log("called it!");
            }

        }

        
        
        
    }

    

    private void OnTriggerStay(Collider other) {

        if (!GameManager.Instance.GetLevelFinished()) { //if level has not finished yet

            if (other.gameObject.CompareTag(TagManager.BRIDGE_TAG)) {
                flag = other.GetComponent<BridgeHandler>().Slide();
            }
            else if (other.gameObject.CompareTag(TagManager.END_BRIDGE_TAG)) {
                flag = other.GetComponent<BridgeHandler>().Slide();
            }

            if (flag) {
            
                //wait for 2 seconds before restarting maybe? maybe a sad animation would be cool
                GameManager.Instance.LevelFinished(false);
            }

        }        
        
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.CompareTag(TagManager.BRIDGE_TAG)) {

            other.GetComponent<BridgeHandler>().OnTriggerExitBridge();
        }
    }

    private void OnCollisionEnter(Collision other) {
        
        if (other.gameObject.CompareTag(TagManager.CHEST_TAG)) {

            RigidbodyPlayerController.Instance.ChestStop();

        }
        
    }
    


    
    private IEnumerator EnterBridge(Collider other, bool enteredEndPlatform) {

        yield return new WaitForSeconds(0.05f);

        other.GetComponent<BridgeHandler>().OnEnterBridge(enteredEndPlatform);

    }
    
    
}
