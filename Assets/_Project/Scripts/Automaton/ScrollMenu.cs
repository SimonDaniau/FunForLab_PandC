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
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FunForLab.Automaton
{
    public class ScrollMenu : Menu
    {
        public Transform ScrollBar;
        public Transform BackGround;

        protected override void Awake()
        {
            base.Awake();
            var triggerScrollBar = ScrollBar.gameObject.AddComponent<EventTrigger>();
            AddEventTriggerListener(triggerScrollBar, EventTriggerType.PointerDown, OnBeginDrag);
            AddEventTriggerListener(triggerScrollBar, EventTriggerType.PointerUp, OnEndDrag);
            var triggerBackGround = BackGround.gameObject.AddComponent<EventTrigger>();
            AddEventTriggerListener(triggerBackGround, EventTriggerType.PointerDown, OnBeginDrag);
            AddEventTriggerListener(triggerBackGround, EventTriggerType.PointerUp, OnEndDrag);
        }

        public void Grow(float duration = .5f, float delay = .7f)
        {
            StartCoroutine(GrowRoutine(duration, delay));
        }

        public void Shrink(float duration = .5f, float delay = 0f)
        {
            StartCoroutine(ShrinkRoutine(duration, delay));
        }

        public void Grow() => Grow(.5f, .7f);
        public void Shrink() => Shrink(.5f, 0f);

        public IEnumerator GrowRoutine(float duration, float delay)
        {
            float timeElapsed = 0f;
            yield return new WaitForSeconds(delay);
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float localPercent = timeElapsed / duration;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, localPercent);
                yield return null;
            }

            transform.localScale = Vector3.one;
        }

        public IEnumerator ShrinkRoutine(float duration, float delay)
        {
            float timeElapsed = 0f;
            yield return new WaitForSeconds(delay);
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float localPercent = timeElapsed / duration;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, localPercent);
                yield return null;
            }

            transform.localScale = Vector3.zero;
        }
    }
}