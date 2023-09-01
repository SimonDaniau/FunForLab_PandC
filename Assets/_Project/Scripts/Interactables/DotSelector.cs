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
using FunForLab.Interactables;
using FunForLab.OrbitCamera;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace FunForLab
{
    public class DotSelector : MonoBehaviour, ISelector
    {
        [SerializeField] private AnimationCurve _thresholdSimsZoomCurve;
        [SerializeField] private AnimationCurve _thresholdOrbitCurve;
        public Dictionary<GameObject, IInteractable> InteractablesDictionary;
        private Camera _camera;
        private GameObject _selection;
        private GameObject _selectionPrevious;
        private OrbitController _controller;
        private float _normalizedZoomMagnitude;
        public bool EnableHeatMap;
        public float HeatMapSize;
        public int HeatMapIterations;
        private float _correctedThreshold;
        public bool HighlightOverridden { get; set; }

        private bool _wasOverridden;
        private GameObject _lastValidSelection;

        public IInteractable GetCurrentInteractable =>
            (_selection == null || InteractablesDictionary == null || InteractablesDictionary[_selection] == null)
                ? null
                : InteractablesDictionary[_selection];

        private void Start()
        {
            _controller = OrbitController.Instance;
            _camera = Camera.main;
            InteractablesDictionary = new Dictionary<GameObject, IInteractable>();
            var list = InterfaceHelper.FindObjectsWithInterface<IInteractable>();
            foreach (var item in list)
            {
                if (!InteractablesDictionary.ContainsKey(item))
                    InteractablesDictionary.Add(item, item.GetComponent<IInteractable>());
            }

            _wasOverridden = false;
        }

        private void Update()
        {
            _correctedThreshold = 0f;
            _normalizedZoomMagnitude = Mathf.InverseLerp(_controller.OrbitData.ZoomMinMax.x,
                _controller.OrbitData.ZoomMinMax.y, _controller.OrbitData.NewZoom.magnitude);

            if (!_controller.OrbitData.IsOrbit)
                _correctedThreshold = _thresholdSimsZoomCurve.Evaluate(_normalizedZoomMagnitude);
            else if (_controller.CurrentOrbit != null)
                _correctedThreshold = _thresholdOrbitCurve.Evaluate(_controller.CurrentOrbit.Radius);

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            float closest = 0f;
            _selection = null;

            for (int i = 0; i < InteractablesDictionary.Count; i++)
            {
                var go = InteractablesDictionary.Keys.ElementAt(i);
                var interactable = InteractablesDictionary[go];
                if (!go.activeSelf) continue;
                float lookPercentage = Vector3.Dot(ray.direction.normalized,
                    ((go.transform.position + interactable.DotOffset) - ray.origin).normalized);
                if (interactable.Conditional && lookPercentage > _correctedThreshold && lookPercentage > closest)
                {
                    closest = lookPercentage;
                    _selection = go;
                }
            }


            if (_selectionPrevious != _selection && !HighlightOverridden)
            {
                if (_selectionPrevious != null) InteractablesDictionary[_selectionPrevious].SetHighlight(false);
                if (_selection != null) InteractablesDictionary[_selection].SetHighlight(true);
            }

            if (!_wasOverridden && HighlightOverridden)
            {
                foreach (var pair in InteractablesDictionary)
                {
                    pair.Value.SetHighlight(false);
                }
            }

            if (_wasOverridden && !HighlightOverridden)
            {
                if (_selection != null) InteractablesDictionary[_selection].SetHighlight(true);
            }

            _selectionPrevious = _selection;
            _wasOverridden = HighlightOverridden;
        }

        public Transform GetSelection(Ray ray)
        {
            if (_selection == null) return null;
            return _selection.transform;
        }

        private void OnDrawGizmos()
        {
            if (!EnableHeatMap) return;
            if (_selection != null)
            {
                _lastValidSelection = _selection;
            }

            if (_lastValidSelection == null) return;
            var interactable = InteractablesDictionary[_lastValidSelection];

            Gizmos.color = Color.red;
            for (int i = 0; i < HeatMapIterations; i++)
            {
                float t = i / (HeatMapIterations - 1f);
                float inclination = Mathf.Acos(1 - 2 * t);
                float azimuth = 2 * Mathf.PI * 0.618033f * i;
                Vector3 pos = new Vector3(
                    Mathf.Sin(inclination) * Mathf.Cos(azimuth),
                    Mathf.Sin(inclination) * Mathf.Sin(azimuth),
                    Mathf.Cos(inclination)
                );
                //var ray = new Ray(_camera.transform.position,  pos);
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                float lookPercentage = Vector3.Dot(ray.direction.normalized,
                    ((_lastValidSelection.transform.position + interactable.DotOffset) - ray.origin + pos).normalized);
                if (lookPercentage > _correctedThreshold)
                {
                    Gizmos.DrawLine(_camera.transform.position,
                        _lastValidSelection.transform.position + interactable.DotOffset + pos);
                }
            }
        }
    }
}