//Shady
using UnityEngine;
using Shady.Core.Singletons;
using Sirenix.OdinInspector;

public sealed class GameManager : Singleton<GameManager>
{
    //===================================================
    // FIELDS
    //===================================================
    [Title("GAME MANAGER", titleAlignment: TitleAlignments.Centered)]

    //===================================================
    // PROPERTIES
    //===================================================


    //===================================================
    // METHODS
    //===================================================
    internal override void Init()
    {
        base.Init();

        if(InstanceNotSelf)
            return;

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount  = 1;
        // FadeSystem.Instance.Splash();
    }//Init() end

}//class end