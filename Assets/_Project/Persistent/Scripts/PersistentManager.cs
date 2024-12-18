using UnityEngine;
using System.Linq;

namespace Persistent
{
    [DefaultExecutionOrder(-1)]
    public class PersistentManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _managers;
        private static bool _instancePresentOnScene;

        private void Awake()
        {
            if (TryCreateInstance())
            {
                CreatePersistentManagers();
            }
        }
        
        private bool TryCreateInstance()
        {
            if (_instancePresentOnScene)
            {
                Destroy(gameObject);
                return false;
            }
            _instancePresentOnScene = true;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        
        private void CreatePersistentManagers()
        {
            _managers.ToList().ForEach(manager=> Instantiate(manager, transform));
        }        
    }
}