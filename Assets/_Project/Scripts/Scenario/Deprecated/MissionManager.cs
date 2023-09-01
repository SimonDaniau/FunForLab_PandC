using System;
using System.Collections;
using System.Collections.Generic;
using FunForLab.Localization;
using FunForLab.Modules;
using UnityEngine;
using UnityEngine.SceneManagement;
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
namespace FunForLab.Scenario
{
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance;
        public string ObjectiveDescription;
        public string ObjectiveStatus;

       
        public float CurrentMissionCompletion
        {
            get
            {
                if (_currentMission != null)
                    return _currentMission.Completion;
                else return 0f;
            }
        }

        public IMission ScenarioMission;
        public IMission TutorialMission;
        public IMission IntroMission;
        private MissionData _scenarioMissionData;
        private MissionData _tutorialMissionData;
        private MissionData _introMissionData;
        private MissionTracker _currentMission;

        public event Action<string> OnDescriptionChange;
        public event Action< int, int, int> OnMissionStepChange;
        public event Action<int, int> OnMissionChapterChange;
        public event Action<bool> OnNotifyKeyPressChange;

        private class MissionTracker
        {
            public MissionData MissionData;
            public float Completion;
            public List<Objective> Objectives;

            public MissionTracker( MissionData missionData, List<Objective> objectives)
            {
                MissionData = missionData;
                Completion = 0.0f;
                Objectives = objectives;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
/*
        public IEnumerator Initialize()
        {
            if (TutorialMission == null)
                TutorialMission = new NullMission();
            if (IntroMission == null)
                IntroMission = new NullMission();

            Action onMissionFinished;
            Action onIntroFinished;

            if (!Localizator.ContentProcessed) yield return null;

            _currentScenario = (ScenarioNames) PlayerPrefs.GetInt("CurrentScenario");
            switch (_currentScenario)
            {
                case ScenarioNames.Scenario1:
                {
                    onMissionFinished = () => StartCoroutine(DelayLoadScene(SceneToSceneName(Scenes.Menu)));
                    onIntroFinished = () => _currentMission = LoadMission(ScenarioMission, _scenarioMissionData);

                    _scenarioMissionData = new MissionData(
                        onMissionFinished,
                        ScenarioNames.Scenario1,
                        receptionModule: FindObjectOfType<ReceptionModule>(),
                        hematologyAutomaton: FindObjectOfType<HematologyAutomaton>(),
                        cutsceneModule: FindObjectOfType< CutsceneModule>(),
                        highlightModule: FindObjectOfType<HighlightModule>(),
                        tubeScannerModule: FindObjectOfType<TubeScannerModule>(),
                        quizModule : FindObjectOfType<QuizModule>()
                    );

                    _introMissionData = new MissionData(
                        onIntroFinished,
                        ScenarioNames.Tutorial,
                        cutsceneModule: FindObjectOfType<CutsceneModule>(),
                        quizModule : FindObjectOfType<QuizModule>()
                    );

                    _currentMission = LoadMission(IntroMission, _introMissionData);
                }
                    break;
                default:
                {
                    onMissionFinished = () => StartCoroutine(DelayLoadScene(SceneToSceneName(Scenes.Menu)));

                    _tutorialMissionData = new MissionData(
                        onMissionFinished,
                        ScenarioNames.Tutorial,
                        cutsceneModule: FindObjectOfType< CutsceneModule>(),
                        highlightModule: FindObjectOfType<HighlightModule>(),
                        quizModule : FindObjectOfType<QuizModule>()
                    );

                    _currentMission = LoadMission(TutorialMission, _tutorialMissionData);
                }
                    break;
            }
        }
*/
        IEnumerator DelayLoadMission(IMission mission, MissionData missionData, float delay = 3f)
        {
            yield return new WaitForSeconds(delay);
            _currentMission = LoadMission(mission, missionData);
        }

        IEnumerator DelayLoadScene(string scene, float delay = 3f)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(scene);
        }

        private MissionTracker LoadMission(IMission mission, MissionData missionData)
        {
            StopAllCoroutines();
            var ret = new MissionTracker(missionData, mission.Load(missionData));
            StartCoroutine(StartMissions(ret));
            return ret;
        }

        public void ChangeDescription(string msg)
        {
            ObjectiveDescription = msg;
            OnDescriptionChange?.Invoke(msg);
        }

        public void DisplayKeyPressNotifier(bool state) => OnNotifyKeyPressChange?.Invoke(state);

        IEnumerator StartMissions(MissionTracker missionTracker)
        {
            int stepCount = 0;
            int step = 1;
            int chapter = 1;
            string msg = "";
            missionTracker.Completion = 0;

            int totalChapters = missionTracker.Objectives.Count;
            int totalSteps = 0;
            for (var i = 0; i < missionTracker.Objectives.Count; i++)
            {
                var currentObjectiveSubCount = missionTracker.Objectives[i].Count();
                if (currentObjectiveSubCount > 0)
                    totalSteps += currentObjectiveSubCount;
            }

            foreach (var item in missionTracker.Objectives)
            {
                var currentObjectiveSubCount = item.Count();
                int totalStepsInChapter = currentObjectiveSubCount > 0 ? currentObjectiveSubCount : 0;
                while (!item.IsFinished)
                {
                    var desc = item.Description();
                    if (desc != msg && desc != "")
                    {
                        msg = desc;
                        if (totalStepsInChapter > 1)
                            ObjectiveStatus = $"Chapter {chapter} - Step {step}/{totalStepsInChapter}";
                        else
                            ObjectiveStatus = $"Chapter {chapter}";
                        ObjectiveDescription = msg;
                        //print(ObjectiveDescription);
                        if (currentObjectiveSubCount > 0)
                        {
                            missionTracker.Completion = stepCount / (float) totalSteps;

                            OnDescriptionChange?.Invoke(msg);
                            OnMissionStepChange?.Invoke(chapter, step, totalStepsInChapter);
                            step ++;
                            stepCount++;
                        }
                        else if (currentObjectiveSubCount < 0)
                        {
                            OnDescriptionChange?.Invoke(msg);
                        }
                    }

                    if (item.Criteria())
                    {
                        item.Finish();
                        // print($"Objective {chapter} : Completed !");
                        OnMissionChapterChange?.Invoke(chapter, totalChapters);
                        if (item.Count() > 0)
                        {
                            chapter++;
                        }

                        step = 1;
                    }

                    yield return null;
                }

                missionTracker.Completion = 1f;
                OnMissionStepChange?.Invoke(chapter, step, totalStepsInChapter);
                OnDescriptionChange?.Invoke("");
            }
        }
    }
}