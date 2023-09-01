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
using FunForLab.Modules;
using FunForLab.OrbitCamera;
using FunForLab.Scenario.Tasks;
using FunForLab.UI;
using Random = UnityEngine.Random;

namespace FunForLab.Scenario.Missions
{
    public class TutorialMission : Mission
    {
        private CutsceneModule _cutsceneModule;
        private QuizModule _quizModule;
        private MissionData _missionData;
        private HighlightModule _highlightModule;

        public override List<Objective> Load()
        {
            var orbitController = OrbitController.Instance;
            var obj = new List<Objective>();
            Data data = default;
            PatientData patientData = default;
            _missionData.PatientIsMale = Random.Range(0f, 100f) > 50f;

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new SetupTask(() =>
                {
                    MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.SquareNoCompletion, false);
                }),
                new DisplayTextTask(""),
                new SetupTask(() => _cutsceneModule.CameraLock(true)),
                new SetupTask(() => _cutsceneModule.Setup(new List<string>
                {
                    "Tuto_Cutscene_1".Localize(),
                    "Tuto_Cutscene_2".Localize(),
                })),
                new SetupTask(() => _cutsceneModule.SetCamera(CutsceneModule.SceneType.BloodDrawingPatientNurse)),
                new SetupTask(() => _cutsceneModule.SetupScene(_missionData)),
                new SetupTask(() => _cutsceneModule.PlayCutscene(_missionData)),
                new SimpleTask("", () => _cutsceneModule.CurrentState.Playing == false)
            }, false));

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new DisplayTextTask("Tuto_Quizz_1_Question".Localize()),
                new SetupTask(() => _quizModule.PlayQuiz(
                    new List<string>
                    {
                        "4"
                    },
                    new List<string>
                    {
                        "3",
                        "5",
                        "8"
                    },
                    new List<string>
                    {
                        "Tuto_Quizz_1_CorrectFollowup_1".Localize(),
                        "Tuto_Quizz_1_CorrectFollowup_2".Localize(),
                        "Tuto_Quizz_1_CorrectFollowup_3".Localize()
                    },
                    new List<string>
                    {
                        "Tuto_Quizz_1_IncorrectFollowup_1".Localize(),
                        "Tuto_Quizz_1_IncorrectFollowup_2".Localize(),
                        "Tuto_Quizz_1_IncorrectFollowup_3".Localize()
                    },
                    _missionData, true)),
                new SimpleTask( () => _quizModule.CurrentState.Finished == true)
            }, false));

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new SetupTask(() => _cutsceneModule.Setup(new List<string>() { "Tuto_Cutscene_3".Localize() })),
                new SetupTask(() => _cutsceneModule.PlayCutscene(_missionData)),
                new SimpleTask( () => _cutsceneModule.CurrentState.Playing == false),
                new SetupTask(() => _cutsceneModule.SetCamera(CutsceneModule.SceneType.Origin)),
                new SetupTask(() => _cutsceneModule.CameraLock(false)),
                new SetupTask(() => MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.Square, true)),
                new SetupTask(() => HighlightModule.HighlightWhenNotInTargetOrbitTree = true),
                new SetupTask(() => HighlightModule.HighlightHigherWhenTargetIsSubOrbit = true)
            }, false));

            obj.Add(new ChapterTask(new List<Objective>
            {
                new SetupTask(() => _highlightModule.HighlightAll()),
                new OrbitControllerTask( orbitController, "Tuto_Orbit_1".Localize(), OrbitControllerTask.OrbitSubTask.OrbitMoveIn),
                new SetupTask(() => _highlightModule.UnHighlightAll()),
                new OrbitControllerTask( orbitController, "Tuto_Orbit_2".Localize(), OrbitControllerTask.OrbitSubTask.OrbitMoveAround),
                new OrbitControllerTask( orbitController, "Tuto_Orbit_3".Localize(), OrbitControllerTask.OrbitSubTask.OrbitMoveOut),
            }));

            obj.Add(new ChapterTask(new List<Objective>
            {
                new DisplayTextTask("Tuto_End".Localize(), true),
                new SetupTask(() => MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.None, false)),
                new DisplayTextTask("")
            }, false));

            obj.Add(new SetupTask(() => _missionData.OnMissionFinished?.Invoke() ));

            return obj;
        }

        public override void Setup(MissionData data)
        {
            _missionData = data;

            if (data.CutsceneModule == null) throw new ArgumentNullException(nameof(data.CutsceneModule));
            _cutsceneModule = data.CutsceneModule;

            if (data.QuizModule == null) throw new ArgumentNullException(nameof(data.QuizModule));
            _quizModule = data.QuizModule;

            if (data.HighlightModule == null) throw new ArgumentNullException(nameof(data.HighlightModule));
            _highlightModule = data.HighlightModule;
        }
    }
}