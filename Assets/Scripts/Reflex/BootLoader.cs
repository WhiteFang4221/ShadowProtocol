using Reflex.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BootLoader : MonoBehaviour
{
     [SerializeField] private string _sceneAddress = "Main";
    
     private void Start()
     {
         Addressables.LoadSceneAsync(_sceneAddress, activateOnLoad: false).Completed += OnSceneLoaded;
     }

     private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
     {
         if (handle.Status == AsyncOperationStatus.Succeeded)
         {
             var scene = handle.Result.Scene;
            
             ReflexSceneManager.PreInstallScene(scene, builder =>
             {
                 // Здесь можно добавить глобальные зависимости, например:
                 builder.AddSingleton("SomeGlobalDependency");
             });

             handle.Result.ActivateAsync();
         }
         else
         {
             Debug.LogError("Failed to load scene via Addressables: " + _sceneAddress);
         }
     }
}
