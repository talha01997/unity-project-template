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
        _levelText.text = $"Current Level: {SaveData.Instance.Level}";
        _levelComplete.onClick.AddListener(LevelComplete);
    }//Start() end

    private void LevelComplete()
    {
        SaveData.Instance.Level++;
        SaveSystem.SaveProgress();
        _levelText.text = $"Current Level: {SaveData.Instance.Level}";
    }//LevelComplete() end

}//class end