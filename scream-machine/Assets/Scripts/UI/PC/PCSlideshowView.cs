using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class PCSlideshowView : MonoBehaviour {

    [SerializeField] private Image image = null;
    [SerializeField] private Text text = null;

    private PCSlideshowData model;

    public void Populate(PCSlideshowData model) {
        this.model = model;
        image.sprite = model.slides[0].sprite;
        text.text = model.slides[0].text;
    }

    public async Task DoSlideshowAsync() {
        await Global.Instance().Input.ConfirmRoutine();
        for (var i = 1; i < model.slides.Count; i += 1) {
            var slide = model.slides[i];
            image.sprite = slide.sprite;
            text.text = slide.text;
            if (!slide.invertColor) text.color = new Color(.05f, 0, 0, .95f);
            else text.color = new Color(1, .9f, .9f, .9f);
            await Global.Instance().Input.ConfirmRoutine();
        }
        await Global.Instance().Messenger.Lua.RunRoutine(model.postLua, true);
    }
}
