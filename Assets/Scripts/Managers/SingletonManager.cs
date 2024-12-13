using System;
using UnityEngine;

namespace PSG.IsleOfColors.Managers
{
    public abstract class SingletonManager<T> : MonoBehaviour where T : UnityEngine.Object 
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = FindFirstObjectByType<T>();

                if (instance == null)
                    instance = new GameObject(typeof(T).Name, new Type[] { typeof(T) }).GetComponent<T>();

                return instance;
            }
        }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
