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
using FunForLab.Analytics;
using FunForLab.Automaton;
using FunForLab.Dialogue;
using FunForLab.Interactables;
using FunForLab.Inventory;
using UnityEngine;
using UnityEngine.Events;


namespace FunForLab.Modules
{
    public class HematologyAutomaton : HighlightableObject
    {
        public ToggleBlinkingButtonHandle BlueButton;
        public ControlledHandle Door;
        public ScrollMenu ScrollMenu;
        public GameObject ProcessIndicator;
        public Material ProcessIndicatorDefault;
        public Material ProcessIndicatorWorking;
        public Material ProcessIndicatorFinished;

        public Data CurrentSampledData;
        public DisplayModule DisplayModule;

        public float CurrentError;
        public float ErrorIncrement;
        public State CurrentState { get; private set; }

        private Renderer _processRend;

        private string _buttonBeingCreatedName;
        private string _buttonBeingCreatedLuaCondition;
        private string _buttonBeingCreatedLuaCode;
        private string _buttonBeingCreatedEvent;
        public List<ItemAction> events;
        public Dictionary<string, GameObject> NamedButtons;

        private bool _delayedEventPlaying;

        public class State
        {
            public bool InTransition;
            public bool DoorOpened;
            public bool SampleLoaded;
            public bool ProcessFinished;
        }

        [Serializable]
        public class ItemAction
        {
            public string ActionName;
            public UnityEvent ActionEvent;
            public float delay;
            public UnityEvent DelayedEvent;
        }

        public Action<State> OnStateChange;

        private void Awake()
        {
            _processRend = ProcessIndicator.GetComponent<Renderer>();
            _processRend.sharedMaterial = ProcessIndicatorDefault;
            NamedButtons = new Dictionary<string, GameObject>();
        }

        public void ButtonReset()
        {
            for (int i = 0; i < ScrollMenu.ButtonsGroupHolder.childCount; i++)
            {
                ScrollMenu.RemoveButton(ScrollMenu.ButtonsGroupHolder.GetChild(i).gameObject);
            }

            OnStateChange = null;
        }

        public void AddButton(string functionName)
        {
            _buttonBeingCreatedName = functionName;
        }

        public void RemoveButton(string functionName)
        {
            ScrollMenu.RemoveButton(NamedButtons[functionName]);
        }

        public void SetButtonLuaCode(string luaCode)
        {
            _buttonBeingCreatedLuaCode = luaCode;
        }

        public void SetButtonCondition(string luaCondition)
        {
            _buttonBeingCreatedLuaCondition = luaCondition;
        }

        public void SetButtonEvent(string eventName)
        {
            _buttonBeingCreatedEvent = eventName;
        }

        public void ConfirmButton()
        {
            GameObject go = default;

            if (NamedButtons.ContainsKey(_buttonBeingCreatedName))
            {
                _buttonBeingCreatedName = "";
                _buttonBeingCreatedLuaCondition = "";
                return;
            }
            go = ButtonSetup(_buttonBeingCreatedName, _buttonBeingCreatedLuaCode, _buttonBeingCreatedEvent, events,
                this, _buttonBeingCreatedLuaCondition);
            NamedButtons.Add(_buttonBeingCreatedName, go);
            _buttonBeingCreatedName = "";
            _buttonBeingCreatedLuaCondition = "";
        }

        public void Setup()
        {
            CurrentState = new State();
            OnStateChange?.Invoke(CurrentState);
        }

        public GameObject ButtonSetup(string functionName, string luaCode, string luaEvent, List<ItemAction> events,
            HematologyAutomaton source, string luaCondition)
        {
            var res = ScrollMenu.AddButton(functionName, luaCode, luaEvent, events, source, luaCondition);
            res?.SetActive(ScrollMenu.GetFunctionAllowed(functionName));
            return res;
        }

        public GameObject ButtonSetup(string functionName, Action function)
        {
            var res = ScrollMenu.AddButton(functionName, function);
            res?.SetActive(ScrollMenu.GetFunctionAllowed(functionName));
            return res;
        }

        public GameObject ButtonSetup(string functionName, Action function, Predicate<State> stateWhenAvailable)
        {
            var res = ScrollMenu.AddButton(functionName, function);
            OnStateChange += x =>
                res?.SetActive(ScrollMenu.GetFunctionAllowed(functionName) && stateWhenAvailable.Invoke(x));
            return res;
        }

        public string GetReading(Enums.ReadingType reading)
        {
            if (CurrentSampledData == null) return "No data results stored";
            switch (reading)
            {
                case Enums.ReadingType.HematologyCompleteBloodCount:
                    return CurrentSampledData.GetFullReading(CurrentError);
                default: throw new NotImplementedException();
            }
        }

        public void DisplayReading(Enums.ReadingType reading)
        {
            if (CurrentSampledData == null) return;
            DisplayModule.DisplayReading(reading, CurrentSampledData);
        }

        public void LoadSampleManual(Data tubeData)
        {
            CurrentState.SampleLoaded = true;
            CurrentSampledData = tubeData;
            OnStateChange?.Invoke(CurrentState);
        }

        public void RemoveSampleManual()
        {
            _processRend.sharedMaterial = ProcessIndicatorDefault;
            CurrentState.ProcessFinished = false;
            CurrentState.SampleLoaded = false;
            CurrentSampledData = null;
            OnStateChange?.Invoke(CurrentState);
        }

        public void OpenDoorManual() => StartCoroutine(ChangeDoorStateRoutine(true));
        public void CloseDoorManual() => StartCoroutine(ChangeDoorStateRoutine(false));
        public void StartProcess() => StartCoroutine(StartProcessRoutine());

        public void Calibrate()
        {
            CurrentError = 0;
        }

        public void DelayedEvent(Action eventItem, float delay)
        {
            if (_delayedEventPlaying) return;
            StartCoroutine(DelayedEventCR(eventItem, delay));
        }

        private IEnumerator DelayedEventCR(Action eventItem, float delay)
        {
            if (_delayedEventPlaying) yield return null;
            _delayedEventPlaying = true;
            yield return new WaitForSeconds(delay);
            eventItem?.Invoke();
            _delayedEventPlaying = false;
        }

        private IEnumerator StartProcessRoutine()
        {
            _processRend.sharedMaterial = ProcessIndicatorWorking;
            CurrentState.InTransition = true;
            OnStateChange?.Invoke(CurrentState);
            CurrentError += ErrorIncrement;
            float duration = 3f;
            float elapsedTime = 0;
            BlueButton.PulseDuration = 3f;
            BlueButton.SetPulse(true);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _processRend.sharedMaterial = ProcessIndicatorFinished;
            CurrentState.InTransition = false;
            CurrentState.ProcessFinished = true;
            OnStateChange?.Invoke(CurrentState);
        }

        private IEnumerator ChangeDoorStateRoutine(bool open)
        {
            CurrentState.InTransition = true;
            OnStateChange?.Invoke(CurrentState);

            if (open)
                Door.OnFunction();
            else
                Door.OffFunction();
            float duration = 3f;
            BlueButton.PulseDuration = duration;
            BlueButton.SetPulse(!open);
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            CurrentState.InTransition = false;
            CurrentState.DoorOpened = open;
            OnStateChange?.Invoke(CurrentState);
        }
    }
}