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
using UnityEngine;
using UnityEngine.Events;

namespace FunForLab.Interactables
{
    public class ToggleHandle : ButtonHandle
    {
        public Material HighlightActive;
        public Material Active;

        public State StartingState;
        public UnityEvent ToggleOnEvent;
        public UnityEvent ToggleOffEvent;

        public enum State
        {
            On,
            Off,
        }

        private State _state;
        public int CurrentState;

        public override void Start()
        {
            base.Start();
            _state = StartingState;
            CurrentState = _state == State.Off ? 0 : 1;
        }

        public void Update()
        {
            if (_return)
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
            }
        }

        protected override void OnMouseDownFunction()
        {
            base.OnMouseDownFunction();
            if (_return) return;
            if (_state == State.Off)
            {
                _state = State.On;
                ToggleOnEvent?.Invoke();
                CurrentState = 1;
            }
            else
            {
                _state = State.Off;
                ToggleOffEvent?.Invoke();
                CurrentState = 0;
            }
        }
    }
}