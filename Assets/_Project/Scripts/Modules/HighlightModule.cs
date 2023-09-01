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
using System.Collections.Generic;
using FunForLab.OrbitCamera;
using UnityEngine;

namespace FunForLab.Modules
{
    public class HighlightModule : MonoBehaviour
    {
        public static bool HighlightWhenNotInTargetOrbitTree;
        public static bool HighlightHigherWhenTargetIsSubOrbit;
        public List<HighlightableObject> HighlightableObjects;
        public HighlightableObject CurrentTarget;
        private OrbitController _orbitController;
        public float GlobalHighlightInterval = .75f;

        private void Awake()
        {
            HighlightableObjects = new List<HighlightableObject>(FindObjectsOfType<HighlightableObject>());
            foreach (var item in HighlightableObjects)
            {
                item.Interval = GlobalHighlightInterval;
            }
        }

        private void Start()
        {
            _orbitController = OrbitController.Instance;
            _orbitController.OnOrbitChangeBegin += OnOrbitChange;
        }

        public void OnOrbitChange()
        {
            if (CurrentTarget != null)
            {
                if (!OrbitCalculation.IsTargetCommonOrbit( CurrentTarget.CorrespondingOrbit, _orbitController.CurrentOrbit))
                {
                    if (CurrentTarget.CorrespondingOrbit != _orbitController.CurrentOrbit && HighlightWhenNotInTargetOrbitTree)
                        Highlight(CurrentTarget);
                }
            }
        }

        public void HighlightAll()
        {
            CurrentTarget = null;
            foreach (var item in HighlightableObjects)
                item.Highlight(true);
        }

        public void UnHighlightAll()
        {
            CurrentTarget = null;
            foreach (var item in HighlightableObjects)
                item.Highlight(false);
        }

        public void Highlight(GameObject o)
        {
            UnHighlightAll();
            CurrentTarget = o.GetComponent<HighlightableObject>();
            if (CurrentTarget != null)
                CurrentTarget.Highlight(true);
        }

        public void Highlight(HighlightableObject ho)
        {
            CurrentTarget = ho;
            if (CurrentTarget.CorrespondingOrbit != _orbitController.CurrentOrbit)
            {
                if (HighlightHigherWhenTargetIsSubOrbit)
                {
                    if (CurrentTarget.HighlightChainParent != null)
                    {
                        if (CurrentTarget.HighlightChainParent.CorrespondingOrbit != _orbitController.CurrentOrbit)
                        {
                            CurrentTarget.HighlightChainParent.Highlight(true);
                            return;
                        }
                    }
                }

                CurrentTarget.Highlight(true);
            }
        }

        public void Highlight(string s)
        {
            foreach (var item in HighlightableObjects)
            {
                if (item.gameObject.name == s)
                {
                    Highlight(item);
                    return;
                }
            }
        }

        public void Highlight(Orbit o)
        {
            if (IsHighlight(o, out var ho))
            {
                Highlight(ho);
            }
        }

        public bool IsHighlight(Orbit o, out HighlightableObject ho)
        {
            ho = null;
            foreach (var item in HighlightableObjects)
            {
                if (item.CorrespondingOrbit == o)
                {
                    ho = item;
                    return true;
                }
            }

            return false;
        }
    }
}