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
using cakeslice;
using FunForLab.NPC;
using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;

namespace FunForLab.Interactables
{
    public class NpcInteractable : MonoBehaviour, IInteractable
    {
        private DialogueSystemTrigger _trigger;
        private NPCController _npcController;
        public Outline Outline;
        public List<Outline> Outlines;
        public Vector3 DotOffset => Vector3.zero;

        [SerializeField] private float _interactionRadius = 1;
        public string GetName => name;
        public float InteractionRadius => _interactionRadius;

        private void Start()
        {
            if (Outline == null && Outlines.Count == 0)
                Outline = GetComponentInChildren<Outline>();
            _trigger = GetComponent<DialogueSystemTrigger>();
            _npcController = GetComponent<NPCController>();
        }

        public void OnClick() => _trigger.OnUse();

        public virtual bool Conditional => _npcController.CanUse;

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