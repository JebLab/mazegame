using UnityEngine.UIElements;
using UnityEngine;


// These are so I can use the 'Percent' and 'Relative' enums
using static UnityEngine.UIElements.Position;
using static UnityEngine.UIElements.LengthUnit;

public class UIElementFactory {

    public UIElementFactory() {
        #if UNITY_EDITOR
        Debug.Log("I've created a factory!");
        #endif
    }

    public Button createButton() {
        #if UNITY_EDITOR
        Debug.Log("I've created a button!");
        #endif
        return new Button();
    }

    public Button createButton(string name) {
        Button btn = new Button();
        btn.name = name;
        return btn;
    }


    /* I call it "standard" bc I'm giving it the same dimensions as most other UI elements
     * Which can be found in StartScreen.uxml
     * This is true of all "createStandard" methods
     */
    public Button createStandardButton(string name) {
        Button btn = this.createButton(name);
        
        /* sldr.style.width = new StyleLength(new Length(50, Percent));
        sldr.style.position = new StyleEnum<Position>(Relative);
        sldr.style.left = new StyleLength(new Length(25, Percent)); */
        
        this.giveStandardDimensions(btn);

        return btn;
    }

    public Label createLabel() {
        return new Label();
    }

    public Label createLabel(string name) {
        Label lbl = new Label();
        lbl.name = name;
        return lbl;
    }

    public Label createStandardLabel(string name) {
        Label lbl = this.createLabel(name);
        
        /* sldr.style.width = new StyleLength(new Length(50, Percent));
        sldr.style.position = new StyleEnum<Position>(Relative);
        sldr.style.left = new StyleLength(new Length(25, Percent)); */

        this.giveStandardDimensions(lbl);

        return lbl;
    }

    public Slider createSlider() {
        return new Slider();
    }

    public Slider createSlider(string name) {
        Slider sldr = new Slider();
        sldr.name = name;
        return sldr;
    }

    public Slider createStandardSlider(string name) {
        Slider sldr = this.createSlider(name);

        /* sldr.style.width = new StyleLength(new Length(50, Percent));
        sldr.style.position = new StyleEnum<Position>(Relative);
        sldr.style.left = new StyleLength(new Length(25, Percent)); */
        giveStandardDimensions(sldr);

        return sldr;
    }

    public Slider createNormalizedSlider(string name) {
        Slider sldr = this.createStandardSlider(name);

        // Our range will be from 0 to 1, hence the "normalized"
        sldr.lowValue = 0.0f;
        sldr.highValue = 1.0f;

        return sldr;
    }

    public Box createBox() {
        return new Box();
    }

    public Box createBox(string name) {
        Box bx = new Box();
        bx.name = name;
        return bx;
    }

    public Box createStandardBox(string name) {
        Box bx = createBox(name);
        giveStandardDimensions(bx);
        return bx;
    }

    // This is the function where we give a UIElement standard dimensions
    // Just so we don't repeat ourself repeatedly
    private void giveStandardDimensions(VisualElement UIElem) {
        UIElem.style.width = new StyleLength(new Length(50, Percent));
        UIElem.style.position = new StyleEnum<Position>(Relative);
        UIElem.style.left = new StyleLength(new Length(25, Percent));
    }

}
