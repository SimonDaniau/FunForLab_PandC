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
using FunForLab.Interactables;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace FunForLab.OrbitCamera
{
    [Serializable]
    public class OrbitData
    {
        public float ZoomSpeedFactor;
        public Vector3 NewPosition;
        public Quaternion NewRotation;
        public Quaternion PreviousRotation;
        public Vector3 NewZoom;
        public Vector3 StartingZoom;
        public float CorrectDot;
        public Vector3 LastValidPosition;
        public Vector3 DragStartPosition;
        public Vector3 DragCurrentPosition;
        public Ray StartRay;
        public bool IsOrbit;
        public Quaternion FinalTargetRotation;
        public Vector3 FinalTargetPosition;
        public Vector3 ClickPos;
        public Camera Camera;
        public Matrix4x4 OrbitMatrix;
        public Vector3 TargetPosition;
        public Vector2 Velocity;
        public Vector2 ZoomMinMax;
        public Vector2 ZoomSpeedFactorMinMax;
        public Vector3 ZoomAmount;
        public Matrix4x4 SimsMatrix;
        public Matrix4x4 SimsHolderMatrix;
        public bool Moving;
        public bool Rotating;
        public bool MouseZooming;
        public bool OrbitZooming;
    }

    public class OrbitController : MonoBehaviour
    {
        [SerializeField] private Transform CameraHolder;

        [Header("Orbit")] public static OrbitController Instance;

        //public Orbit globalCamTarget;
        public Transform StartingPos;
        public Orbit CurrentOrbit;
        public Collider CurrentOrbitCollider;
        public Vector2 Speed = new Vector2(10, 10);
        public float TravelSpeed = 2f;
        public float CenterFactor;
        public float MouseDragSpeed = 2f;
        private Vector2 _rotationAxis = Vector2.zero;
        private Quaternion _targetRotation;
        public bool inTransition;
        private int _override;

        public bool AllowMovementInDialogue { get; set; }

        public int Override
        {
            get => _override;
            set
            {
                _override = value;
                if (_override < 0) _override = 0;
                if (inTransition)
                {
                    inTransition = false;
                    StopAllCoroutines();
                }
            }
        }

        private int _overrideAllowInteractionInOrbitState;

        public int OverrideAllowInteractionInOrbitState
        {
            get => _overrideAllowInteractionInOrbitState;
            set
            {
                _overrideAllowInteractionInOrbitState = value;
                if (_overrideAllowInteractionInOrbitState < 0) _overrideAllowInteractionInOrbitState = 0;
            }
        }

        public void AllowInteractionInOrbitInc() => OverrideAllowInteractionInOrbitState++;
        public void AllowInteractionInOrbitDec() => OverrideAllowInteractionInOrbitState--;

        public OrbitData _orbitData;
        public OrbitData OrbitData => _orbitData;

        public float ClickDistanceThreshold = 2f;
        public event Action OnOrbitChangeBegin;
        public event Action OnOrbitChangeEnd;

        [Header("Parameters")] [SerializeField]
        private float MovementTime = 3f;

        public bool UseBounds;
        public int OverrideBounds;
        public List<Collider> SimsCamBounds;
        private OrbitInput _orbitInput;

        public IInteractable GetCurrentInteractable => _orbitInput.Selector.GetCurrentInteractable;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _orbitData = new OrbitData();
            _orbitInput = GetComponent<OrbitInput>();
            _orbitData.IsOrbit = false;
            _orbitData.Camera = Camera.main;
            var startPos = new Vector3(-50.4f, 0, 70.3f);
            if (StartingPos != null)
                startPos = new Vector3(StartingPos.position.x, 0, StartingPos.position.z + StartingPos.position.y);
            _orbitData.OrbitMatrix =
                Matrix4x4.TRS(startPos, Quaternion.Euler(45, 0, 0), Vector3.one);

            Vector3 angles = _orbitData.OrbitMatrix.GetLocalRotation().eulerAngles;
            _rotationAxis = new Vector2(angles.x, angles.y);
            CurrentOrbit = null;

            _orbitData.FinalTargetRotation = _orbitData.OrbitMatrix.GetLocalRotation();
            _orbitData.FinalTargetPosition = _orbitData.OrbitMatrix.GetLocalPosition();

            _orbitData.SimsHolderMatrix =
                Matrix4x4.TRS(startPos, Quaternion.identity, Vector3.one);
            _orbitData.NewPosition = _orbitData.SimsHolderMatrix.GetLocalPosition();
            _orbitData.NewRotation = _orbitData.SimsHolderMatrix.GetLocalRotation();

            _orbitData.SimsMatrix =
                Matrix4x4.TRS(new Vector3(0, 10f, -10f), Quaternion.Euler(45, 0, 0), Vector3.one);
            _orbitData.StartingZoom = _orbitData.SimsMatrix.GetLocalPosition();

            _orbitData.NewZoom = _orbitData.StartingZoom;
            _orbitData.CorrectDot = Vector3.Dot(_orbitData.NewZoom, Vector3.up);
            _orbitData.LastValidPosition = _orbitData.NewPosition;
            _orbitData.ZoomMinMax = new Vector2(5, 20);
            _orbitData.ZoomSpeedFactorMinMax = new Vector2(.3f, 2f);
            if (StartingPos != null)
                _orbitData.ZoomAmount = new Vector3(0, -StartingPos.position.y, StartingPos.position.y);
            else
                _orbitData.ZoomAmount = new Vector3(0, -15, 15);
        }

        public void SetSimsControllerPosition(string position)
        {
            float.TryParse(position.Split('/')[0], out float x);
            float.TryParse(position.Split('/')[1], out float y);
            float.TryParse(position.Split('/')[2], out float z);
            _orbitData.NewPosition = new Vector3(x, y, z);
        }

        public void SetOrbitControllerPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            _orbitData.FinalTargetRotation = rotation;
            _orbitData.FinalTargetPosition = position;
        }

        void LateUpdate()
        {
            if (_orbitData.IsOrbit)
            {
                _orbitData.FinalTargetRotation = _orbitData.OrbitMatrix.GetLocalRotation();
                _orbitData.FinalTargetPosition = _orbitData.OrbitMatrix.GetLocalPosition();
            }
            else
            {
                _orbitData.FinalTargetRotation = Extensions
                    .CombinedMatrix(_orbitData.SimsHolderMatrix, _orbitData.SimsMatrix).GetLocalRotation();
                _orbitData.FinalTargetPosition = Extensions
                    .CombinedMatrix(_orbitData.SimsHolderMatrix, _orbitData.SimsMatrix).GetLocalPosition();
                _orbitData.OrbitMatrix = _orbitData.OrbitMatrix.SetLocalPosition(_orbitData.FinalTargetPosition);
                _orbitData.OrbitMatrix = _orbitData.OrbitMatrix.SetLocalRotation(_orbitData.FinalTargetRotation);
            }

            CameraHolder.position = _orbitData.FinalTargetPosition;
            CameraHolder.rotation = _orbitData.FinalTargetRotation;

            if (!_orbitData.IsOrbit && CurrentOrbit != null && CurrentOrbit.IsGlobal && !inTransition)
                CurrentOrbit = null;

            if (!inTransition)
                _orbitData.IsOrbit = CurrentOrbit != null;


            if (Override > 0 || (DialogueManager.Instance.IsConversationActive && !AllowMovementInDialogue))
                return;

            if (Input.GetMouseButtonDown(0))
                _orbitData.ClickPos = Input.mousePosition;

            if (inTransition)
            {
                return;
            }

            if (Input.GetMouseButtonDown(1) && _orbitData.IsOrbit)
            {
                _orbitInput.HandleUnzoom(this, _orbitData);
                return;
            }

            if (Input.GetMouseButtonUp(0))
                if (_orbitInput.HandleZoom(this, _orbitData))
                    return;

            if (!_orbitData.IsOrbit || _overrideAllowInteractionInOrbitState > 0)
            {
                _orbitInput.HandleMouseInput(_orbitData);
                ApplyMovement();
            }

            if (Input.GetMouseButton(0) && _orbitData.IsOrbit)
                _orbitInput.HandleDrag(this, _orbitData);

            if (_orbitData.IsOrbit)
            {
                OrbitCalculation.ComputeOrbitalVelocity(CurrentOrbit, _orbitData.OrbitMatrix, MouseDragSpeed,
                    ref _rotationAxis,
                    ref _orbitData.Velocity, out var targetPosition,
                    out var targetRotation);
                _orbitData.OrbitMatrix = _orbitData.OrbitMatrix.SetLocalPosition(targetPosition);
                _orbitData.OrbitMatrix = _orbitData.OrbitMatrix.SetLocalRotation(targetRotation);
            }
        }

        void ApplyMovement()
        {
            _orbitData.SimsHolderMatrix =
                _orbitData.SimsHolderMatrix.SetLocalPosition(Vector3.Lerp(
                    _orbitData.SimsHolderMatrix.GetLocalPosition(),
                    _orbitData.NewPosition,
                    Time.deltaTime * MovementTime));
            _orbitData.SimsHolderMatrix =
                _orbitData.SimsHolderMatrix.SetLocalRotation(Quaternion.Lerp(
                    _orbitData.SimsHolderMatrix.GetLocalRotation(), _orbitData.NewRotation,
                    Time.deltaTime * MovementTime));
            _orbitData.SimsMatrix =
                _orbitData.SimsMatrix.SetLocalPosition(Vector3.Lerp(_orbitData.SimsMatrix.GetLocalPosition(),
                    _orbitData.NewZoom, Time.deltaTime * MovementTime));
        }

        public void SwitchToSimsMode()
        {
            if (CurrentOrbit == null) return;
            CurrentOrbitCollider = CurrentOrbit.GetComponent<Collider>();
            if (CurrentOrbitCollider != null) CurrentOrbitCollider.enabled = true;
            CurrentOrbit = null;
            ChangeOrbitSims(Vector2.zero);
        }

        public void ChangeOrbitExt(Orbit orbit) => _orbitInput.HandleZoom(this, _orbitData, orbit);

        public void ChangeOrbit(Orbit orbit, bool updateColliders = true)
        {
            if (CurrentOrbit != null)
                CurrentOrbitCollider = CurrentOrbit.GetComponent<Collider>();
            if (updateColliders && CurrentOrbitCollider != null)
            {
                if (CurrentOrbitCollider != null) CurrentOrbitCollider.enabled = true;
            }

            if (orbit == null)
            {
                SwitchToSimsMode();
                return;
            }

            CurrentOrbit = orbit;
            _orbitData.IsOrbit = true;
            if (updateColliders)
            {
                if (CurrentOrbitCollider != null) CurrentOrbitCollider.enabled = false;
            }

            var angleLimit = orbit.ArcFillPercent * 180f;
            Vector2 rotationAxisFromPosition =
                OrbitCalculation.GetRotationAxisFromPosition(orbit, _orbitData.TargetPosition);
            rotationAxisFromPosition.x = Extensions.ClampAngle(rotationAxisFromPosition.x, -angleLimit, angleLimit);
            rotationAxisFromPosition.y = Extensions.ClampAngle(rotationAxisFromPosition.y, -angleLimit, angleLimit);
            _orbitData.TargetPosition =
                OrbitCalculation.GetPositionFromRotationAxis(orbit, rotationAxisFromPosition, angleLimit);
            _targetRotation = Quaternion.LookRotation(orbit.GetCenter() - _orbitData.TargetPosition,
                Vector3.Lerp(Vector3.up, Vector3.Cross(orbit.GetNormalDirection(), orbit.transform.right),
                    Mathf.Clamp01(Vector3.Dot(-Vector3.up, orbit.GetCenter() - _orbitData.TargetPosition))));
            ChangeOrbit(rotationAxisFromPosition);
        }

        private void ChangeOrbit(Vector2 rotationAxisFromPosition)
        {
            StartCoroutine(
                Transition(_orbitData.TargetPosition, _targetRotation, rotationAxisFromPosition,
                    Vector3.Distance(_orbitData.OrbitMatrix.GetLocalPosition(), _orbitData.TargetPosition), TravelSpeed,
                    OnTransitionEnd));
        }

        private void ChangeOrbitSims(Vector2 rotationAxisFromPosition)
        {
            StartCoroutine(
                TransitionSims(rotationAxisFromPosition,
                    Vector3.Distance(_orbitData.OrbitMatrix.GetLocalPosition(), _orbitData.TargetPosition), TravelSpeed,
                    OnTransitionEnd));
        }

        private void OnTransitionEnd(Vector2 anticipatedRotAxis)
        {
            _rotationAxis = anticipatedRotAxis;
        }

        private IEnumerator TransitionSims(Vector2 anticipatedRotAxis, float distance,
            float travelSpeed, Action<Vector2> callback)
        {
            var destPos = Extensions.CombinedMatrix(_orbitData.SimsHolderMatrix, _orbitData.SimsMatrix)
                .GetLocalPosition();
            var destRot = Extensions.CombinedMatrix(_orbitData.SimsHolderMatrix, _orbitData.SimsMatrix)
                .GetLocalRotation();
            yield return Transition(destPos, destRot, anticipatedRotAxis, distance, travelSpeed, callback);
        }


        private IEnumerator Transition(Vector3 destPos, Quaternion destRot, Vector2 anticipatedRotAxis, float distance,
            float travelSpeed, Action<Vector2> callback)
        {
            OnOrbitChangeBegin?.Invoke();
            inTransition = true;
            var oldOrbit = CurrentOrbit;

            _rotationAxis = Vector2.zero;
            _orbitData.Velocity = Vector2.zero;
            Vector3 startPos = _orbitData.OrbitMatrix.GetLocalPosition();
            Quaternion startRot = _orbitData.OrbitMatrix.GetLocalRotation();
            float elapsedTime = 0;
            float currentSpeed = 0;
            float duration = distance / travelSpeed;
            if (duration < 1f) duration = 1f;
            if (duration > 2f) duration = 2f;
            while (elapsedTime < duration)
            {
                float localPercent = 0;
                elapsedTime += Time.deltaTime;
                localPercent = elapsedTime / duration;
                _orbitData.OrbitMatrix = _orbitData.OrbitMatrix.SetLocalPosition(Vector3.Lerp(startPos, destPos,
                    Extensions.EaseInOut(localPercent)));
                if (CurrentOrbit != null)
                {
                    Quaternion rot = Quaternion.LookRotation(
                        CurrentOrbit.GetCenter() - _orbitData.OrbitMatrix.GetLocalPosition(),
                        Vector3.Lerp(Vector3.up,
                            Vector3.Cross(CurrentOrbit.GetNormalDirection(), CurrentOrbit.transform.right),
                            Mathf.Clamp01(Vector3.Dot(-Vector3.up, _orbitData.OrbitMatrix.GetForwardDir()))));
                    _orbitData.OrbitMatrix = _orbitData.OrbitMatrix.SetLocalRotation(Quaternion.Slerp(startRot, rot,
                        1 - Mathf.Pow(1 - localPercent, 3)).normalized);
                }
                else
                {
                    _orbitData.OrbitMatrix =
                        _orbitData.OrbitMatrix.SetLocalRotation(Quaternion.Slerp(startRot, destRot, localPercent)
                            .normalized);
                }

                yield return null;
            }

            CurrentOrbit = oldOrbit;
            if (CurrentOrbit == null)
                _orbitData.IsOrbit = false;
            inTransition = false;
            callback(anticipatedRotAxis);
            OnOrbitChangeEnd?.Invoke();
        }
    }
}