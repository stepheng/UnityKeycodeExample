using UnityEngine;
using System.Collections;
using System;

public class KeypadHandler : MonoBehaviour, IKeypadInterface {

    [SerializeField]
    Light failLight = null, passLight = null;

    void Awake() {
        failLight.enabled = false;
        passLight.enabled = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void KeypadSuccess(Keypad keypad) {
        passLight.enabled = true;
    }

    public void KeypadFailure(Keypad keypad) {
        failLight.enabled = true;
    }

    public void KeypadUpdated(Keypad keypad) {
        Debug.Log(keypad.KeypadValue);
        passLight.enabled = false;
        failLight.enabled = false;
    }
}
