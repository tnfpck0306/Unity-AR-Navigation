using UnityEngine;
using System.Collections;

namespace ArchToolkit.Utils
{

    [System.Serializable]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        protected static T _instance;

        [SerializeField]
        private static Object _lock = new Object();

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed on application quit." +
                                     " Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                           " - there should never be more than 1 singleton!" +
                                           " Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>() as T;
                            singleton.name = "(singleton) " + typeof(T).ToString();

                            // DontDestroyOnLoad(singleton);
#if DEBUG
                            Debug.Log("[Singleton] An instance of " + typeof(T) +
                                      " is needed in the scene, so '" + singleton +
                                      "' was created with DontDestroyOnLoad.");
#endif
                        }
                        else
                        {
                            Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
                        }
                    }

                    return _instance;
                }
            }
        }
        
        protected static bool applicationIsQuitting = false;
        
        /// <summary>
        /// When Unity quits, it destroys Models in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost Model that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost Model.
        /// </summary>
        public virtual void OnDestroy()
        {
            _instance = null;
        }
        public virtual void NullInstance()
        {
            _instance = null;
        }

        public static bool IsInstanced()
        {
            _instance = (T)FindObjectOfType(typeof(T));

            if (FindObjectsOfType(typeof(T)).Length > 1)
            {
                Debug.LogError("[Singleton] Something went really wrong " +
                               " - there should never be more than 1 singleton!" +
                               " Reopening the scene might fix it.");
                return true;
            } 

            return (_instance != null);
        }
    }
}