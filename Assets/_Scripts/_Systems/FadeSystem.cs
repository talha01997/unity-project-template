//FadeSystem v1.1_2
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Shady.Core.Singletons;

public delegate void Callback();

[HideMonoScript]
public sealed class FadeSystem : Singleton<FadeSystem>
{
    //===================================================
    // FIELDS
    //===================================================
    [Title("FADE SYSTEM", titleAlignment: TitleAlignments.Centered)]
    [SerializeField] Image _panel = null;
    [Range(0.2f, 1.0f)]
    [SerializeField] float _fadeSpeed = 1.0f;

    //===================================================
    // METHODS
    //===================================================
    internal override void Init()
    {
        base.Init();

        if(InstanceNotSelf)
            return;

        if(!_panel)
            _panel = GetComponentInChildren<Image>();
        _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, 0f);
    }//Awake() end

    public void Splash() =>
        _panel.DOFade(0.0f, _fadeSpeed).SetDelay(0.25f).OnComplete(()=>_panel.raycastTarget = false);

    /// <summary>
    /// Method Fades In Screen and toggles GameObjects and Fades Out.
    /// </summary>
    public void Fade(GameObject TurnOff = null, GameObject TurnOn = null, float interval = 0.5f)
    {
        _panel.raycastTarget = true;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(_panel.DOFade(1.0f, _fadeSpeed).OnComplete(()=>
        {
            TurnOff?.SetActive(false);
            TurnOn?.SetActive(true);
        }));
        mySequence.AppendInterval(interval);
        mySequence.Append(_panel.DOFade(0.0f, _fadeSpeed).OnComplete(()=>_panel.raycastTarget = false));
        DOTween.Kill(mySequence);
    }//Fade() end

    /// <summary>
    /// Method Fades In Screen and executes the Callback and Fades Out.
    /// </summary>
    public void Fade(Callback Action = null, float interval = 0.5f)
    {
        _panel.raycastTarget = true;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(_panel.DOFade(1.0f, _fadeSpeed).OnComplete(()=>Action?.Invoke()));
        mySequence.AppendInterval(interval);
        mySequence.Append(_panel.DOFade(0.0f, _fadeSpeed).OnComplete(()=>_panel.raycastTarget = false));
        DOTween.Kill(mySequence);
    }//Fade() end

    /// <summary>
    /// Method Fades In Screen and executes the Callback.
    /// </summary>
    public void FadeIn(Callback Action)
    {
        _panel.raycastTarget = true;
        _panel.DOFade(1.0f, _fadeSpeed).OnComplete(()=>Action?.Invoke());
    }//FadeIn() end

    /// <summary>
    /// Method Fades Out Screen and executes the Callback.
    /// </summary>
    public void FadeOut(Callback Action = null)
    {
        _panel.DOFade(0.0f, _fadeSpeed).OnComplete(()=>
            {
                Action?.Invoke();
                _panel.raycastTarget = false;
            }//Callbacks
        );
    }//FadeOut() end

}//class end