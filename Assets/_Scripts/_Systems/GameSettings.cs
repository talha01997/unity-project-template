// Shady
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Shady.Core.Singletons;

[System.Serializable]
[HideLabel]
public sealed class Toggle
{
    [Required]
    [Space]
    [SerializeField] Button _button    = null;
    
    [HorizontalGroup("Split", 0.5f, LabelWidth = 20)]
    [BoxGroup("Split/Enabled")]
    [Required]
    [HideLabel]
    [InlineEditor(InlineEditorModes.LargePreview, Expanded = true, PreviewHeight = 100)]
    [SerializeField] Sprite _onSprite = null;

    [BoxGroup("Split/Disabled")]
    [Required]
    [HideLabel]
    [InlineEditor(InlineEditorModes.LargePreview, Expanded = true, PreviewHeight = 100)]
    [SerializeField] Sprite _offSprite = null;

    private Image _spriteImage = null;
    
    public void AddListener(UnityAction call)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(call);
        _spriteImage = _button.GetComponent<Image>();
    }//AddListener() end

    public void SetState(bool _toggle) => _spriteImage.sprite = _toggle ? _onSprite : _offSprite;

}//class end

[HideMonoScript]
public sealed class GameSettings : Singleton<GameSettings>
{
    //===================================================
    // FIELDS
    //===================================================
    [Title("GAME SETTINGS", null, titleAlignment: TitleAlignments.Centered)]
    [Required]
    [SerializeField] Button        _settingButton = null;
    [Required]
    [SerializeField] RectTransform _settingPanel  = null;
    [FoldoutGroup("Music Toggle")]
    [SerializeField] Toggle _musicToggle  = null;
    [FoldoutGroup("SFX Toggle")]
    [SerializeField] Toggle _sfxToggle    = null;
    [FoldoutGroup("Haptic Toggle")]
    [SerializeField] Toggle _hapticToggle = null;

    //===================================================
    // PRIVATE FIELDS
    //===================================================
    private float _posY      = 0f;
    private bool  _toggle    = false;
    private bool  _canToggle = true;

    //===================================================
    // METHODS
    //===================================================
    internal override void Init()
    {
        base.Init();

        if(InstanceNotSelf)
            return;

        Initialize();
    }//Awake() end

    private void Initialize()
    {
        _settingPanel.anchoredPosition = new Vector2(_settingPanel.anchoredPosition.x, -_settingPanel.anchoredPosition.y);
        _posY = -_settingPanel.anchoredPosition.y;
        _settingButton.onClick.RemoveAllListeners();
        _settingButton.onClick.AddListener(TogglePanel);
        _musicToggle. AddListener(ToggleMusic);
        _sfxToggle.AddListener(ToggleSFX);
        _hapticToggle.AddListener(ToggleHaptic);
        LoadSettings();
        _settingButton.gameObject.SetActive(false);
    }//Start() end
    
    public void InitializeSettings() => _settingButton.gameObject.SetActive(true);

    private void TogglePanel()
    {
        if(!_canToggle)
            return;
        _canToggle = false;
        _toggle    = !_toggle;
        AudioManager.Instance?.PlaySFX(SFX.Click);
        if(_toggle)
        {
            _settingPanel.DOAnchorPos3DY(_posY, 0.25f, false).OnComplete(()=>_canToggle = true);
            Invoke(nameof(ClosePanel), 3.0f);
        }//if end
        else   
            ClosePanel();
    }//TogglePanel() end

    public void ClosePanel()
    {
        CancelInvoke();
        _toggle = false;
        _settingPanel.DOAnchorPos3DY(-_posY, 0.25f, false).OnComplete(()=>_canToggle = true);
    }//ClosePanel() end

    private void LoadSettings()
    {
        _musicToggle.SetState(SaveData.Instance.Music);
        _sfxToggle.SetState(SaveData.Instance.SFX);
        _hapticToggle.SetState(SaveData.Instance.Haptic);
    }//LoadSettings() end

    private void ToggleMusic()
    {
        SaveData.Instance.Music = !SaveData.Instance.Music;
        _musicToggle.SetState(SaveData.Instance.Music);
        AudioManager.Instance?.PlaySFX(SFX.Click);
        AudioManager.Instance?.SetBGSetting(SaveData.Instance.Music);
        SaveSystem.SaveProgress();
    }//ToggleMusic() end

    private void ToggleSFX()
    {
        SaveData.Instance.SFX = !SaveData.Instance.SFX;
        _sfxToggle.SetState(SaveData.Instance.SFX);
        AudioManager.Instance?.PlaySFX(SFX.Click);
        AudioManager.Instance?.SetSFXSetting(SaveData.Instance.SFX);
        SaveSystem.SaveProgress();
    }//ToggleSFX() end

    private void ToggleHaptic()
    {
        SaveData.Instance.Haptic = !SaveData.Instance.Haptic;
        _hapticToggle.SetState(SaveData.Instance.Haptic);
        AudioManager.Instance?.PlaySFX(SFX.Click);
        SaveSystem.SaveProgress();
    }//ToggleHaptic() end

}//class end