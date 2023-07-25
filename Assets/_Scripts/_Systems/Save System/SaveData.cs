//Shady
// Introduce your new variables under Game Variables and pass them accordingly
// in the Constructor => SaveData();
using UnityEngine;
using Sirenix.OdinInspector;

public sealed class SaveData
{
    //===================================================
    // PRIVATE FIELDS
    //===================================================
    private static SaveData _instance = null;

    //===================================================
    // PROPERTIES
    //===================================================
    public static SaveData Instance
    {
        get
        {
            if(_instance is null)
            {
                _instance = new SaveData();
                SaveSystem.LoadProgress();
            }//if end
            return _instance;
        }//get end
    }//Property end

    //===================================================
    // FIELDS
    //===================================================
    [Title("Game Settings")]
    [DisplayAsString]
    public bool Music  = true;
    [DisplayAsString]
    public bool SFX    = true;
    [DisplayAsString]
    public bool Haptic = true;
    [Space]
    [Title("Game Values")]
    [DisplayAsString]
    public int Level  = 1;
    [DisplayAsString]
    public int Coins  = 0;

    [HideInInspector]
    public string HashOfSaveData = null;

    public SaveData(){}

    private SaveData(SaveData data)
    {
        Music  = data.Music;
        SFX    = data.SFX;
        Haptic = data.Haptic;

        Level  = data.Level;
        Coins  = data.Coins;
    }//CopyConstructor() end

    public SaveData CreateSaveObject() => new SaveData(_instance);

    public void Reset() => _instance = new SaveData();

}//class end