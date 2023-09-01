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

using FunForLab.Interactables;
using FunForLab.OrbitCamera;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FunForLab.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private float _interactionRadius;
        private OrbitController _orbitController;
        private NavMeshAgent _agent;
        private IInteractable _nextInteraction;
        private string _nextInteractionComponentName;
        private Vector3 _nextInteractionPos;
        public ThirdPersonCharacter tpc;
        public Animator PlayerAnimator;
        public Vector2 VelocityComponent;
        private Vector3 directionPrevious;
        public bool IsMoving;
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _orbitController = OrbitController.Instance;
            _orbitController.GetComponent<OrbitInput>().OnWorldLeftClick += NavigateToValidPosIfAvailable;
            tpc.OverrideGroundCheck = true;
        }

        void Update()
        {
            //VelocityComponent.x = _agent.velocity.magnitude / _agent.speed;
            //VelocityComponent.y =
            // (Vector3.SignedAngle(directionPrevious, transform.forward, Vector3.up) / Time.deltaTime) /
            //  _agent.angularSpeed;

            //PlayerAnimator.SetFloat("Forward", VelocityComponent.x);
            //PlayerAnimator.SetFloat("Turning", VelocityComponent.y);
            IsMoving = _agent.remainingDistance > _agent.stoppingDistance;
            if (IsMoving)
                tpc.Move(_agent.desiredVelocity, false, false);
            else
                tpc.Move(Vector3.zero, false, false);
            tpc.SetGroundedStatus(true);
            directionPrevious = transform.forward;
            var pos = transform.position;
            _nextInteractionComponentName = _nextInteraction == null ? "null" : _nextInteraction.ToString();
            if (_nextInteraction == null) return;
            var heightAgnosticPosition = new Vector3(pos.x, 0, pos.z);
            if (_nextInteraction.InteractionRadius > 0)
            {
                if (!(Vector3.Distance(heightAgnosticPosition, _nextInteractionPos) <
                      _nextInteraction.InteractionRadius))
                    return;
            }

            if (_nextInteraction.Conditional)
            {
                _nextInteraction.OnClick();
                _nextInteraction = null;
            }
        }

        bool NavigateToValidPosIfAvailable(Vector3 worldPoint)
        {
            if (DialogueManager.Instance.IsConversationActive) return false;
            // bool result = NavMesh.SamplePosition(worldPoint, out var hit, 0.2f, NavMesh.AllAreas);
            _agent.isStopped = false;
            bool result = _agent.SetDestination(worldPoint);
            return result;
        }

        public void SetNextInteraction(IInteractable interactable, Vector3 interactablePosition)
        {
            _nextInteraction = interactable;
            _nextInteractionPos = new Vector3(interactablePosition.x, 0, interactablePosition.z);
            Vector3 direction = (_nextInteractionPos - transform.position).normalized;
            NavigateToValidPosIfAvailable(_nextInteractionPos - direction * _interactionRadius);
        }
    }
}