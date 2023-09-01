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
using FunForLab.Automaton;
using UnityEngine;
using UnityEngine.Events;

namespace FunForLab.Interactables
{
    public class ToggleBlinkingButtonHandle : ButtonHandle
    {
        public float PulseBlinkTiming;
        public float PulseDuration;

        private float _nextPulse;
        private float _endPulse;

        public Material HighlightActive;
        public Material Active;

        private State _nextState;
        private bool _pulseState;
        private State _state;

        public UnityEvent ToggleOnBeginEvent;
        public UnityEvent ToggleOnEndEvent;
        public UnityEvent ToggleOffBeginEvent;
        public UnityEvent ToggleOffEndEvent;
        public event Action OnStateChange;

        public override void Awake()
        {
            Renderer = GetComponent<Renderer>();
            StateProvider = GetComponent<StateProvider>();
            if (StateProvider != null)
            {
                OnStateChange += () => StateProvider.OnStateChange();
                StateProvider.GetCurrentState = () => StateProvider.GetStateFull(_state == State.Off, _nextState == State.Off, _state == State.Pulsing);
            }
        }

        public override void Start()
        {
            base.Start();
            _state = State.Off;
            if (_state == State.Pulsing)
            {
                Debug.LogWarning("Starting state is pulsing, defaulting next state to Off", this);
                _nextState = State.Off;
                _endPulse = Time.time + PulseDuration;
                ToggleOffBeginEvent?.Invoke();
            }
        }

        private enum State
        {
            Off,
            Pulsing,
            On,
        }

        // Update is called once per frame
        public void Update()
        {
            if (_return || _state == State.Pulsing)
                MouseIn = false;
            switch (_state)
            {
                case State.Off:
                {
                    Renderer.sharedMaterial = MouseIn ? HighlightStandard : Standard;
                }
                    break;
                case State.On:
                {
                    Renderer.sharedMaterial = MouseIn ? HighlightActive : Active;
                }
                    break;
                case State.Pulsing :
                {
                    if (Time.time > _nextPulse)
                    {
                        _nextPulse = PulseBlinkTiming + Time.time;
                        _pulseState = !_pulseState;
                        Renderer.sharedMaterial = _pulseState ? HighlightStandard : HighlightActive;
                    }

                    if (Time.time > _endPulse)
                    {
                        _pulseState = false;
                        _state = _nextState;
                        OnStateChange?.Invoke();
                        if (_state == State.On)
                            ToggleOnEndEvent?.Invoke();
                        else
                            ToggleOffEndEvent?.Invoke();
                    }
                }
                    break;
            }
        }

        public void SetPulse(bool endStateOff )
        {
            _state = State.Pulsing;
            _endPulse = Time.time + PulseDuration;
            if (endStateOff)
            {
                _nextState = State.Off;
                ToggleOffBeginEvent?.Invoke();
            }
            else
            {
                _nextState = State.On;
                ToggleOnBeginEvent?.Invoke();
            }

            OnStateChange?.Invoke();
        }

        protected override void OnMouseDownFunction()
        {
            base.OnMouseDownFunction();
            if ( _return || _state == State.Pulsing) return;
            SetPulse(_state == State.Off);
        }
    }
}