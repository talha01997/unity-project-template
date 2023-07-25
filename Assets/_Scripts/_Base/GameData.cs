//Shady
using UnityEngine;
using Sirenix.OdinInspector;
using Shady.Core.Singletons;

public sealed class GameData : SingletonScriptableObject<GameData>
{
    //===================================================
    // FIELDS
    //===================================================
    [Title("GAME DATA", "SCRIPTABLE SINGLETON", titleAlignment: TitleAlignments.Centered, true, true)]
    [Switch]
    [SerializeField] bool _initializeLogs = true;
    [Switch]
    [SerializeField] bool _enableLogs = true;

    [InlineProperty]
    [ShowInInspector]
    private SaveData _saveData
    {
        get
        {
            if(Application.isPlaying)
                return SaveData.Instance;
            else
                return null;
        }//get end
    }//SaveData end

    //===================================================
    // PROPERTIES
    //===================================================
    public bool InitLogs   => _initializeLogs;
    public bool EnableLogs => _enableLogs;

    //===================================================
    // METHODS
    //===================================================
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Shady/Scriptable Objects/Game Data %#g", false, 1)]
    public static void CreateObject() => SelectHolder();
#endif

}//class end