using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListStringCell : ListCell {

    public Text text;

    public void Populate(string optionName) {
        text.text = optionName;
    }
}
