using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour
{
    
    public static StackHandler Instance;

    [SerializeField] private List<GameObject> _stacks = new List<GameObject>();
    [SerializeField] private GameObject _character;
    [SerializeField] private GameObject _stackGarbage;

    private float _deltaY = 0.5f;

    private void Awake() {
        
        if (Instance != null) {

            Destroy(this);
        }
        else {
            Instance = this;
        }

    }

    public int GetStackCount() {

        return _stacks.Count;
    }

    //removes the last stack from the stack list
    public void RemoveStackFromStackList() {

        GameObject stack = _stacks[_stacks.Count - 1];

        _stacks.Remove(stack);

        _character.transform.localPosition += Vector3.down * _deltaY; // move player down a bit

        stack.transform.SetParent(_stackGarbage.transform);

        stack.SetActive(false);
    }

    // adds an input stack to stack list
    // updates input stack's position and parent according to the last stack of the stack list
    public void AddStack(GameObject other) {

        GameObject lastStack = _stacks[_stacks.Count - 1];

        other.transform.SetParent(lastStack.transform.parent);

        other.transform.localPosition = lastStack.transform.localPosition + Vector3.up * _deltaY;

        _stacks.Add(other);

        _character.transform.localPosition += Vector3.up * _deltaY; // move player up a bit

    }
}
