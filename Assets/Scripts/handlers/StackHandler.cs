using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour
{
    
    public static StackHandler Instance;

    [SerializeField] private List<GameObject> _stacks = new List<GameObject>();
    [SerializeField] private GameObject _character;

    private float _deltaY = 0.5f;

    private void Awake() {
        
        if (Instance != null) {

            Destroy(this);
        }
        else {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject); //NEED TO TRY

    }

    public int GetStackCount() {

        return _stacks.Count;
    }

    public GameObject RemoveStack() {

        GameObject stack = _stacks[_stacks.Count - 1];

        _stacks.Remove(stack);

        _character.transform.localPosition += Vector3.down * _deltaY;

        return stack;
    }

    public void AddStack(GameObject other) {

        GameObject lastStack = _stacks[_stacks.Count - 1];

        other.transform.SetParent(lastStack.transform.parent);

        other.transform.localPosition = lastStack.transform.localPosition + Vector3.up * _deltaY;

        _stacks.Add(other);

        _character.transform.localPosition += Vector3.up * _deltaY;

    }
}
