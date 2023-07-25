/// Shady
/// SingletonBase v1.4
using UnityEngine;

namespace Shady.Core.Singletons
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    [HideMonoScript]
#endif
    /// <summary>
    /// Main Base class that holds some setting variables on how to Initialize the Singleton.
    /// </summary>
    public abstract class SingletonBase : MonoBehaviour
    {
        //===================================================
        // FIELDS
        //===================================================

#if ODIN_INSPECTOR
        [BoxGroup("SINGLETON BASE", showLabel: false)]
        [Title("SINGLETON BASE", titleAlignment: TitleAlignments.Centered)]
        [LabelWidth(140)]
        [DisplayAsString]
        [LabelText("Initialized : ")]
#endif

        [SerializeField] private bool _initialized = false;

#if ODIN_INSPECTOR
        [BoxGroup("SINGLETON BASE")]
        [Switch]
        [LabelWidth(140)]
        [DisableInPlayMode]
        [LabelText("Initialize In Awake")]
#endif

        [SerializeField] private bool _initInAwake = false;

#if ODIN_INSPECTOR
        [BoxGroup("SINGLETON BASE")]
        [Switch]
        [LabelWidth(140)]
        [DisableInPlayMode]
        [LabelText("Dont Destroy On Load")]
#endif

        [SerializeField] private bool _dontDestroyOnLoad = false;

        //===================================================
        // PROPERTIES
        //===================================================
        protected bool Initialized => _initialized;
        protected bool InitInAwake => _initInAwake;
        protected bool DontDestroy => _dontDestroyOnLoad;

        //===================================================
        // METHODS
        //===================================================
        /// <summary>
        /// Virtual Method being overrided in the child class
        /// </summary>
        internal virtual void Init() => _initialized = true;

    }//class end

    /// <summary>
    /// class taking a Generic type for creating a Singelton of the passed type.
    /// Inherit from this class to make that class a Singleton.
    /// </summary>
    public abstract class Singleton<T> : SingletonBase where T : Component
    {
        //===================================================
        // FIELDS
        //===================================================
        public static T Instance {get; private set;}

        /// <summary>
        /// This property was specifically created to check when the Init method is being override.
        /// As if a Singleton object was present more than one time or present in multiple scenes it did destroy but
        /// the Initialization was also called in the derived class and the object was destoryed a bit late.
        /// </summary>
        protected bool InstanceNotSelf => Instance.gameObject != gameObject;

        //===================================================
        // METHODS
        //===================================================

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Instance will only be created if the field InitInAwake is true.
        /// Otherwise you have to manually call the Init method externally for
        /// sequential initializations of multiple singletons which have cross dependencies.
        /// </summary>
        private void Awake()
        {
            if(InitInAwake)
                Init();
        }//Awake() end

        /// <summary>
        /// Method Override in which instance is being created.
        /// This method can be overrided by your derived class for any extended functionality
        /// you require to run in the Initialization process(Awake) but do call base.Init() first 
        /// in your override method.
        /// </summary>
        internal override void Init()
        {
            if(Instance is null && Initialized is false)
            {
                Instance = this as T;

                if(DontDestroy is true)
                {
                    transform.SetParent(null);
                    DontDestroyOnLoad(gameObject);
                }//if end
                
                base.Init();
                GameLog.InitMessage<T>();
            }//if end
            else
            {
                Destroy(this);
                Destroy(gameObject);
            }//else end
        }//Init() end

        /// <summary>
        /// Method called upon Destroying this Game Object or Scene Reload.
        /// This method was implemented in the Singleton Base class to make the Instance null, 
        /// because the Instance does not get destroyed upon scene reload it lives in the memory.
        /// This method was marked virtual so it can be overrided by the derived class
        /// for any extended functionality but do call base.OnDestroy() in your override method.
        /// </summary>
        public virtual void OnDestroy()
        {
            if(DontDestroy is false)
                Instance = null;
        }//OnDestroy() end

    }//class end
}//namespace end