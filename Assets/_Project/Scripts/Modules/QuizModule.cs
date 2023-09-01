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
using FunForLab.Automaton;
using FunForLab.Scenario;
using UnityEngine;

namespace FunForLab.Modules
{
    public class QuizModule : MonoBehaviour
    {
        public ScrollMenu ScrollMenu;

        public class State
        {
            public bool Finished;
            public bool WaitingAnswer;
            public bool GivingExplanation;
            public bool Congratulating;
        }

        public State CurrentState { get; private set; }

        public Action<State> OnStateChange;

        public void PlayQuiz( List<string> correct, List<string> incorrect, List<string> congratulation, List<string> explanation, MissionData data, bool mix = false)
        {
            ScrollMenu.Grow(0f, 0f);
            for (int i = 0 ; i < ScrollMenu.ButtonsGroupHolder.childCount; i++)
            {
                ScrollMenu.RemoveButton(ScrollMenu.ButtonsGroupHolder.GetChild(i).gameObject);
            }


            if (mix)
            {
                var tmplist = new List<Tuple<string, Action>>();
                foreach (var item in correct)
                {
                    tmplist.Add(new Tuple<string, Action>(item, () => TriggerCorrectAnswer(congratulation)));
                }

                foreach (var item in incorrect)
                {
                    tmplist.Add(new Tuple<string, Action>(item, () => TriggerIncorrectAnswer(explanation)));
                }

                tmplist = tmplist.Randomize().ToList();
                foreach (var item in tmplist)
                {
                    ScrollMenu.AddButton(item.Item1, item.Item2);
                }
            }
            else
            {
                foreach (var item in correct)
                {
                    ScrollMenu.AddButton(item, () => TriggerCorrectAnswer(congratulation));
                }

                foreach (var item in incorrect)
                {
                    ScrollMenu.AddButton(item, () => TriggerIncorrectAnswer(explanation));
                }
            }

            CurrentState = new State();
            CurrentState.WaitingAnswer = true;
            OnStateChange?.Invoke(CurrentState);
        }

        public void TriggerCorrectAnswer(List<string> text)
        {
            CurrentState.WaitingAnswer = false;
            CurrentState.Congratulating = true;
            OnStateChange?.Invoke(CurrentState);
            StartCoroutine(PlayDialog(text));
            ScrollMenu.Shrink(0f, 0f);
        }

        public void TriggerIncorrectAnswer(List<string> text)
        {
            CurrentState.WaitingAnswer = false;
            CurrentState.GivingExplanation = true;
            OnStateChange?.Invoke(CurrentState);
            StartCoroutine(PlayDialog(text));
            ScrollMenu.Shrink(0f, 0f);
        }

        IEnumerator PlayDialog(List<string> text)
        {
            MissionManager.Instance.DisplayKeyPressNotifier(false);
            if (text.Count <= 0)
            {
                yield return new WaitForSeconds(.5f);
            }
            else
            {
                int cnt = 0;
                MissionManager.Instance.ChangeDescription(text[cnt]);
                while (cnt < text.Count)
                {
                    if (Input.anyKeyDown)
                    {
                        if (cnt < text.Count - 1)
                            MissionManager.Instance.ChangeDescription(text[cnt + 1]);

                        cnt++;
                        MissionManager.Instance.DisplayKeyPressNotifier(false);
                        yield return new WaitForSeconds(.5f);
                    }

                    if (!CurrentState.WaitingAnswer)
                        MissionManager.Instance.DisplayKeyPressNotifier(true);
                    yield return null;
                }
            }

            MissionManager.Instance.DisplayKeyPressNotifier(false);
            CurrentState.WaitingAnswer = false;
            CurrentState.Finished = true;
            OnStateChange?.Invoke(CurrentState);
        }
    }
}