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
using UnityEngine;
using UnityEngine.UI;

namespace FunForLab.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance;
        public List<Pickable> Inventory;
        public Image[] Slots;
        public int SlotsRemaining => Mathf.Max(0, Slots.Length - Inventory.Count);

        private void Awake()
        {
            Instance = this;
            Inventory = new List<Pickable>();
            UpdateInventoryInterface();
        }

        public void SetInventoryVisibility(bool value)
        {
            foreach (Image t in Slots)
            {
                t.transform.parent.gameObject.SetActive(value);
            }
        }

        public void AddToInventory(Pickable item)
        {
            if (Inventory.Contains(item)) return;
            Inventory.Add(item);
            UpdateInventoryInterface();
        }
    
        public void RemoveFromInventory(Pickable item)
        {
            Inventory.Remove(item);
            UpdateInventoryInterface();
        }

        private void UpdateInventoryInterface()
        {
            for (var i = 0; i < Inventory.Count; i++)
            {
                Slots[i].sprite = Inventory[i].InventoryIcon;
                Slots[i].gameObject.SetActive(true);
            }

            for (var i = Inventory.Count; i < Slots.Length; i++)
            {
                Slots[i].sprite = null;
                Slots[i].gameObject.SetActive(false);
            }
        }
    }
}