using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using DG.Tweening;
using UnityEngine.Video;

public class BigPhoneComponent : PhoneComponent {

    [SerializeField] MessageDisplayComponent toDisplay = null;
    [SerializeField] MessageDisplayComponent fromDisplay = null;
    [Space]
    [SerializeField] GameObject messageSelectMode = null;
    [SerializeField] GameObject toMode = null;
    [SerializeField] GameObject fromMode = null;
    [SerializeField] GameObject videoMode = null;
    [Space]
    [SerializeField] private ListView ConversationList = null;
    [SerializeField] private GenericSelector ConversationSelector = null;

    private bool isFirstMessage = true;

    public void Populate(Messenger messenger) {
        this.messenger = messenger;

        if (messageSelectMode.activeInHierarchy) {
            UpdateSelector();
        }
        if (toMode.activeInHierarchy && messenger.ActiveConvo != null) {
            toDisplay.Populate(messenger.ActiveConvo.LastMessage, useToMode:true);
        }
        if (fromMode.activeInHierarchy && messenger.ActiveConvo != null) {
            fromDisplay.Populate(messenger.ActiveConvo.LastMessage, useToMode:false);
        }
    }

    public override void UpdateFromMessenger(Messenger messenger) {
        base.UpdateFromMessenger(messenger);
        if (messenger == this.messenger) {
            Populate(messenger);
        }
    }

    public async Task DoMenu() {
        Populate(messenger);
        ConversationSelector.Selection = 0;
        while (true) {
            var index = await ConversationSelector.SelectItemAsync(null, true);
            if (index < 0) {
                break;
            }

            var convo = ConversationList.GetCell(index).GetComponent<ConversationCell>().Convo;
            await ShowConversation(convo);
            if (!MapOverlayUI.Instance().phoneSystem.IsFlipped && !MapOverlayUI.Instance().phoneSystem.IsFlippedForeign) {
                break;
            }
        }
    }

    public void SwitchToSelectMode() {
        isFirstMessage = true;
        messageSelectMode.SetActive(true);
        toMode.SetActive(false);
        fromMode.SetActive(false);

        UpdateSelector();
    }

    public async Task ShowConversation(Conversation convo) {
        messenger.ActiveConvo = convo;
        if (convo.PendingScript != null) {
            await convo.PlayScriptRoutine();
        } else {
            var msg = convo.LastMessage;
            await PlayMessageRoutine(msg, 0, true);
            UpdateFromMessenger(messenger);
        }
        SwitchToSelectMode();
    }

    private void SwitchToFromMode() {
        messageSelectMode.SetActive(false);
        toMode.SetActive(false);
        fromMode.SetActive(true);
    }

    private void SwitchToToMode() {
        messageSelectMode.SetActive(false);
        toMode.SetActive(true);
        fromMode.SetActive(false);
    }

    private void UpdateSelector() {
        var convos = messenger.GetRecentConversations();
        ConversationList.Populate(convos.GetRange(0, Mathf.Min(6, convos.Count)), (obj, data) => {
            obj.GetComponent<ConversationCell>().Populate(data);
        });
    }

    public IEnumerator PlayMessageRoutine(string sender, string text, float forceLength = 0.0f, bool isStale = false) {
        if (text.Length > 0) {
            if (messenger == null) {
                messenger = Global.Instance().Messenger;
            }
            if (messenger.ActiveConvo == null) {
                messenger.ActiveConvo = messenger.GetConversation("SIS");
            }
            var convo = messenger.ActiveConvo;

            var message = new Message(convo, sender == "YOU" ? messenger.Me : convo.Client, text);
            
            convo.AddMessage(message);
            yield return PlayMessageRoutine(message, forceLength, isStale);
        }
    }

    public IEnumerator PlayMessageRoutine(Message message, float forceLength = 0.0f, bool isStale = false) {
        if (message.Client != messenger.Me) {
            SwitchToFromMode();
            UpdateFromMessenger(messenger);
        } else {
            SwitchToToMode();
            UpdateFromMessenger(messenger);
        }

        if (!isStale && !isFirstMessage) {
            if (message.Client == messenger.Me) {
                Global.Instance().Audio.PlaySFX("type_message");
            } else if (!isStale) {
                Global.Instance().Audio.PlaySFX("receive_message");
            }
        }
        isFirstMessage = false;

        if (forceLength > 0) {
            yield return CoUtils.Wait(forceLength);
        } else {
            yield return Global.Instance().Input.ConfirmRoutine();
        }
    }

    public IEnumerator VideoRoutine() {
        float duration = 0.8f;
        bool fadeBGM = !Global.Instance().Data.GetSwitch("day4");
        var sizeTween = transform.DOScale(new Vector3(2, 2, 1), duration);
        var translateTween = transform.DOLocalMoveY(-30f, duration);
        var clearTween = fromMode.GetComponent<CanvasGroup>().DOFade(0.0f, duration);
        sizeTween.SetEase(Ease.InQuad);
        translateTween.SetEase(Ease.InQuad);
        clearTween.SetEase(Ease.InQuad);
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(sizeTween),
            CoUtils.RunTween(translateTween),
            CoUtils.RunTween(clearTween),
            !fadeBGM ? CoUtils.Wait(0.0f) : Global.Instance().Audio.FadeOutRoutine(duration),
        }, this);
        yield return CoUtils.Wait(0.5f);

        Global.Instance().Data.SetSwitch("snow_disabled", true);
        videoMode.SetActive(true);
        if (!fadeBGM) videoMode.GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.None;
        videoMode.GetComponent<VideoPlayer>().Play();
        yield return CoUtils.Wait(3.0f);
        while (videoMode.GetComponent<VideoPlayer>().isPlaying) {
            yield return null;
        }
        videoMode.SetActive(false);
        Global.Instance().Data.SetSwitch("snow_disabled", false);

        yield return CoUtils.Wait(1.0f);
        if (fadeBGM) Global.Instance().Audio.ResumeBGM();
        sizeTween = transform.DOScale(new Vector3(1, 1, 1), duration);
        translateTween = transform.DOLocalMoveY(0, duration);
        clearTween = fromMode.GetComponent<CanvasGroup>().DOFade(1.0f, duration);
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(sizeTween),
            CoUtils.RunTween(translateTween),
            CoUtils.RunTween(clearTween),
         }, this);
    }
}
