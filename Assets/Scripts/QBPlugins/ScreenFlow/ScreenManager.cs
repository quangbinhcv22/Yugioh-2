using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace QBPlugins.ScreenFlow
{
    [DefaultExecutionOrder(int.MinValue)]
    public class ScreenManager : MonoBehaviour
    {
        [ShowInInspector] public static string debug;
        
        #region MonoBehaviour

        public static Canvas Main;
        public static Transform Root;

        private void OnEnable()
        {
            Main = GetComponent<Canvas>();
            Root = transform;
        }

        #endregion


        #region Data

        private static readonly List<Screen> CurrentWindows = new();

        #endregion


        #region APIs


        public static void ReleaseAll_ToMain()
        {
            foreach (Transform child in Root)
            {
                Destroy(child.gameObject);
            }
            
            CurrentWindows.Clear();
            
            Open(ScreenKey.MainHome);
        }
        
        [Button]
        public static void Open(string screenKey)
        {
            switch (TypeOf(screenKey))
            {
                case ScreenType.Window:
                    OpenWindow(screenKey);
                    break;
                case ScreenType.Popup:
                    OpenPopup(screenKey);
                    break;
            }
        }


        [Button]
        private static async void OpenWindow(string screenKey)
        {
            await Close_CurrentWindow();

            var screen = await GetScreen(screenKey);
            
            CurrentWindows.Add(screen);
            screen.PresentOpen();
        }

        private static async void OpenPopup(string screenKey)
        {
            var screen = await GetScreen(screenKey);
            screen.PresentOpen();
        }


        // [Button]
        public static async void ClosePopup(string screenKey)
        {
            var screen = CurrentWindow;
            if (screen == null) return;

            if (screen.useOnce)
            {
                await screen.PresentClose();
                Release(screen.Key);
            }
            else
            {
                screen.PresentClose();
            }
        }

        [Button]
        public static async UniTask Close_CurrentWindow()
        {
            var window = CurrentWindow;
            if (window == null) return;

            if (window.useOnce)
            {
                await window.PresentClose();
                Release(window.Key);
            }
            else
            {
                window.PresentClose();
            }
            
            CurrentWindows.Remove(window);
        }

        private static void Close_Popup(string screenKey)
        {
        }

        #endregion


        #region Memory

        private static readonly Dictionary<string, GameObject> LoadedScreens = new();
        private static readonly Dictionary<string, Screen> InstantiatedScreens = new();


        private static AsyncOperationHandle<GameObject> Instantiate(string screenKey)
        {
            var handle = Addressables.InstantiateAsync(screenKey, Root);
            OnLoad(screenKey, handle);

            return handle;
        }

        private static AsyncOperationHandle<GameObject> Load(string screenKey)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(screenKey);
            OnLoad(screenKey, handle);

            return handle;
        }

        private static void OnLoad(string screenKey, AsyncOperationHandle<GameObject> handle)
        {
            if (IsLoaded(screenKey)) return;

            try
            {
                handle.Completed += operationHandle =>
                {
                    LoadedScreens.Add(screenKey, operationHandle.Result);
                };
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                throw;
            }
        }

        private static void Release(string screenKey)
        {
            if (!IsLoaded(screenKey)) return;

            var releaseScreen = LoadedScreens[screenKey];
            LoadedScreens.Remove(screenKey);

            Addressables.Release(releaseScreen);
        }

        #endregion


        #region Query

        private static ScreenType TypeOf(string screenKey)
        {
            const string windowSignal = "screen";
            const string popupSignal = "popup";

            if (screenKey.Contains(windowSignal)) return ScreenType.Window;
            if (screenKey.Contains(popupSignal)) return ScreenType.Popup;
            return ScreenType.Unset;
        }

        private static bool IsLoaded(string screenKey) => LoadedScreens.ContainsKey(screenKey);
        private static bool IsInstantiated(string screenKey) => InstantiatedScreens.ContainsKey(screenKey);

        public static Screen CurrentWindow => !CurrentWindows.Any() ? null : CurrentWindows.Last();

        private static bool HaveWindow => CurrentWindows.Any();
        

        private static Screen GetInstantiated(string screenKey)
        {
            return IsInstantiated(screenKey) ? InstantiatedScreens[screenKey] : null;
        }

        private static async UniTask<Screen> GetScreen(string screenKey)
        {
            var window = GetInstantiated(screenKey);
            window ??= (await Instantiate(screenKey)).GetComponent<Screen>();

            return window;
        }

        #endregion
    }
}