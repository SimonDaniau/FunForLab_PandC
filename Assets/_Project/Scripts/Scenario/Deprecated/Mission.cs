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
using System.Runtime.InteropServices;
using FunForLab.Analytics;
using FunForLab.Modules;
using FunForLab.OrbitCamera;
using UnityEngine;

namespace FunForLab.Scenario
{
    public interface IMission
    {
        List<Objective> Load();
        void Setup(MissionData data);
        List<Objective> Load(MissionData data);
    }

    public class NullMission : IMission
    {
        public List<Objective> Load()
        {
            Debug.LogWarning("No Mission Assigned");
            return default;
        }

        public void Setup(MissionData data)
        {
            Debug.LogWarning("No Mission Assigned");
        }

        public List<Objective> Load(MissionData data)
        {
            Debug.LogWarning("No Mission Assigned");
            return default;
        }
    }

    public abstract class Mission : MonoBehaviour, IMission
    {
        public string SceneToLoadName;

        public virtual List<Objective> Load()
        {
            return new List<Objective>();
        }

        public abstract void Setup(MissionData data);

        public virtual List<Objective> Load(MissionData data)
        {
            Setup(data);
            return Load();
        }
    }

    public struct MissionData
    {
        public Enums.ScenarioNames SceneToLoad;
        public bool MissionFinished;
        public Action OnMissionFinished;
        public DiseaseTag DiseaseTag;
        public OrbitController OrbitController;
        public ReceptionModule ReceptionModule;
        public HematologyAutomaton HematologyAutomaton;
        public CutsceneModule CutsceneModule;
        public QuizModule QuizModule;
        public HighlightModule HighlightModule;
        public TubeScannerModule TubeScannerModule;
        public bool PatientIsMale;

        public  MissionData(
            Action onMissionFinished,
            Enums.ScenarioNames sceneToLoad,
            [Optional] HematologyAutomaton hematologyAutomaton,
            [Optional] bool patientIsMale,
            [Optional] DiseaseTag diseaseTag,
            [Optional] OrbitController orbitController,
            [Optional] CutsceneModule cutsceneModule,
            [Optional] ReceptionModule receptionModule,
            [Optional] HighlightModule highlightModule,
            [Optional] TubeScannerModule tubeScannerModule,
            [Optional] QuizModule quizModule)
        {
            MissionFinished = false;
            SceneToLoad = sceneToLoad;
            OnMissionFinished = onMissionFinished;
            PatientIsMale = patientIsMale;
            DiseaseTag = diseaseTag;
            OrbitController = orbitController;
            ReceptionModule = receptionModule;
            CutsceneModule = cutsceneModule;
            HematologyAutomaton = hematologyAutomaton;
            HighlightModule = highlightModule;
            TubeScannerModule = tubeScannerModule;
            QuizModule = quizModule;
        }
    }
}