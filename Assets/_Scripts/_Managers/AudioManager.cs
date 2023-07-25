//Shady
using UnityEngine;
using Sirenix.OdinInspector;
using Shady.Core.Singletons;
using System.Collections.Generic;

public enum SFX
{
    Click,
    Spawn,
    GameStart,
    GameWin,
    GameLose
}//enum end

[System.Serializable]
public sealed class SFXClip
{
    //===================================================
    // FIELDS
    //===================================================
    [HorizontalGroup("Split")]
    [VerticalGroup("Split/Enabled")]
    [DisplayAsString]
    [HideLabel]
    [SerializeField] SFX _sfx;

    [VerticalGroup("Split/Disabled")]
    [HideLabel]
    [SerializeField] AudioClip _clip = null;

    // Constructor
    public SFXClip(SFX sfx) => _sfx = sfx;

    //===================================================
    // PROPERTIES
    //===================================================
    public SFX       SFX  => _sfx;
    public AudioClip Clip => _clip;

}//struct end

public sealed class AudioManager : Singleton<AudioManager>
{
    //===================================================
    // FIELDS
    //===================================================
    [Title("AUDIO MANAGER", titleAlignment: TitleAlignments.Centered)]
    [SerializeField] AudioSource _bgSource   = null;
    [SerializeField] AudioSource _sfxSource  = null;
    [Space]
    [SerializeField] AudioClip _bgMusic = null;
    [Space]
    [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = true)]
    [InlineButton(nameof(Create))]
    [LabelText("SFX Clips")]
    [SerializeField] List<SFXClip> _sfxClips = new List<SFXClip>();

    //===================================================
    // METHODS
    //===================================================
    internal override void Init()
    {
        base.Init();

        if(InstanceNotSelf)
            return;

        SetBGSetting(SaveData.Instance.Music);
        SetSFXSetting(SaveData.Instance.SFX);
    }//Start() end

    /// <summary>
    /// Method for creating Array of SFX Clips.
    /// </summary>
    private void Create()
    {
        int length = System.Enum.GetValues(typeof(SFX)).Length;

        if(_sfxClips == null || _sfxClips.Count == 0)
        {
            _sfxClips = new List<SFXClip>();
            for(int i=0 ; i<length ; i++)
            {
                _sfxClips.Add(new SFXClip((SFX)i));
            }//loop end
        }//if end
        else
        {
            for(int i=0 ; i<length - _sfxClips.Count ; i++)
                _sfxClips.Add(new SFXClip((SFX)0));

            for(int i=0 ; i<length ; i++)
            {
                if(_sfxClips[i].SFX != (SFX)i)
                    _sfxClips[i] = new SFXClip((SFX)i);
            }//loop end
        }//else end
        
    }//Create() end

    /// <summary>
    /// Toggle Background Music Audio Source.
    /// </summary>
    public void SetBGSetting(bool Toggle)  => _bgSource.mute  = !Toggle;

    /// <summary>
    /// Toggle SFX Audio Source.
    /// </summary>
    public void SetSFXSetting(bool Toggle) => _sfxSource.mute = !Toggle;

    /// <summary>
    /// Call when Game Starts to play Background Music.
    /// </summary>
    public void StartGame()
    {
        if(_bgSource.isPlaying)
            return;

        _bgSource.clip = _bgMusic;
        _bgSource.loop = true;
        _bgSource.Play();
    }//StartGame() end

    /// <summary>
    /// Call when Game Starts to stop playing Background Music.
    /// </summary>
    public void GameEnd() => _bgSource.Stop();

    /// <summary>
    /// Call to play specific SFX clip against enum.
    /// </summary>
    public void PlaySFX(SFX sfx, float volume = 1f) => 
        _sfxSource.PlayOneShot(_sfxClips[(int)sfx].Clip, volume);

    /// <summary>
    /// Call to play custom Audio Clip.
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f) => 
        _sfxSource.PlayOneShot(clip, volume);

}//class end