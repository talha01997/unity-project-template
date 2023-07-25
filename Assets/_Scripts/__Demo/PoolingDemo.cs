//Shady
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[HideMonoScript]
public class PoolingDemo : MonoBehaviour
{
    [Title("DEMO", titleAlignment: TitleAlignments.Centered)]
    [SerializeField] Button _spawnButton   = null;
    [SerializeField] Button _destroyButton = null;
    
    private Character _character = null;

    private void Start()
    {
        _spawnButton.onClick.AddListener(Spawn);
        _destroyButton.onClick.AddListener(Destroy);
    }//Start() end

    private void Spawn()
    {
        _character = PoolingSystem.Instance?.GetObjectOfType<Character>(1);
        _character.gameObject.SetActive(true);
        
    }//Spawn() end

    private void Destroy()
    {
        PoolingSystem.Instance?.BackToPool<Character>(_character.ID, _character);
    }//Destroy() end

}//class end