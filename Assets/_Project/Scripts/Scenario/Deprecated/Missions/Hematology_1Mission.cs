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
using System.Collections.Generic;
using FunForLab.Analytics;
using FunForLab.Inventory;
using FunForLab.Modules;
using FunForLab.OrbitCamera;
using FunForLab.Scenario.Tasks;
using FunForLab.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FunForLab.Scenario.Missions
{
    public class Hematology_1Mission : Mission
    {
        public DiseaseTag DiseaseTag;
        private ReceptionModule _receptionModule;
        private HematologyAutomaton _hematologyAutomatonModule;
        private CutsceneModule _cutsceneModule;
        private QuizModule _quizModule;
        private MissionData _missionData;
        private HighlightModule _highlightModule;
        private TubeScannerModule _tubeScannerModule;

        public void OnValidate()
        {
            if (DiseaseTag == null) Debug.LogWarning("DiseaseTag field empty");
        }

        public override List<Objective> Load()
        {
            var orbitController = OrbitController.Instance;
            var obj = new List<Objective>();
            Data data = default;
            PatientData patientData = default;
            _missionData.PatientIsMale = Random.Range(0f, 100f) > 50f;
            _receptionModule.GetNextTube();

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new SetupTask(() =>
                {
                    MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.SquareNoCompletion, false);
                    _hematologyAutomatonModule.ScrollMenu.CanShrinkGrow = true;
                    _hematologyAutomatonModule.ScrollMenu.Shrink(0, 0);
                }),
                new DisplayTextTask(""),
                new SetupTask(() => _cutsceneModule.CameraLock(true)),
                new SetupTask(() => _cutsceneModule.Setup(new List<string>
                {
                    "Placeholder cutscene text 1",
                    "Placeholder cutscene text 2"
                })),
                new SetupTask(() => _cutsceneModule.SetCamera(CutsceneModule.SceneType.BloodDrawingPatientNurse)),
                new SetupTask(() => _cutsceneModule.SetupScene(_missionData)),
                new SetupTask(() => _cutsceneModule.PlayCutscene(_missionData)),
                new SimpleTask("", () => _cutsceneModule.CurrentState.Playing == false)
            }, false));

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new DisplayTextTask(" Quizz 1"),
                new SetupTask(() => _quizModule.PlayQuiz(
                    new List<string>
                    {
                        "Correct"
                    },
                    new List<string>
                    {
                        "Incorrect",
                    },
                    new List<string>
                    {
                        "Correct !",
                        "The answer was",
                        "Good job !"
                    },
                    new List<string>
                    {
                        "Incorrect !",
                        "The answer was",
                        "You will do better next time !"
                    },
                    _missionData, true)),
                new SimpleTask( () => _quizModule.CurrentState.Finished == true)
            }, false));

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new SetupTask(() => _cutsceneModule.Setup(new List<string>() { "This is the cutscene end" })),
                new SetupTask(() => _cutsceneModule.PlayCutscene(_missionData)),
                new SimpleTask( () => _cutsceneModule.CurrentState.Playing == false),
                new SetupTask(() => _cutsceneModule.SetCamera(CutsceneModule.SceneType.Origin)),
                new SetupTask(() => _cutsceneModule.CameraLock(false)),
                new SetupTask(() => MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.Square, true)),
                new SetupTask(() => _tubeScannerModule.DisplayModule.Clear()),
                new SetupTask(() => HighlightModule.HighlightWhenNotInTargetOrbitTree = true),
                new SetupTask(() => HighlightModule.HighlightHigherWhenTargetIsSubOrbit = true)
            }, false));

            Pickable tube = null; 
            obj.Add(new ChapterTask(new List<Objective>
            {
                new SetupTask(
                    () =>
                    {
                        _receptionModule.AddToTubeQueue(data = DataGenerator.Instance.ProcessDiseaseTag(CapColors.Caps.Purple, _missionData.PatientIsMale, DiseaseTag));
                        data.ReadingType = Enums.ReadingType.HematologyCompleteBloodCount;
                        patientData = DataGenerator.Instance.GetNewPatientData(ref data);
                    }),
                new SetupTask(() => tube = _receptionModule.SetNextTube(DiseaseTag).GetComponent<Pickable>()),
                new SetupTask(() => _highlightModule.Highlight("ReceptionRack")),
                new SimpleTask( "Get tube at reception by clicking on it", () => !_receptionModule.IsFull),
                new SetupTask(() => _highlightModule.UnHighlightAll())
            }));
            bool tubeScanned = false;
            obj.Add(new ChapterTask(new List<Objective>
            {
                new SetupTask(() =>
                {
                    _tubeScannerModule.ButtonReset();
                    _tubeScannerModule.CurrentSampledPatientData = patientData;
                    _tubeScannerModule.Setup(() => tubeScanned = true);
                } ),
                new SetupTask(() => _highlightModule.Highlight("ScannerDisplayModule")),
                new SimpleTask( "Scan sample on the computer near the reception desk", () => tubeScanned),
                new SetupTask(() => _highlightModule.UnHighlightAll())
            }));
            obj.Add(new ChapterTask(new List<Objective>
            {
                new SetupTask(() =>
                {
                    _hematologyAutomatonModule.ButtonReset();
                    _hematologyAutomatonModule.ButtonSetup("Load Sample", () => _hematologyAutomatonModule.LoadSampleManual(data),
                        x => !x.InTransition && !x.SampleLoaded);
                    _hematologyAutomatonModule.ButtonSetup("Start Process", _hematologyAutomatonModule.StartProcess ,
                        x => !x.InTransition && x.SampleLoaded && !x.ProcessFinished);
                    _hematologyAutomatonModule.ButtonSetup("Remove Sample", _hematologyAutomatonModule.RemoveSampleManual,
                        x => !x.InTransition && x.ProcessFinished);
                    _hematologyAutomatonModule.Setup();
                }),
                new SetupTask(() => _highlightModule.Highlight(_hematologyAutomatonModule)),
                new SimpleTask( "Load Sample in Hematology automaton", () => _hematologyAutomatonModule.CurrentState.SampleLoaded),
                new SetupTask(() => tube.TransferToWorldInvisible()),
                new SetupTask(() => _highlightModule.UnHighlightAll()),
                new SimpleTask( "Process sample", () => _hematologyAutomatonModule.CurrentState.ProcessFinished),
                new SetupTask(() => { _hematologyAutomatonModule.DisplayReading(Enums.ReadingType.HematologyCompleteBloodCount); }),
                new SetupTask(() => { _hematologyAutomatonModule.ScrollMenu.SetFunctionVisibility("Load Sample", false); }),
                new SimpleTask( "Check results on the screen and remove sample when done", () => !_hematologyAutomatonModule.CurrentState.SampleLoaded),
                new SetupTask(() => tube.TransferToInventory()),
                new SetupTask(() => _hematologyAutomatonModule.ScrollMenu.Shrink()),
                new SetupTask(() => _hematologyAutomatonModule.ScrollMenu.CanShrinkGrow = false),
            }));
        
            obj.Add(new ChapterTask(new List<Objective>()
            {
                new DisplayTextTask("Which is the most likely diagnostic ?"),
                new SetupTask(() => _quizModule.PlayQuiz(
                    new List<string>
                    {
                        DiseaseTag.DiseaseName
                    },
                    new List<string>
                    {
                        "Hemochromatosis",
                        "Leukemia",
                        "Thrombocythemia"
                    },
                    new List<string>
                    {
                        "Correct !",
                        "The answer is indeed "+DiseaseTag.DiseaseName+" !",
                        "Good job !"
                    },
                    new List<string>
                    {
                        "Incorrect !",
                        "The answer was "+DiseaseTag.DiseaseName+".",
                        "You will do better next time !"
                    },
                    _missionData, true)),
                new SimpleTask( () => _quizModule.CurrentState.Finished == true)
            }, false));
        
            obj.Add(new ChapterTask(new List<Objective>
            {
                new DisplayTextTask("This is the end of the hematology 1 scenario, Good job !", true),
                new SetupTask(() => MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.None, false)),
                new DisplayTextTask("")
            }, false));

            obj.Add(new SetupTask(() => _missionData.OnMissionFinished?.Invoke() ));

            return obj;
        }

        public override void Setup(MissionData data)
        {
            _missionData = data;
            if (data.DiseaseTag != null)
            {
                DiseaseTag = data.DiseaseTag;
            }

            if (data.ReceptionModule == null) throw new ArgumentNullException(nameof(data.ReceptionModule));
            _receptionModule = data.ReceptionModule;

            if (data.HematologyAutomaton == null) throw new ArgumentNullException(nameof(data.HematologyAutomaton));
            _hematologyAutomatonModule = data.HematologyAutomaton;

            if (data.CutsceneModule == null) throw new ArgumentNullException(nameof(data.CutsceneModule));
            _cutsceneModule = data.CutsceneModule;

            if (data.QuizModule == null) throw new ArgumentNullException(nameof(data.QuizModule));
            _quizModule = data.QuizModule;

            if (data.HighlightModule == null) throw new ArgumentNullException(nameof(data.HighlightModule));
            _highlightModule = data.HighlightModule;

            if (data.TubeScannerModule == null) throw new ArgumentNullException(nameof(data.TubeScannerModule));
            _tubeScannerModule = data.TubeScannerModule;
        }
    }
}