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
using System.Linq;
using FunForLab.NPC;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FunForLab.Dialogue
{
    public class DebugModuleStorySkip : MonoBehaviour
    {
        public static DebugModuleStorySkip Instance;
        private GameObject _robot;
        private NPCWaypoint _inEntranceNextToLockerDoorWaypoint;

        public enum StoryStep
        {
            Null,
            Tuto_Start,
            Tuto_RobotFellNeedsRepair,
            Tuto_RobotRepaired,
            Tuto_RobotInLockerRoom,
            Tuto_RobotInReception,
            Tuto_RobotInLaboratory,
            Tuto_RobotInArchives,
            Tuto_RobotInOffice,
            Tuto_RobotInBedroom,
        }

        public Enums.ScenarioNames CurrentScenario;
        public Dictionary<Enums.ScenarioNames, List<StoryStep>> ValidStepsForScenario;
        public Dictionary<StoryStep, Func<bool, IEnumerator>> StepSetup;

        public StoryStep CurrentStep;
        public StoryStep StepToGo;
        public bool Go;
        public bool NextStep;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        void Start()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            //InitializeVariables();
           // InitializeValidStepsForCurrentScenes();
           // SetStartingStepForCurrentScene();
            //SetupActionsToDoToGetAtGivenStep();
        }

        private void InitializeVariables()
        {
            _robot = FindObjectsOfType<DialogueActor>().First(x => x.GetActorName() == "Robot").gameObject;
            _inEntranceNextToLockerDoorWaypoint = NPCWaypoint.AllWaypoints.First(x => x.WaypointName == "InEntranceNextToLockerDoor");
        }

        void Update()
        {
            if (Go)
            {
                Go = false;
                switch (StepToGo)
                {
                    case StoryStep.Tuto_Start:
                    case StoryStep.Tuto_RobotFellNeedsRepair:
                    case StoryStep.Tuto_RobotRepaired:
                    {
                        bool isStepBefore = (int) CurrentStep > (int) StepToGo;
                        bool isStepJustAfter = (int) CurrentStep + 1 == (int) StepToGo;
                        if (ValidStepsForScenario[CurrentScenario].Contains(StepToGo) && CurrentStep != StepToGo)
                            StartCoroutine(StepSetup[StepToGo].Invoke(isStepBefore || !isStepJustAfter));
                    }
                        break;
                }
            }

            if (NextStep)
            {
                NextStep = false;
                StepToGo = (StoryStep) ((int) CurrentStep + 1);
                Go = true;
            }
        }

        private void SetupActionsToDoToGetAtGivenStep()
        {
            StepSetup = new Dictionary<StoryStep, Func<bool, IEnumerator>>();
            StepSetup.Add(StoryStep.Tuto_Start, Tuto_Start_CR);
            StepSetup.Add(StoryStep.Tuto_RobotFellNeedsRepair, Tuto_RobotFellNeedsRepair_CR);
            StepSetup.Add(StoryStep.Tuto_RobotRepaired, Tuto_RobotRepaired_CR);
        }

        private IEnumerator Tuto_RobotRepaired_CR(bool loadPrevious)
        {
            if (loadPrevious)
                yield return StepSetup[StoryStep.Tuto_RobotFellNeedsRepair].Invoke(true);

            DialogueLua.SetVariable("CircuitBoardFound", true);
            DialogueLua.SetVariable("DamagedRobotFound", true);
            DialogueLua.SetVariable("RobotRepaired", true);
            DialogueLua.SetVariable("RobotInEntranceNextToLockerDoor", true);
            DialogueLua.SetVariable("RobotFound", true);
            var RobotNpcController = _robot.GetComponent<NPCController>();
            RobotNpcController.GotoWaypoint(_inEntranceNextToLockerDoorWaypoint, 0.01f);
            var indic = _robot.GetComponent<IndicatorUpdater>();
            indic.SetIndicator(2);
            yield return null;
            CurrentStep = StoryStep.Tuto_RobotRepaired;
        }

        private IEnumerator Tuto_RobotFellNeedsRepair_CR(bool loadPrevious)
        {
            if (loadPrevious)
                yield return StepSetup[StoryStep.Tuto_Start].Invoke(true);

            DialogueLua.SetVariable("CircuitBoardFound", false);
            DialogueLua.SetVariable("DamagedRobotFound", false);
            DialogueLua.SetVariable("RobotRepaired", false);
            DialogueLua.SetVariable("RobotInEntranceNextToLockerDoor", false);
            DialogueLua.SetVariable("RobotFound", true);
            var anim = _robot.GetComponent<Animator>();
            anim.CrossFade("FallFromShelf", 0f, 0, 1f);
            var indic = _robot.GetComponent<IndicatorUpdater>();
            indic.SetIndicator(2);
            yield return null;
            anim.enabled = false;
            CurrentStep = StoryStep.Tuto_RobotFellNeedsRepair;
        }

        private IEnumerator Tuto_Start_CR(bool loadPrevious)
        {
            var loading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            while (!loading.isDone)
            {
                yield return null;
            }

            InitializeVariables();
            CurrentStep = StoryStep.Tuto_Start;
        }

        private void SetStartingStepForCurrentScene()
        {
            CurrentStep = StoryStep.Null;

            switch (CurrentScenario)
            {
                case Enums.ScenarioNames.Tutorial:
                    CurrentStep = StoryStep.Tuto_Start;
                    break;
            }
        }

        private void InitializeValidStepsForCurrentScenes()
        {
            ValidStepsForScenario = new Dictionary<Enums.ScenarioNames, List<StoryStep>>();

            ValidStepsForScenario.Add(Enums.ScenarioNames.Tutorial, new List<StoryStep>
            {
                StoryStep.Tuto_Start,
                StoryStep.Tuto_RobotFellNeedsRepair,
                StoryStep.Tuto_RobotRepaired,
                StoryStep.Tuto_RobotInLockerRoom,
                StoryStep.Tuto_RobotInReception,
                StoryStep.Tuto_RobotInLaboratory,
                StoryStep.Tuto_RobotInArchives,
                StoryStep.Tuto_RobotInOffice,
                StoryStep.Tuto_RobotInBedroom
            });
        }
    }
}