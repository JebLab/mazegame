using UnityEngine.UIElements;
using UnityEngine; //.CoreModule;

public class UIElemFact {

    public UIElemFact() {
        Debug.Log("I've created a factory!");
    }

    public Button createButton() {
        Debug.Log("I've created a button!");
        return new Button();
    }

    public Label createLabel() {
        return new Label();
    }

    public Slider createSlider() {
        return new Slider();
    }

    

}
