﻿using UnityEngine;
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
            messenger.ActiveConvo = convo;
            if (convo.PendingScript != null) {
                await convo.PlayScriptRoutine();
            } else {
                var msg = convo.LastMessage;
                await PlayMessageRoutine(msg);
                UpdateFromMessenger(messenger);
            }
            SwitchToSelectMode();
            if (!MapOverlayUI.Instance().phoneSystem.IsFlipped && !MapOverlayUI.Instance().phoneSystem.IsFlippedForeign) {
                break;
            }
        }
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

    private void SwitchToSelectMode() {
        messageSelectMode.SetActive(true);
        toMode.SetActive(false);
        fromMode.SetActive(false);

        UpdateSelector();
    }

    private void UpdateSelector() {
        var convos = messenger.GetRecentConversations();
        ConversationList.Populate(convos.GetRange(0, Mathf.Min(6, convos.Count)), (obj, data) => {
            obj.GetComponent<ConversationCell>().Populate(data);
        });
    }

    public IEnumerator PlayMessageRoutine(string sender, string text) {
        if (text.Length > 0) {
            var convo = messenger.ActiveConvo;
            var message = new Message(convo, sender == "YOU" ? messenger.Me : convo.Client, text);
            convo.AddMessage(message);
            yield return PlayMessageRoutine(message);
        }
    }

    public IEnumerator PlayMessageRoutine(Message message) {
        if (message.Client != messenger.Me) {
            SwitchToFromMode();
            UpdateFromMessenger(messenger);
        } else {
            SwitchToToMode();
            UpdateFromMessenger(messenger);
        }
        yield return Global.Instance().Input.ConfirmRoutine();
    }

    public IEnumerator VideoRoutine() {
        float duration = 0.8f;
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
            Global.Instance().Audio.FadeOutRoutine(duration),
        }, this);
        yield return CoUtils.Wait(0.5f);

        Global.Instance().Data.SetSwitch("snow_disabled", true);
        videoMode.SetActive(true);
        videoMode.GetComponent<VideoPlayer>().Play();
        yield return CoUtils.Wait(3.0f);
        while (videoMode.GetComponent<VideoPlayer>().isPlaying) {
            yield return null;
        }
        videoMode.SetActive(false);
        Global.Instance().Data.SetSwitch("snow_disabled", false);

        yield return CoUtils.Wait(1.0f);
        Global.Instance().Audio.ResumeBGM();
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
