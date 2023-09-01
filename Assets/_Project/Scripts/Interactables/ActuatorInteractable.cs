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
using System.Collections.Generic;
using cakeslice;
using DG.Tweening;
using FunForLab.Dialogue;
using PixelCrushers.DialogueSystem;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace FunForLab.Interactables
{
    public class ActuatorInteractable : MonoBehaviour, IInteractable
    {
        public Transform Target;
        public bool StartingState;
        public bool OneShot;
        public bool ShotSpent;
        public Transform TargetMirrored;

        public string LuaCondition;

        public float TransitionDuration;

        [Header("Position")] public bool DoPos;

        public Vector3 PosOn;
        public Vector3 PosOff;

        [Header("Rotation")] public bool DoRot;

        public Vector3 RotOn;
        public Vector3 RotOff;

        public UnityEvent OnToggleOnBegin;
        public UnityEvent OnToggleOnEnd;
        public UnityEvent OnToggleOffBegin;
        public UnityEvent OnToggleOffEnd;

        private bool _currentState;
        public Outline Outline;
        public Vector3 DotOffset => Vector3.zero;

        [SerializeField] private float _interactionRadius = 1;
        public float InteractionRadius => _interactionRadius;
        public string GetName => name;

        public void Awake()
        {
           if(Outline == null) Outline = Target.GetComponentInChildren<Outline>();
            _currentState = StartingState;
            Transform t = Target;
            if (Target == null)
                t = transform;
            ToggleAction(t, 0.01f, false);
            ShotSpent = false;
        }

        public void Destruct()
        {
            Destroy(this);
        }

        public void ResetSpent()
        {
            ShotSpent = false;
        }

        [ContextMenu("Shut")]
        public void Shut()
        {
            Transform t = Target;
            if (Target == null)
                t = transform;
            float duration = TransitionDuration;

            if (DoPos)
            {
                if (TargetMirrored != null) TargetMirrored.DOLocalMove(-PosOff, duration);
                t.DOLocalMove(PosOff, duration);
            }

            if (DoRot)
                t.DOLocalRotate(RotOff, duration);
            _currentState = false;
        }
        [ContextMenu("Open")]
        public void Open()
        {
            Transform t = Target;
            if (Target == null)
                t = transform;
            float duration = TransitionDuration;

            if (DoPos)
            {
                t.DOLocalMove(PosOn, duration);
                if (TargetMirrored != null) TargetMirrored.DOLocalMove(-PosOn, duration);
            }

            if (DoRot)
                t.DOLocalRotate(RotOn, duration);
            _currentState = true;
        }
        
        public void OpenDelay(float delay)
        {
            StartCoroutine(OpenCR(delay));
        }
        private IEnumerator OpenCR(float d)
        {
            yield return new WaitForSeconds(d);
            Open();
        }

        public void Toggle()
        {
            if (LuaCondition.IsNullOrWhitespace() || Lua.IsTrue(LuaCondition))
            {
                _currentState = !_currentState;
                Transform t = Target;
                if (Target == null)
                    t = transform;

                float duration = TransitionDuration;

                ToggleAction(t, duration);
            }
        }

        private void ToggleAction(Transform t, float duration, bool fire = true, bool over = false)
        {
            if (ShotSpent && !over) return;
            if (_currentState)
            {
                if (DoPos)
                {
                    if (TargetMirrored != null) TargetMirrored.DOLocalMove(-PosOn, duration);
                    t.DOLocalMove(PosOn, duration).OnComplete(() =>
                    {
                        if (fire) OnToggleOnEnd.Invoke();
                    });
                }

                if (DoRot)
                    t.DOLocalRotate(RotOn, duration).OnComplete(() =>
                    {
                        if (!DoPos && fire) OnToggleOnEnd.Invoke();
                    });
                if (fire) OnToggleOnBegin?.Invoke();
            }
            else
            {
                if (DoPos)
                {
                    if (TargetMirrored != null) TargetMirrored.DOLocalMove(-PosOff, duration);
                    t.DOLocalMove(PosOff, duration).OnComplete(() =>
                    {
                        if (fire) OnToggleOffEnd.Invoke();
                    });
                }

                if (DoRot)
                    t.DOLocalRotate(RotOff, duration).OnComplete(() =>
                    {
                        if (!DoPos && fire) OnToggleOffEnd.Invoke();
                    });
                if (fire) OnToggleOffBegin?.Invoke();
            }

            if (OneShot) ShotSpent = true;
        }

        public void OnClick() => Toggle();

        public bool Conditional
        {
            get
            {
                bool lua = LuaController.Instance.CheckLua(LuaCondition);
                return (!OneShot && lua) || (!ShotSpent && lua);
            }
        }

        public void SetHighlight(bool value)
        {
            if (Outline == null) return;
            Outline.eraseRenderer = !value;
        }
    }
}