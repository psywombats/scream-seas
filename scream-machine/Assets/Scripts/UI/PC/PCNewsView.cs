using UnityEngine;
using UnityEngine.UI;

public class PCNewsView : MonoBehaviour {

    [SerializeField] private Text headline = null;
    [SerializeField] private Text story1 = null;
    [SerializeField] private Text story2 = null;
    [SerializeField] private Text story3 = null;
    [SerializeField] private Text story4 = null;
    [SerializeField] private Text date = null;

    public void Populate(PCNewsData model) {
        headline.text = model.headline;
        story1.text = model.story1;
        story2.text = model.story2;
        story3.text = model.story3;
        story4.text = model.story4;
        date.text = model.date;
    }
}
