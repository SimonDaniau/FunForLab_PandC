/*
 * Copyright 2023 Interreg V EMR 145 - FunForLab
 *
 * Licensed under the EUPL, Version 1.2 or – as soon they will be approved by the European Commission - subsequent versions of the EUPL (the "Licence");
 * You may not use this work except in compliance with the Licence.
 * You may obtain a copy of the Licence at:
 *
 * https://joinup.ec.europa.eu/software/page/eupl
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the Licence is distributed on an "AS IS" basis,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the Licence for the specific language governing permissions and limitations under the Licence.
 * 
 *  Author: Daniau Simon
 *  
 *  Project: Interreg V EMR 145 - FunForLab
 *  Website: http://funforlab.eu
 *  Project Date: March 2021 - August 2023
 *  Contributors:
 *      Centre de Recherche des Instituts Groupés de la Haute Ecole Libre Mosane (CRIG):
 *          - Isabelle Bragard
 *          - Birgit Quinting 
 *          - Sonia El Guendi
 *          - Annabelle Lejeune
 *          - Ingrid Hamer
 *          - Mélanie Zenner
 *          - Jérome Foguenne
 *      Centre de recherche et de formation continue de la Haute Ecole Namur Liège Luxembourg (FoRS):
 *          - Julien Lecointre
 *          - Simon Daniau
 *          - Laura Ramonfosse
 *          - Christophe Clément
 *          - Amandine Schreiber
 *      Ausbildungsakademie für Gesundheitsberufe, Uniklinik RWTH Aachen (UKAachen):
 *          - Eva Schönen
 *          - Giannina Lindt
 *          - Patricia Büts
 *          - Silvia Schneiders
 *          - Miriam Scheld
 *          - Monika Krichel-Frings
 *          - Christiane  Stickelmann
 *      Centre de Coopération Technique et Pédagogique (CeCoTePe):
 *          - Frédéric Kotnik
 *          - Brian Deschamps
 *          - Mélanie Zenner
 *          - Aurélien Bolkaerts
 *          - Guillaume Vilvorder
 *          - Maxime Palmisano
 *      Zuyd Hogeschool:
 *          - Olivier Segers
 *          - Olaf Brouwers
 *          - Jeroen Heijdeman
 *          - Marliene Bos
 *          - Ron Reuleaux 
 *      UC Leuven & Limburg (UCLL):
 *          - Eveline Strackx
 *          - Karolien Decamps
 *          - Evi Lemmens
 *          - Laura De Bock
 *          - Raf Donders
 *          - Evelyn Jans
 */ 
using System;
using System.Collections;
using System.Collections.Generic;
using FunForLab.Scenario;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FunForLab
{
    public class ScenarioChanger : MonoBehaviour
    {
        public static ScenarioChanger Instance;
        private Enums.ScenarioNames _currentScenario;

        public MissionsScenes[] MissionsScenesLinks;
        public ScenesNames[] Scenes;

        public LoadingScreen LoadingScreenPrefab;
        private LoadingScreen _loadingScreenCurrent;
        [HideInInspector] public List<AsyncOperation> ScenesLoading;
        private float _progress;

        public static bool IsMenuLoaded;
        public static bool IsScenarioLoaded;

        public bool IsMenuLoadedLabel;
        public bool IsScenarioLoadedLabel;

        [Serializable]
        public struct MissionsScenes
        {
            public Enums.ScenarioNames ScenarioName;
            public Enums.Scenes Scene;
        }

        [Serializable]
        public struct ScenesNames
        {
            public Enums.Scenes Scene;
            public string Name;
        }

        private void Awake()
        {
            transform.SetParent(null);
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
            _loadingScreenCurrent = FindObjectOfType<LoadingScreen>();
            if (_loadingScreenCurrent == null) _loadingScreenCurrent = Instantiate(LoadingScreenPrefab);
            _loadingScreenCurrent.transform.SetParent(transform.GetChild(0));
            _loadingScreenCurrent.transform.localScale = Vector3.one;
            var rect = _loadingScreenCurrent.GetComponent<RectTransform>();
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            _loadingScreenCurrent.gameObject.SetActive(false);
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                IsMenuLoaded = true;
                InitializeResumeButtonBG();
            }
        }


        public static void ChangeScenario(Enums.ScenarioNames scenarioName)
        {
            LoadAsync(ScenarioToSceneName(scenarioName));
            PlayerPrefs.SetInt("CurrentScenario", (int) scenarioName);
        }

        public static void LoadAsync(string scene, bool setAdditiveMenu = false)
        {
            DialogueManager.StopAllConversations();
            DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded);
            Instance.ScenesLoading = new List<AsyncOperation>();
            if (setAdditiveMenu) Instance.ScenesLoading.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            else Instance.ScenesLoading.Add(SceneManager.LoadSceneAsync(scene));
            Instance.StartCoroutine(Instance.GetSceneLoadProgress());
            PlayerPrefs.SetString("LastLoadedScene", scene);
            IsScenarioLoaded = true;
            IsMenuLoaded = false;
        }

        public IEnumerator GetSceneLoadProgress(bool addedTime = true, Action onFinishCallback = default)
        {
            _loadingScreenCurrent.SetProgression(0f);
            _loadingScreenCurrent.gameObject.SetActive(true);
            for (int i = 0; i < ScenesLoading.Count; i++)
            {
                var op = ScenesLoading[i];
                if (op == null) continue;
                while (!op.isDone)
                {
                    _progress = 0;
                    foreach (var item in ScenesLoading)
                    {
                        if (item != null)
                            _progress += item.progress;
                    }

                    _progress = (_progress / ScenesLoading.Count);
                    _loadingScreenCurrent.SetProgression(_progress);
                    yield return null;
                }
            }

            if (addedTime)
            {
                /* fake additional length*/
                float addTime = 0;
                float length = Random.Range(.2f, .4f);
                while (addTime < length)
                {
                    addTime += Time.deltaTime;

                    if (addTime >= length)
                    {
                        addTime = length;
                    }

                    _progress = Mathf.Lerp(0.9f, 1f, addTime / length);
                    _loadingScreenCurrent.SetProgression(_progress);
                    yield return null;
                }
            }
            //~~~~~~~~

            _loadingScreenCurrent.gameObject.SetActive(false);
            onFinishCallback?.Invoke();
        }

        public static void ChangeScenario(ScenarioNameComponent mission)
        {
            ChangeScenario(mission.ScenarioNameSelected);
        }

        private void Update()
        {
            IsMenuLoadedLabel = IsMenuLoaded;
            IsScenarioLoadedLabel = IsScenarioLoaded;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!IsMenuLoaded)
                    GoToMenu(true);
            }
        }

        public void GoToMenu(bool setAdditiveMenu)
        {
            IsMenuLoaded = true;
            DialogueManager.StopAllConversations();
            DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded);
            Instance.ScenesLoading = new List<AsyncOperation>();
            if (setAdditiveMenu)
                Instance.ScenesLoading.Add(SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive));
            else
                Instance.ScenesLoading.Add(SceneManager.LoadSceneAsync("Menu"));
            Instance.StartCoroutine(Instance.GetSceneLoadProgress(false, (() => { InitializeResumeButtonBG(); })));
        }

        private static void InitializeResumeButtonBG()
        {
            var sceneToResume = PlayerPrefs.GetString("LastLoadedScene", "default");
            Image resumeBG = FindObjectOfType<ScenarioResumeBGLocator>().GetComponent<Image>();
            if (sceneToResume == "default")
                resumeBG.color = Color.gray;
            else
                resumeBG.color = Color.white;
        }

        public void Resume()
        {
            var sceneToResume = PlayerPrefs.GetString("LastLoadedScene", "default");
            if (IsMenuLoaded && IsScenarioLoaded)
            {
                SceneManager.UnloadSceneAsync("Menu");
                IsMenuLoaded = false;
                if (sceneToResume == "Prologue" || sceneToResume == "Epilogue")
                {
                    var video = FindObjectOfType<ContentLocalizationVideo>();
                    if (video != null) video.StartVideo();
                }
            }
            else if (IsMenuLoaded && sceneToResume != "default" && sceneToResume != "Menu")
            {
                IsMenuLoaded = false;
                LoadAsync(sceneToResume);
            }
        }

        public static string SceneToSceneName(Enums.Scenes scene)
        {
            foreach (var item in Instance.Scenes)
            {
                if (item.Scene == scene)
                {
                    return item.Name;
                }
            }

            return "";
        }

        public static string ScenarioToSceneName(Enums.ScenarioNames scenarioName) =>
            SceneToSceneName(MissionToScene(scenarioName));

        public static Enums.Scenes MissionToScene(Enums.ScenarioNames scenarioName)
        {
            foreach (var item in Instance.MissionsScenesLinks)
            {
                if (scenarioName == item.ScenarioName)
                {
                    return item.Scene;
                }
            }

            return 0;
        }
    }
}