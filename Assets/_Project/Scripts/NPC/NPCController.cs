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
using DG.Tweening;
using FunForLab.Dialogue;
using UnityEngine;
using UnityEngine.AI;

namespace FunForLab.NPC
{
    public class NPCController : MonoBehaviour
    {
        public string NPCName;
        public float MoveSpeed = 3f;
        public float RotationSpeed = 180f;

        private NPCWaypoint _currentWaypoint;
        private Animator _animator;
        public bool CanUseOverride;
        public Action<string, NPCWaypoint> OnWaypointComplete;

        public bool NotifyLocationVariableOnWaypointReached;

        private NavMeshAgent _agent;
        private NavMeshObstacle _obstacle;
        private Queue<Vector3> _cornerQueue;
        private Vector3 _currentDestination;
        private bool _hasPath;
        private float _currentDistance;
        private float _minDistanceArrived = .5f;
        private NavMeshPath _path;
        private Vector3 _direction;
        private bool _shouldLookAt;
        private Transform _lookTargetTransform;
        private bool _shouldFollowTarget;
        private Vector3 _lastLookTargetPosition;

        public void SetCanUseOverride(bool value) => CanUseOverride = value;

        public bool CanUse => CanUseOverride && !_hasPath && _currentWaypoint == null;

        public void GotoWaypoint(string waypointName)
        {
            GotoWaypoint(NPCWaypoint.AllWaypoints.FirstOrDefault(x => x.WaypointName == waypointName));
        }

        public void GotoWaypoint(NPCWaypoint waypoint, float duration = -1)
        {
            LuaController.Instance.RunLua("Variable[\"InWaypointTransit\"] = true");
            if (_animator != null) _animator.enabled = false;
            StartCoroutine(CalculatePath(waypoint));
        }

        private IEnumerator CalculatePath(NPCWaypoint waypoint)
        {
            _obstacle.enabled = false;
            _agent.enabled = true;
            yield return null;
            _currentWaypoint = waypoint;
            _agent.CalculatePath(waypoint.transform.position, _path);
            SetupPath(_path);
            yield return null;
            _agent.enabled = false;
        }

        public void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _obstacle = GetComponent<NavMeshObstacle>();
            _agent.enabled = false;
            _path = new NavMeshPath();
            _animator = GetComponent<Animator>();
            _currentWaypoint = null;
            if (NotifyLocationVariableOnWaypointReached)
            {
                OnWaypointComplete += (npcName, waypoint) =>
                {
                    LuaController.Instance.RunLua("Variable[\"" + npcName + "Location\"] = \"" +
                                                  waypoint.WaypointName + "\"");
                };
            }
        }

        void SetupPath(NavMeshPath path)
        {
            _cornerQueue = new Queue<Vector3>();
            foreach (Vector3 corner in path.corners)
            {
                _cornerQueue.Enqueue(corner);
            }

            GetNextCorner();
            _currentDistance = (transform.position - _currentDestination).sqrMagnitude;
            _hasPath = true;
        }

        private void GetNextCorner()
        {
            if (_cornerQueue.Count > 0)
            {
                _currentDestination = _cornerQueue.Dequeue();
                _direction = _currentDestination - transform.position;
                _direction = new Vector3(_direction.x, 0, _direction.z);
                _hasPath = true;
            }
            else
            {
                transform.DORotateQuaternion(_currentWaypoint.transform.rotation,
                        GetDuration(transform.rotation, _currentWaypoint.transform.rotation))
                    .onComplete = () =>
                {
                    OnWaypointComplete?.Invoke(NPCName, _currentWaypoint);
                    _currentWaypoint.OnCompletion();
                    _currentWaypoint = null;
                    LuaController.Instance.RunLua("Variable[\"InWaypointTransit\"] = false");
                    _obstacle.enabled = true;
                };
                _hasPath = false;
            }
        }

        void FixedUpdate()
        {
            if (_hasPath)
            {
                MoveAlongPath();
            }
            else if (_shouldLookAt)
            {
                if (_shouldFollowTarget)
                    _lastLookTargetPosition = _lookTargetTransform.position;
                LookAtTarget();
            }
        }

        public void FollowTransform(Transform target, bool followTarget = true)
        {
            _shouldLookAt = true;
            _shouldFollowTarget = followTarget;
            _lookTargetTransform = target;
            _lastLookTargetPosition = target.position;
        }
        
        public void LookAtPos(Vector3 targetPos)
        {
            _shouldLookAt = true;
            _shouldFollowTarget = false;
            _lastLookTargetPosition = targetPos;
        }

        public void StopFollow(bool stopLookAt = true)
        {
            _shouldFollowTarget = false;
            _shouldLookAt = !stopLookAt;
        }
        
        public void ResumeFollow()
        {
            _shouldFollowTarget = true;
            _shouldLookAt = true;
        }

        private void LookAtTarget()
        {
            Vector3 newLookPos = new Vector3(_lastLookTargetPosition.x, transform.hierarchyCapacity,
                _lastLookTargetPosition.z);

            var targetRot = Quaternion.LookRotation(newLookPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5 * Time.deltaTime);
        }

        private void MoveAlongPath()
        {
            var a = new Vector3(transform.position.x, 0, transform.position.z);
            var b = new Vector3(_currentDestination.x, 0, _currentDestination.z);
            _currentDistance = Vector3.Distance(a, b);

            if (_currentDistance > _minDistanceArrived)
            {
                transform.position += _direction.normalized * (MoveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(_direction.normalized);
            }
            else
                GetNextCorner();
        }

        public float GetDuration(Quaternion current, Quaternion target)
        {
            float dRot = Quaternion.Angle(current, target) / RotationSpeed;
            return dRot;
        }
    }
}