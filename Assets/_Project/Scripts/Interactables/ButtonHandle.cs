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
using cakeslice;
using FunForLab.Automaton;
using FunForLab.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace FunForLab.Interactables
{
    public class ButtonHandle : Handle
    {
        protected StateProvider StateProvider;
        public Material HighlightStandard;
        public Material Standard;
        public UnityEvent MouseDownEvent;
        public UnityEvent MouseUpEvent;

        public Outline Outline;
        protected Renderer Renderer;
        protected bool MouseIn;
        protected bool ButtonPressed;

        public bool UseOutlineHighlight;

        public string HighlightLuaConditional;

        // Start is called before the first frame update
        public virtual void Awake()
        {
            Renderer = GetComponent<Renderer>();
            StateProvider = GetComponent<StateProvider>();
            if (StateProvider != null)
                StateProvider.GetCurrentState = () => StateProvider.GetState(ButtonPressed);
        }

        public void OnMouseEnter()
        {
            MouseIn = true;
            if (UseOutlineHighlight && Outline != null && LuaController.Instance.CheckLua(HighlightLuaConditional))
            {
                Outline.eraseRenderer = !MouseIn;
            }
        }

        public void OnMouseExit()
        {
            MouseIn = false;
            if (UseOutlineHighlight && Outline != null)
            {
                Outline.eraseRenderer = !MouseIn;
            }
        }

        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            OnMouseDownFunction();
            ButtonPressed = true;
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            OnMouseUpFunction();
            ButtonPressed = false;
        }

        protected virtual void OnMouseDownFunction()
        {
            if (_return) return;
            MouseDownEvent?.Invoke();
            StateProvider?.OnStateChange();
        }

        protected virtual void OnMouseUpFunction()
        {
            if (_return) return;
            MouseUpEvent?.Invoke();
            StateProvider?.OnStateChange();
        }
    }
}