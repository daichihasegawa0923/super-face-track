using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFaceTrack.Util
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : MonoBehaviour
    {

        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != null && !Instance.Equals(this))
            {
                Destroy(gameObject);
                return;
            }

            if (Instance == null)
            {
                var manager = GetComponent<T>();
                if (manager == null)
                {
                    manager = gameObject.AddComponent<T>();
                }

                _instance = manager;
            }
        }
    }
}