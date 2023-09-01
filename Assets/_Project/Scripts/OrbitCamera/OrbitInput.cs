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
using FunForLab.Analytics;
using FunForLab.Interactables;
using FunForLab.Player;
using UnityEngine;

namespace FunForLab.OrbitCamera
{
    [RequireComponent(typeof(OrbitController))]
    public class OrbitInput : MonoBehaviour
    {
        private OrbitController _orbitController;
        [SerializeField] private float DragDistanceThreshold = 0.2f;
        [SerializeField] private ISelector _selector;
        public ISelector Selector => _selector;
        public event Func<Vector3, bool> OnWorldRightClick;
        public event Func<Vector3, bool> OnWorldLeftClick;

        private bool _dragging;

        private float _mouseDownTime;

        private PlayerCharacter _playerCharacter;
        public static int OverrideHighlight;
        private OrbitData _orbitData;

        private void Start()
        {
            _orbitController = GetComponent<OrbitController>();
            _playerCharacter = FindObjectOfType<PlayerCharacter>();
            _selector = GetComponent<ISelector>();
            if (_selector == null) Debug.LogError("missing Selector Component on orbit camera");
        }

        private void Update()
        {
            _selector.HighlightOverridden = OverrideHighlight > 0;
            if (OverrideHighlight < 0) OverrideHighlight = 0;
            if (_orbitData != null)
            {
                _orbitData.Moving = _dragging && Input.GetMouseButton(0);
                _orbitData.Rotating = _dragging && Input.GetMouseButton(1);
                _orbitData.MouseZooming = Input.GetAxisRaw("Mouse ScrollWheel") != 0;
            }
        }

        public void HandleMouseInput(OrbitData orbitData)
        {
            if (_orbitData == null) _orbitData = orbitData;
            HandleScrollWheel(orbitData);

            if (Input.GetMouseButtonDown(0))
            {
                HandleLeftDown(orbitData);
            }

            if (Input.GetMouseButtonUp(0))
            {
                HandleLeftUp(orbitData);
            }

            if (Input.GetMouseButton(0))
            {
                HandleLeftHeld(orbitData);
            }

            if (Input.GetMouseButtonDown(1))
            {
                HandleRightDown(orbitData);
            }

            if (Input.GetMouseButton(1))
            {
                HandleRightHeld(orbitData);
            }
        }

        private void HandleRightHeld(OrbitData orbitData)
        {
            Matrix4x4 simsHolderInversedMatrix = orbitData.SimsHolderMatrix.inverse;
            if (Vector3.Distance(orbitData.DragCurrentPosition, orbitData.DragStartPosition) > DragDistanceThreshold)
                _dragging = true;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = orbitData.Camera.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                orbitData.DragCurrentPosition = ray.GetPoint(entry);
                Vector3 dragVector = orbitData.DragStartPosition - orbitData.DragCurrentPosition;
                Vector3 startVectorPos = orbitData.NewPosition - orbitData.DragStartPosition;
                orbitData.NewRotation = orbitData.PreviousRotation *
                                        Quaternion.Euler(
                                            Vector3.up *
                                            ((simsHolderInversedMatrix * dragVector).x *
                                             -4f *
                                             (1 / orbitData.ZoomSpeedFactor) *
                                             Mathf.Sign((simsHolderInversedMatrix * startVectorPos).z)));
            }

            orbitData.NewPosition = new Vector3(orbitData.NewPosition.x, 0, orbitData.NewPosition.z);
        }

        private void HandleRightDown(OrbitData orbitData)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = orbitData.Camera.ScreenPointToRay(Input.mousePosition);
            float entry;

            Vector3 worldPos = Vector3.zero;
            if (plane.Raycast(ray, out entry))
            {
                worldPos = ray.GetPoint(entry);
                orbitData.DragStartPosition = worldPos;
                orbitData.DragCurrentPosition = orbitData.DragStartPosition;
            }

            _dragging = true;
            orbitData.PreviousRotation = orbitData.NewRotation;

            if (OnWorldRightClick != null && OnWorldRightClick(worldPos))
                _dragging = false;
        }

        private void HandleLeftHeld(OrbitData orbitData)
        {
            if (Vector3.Distance(orbitData.DragCurrentPosition, orbitData.DragStartPosition) > DragDistanceThreshold)
                _dragging = true;

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = orbitData.Camera.ScreenPointToRay(Input.mousePosition);
            orbitData.StartRay = ray;
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                orbitData.DragCurrentPosition = ray.GetPoint(entry);
                orbitData.NewPosition = orbitData.SimsHolderMatrix.GetLocalPosition() + orbitData.DragStartPosition -
                                        orbitData.DragCurrentPosition;
            }


            if (_orbitController.UseBounds && _orbitController.OverrideBounds == 0)
            {
                Vector3 point = OrbitCalculation.GetSimsPointOnGround(orbitData.SimsMatrix, orbitData.NewRotation,
                    orbitData.NewPosition);
                bool isPointContained = false;
                foreach (var collider in _orbitController.SimsCamBounds)
                {
                    if (collider.bounds.Contains(point))
                    {
                        isPointContained = true;
                        break;
                    }
                }

                Vector3 closestPoint = OrbitCalculation.GetSimsPointOnGround(orbitData.SimsMatrix,
                    orbitData.NewRotation, orbitData.LastValidPosition);
                foreach (var collider in _orbitController.SimsCamBounds)
                {
                    var colliderPoint = collider.bounds.ClosestPoint(point);
                    if (Vector3.Distance(point, colliderPoint) < Vector3.Distance(closestPoint, point))
                    {
                        closestPoint = colliderPoint;
                    }
                }

                if (!isPointContained)
                {
                    orbitData.NewPosition = closestPoint;
                }
            }

            orbitData.LastValidPosition = orbitData.NewPosition;
        }

        private void HandleLeftUp(OrbitData orbitData)
        {
            if (!_dragging)
            {
                var t = _selector.GetSelection(orbitData.StartRay);
                var interactable = t == null ? null : t.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    if (_playerCharacter != null)
                    {
                        _playerCharacter.SetNextInteraction(interactable, t.position);
                    }
                }
                else
                {
                    OnWorldLeftClick?.Invoke(orbitData.DragStartPosition);
                }
            }

            _dragging = false;
        }

        private void HandleLeftDown(OrbitData orbitData)
        {
            _mouseDownTime = Time.time;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = orbitData.Camera.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                var worldPos = ray.GetPoint(entry);
                orbitData.DragStartPosition = ray.GetPoint(entry);
                orbitData.DragCurrentPosition = orbitData.DragStartPosition;
            }
        }

        private void HandleScrollWheel(OrbitData orbitData)
        {
            float zoomDelta = Input.GetAxisRaw("Mouse ScrollWheel");
            if (zoomDelta != 0)
            {
                orbitData.NewZoom += orbitData.ZoomAmount * zoomDelta;
                orbitData.NewZoom = orbitData.NewZoom.ClampMagnitude(orbitData.ZoomMinMax.x, orbitData.ZoomMinMax.y,
                    orbitData.CorrectDot);
            }

            orbitData.ZoomSpeedFactor = orbitData.NewZoom.magnitude.Remap(orbitData.ZoomMinMax.x,
                orbitData.ZoomMinMax.y, orbitData.ZoomSpeedFactorMinMax.x,
                orbitData.ZoomSpeedFactorMinMax.y);
        }

        public bool HandleZoom(OrbitController orbitController, OrbitData orbitData)
        {
            //releasing click inside threshold radius around initial click will be considered a valid click
            if (Vector3.Distance(orbitData.ClickPos, Input.mousePosition) >
                orbitController.ClickDistanceThreshold) return false;

            //Physics.Raycast(orbitData.Camera.ScreenPointToRay(orbitData.ClickPos), out var hit);
            var hitTransform = _selector.GetSelection(orbitData.Camera.ScreenPointToRay(orbitData.ClickPos));

            if (hitTransform != null)
            {
                Orbit other = hitTransform.GetComponent<Orbit>();
                if (HandleOrbitSelection(orbitController, orbitData, other, out var handleZoom)) return handleZoom;
            }

            return false; //didn't click on valid orbit
        }

        public bool HandleZoom(OrbitController orbitController, OrbitData orbitData, Orbit targetOrbit)
        {
            if (HandleOrbitSelection(orbitController, orbitData, targetOrbit, out var handleZoom))
                return handleZoom;
            return false;
        }

        private static bool HandleOrbitSelection(OrbitController orbitController, OrbitData orbitData, Orbit other,
            out bool handleZoom)
        {
            handleZoom = false;
            if (other != null && other != orbitController.CurrentOrbit) //valid and not current orbit
            {
                var conditionalOrbitInteractable = other.GetComponent<ConditionalOrbitInteractable>();
                if (conditionalOrbitInteractable != null && !conditionalOrbitInteractable.Conditional) return false;
                if (orbitController.CurrentOrbit != null && other == orbitController.CurrentOrbit.Parent)
                {
                    handleZoom = false;
                    return true;
                }

                var target = other;

                bool foundSelf = false;
                if (!target.Parent.IsGlobal) // target parent isn't global => target is a subOrbit
                {
                    while (!target.Parent.IsGlobal) // while target is subOrbit
                    {
                        if (target.Parent == orbitController.CurrentOrbit
                        ) // other is common to our orbitChain, we go deeper
                        {
                            foundSelf = true;
                            break;
                        }

                        target = target.Parent;
                    }

                    if (!foundSelf) other = target;
                }

                Vector3 ndst = other.GetCenter() + other.GetNormalDirection() * other.Radius;
                Vector3 cdst = OrbitCalculation.GetClosestValidPosFrom(other.Radius, other.GetCenter(),
                    other.transform, other.LocalRotation, other.ArcFillPercent,
                    orbitData.OrbitMatrix.GetLocalPosition());
                orbitData.TargetPosition = OrbitCalculation.GetClosestValidPosFrom(other.Radius, other.GetCenter(),
                    other.transform, other.LocalRotation,
                    other.ArcFillPercent,
                    Vector3.Lerp(ndst, cdst, orbitController.CenterFactor));
                orbitController.ChangeOrbit(other);
                {
                    handleZoom = true;
                    return true;
                }
            }

            return false;
        }

        public void HandleUnzoom(OrbitController orbitController, OrbitData orbitData)
        {
            if (orbitController.CurrentOrbit != null && orbitController.CurrentOrbit.Parent != null &&
                !orbitController.CurrentOrbit.IsGlobal)
            {
                Vector3 ndst = orbitController.CurrentOrbit.Parent.GetCenter() +
                               orbitController.CurrentOrbit.Parent.GetNormalDirection() *
                               orbitController.CurrentOrbit.Parent.Radius;
                Vector3 cdst = OrbitCalculation.GetClosestValidPosFrom(orbitController.CurrentOrbit.Parent.Radius,
                    orbitController.CurrentOrbit.Parent.GetCenter(),
                    orbitController.CurrentOrbit.Parent.transform, orbitController.CurrentOrbit.Parent.LocalRotation,
                    orbitController.CurrentOrbit.Parent.ArcFillPercent,
                    orbitData.OrbitMatrix.GetLocalPosition());
                orbitData.TargetPosition = OrbitCalculation.GetClosestValidPosFrom(
                    orbitController.CurrentOrbit.Parent.Radius,
                    orbitController.CurrentOrbit.Parent.GetCenter(),
                    orbitController.CurrentOrbit.Parent.transform, orbitController.CurrentOrbit.Parent.LocalRotation,
                    orbitController.CurrentOrbit.Parent.ArcFillPercent,
                    Vector3.Lerp(ndst, cdst, orbitController.CenterFactor));
                if (orbitController.CurrentOrbit.Parent.IsGlobal)
                    orbitController.ChangeOrbit(null);
                else
                    orbitController.ChangeOrbit(orbitController.CurrentOrbit.Parent);
            }
            else if (orbitController.CurrentOrbit == null)
            {
                orbitController.ChangeOrbit(null);
            }
        }

        public void HandleDrag(OrbitController orbitController, OrbitData orbitData)
        {
            orbitData.Velocity += orbitController.Speed *
                                  new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * Time.deltaTime;
        }
    }
}