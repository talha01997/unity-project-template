//Shady
using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
public class GameSettingsDemo : MonoBehaviour
{
    private void Start()
    {
        GameSettings.Instance.InitializeSettings();
    }//Start() end
}//class end