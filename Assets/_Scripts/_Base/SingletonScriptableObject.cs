//Shady
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Shady.Core.Singletons
{
    [HideMonoScript]
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        //===================================================
        // PRIVATE FIELDS
        //===================================================
        private const  string ObjectPath      = "Resources";
        private const  string ObjectExtension = ".asset";
        private static T      _instance       = null;

        //===================================================
        // PROPERTIES
        //===================================================
        public static T Instance
        {
            get
            {
                if(_instance is null)
                {
                    string objectName = typeof(T).Name;
                    _instance = Resources.Load(objectName) as T;

                    if(_instance is null)
                    {
                        _instance = CreateInstance<T>();

    #if UNITY_EDITOR

                            string properPath = Path.Combine(Application.dataPath, ObjectPath);
                            if(!Directory.Exists(properPath))
                            {
                                if(!Directory.Exists("Assets/Resources"))
                                    UnityEditor.AssetDatabase.CreateFolder("Assets", ObjectPath);
                            }//if end

                            string fullPath = Path.Combine(Path.Combine("Assets", ObjectPath), $"{objectName}{ObjectExtension}");
                            UnityEditor.AssetDatabase.CreateAsset(_instance, fullPath);

    #endif
                    }//if end
                }//if end

                return _instance;
            }//get end
        }//Instance end

        //===================================================
        // METHODS
        //===================================================
    #if UNITY_EDITOR
        protected static void SelectHolder() => UnityEditor.Selection.activeObject = Instance;
    #endif

    // Implement this method in the derived class with its name replacing Test Object
    // #if UNITY_EDITOR
    //     [UnityEditor.MenuItem("Shady/Scriptable Objects/Test Object1", false, 1)]
    //     public static void CreateObject() => SelectHolder();
    // #endif

    }//class end

}//namespace end