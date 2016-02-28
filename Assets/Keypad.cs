using UnityEngine;
using UnityEngine.UI;
using System.Collections;

internal interface IKeypadInterface {
    void KeypadSuccess(Keypad keypad);
    void KeypadFailure(Keypad keypad);
    void KeypadUpdated(Keypad keypad);
}

[RequireComponent(typeof(Canvas))]
public class Keypad : MonoBehaviour {

    Canvas parent;

    [SerializeField]
    Button buttonPrefab = null;

    [SerializeField]
    int columnCount = 3;

    [SerializeField]
    string[] buttonValues = { "X", "0", "OK", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    [SerializeField]
    int submitIndex = 2;

    [SerializeField]
    int cancelIndex = 0;

    [SerializeField]
    string keycode = "1234";

    [SerializeField]
    GameObject keypadHandler;
    IKeypadInterface keypadInterface;

    string internalKeypadValue = "";
    public string KeypadValue {
        get { return internalKeypadValue; }
    }

    // Use this for initialization
    void Start() {
        parent = GetComponent<Canvas>();
        keypadInterface = keypadHandler.GetComponent<IKeypadInterface>();

        BuildKeypad();
    }

    private void BuildKeypad() {
        // Get canvas size and correct for scaling
        RectTransform rectTransform = parent.GetComponent<RectTransform>();
        float parentWidth = rectTransform.rect.width * parent.scaleFactor;
        float parentHeight = rectTransform.rect.height * parent.scaleFactor;

        // Calculate button size
        float buttonSize = parentWidth / columnCount;
        // Minimum x position
        float buttonXStart = -(parentWidth - buttonSize) / 2;
        // Minimum y position
        float buttonYStart = -(parentHeight - buttonSize) / 2;
        Vector3 buttonPos = new Vector3(buttonXStart, buttonYStart, 0);
        for (int i = 0; i < buttonValues.Length; i++) {
            // Create new button from prefab
            Button button = Instantiate<Button>(buttonPrefab);
            // Add button to the Canvas
            button.transform.SetParent(transform, false);
            // Set button text
            button.GetComponentInChildren<Text>().text = buttonValues[i];
            // Set position of button
            button.GetComponentInChildren<RectTransform>().anchoredPosition = buttonPos;
            // Set size of button
            button.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(buttonSize, buttonSize);
            // Create a new primitive within the loop for the button, otherwise it will use the last value of i
            int buttonIndex = i;
            // Call ButtonPress with buttonIndex when a button is clicked
            button.onClick.AddListener(delegate { ButtonPress(buttonIndex); });
            // Increment x position
            buttonPos.x += buttonSize;
            // If we're at the far column, reset x and increment y
            if (i % columnCount == columnCount - 1) {
                buttonPos.x = buttonXStart;
                buttonPos.y += buttonSize;
            }
        }
    }

    void ButtonPress(int buttonIndex) {
        string buttonValue = buttonValues[buttonIndex];
        if (buttonIndex == cancelIndex) {
            internalKeypadValue = "";
            keypadInterface.KeypadUpdated(this);
            return;
        } else if (buttonIndex == submitIndex) {
            ValidateKeypadValue();
            return;
        }

        internalKeypadValue += buttonValue;
        if (keypadInterface != null) {
            keypadInterface.KeypadUpdated(this);
        }
    }

    void ValidateKeypadValue() {
        if (internalKeypadValue.Equals(keycode)) {
            keypadInterface.KeypadSuccess(this);
        } else {
            keypadInterface.KeypadFailure(this);
        }
        internalKeypadValue = "";
    }
}
