//Shady
using UnityEngine;

public static class GameLog
{
    //===================================================
    // METHODS
    //===================================================
    public static void InitMessage<T>() where T : Component
    {
        if(GameData.Instance.InitLogs)
            Debug.Log("<b><color=#00be0e>Game Log : </color></b>" + $"{typeof(T).Name} Initialized!");
    }//InitMessage() end

    public static void ShowMessage(string message)
    {
        if(GameData.Instance.EnableLogs)
            Debug.Log("<b><color=#22a7f0>Game Log : </color></b>" + message);
    }//Log() end

    public static void ShowWarning(string message)
    {
        if(GameData.Instance.EnableLogs)
            Debug.LogWarning("<b><color=#22a7f0>Game Log : </color></b>" + message);
    }//Log() end

    public static void ShowError(string message)
    {
        if(GameData.Instance.EnableLogs)
            Debug.LogError("<b><color=#22a7f0>Game Log : </color></b>" + message);
    }//Log() end

}//class end