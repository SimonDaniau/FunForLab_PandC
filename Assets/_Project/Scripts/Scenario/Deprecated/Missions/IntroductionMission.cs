
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
using FunForLab.Inventory;
using FunForLab.Modules;
using FunForLab.Scenario.Tasks;
using FunForLab.UI;

namespace FunForLab.Scenario.Missions
{
    public class IntroductionMission : Mission
    {
        private CutsceneModule _cutsceneModule;
        private QuizModule _quizModule;
        private MissionData _missionData;
    
        public override List<Objective> Load()
        {
            var obj = new List<Objective>();
            obj.Add(new ChapterTask(new List<Objective>()
            {
                new DisplayTextTask(""),
                new SetupTask(()=> PlayerInventory.Instance.SetInventoryVisibility(false)),
                new SetupTask(() => HighlightModule.HighlightWhenNotInTargetOrbitTree = false),
                new SetupTask(() => MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.Wide,false)),
                new SetupTask(() => _cutsceneModule.CameraLock(true)),
           
                new SetupTask(() => _cutsceneModule.Setup(new List<string>
                {
                    "Welcome to the FunForLab environment !".Localize(),
                    "Here is where you'll be working.".Localize(),
                    "Enjoy your stay !".Localize()
                })),
                new SetupTask(() => _cutsceneModule.SetCamera(CutsceneModule.SceneType.HospitalFront)),
                new SetupTask(() => _cutsceneModule.PlayCutscene(_missionData)),
                new SimpleTask("", () => _cutsceneModule.CurrentState.Playing == false)
            }, false));

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new SetupTask(()=> PlayerInventory.Instance.SetInventoryVisibility(true)),
                new SetupTask(() => MissionWindow.Instance.ChangeWindow(MissionWindow.WindowType.SquareNoCompletion,false)),
                /*new DisplayTextTask("Skip tutorial ?"),
            new SetupTask(() => _quizModule.PlayQuiz(
                new List<string>
                {
                    "No"
                },
                new List<string>
                {
                    "Yes"
                },
                new List<string>
                {
                },
                new List<string>
                {
                },
                _missionData)),
            new SimpleTask( () => _quizModule.CurrentState.Finished == true),
            new BranchTask( () => _quizModule.CurrentState.GivingExplanation == true, () => MissionManager.Instance.SkipTutorial = true)*/
            
            }, false));

            obj.Add(new ChapterTask(new List<Objective>()
            {
                new DisplayTextTask(""),
                new SetupTask(() => _cutsceneModule.CameraLock(false))
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
        }
    }
}