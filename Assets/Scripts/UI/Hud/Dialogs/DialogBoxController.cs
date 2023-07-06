using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour
{

    [SerializeField] private GameObject _container;
    [SerializeField] private Animator _animator;

    [Space] [SerializeField] private float _textSpeed = 0.09f;

    [Header("Sounds")] [SerializeField] private AudioClip _typing;
    [SerializeField] private AudioClip _open;
    [SerializeField] private AudioClip _close;

    [Space] [SerializeField] protected DialogContent _content;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    protected DialogData _data;
    protected int _currentSentence;
    private AudioSource _sfxSource;
    private Coroutine _typingRoutine;
    private UnityEvent _onComplete;

    protected Sentence CurrentSentence => _data.Sentences[_currentSentence];

    private void Start()
    {
        _sfxSource = AudioUtils.FindSfxSource();
    }

    public void ShowDialog(DialogData data, UnityEvent onComplete)
    {
        _onComplete = onComplete;
        _data = data;
        _currentSentence = 0;
        CurrentContent.Text.text = string.Empty;

        _container.SetActive(true);
        _sfxSource.PlayOneShot(_open);
        _animator.SetBool(IsOpen, true);
    }

    private IEnumerator TypeDialogText()
    {
        CurrentContent.Text.text = string.Empty;
        //var sentence = CurrentSentence;
         // CurrentContent.TrySetIcon(sentence.Icon);
        var sentence = _data.Sentences[_currentSentence];

       // var localizedSentence = sentence.Value.Localize();

        foreach (var letter in sentence.Value)
        {
            CurrentContent.Text.text += letter;
            _sfxSource.PlayOneShot(_typing);
            yield return new WaitForSeconds(_textSpeed);
        }

        _typingRoutine = null;
    }

    protected virtual DialogContent CurrentContent => _content;

    public void OnSkip()
    {
        if (_typingRoutine == null) return;

        StopTypeAnimation();
        CurrentContent.Text.text = _data.Sentences[_currentSentence].Value;
       // CurrentContent.Text.text = sentence.Localize();
    }

    public void OnContinue()
    {
        StopTypeAnimation();
        _currentSentence++;

        var isDialogCompleted = _currentSentence >= _data.Sentences.Length;
        if (isDialogCompleted)
        {
            HideDialogBox();
            _onComplete?.Invoke();
        }
        else
        {
            OnStartDialogAnimation();
        }
    }

    private void HideDialogBox()
    {
        _animator.SetBool(IsOpen, false);
        _sfxSource.PlayOneShot(_close);
    }

    private void StopTypeAnimation()
    {
        if (_typingRoutine != null)
            StopCoroutine(_typingRoutine);
        _typingRoutine = null;
    }

    protected virtual void OnStartDialogAnimation()
    {
        _typingRoutine = StartCoroutine(TypeDialogText());
    }

    private void OnCloseAnimationComplete()
    {
    }
}