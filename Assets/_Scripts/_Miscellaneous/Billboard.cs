//Shady
using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
public class Billboard : MonoBehaviour
{
    [Title("BILLBOARD", titleAlignment: TitleAlignments.Centered)]
    [SerializeField] Transform _cam = null;

    private void LateUpdate()
    {
        if(_cam == null)
            _cam = Camera.main.transform;
        if(_cam == null)
            return;
        transform.LookAt(transform.position + _cam.forward);
    }//LateUpdate() end

}//class end