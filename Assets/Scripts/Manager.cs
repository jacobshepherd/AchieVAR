using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class Manager : MonoBehaviour
{
    private GestureRecognizer _gestureRecognizer;
    private KeywordRecognizer _keywordRecognizer;
    public GameObject _character;
    private Animator _characterAnimator;
    private Boolean _characterExist = false;

    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    private Dictionary<string, KeywordAction> _keywordDictionary;

    private void Start()
    {
        _gestureRecognizer = new GestureRecognizer();
        _gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        _gestureRecognizer.TappedEvent += Recognizer_TappedEvent;
        _gestureRecognizer.StartCapturingGestures();

        _keywordDictionary = new Dictionary<string, KeywordAction>();

        _keywordDictionary.Add("Stand idle", StandIdleCommand);
        _keywordDictionary.Add("Show me a backflip", BackflipCommand);
        _keywordDictionary.Add("How do I block", BlockingCommand);
        _keywordDictionary.Add("How about kick", KickCommand);
        _keywordDictionary.Add("I want to learn Capoeira", CapoeiraCommand);
        _keywordDictionary.Add("I want to learn Samba Dancing", SambaDanceCommand);
        _keywordDictionary.Add("Thank you very much", BowCommand);


        _keywordRecognizer = new KeywordRecognizer(_keywordDictionary.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();
    }

    private void OnDestroy()
    {
        _gestureRecognizer.TappedEvent -= Recognizer_TappedEvent;
        _keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (_keywordDictionary.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        RaycastHit hitInfo;

        if (!_characterExist && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity))
        {
            GameObject character = Instantiate(_character, hitInfo.point, Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0));
            _characterAnimator = _character.GetComponent<Animator>();
            _characterExist = true;

            Camera.main.gameObject.GetComponent<UnityEngine.VR.WSA.SpatialMappingRenderer>().enabled = false;
        }
    }

    private void StandIdleCommand(PhraseRecognizedEventArgs args)
    {
        _characterAnimator.Play("Idle", -1, 0f);
    }

    private void BackflipCommand(PhraseRecognizedEventArgs args)
    {
        _characterAnimator.Play("Backflip", -1, 0f);
    }

    private void BlockingCommand(PhraseRecognizedEventArgs args)
    {
        _characterAnimator.Play("Blocking", -1, 0f);
    }

    private void KickCommand(PhraseRecognizedEventArgs args)
    {
        _characterAnimator.Play("Inside Crescent Kick", -1, 0f);
    }

    private void CapoeiraCommand(PhraseRecognizedEventArgs args)
    {
        _characterAnimator.Play("Capoeira", -1, 0f);
    }

    private void SambaDanceCommand(PhraseRecognizedEventArgs args)
    {
        _characterAnimator.Play("Samba Dancing", -1, 0f);
    }

    private void BowCommand(PhraseRecognizedEventArgs args)
    {
        _characterAnimator.Play("Quick Formal Bow", -1, 0f);
    }
}