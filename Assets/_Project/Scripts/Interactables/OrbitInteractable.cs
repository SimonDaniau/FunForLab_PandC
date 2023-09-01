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
using cakeslice;
using FunForLab.Interactables;
using FunForLab.OrbitCamera;
using UnityEngine;

namespace FunForLab
{
    public class OrbitInteractable : MonoBehaviour, IInteractable
    {
        public Outline Outline;
        public List<Outline> Outlines;
        private Orbit _orbit;
        private Collider _collider;
        [SerializeField] private Vector3 _dotOffset;
        public Vector3 DotOffset => _dotOffset;
        [SerializeField] private float _interactionRadius = 1;
        public float InteractionRadius => _interactionRadius;

        private OrbitController _orbitController;
        public string GetName => name;

        private void Start()
        {
            _orbitController = OrbitController.Instance;
            _collider = GetComponent<Collider>();
            _orbit = GetComponent<Orbit>();
        }

        public void OnClick()
        {
        }

        public virtual bool Conditional
        {
            get
            {
                if (!_collider.enabled) return false;
                if (_orbitController.inTransition) return false;
                if (!_orbitController.OrbitData.IsOrbit && _orbit.Parent.IsGlobal == false) return false;
                return true;
            }
        }

        public void SetHighlight(bool value)
        {
            if (Outlines.Count > 0)
            {
                foreach (var item in Outlines)
                {
                    item.eraseRenderer = !value;
                }
                return;
            }
            if (Outline == null) return;
            Outline.eraseRenderer = !value;
        }
    }
}