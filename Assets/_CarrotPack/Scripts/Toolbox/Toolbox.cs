using System.Collections.Generic;
using UnityEngine;

namespace CarrotPack
{
    [DisallowMultipleComponent]
    public class Toolbox : Singleton<Toolbox>
    {
        protected Toolbox() { } // guarantee this will be always a singleton only - can't use the constructor!

        private List<IManager> managerList = new List<IManager>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        ///  Calls all the necessary managers upon game start
        /// </summary>
        private void Start()
        {
            // Adds all manager to list first
            foreach (IManager manager in gameObject.GetComponents<IManager>())
            {
                managerList.Add(manager);
            }

            // Separates Init to avoid manager dependency
            for (int i = 0; i < managerList.Count; ++i)
            {
                managerList[i].InitializeManager();
            }

            // To initialize manager to toolbox on run time example:
            //GetManager<CameraManager>();
            //GetManager<UnitManager>();
            //GetManager<InputManager>();
            //GetManager<UIManager>();
        }

        /// <summary>
        /// Tries to get a manager from the toolbox.
        /// If manager does not exist, it will create the manager component,
        /// adds the component to the Toolbox gameobject (instance)
        /// and saves it in managerList.
        /// 
        /// Example Usage (Assuming InputManager is already created):
        /// InputManager manager = Toolbox.Instance.GetManager<InputManager>();
        /// </summary>

        public T GetManager<T>() where T : MonoBehaviour, IManager
        {
            // Check if exist
            for (int i = 0; i < managerList.Count; ++i)
            {
                if (managerList[i] is T)
                {
                    //Debug.Log(typeof(T).ToString() + " exist");
                    return managerList[i] as T;
                }
            }

            // If does not exist
            Debug.Log(typeof(T).ToString() + " doesn't exist, adding component to ToolBox");

            T newManager = gameObject.AddComponent<T>();
            managerList.Add(newManager);
            newManager.InitializeManager();
            return newManager;
        }


    }
}