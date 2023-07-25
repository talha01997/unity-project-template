//Shady
using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Shady.Core.Singletons
{
    [System.Serializable]
    public sealed class Singleton
    {

#if ODIN_INSPECTOR
        [HorizontalGroup("Split")]
        [VerticalGroup("Split/Left"), LabelWidth(100)]
#endif
        [SerializeField] SingletonBase _singletonBase = null;
#if ODIN_INSPECTOR
        [VerticalGroup("Split/Right"), LabelWidth(60)]
        [Switch]
#endif
        [SerializeField] bool _initialize  = true;

        public void Init()
        {
            if(_initialize)
                _singletonBase?.Init();
        }//Init() end

    }//class end

#if ODIN_INSPECTOR
    [HideMonoScript]
#endif
    public sealed class Initializer : MonoBehaviour
    {
        //===================================================
        // FIELDS
        //===================================================
#if ODIN_INSPECTOR
        [Title("INITIALIZER", null, titleAlignment: TitleAlignments.Centered)]
#endif
        [SerializeField] List<Singleton> _singletons = new List<Singleton>();

        //===================================================
        // METHODS
        //===================================================
        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake() =>
            _singletons.ForEach(s => s?.Init());

    }//class end
}//namespace end

