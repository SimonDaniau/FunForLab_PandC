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
using FunForLab.Automaton;
using UnityEngine;

namespace FunForLab.Interactables
{
    public class ControlledHandle : MonoBehaviour
    {
        public StateProvider Provider;
        public bool Translation;
        public bool Rotation;
        public bool StartInOn;
        public bool Invert;
        public float TimeToCompletion;
        public Vector3 LocalOnTranslation;
        public Vector3 LocalOnRotation;
        public Vector3 LocalOffTranslation;
        public Vector3 LocalOffRotation;
        private float _nextCompletion;
        private float _transitionStart;
        private bool _currentState;
        private Vector3 _startTranslation;
        private Vector3 _endTranslation;
        private Quaternion _startRotation;
        private Quaternion _endRotation;
        private bool _inTransition;

        private void Start()
        {
            if (Provider != null)
                Provider.OnStateChange += () => StateProvider.SetStatePoweringOnOff(Provider.GetCurrentState(), OnFunction, OffFunction);

            if (Invert)
            {
                Vector3 tmp = LocalOnTranslation;
                LocalOnTranslation = LocalOffTranslation;
                LocalOffTranslation = tmp;

                tmp = LocalOnRotation;
                LocalOnRotation = LocalOffRotation;
                LocalOffRotation = tmp;
            }

            if (StartInOn)
                OnFunction();
            else
                OffFunction();
            _nextCompletion = 0;
        }

        public void OnFunction()
        {
            if (!_inTransition)
            {
                _transitionStart = Time.time;
                _nextCompletion = _transitionStart + TimeToCompletion;
            }
            else if (!_currentState)
            {
                float delta = Time.time - _transitionStart;
                _nextCompletion = Time.time + delta;
                _transitionStart = _nextCompletion - TimeToCompletion;
            }

            _startTranslation = LocalOffTranslation;
            _endTranslation = LocalOnTranslation;
            _startRotation = Quaternion.Euler(LocalOffRotation);
            _endRotation = Quaternion.Euler(LocalOnRotation);

            _currentState = true;
            _inTransition = true;
        }

        public void OffFunction()
        {
            if (!_inTransition)
            {
                _transitionStart = Time.time;
                _nextCompletion = _transitionStart + TimeToCompletion;
            }
            else if (_currentState)
            {
                float delta = Time.time - _transitionStart;
                _nextCompletion = Time.time + delta;
                _transitionStart = _nextCompletion - TimeToCompletion;
            }

            _startTranslation = LocalOnTranslation;
            _endTranslation = LocalOffTranslation;
            _startRotation = Quaternion.Euler(LocalOnRotation);
            _endRotation = Quaternion.Euler(LocalOffRotation);

            _currentState = false;
            _inTransition = true;
        }

        private void Update()
        {
            if (_inTransition)
            {
                if (_nextCompletion > Time.time)
                {
                    float localPercent = ( _nextCompletion - Time.time ) / TimeToCompletion;
                    if (Rotation)
                        transform.localRotation = Quaternion.Lerp(_startRotation, _endRotation, 1 - localPercent);
                    if (Translation)
                        transform.localPosition = Vector3.Lerp(_startTranslation, _endTranslation, 1 - localPercent);
                }
                else if (Time.time >= _nextCompletion)
                {
                    if (Rotation)
                        transform.localRotation =  _endRotation;
                    if (Translation)
                        transform.localPosition = _endTranslation;
                    _inTransition = false;
                }
            }
        }
    }
}