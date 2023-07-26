//Shady
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[HideMonoScript]
public class SaveDataDemo : MonoBehaviour
{
    //===================================================
    // FIELDS
    //===================================================
    [Title("SAVE DATA DEMO", titleAlignment: TitleAlignments.Centered)]
    [SerializeField] TMP_Text _levelText     = null;
    [SerializeField] Button   _levelComplete = null;

    //===================================================
    // METHODS
    //===================================================
    private void Start()
    {
        _levelText.text = $"Current Level: {SaveData.Instance.Data.levelNum}";
        _levelComplete.onClick.AddListener(LevelComplete);
    }//Start() end

    private void LevelComplete()
    {
        SaveData.Instance.Data.levelNum++;
        SaveSystem.SaveProgress();
        _levelText.text = $"Current Level: {SaveData.Instance.Data.levelNum}";
    }//LevelComplete() end

}//class end