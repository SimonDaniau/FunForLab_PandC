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
using System.Linq;
using UnityEngine;

namespace FunForLab.OrbitCamera
{
    public class Orbit : MonoBehaviour
    {
        public Orbit Parent;
        public const int HALF_RESOLUTION = 20;
        public Vector3 LocalRotation;
        public Vector3 GlobalOffset;
        public float Radius;
        [Range(0, 1)] public float ArcFillPercent;
        public Transform test;
        public bool IsGlobal;

        public Action OnOrbitSwitchedOnEarly;
        public Action OnOrbitSwitchedOn;
        public Action OnOrbitSwitchedOff;
        private bool _currentOrbit;

        private void Start()
        {
            if (Parent == null && IsGlobal == false)
                Parent = FindObjectsOfType<Orbit>().First(x => x.IsGlobal);
            if (Parent == this)
            {
                Parent = null;
            }

            var orbitController = OrbitController.Instance;
            orbitController.OnOrbitChangeBegin += () =>
            {
                if (orbitController.CurrentOrbit == this)
                {
                    OnOrbitSwitchedOnEarly?.Invoke();
                }
            };
            orbitController.OnOrbitChangeEnd += () =>
            {
                if (orbitController.CurrentOrbit == this)
                {
                    OnOrbitSwitchedOn?.Invoke();
                    _currentOrbit = true;
                }
            };
            orbitController.OnOrbitChangeBegin += () =>
            {
                if (_currentOrbit)
                {
                    OnOrbitSwitchedOff?.Invoke();
                    _currentOrbit = false;
                }
            };
        }

        public Vector3 GetNormalDirection()
        {
            return (transform.rotation * Quaternion.Euler(LocalRotation) *
                    new Vector3(Mathf.Sin(0) * Radius, 0, Mathf.Cos(0) * Radius)).normalized;
        }

        public Vector3 GetCenter()
        {
            return GlobalOffset + transform.position;
        }

        public Vector3 GetNeutralPosition()
        {
            return GetCenter() + GetNormalDirection() * Radius;
        }

        public Quaternion GetNeutralRotation()
        {
            return transform.rotation * Quaternion.Euler(LocalRotation);
        }

        public Pose GetNeutralPose()
        {
            return new Pose(GetNeutralPosition(), GetNeutralRotation());
        }

        private void OnDrawGizmos()
        {
            Extensions.DisplaySemiSphere(transform.position, transform.rotation * Quaternion.Euler(LocalRotation),
                Radius, ArcFillPercent, GlobalOffset, HALF_RESOLUTION, Color.green);
        }
    }
}