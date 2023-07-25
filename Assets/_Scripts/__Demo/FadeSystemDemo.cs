//Shady
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[HideMonoScript]
public class FadeSystemDemo : MonoBehaviour
{
    [Title("FADE SYSTEM DEMO", titleAlignment: TitleAlignments.Centered)]
    [SerializeField] GameObject Object1 = null;
    [SerializeField] GameObject Object2 = null;
    [SerializeField] Button _fadeButton    = null;
    [SerializeField] Button _fadeInButton  = null;
    [SerializeField] Button _fadeOutButton = null;

    private void Start()
    {
        _fadeButton.onClick.AddListener(Fade);
        _fadeInButton.onClick.AddListener(FadeIn);
        _fadeOutButton.onClick.AddListener(FadeOut);
    }//Start() end

    private void Fade()
    {
        if(Object1.activeSelf)
            FadeSystem.Instance?.Fade(Object1, Object2, 0.25f);
        else
            FadeSystem.Instance?.Fade(Object2, Object1, 0.25f);
    }//Fade() end

    private void FadeIn()
    {
        FadeSystem.Instance.FadeIn(()=>{
            //Do Something
        });
    }//FadeIn() end

    private void FadeOut()
    {
        FadeSystem.Instance.FadeOut(()=>{
            //Do Something
        });
    }//FadeIn() end

}//class end