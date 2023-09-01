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
using FunForLab.OrbitCamera;
using UnityEngine;

namespace FunForLab.Interactables
{
    public class DragHandle : Handle
    {
        public enum MovementMode
        {
            Rotate,
            RotateFree,
            Line
        };

        public enum ActionAxis
        {
            LocalX,
            LocalY,
            LocalZ
        }

        public MovementMode Mode;
        public ActionAxis Axis;
        public float Speed;
        public float StartPos;
        public float EndPos;
        private Vector3 _reference;
        public float MouseDragSpeed = 2f;

        public float turns;
        private bool _dragged;
        private Vector2 _velocity;
        private Vector3 _position;
        private Camera _camera;
        private Vector3 _positionOnStart;
        private Vector3 _hitPoint;
        private float _offAngle;
        private float _angle;
        private float _totalRotation;
        private Vector3 lastRotPoint;
        public bool Screw;
        public float ScrewFactor = .1f;
        public Vector2 TurnsMinMax;

        public override void Start()
        {
            base.Start();
            _camera = Camera.main;
            _positionOnStart = transform.position;
            _totalRotation = 0;
            lastRotPoint = transform.TransformDirection(Vector3.forward);
            lastRotPoint.y = 0;
        }

        public void Update()
        {
            if (_return || _controller.inTransition) _dragged = false;
            if (_dragged)
            {
                _velocity += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * ( Speed * Time.deltaTime );
                Vector3 localAxis;
                switch (Axis)
                {
                    case ActionAxis.LocalX:
                        localAxis = Vector3.right;
                        break;
                    case ActionAxis.LocalY:
                        localAxis = Vector3.up;
                        break;
                    default:
                        localAxis = Vector3.forward;
                        break;
                }

                switch (Mode)
                {
                    case MovementMode.Line:
                    {
                        Vector3 lineAxis = ( transform.rotation * ( EndPos * localAxis - StartPos * localAxis ) ).normalized;
                        Vector3 disp = _camera.transform.TransformVector(new Vector3( _velocity.x, _velocity.y, 0));
                        _position += Vector3.Project(disp, lineAxis);
                        _position = Limit(StartPos * localAxis, EndPos * localAxis, _position);
                        transform.position = _positionOnStart + transform.rotation * _position;
                    }
                        break;
                    case MovementMode.RotateFree:
                    case MovementMode.Rotate:
                    {
                        lastRotPoint = transform.localRotation * Vector3.forward;
                        lastRotPoint.y = 0;
                        _reference =  Vector3.Scale(_hitPoint - transform.position, localAxis) ;
                        Vector3 o =  _camera.WorldToScreenPoint(transform.position + transform.rotation * _reference );
                        Vector3 delta = Input.mousePosition - o;

                        _angle = Vector3.SignedAngle(Vector3.right, ( delta ).normalized, -Vector3.forward) - _offAngle;
                        Quaternion finalRot = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(localAxis * _angle ), Time.deltaTime * 3f);
                        Vector3 facing =  finalRot * Vector3.forward;
                        float rotangle = Vector3.Angle(lastRotPoint, facing);
                        if (Vector3.Cross(lastRotPoint, facing).y < 0) rotangle *= -1;
                        _totalRotation += rotangle;

                        turns = _totalRotation / 360f;
                        if (turns - TurnsMinMax.y > 0 && Mode == MovementMode.Rotate)
                        {
                            finalRot = Quaternion.Euler(localAxis * ( ( TurnsMinMax.y * 360 ) % 360 ) );
                            facing = transform.TransformDirection(Vector3.forward);
                            facing.y = 0;
                            _totalRotation = TurnsMinMax.y * 360f;
                        }

                        if (turns - TurnsMinMax.x < 0 && Mode == MovementMode.Rotate)
                        {
                            finalRot = Quaternion.Euler(localAxis * ( ( TurnsMinMax.x * 360 ) % 360 ) );
                            facing = transform.TransformDirection(Vector3.forward);
                            facing.y = 0;
                            _totalRotation = TurnsMinMax.x * 360f;
                        }

                        turns = _totalRotation / 360f;
                        transform.localRotation = finalRot;
                        if (Screw)
                            transform.position = _positionOnStart + localAxis * ( ScrewFactor * turns );
                    }
                        break;
                }
            }

            _velocity.x = Mathf.Lerp(_velocity.x, 0, Time.deltaTime * MouseDragSpeed);
            _velocity.y = Mathf.Lerp(_velocity.y, 0, Time.deltaTime * MouseDragSpeed);
        }

        private Vector3 Limit(Vector3 start, Vector3 end, Vector3 value)
        {
            return Vector3.Lerp(start, end,  Mathf.Clamp01(InverseLerp(start, end, value)));
        }

        private float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            if (_return)
            {
                _dragged = false;
                return;
            }

            OrbitController.Instance.Override++;
            _dragged = true;

            if (Physics.Raycast( _camera.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                _hitPoint = hit.point;
                Vector3 o =  _camera.WorldToScreenPoint(transform.position + transform.rotation * _reference );
                Vector3 delta = Input.mousePosition - o;

                _offAngle = Vector3.SignedAngle(Vector3.right, ( delta ).normalized, -Vector3.forward) - transform.localRotation.eulerAngles.magnitude;
            }
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            _dragged = false;
            OrbitController.Instance.Override--;
        }
    }
}