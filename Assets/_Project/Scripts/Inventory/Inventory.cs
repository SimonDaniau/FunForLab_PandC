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
using FunForLab.Automaton;
using FunForLab.Dialogue;
using FunForLab.Interactables;
using FunForLab.OrbitCamera;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FunForLab.Inventory
{
    [Serializable]
    public class MenuShowConditionLink
    {
        public string ActionName;
        public string[] AllowedItems;
    }
    [Serializable]
    public class ItemAction
    {
        public string ActionName;
        public UnityEvent ActionEvent;
    }

    [Serializable]
    public class ContextMenuInfo
    {
        public string ActionName;
        public string ButtonTextKey;
        public string LuaCondition;
        public UnityEvent ActionEvent;
    }

    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;
        public Menu UIMenu;
        public List<MenuShowConditionLink> MenuShowConditionLinks;
        public List<ContextMenuInfo> MenuInfos;
        [SerializeField] private ItemDatabaseSO _itemDatabaseSo;
        public List<string> ContentList;
        public InventorySlot[] Slots;

        private InventorySlot _draggedSlot;
        public int SlotsRemaining => Mathf.Max(0, Slots.Length - ContentList.Count);
        public List<ItemAction> Actions;
        private Vector3 _positionOffset;
        private Vector3 _draggedSlotOriginalPos;
        private Vector3 _pointerDownMousePos;
        private OrbitController _orbitController;
        private IInteractable _currentInteractable;
        private IInteractable _previousInteractable;
        public int HoveringInventory;
        public string CurrentActionName;
        private void Awake()
        {
            Instance = this;
            ContentList = new List<string>();
            UpdateInventoryInterface();
            foreach (var item in Slots)
            {
                item.BeginDrag += OnSlotDragBegin;
                item.EndDrag += OnSlotDragEnd;
                item.Enter += x => HoveringInventory++;
                item.Exit += x => HoveringInventory--;
                item.PointerDown += (x) =>
                {
                    HoveringInventory++;
                    OrbitController.Instance.Override++;
                    OrbitInput.OverrideHighlight++;
                    _pointerDownMousePos = Input.mousePosition;
                };
                item.PointerUp += (x) =>
                {
                    StartCoroutine(ResetOverrideCR());
                    HoveringInventory--;
                };
                item.RightClick += x => { ShowContextMenu(x); };
            }
        }

        public IEnumerator ResetOverrideCR()
        {
            yield return null;
            OrbitController.Instance.Override--;
            OrbitInput.OverrideHighlight--;
        }

        private void Start()
        {
            _orbitController = OrbitController.Instance;
        }

        private void OnSlotDragBegin(InventorySlot slot)
        {
            _draggedSlot = slot;
            _draggedSlotOriginalPos = _draggedSlot.transform.position;
            _positionOffset = _draggedSlot.transform.position - _pointerDownMousePos;
            if (_orbitController._orbitData.IsOrbit && _orbitController.CurrentOrbitCollider != null)
                _orbitController.CurrentOrbitCollider.enabled = true;
        }

        private void OnSlotDragEnd(InventorySlot slot)
        {
            _draggedSlot.transform.position = _draggedSlotOriginalPos;
            if (_orbitController._orbitData.IsOrbit && _orbitController.CurrentOrbitCollider != null)
                _orbitController.CurrentOrbitCollider.enabled = false;
            foreach (var group in _draggedSlot.InventoryItem.Acts)
            {
                if (!DialogueManager.IsConversationActive &&
                    _currentInteractable != null &&
                    group.ValidTargets.Contains(_currentInteractable.GetName) &&
                    LuaController.Instance.CheckLua(group.LuaCondition))
                {
                    _currentInteractable.SetHighlight(false);
                    LuaController.Instance.RunLua(group.LuaAction);
                    Actions.First(x => x.ActionName == group.EventAction).ActionEvent?.Invoke();
                }
            }


            _draggedSlot = null;
        }

        public void SetupMenu(string actionName)
        {
            CurrentActionName = actionName;
            UIMenu.Clear();
            foreach (var item in MenuInfos.Where(x => x.ActionName == actionName))
            {
                if (item.LuaCondition != "")
                    UIMenu.AddButton(item.ButtonTextKey, () => item.ActionEvent?.Invoke(), item.LuaCondition);
                else
                    UIMenu.AddButton(item.ButtonTextKey, () => item.ActionEvent?.Invoke());
            }
        }

        public void ClearMenu()
        {
            UIMenu.Clear();
        }

        private void ShowContextMenu(InventorySlot slot)
        {
            bool allowShow = false;
            foreach (var item in MenuShowConditionLinks)
            {
                if (item.ActionName == CurrentActionName)
                {
                    if (item.AllowedItems.Contains(slot.InventoryItem.ItemName))
                        allowShow = true;
                }
            }

            if (!allowShow)
            {
                UIMenu.HideMenu();
                return;
            }
            UIMenu.ShowMenu();

            var menuPos = UIMenu.transform.position;
            var pos = new Vector3(menuPos.x, slot.transform.position.y, menuPos.z);
            UIMenu.transform.position = pos;
        }

        public void HideContextMenu()
        {
            UIMenu.HideMenu();
        }

        private void Update()
        {
            HandleSlotDrag();
        }

        private void HandleSlotDrag()
        {
            if (_draggedSlot == null) return;

            Vector2 position = Input.mousePosition;

            _draggedSlot.transform.position = (Vector3) position + _positionOffset;

            _currentInteractable = _orbitController.GetCurrentInteractable;
            if (_currentInteractable != null && _currentInteractable != _previousInteractable)
            {
                if (_previousInteractable != null)
                    _previousInteractable.SetHighlight(false);
                foreach (var group in _draggedSlot.InventoryItem.Acts)
                {
                    if (group.ValidTargets.Contains(_currentInteractable.GetName) &&
                        LuaController.Instance.CheckLua(group.LuaCondition))
                        _currentInteractable.SetHighlight(true);
                }
            }

            _previousInteractable = _currentInteractable;
        }

        public void SetInventoryVisibility(bool value)
        {
            foreach (InventorySlot t in Slots)
            {
                t.transform.parent.gameObject.SetActive(value);
            }
        }

        public void AddToInventory(string item)
        {
            if (ContentList.Contains(item)) return;
            ContentList.Add(item);
            UpdateInventoryInterface();
        }

        public void RemoveFromInventory(string item)
        {
            if (!ContentList.Contains(item)) return;
            ContentList.Remove(item);
            UpdateInventoryInterface();
        }

        private void UpdateInventoryInterface()
        {
            for (var i = 0; i < ContentList.Count; i++)
            {
                Slots[i].InventoryItem = GetInventoryItem(ContentList[i]);
                Slots[i].sprite = Slots[i].InventoryItem.InventoryIcon;
                Slots[i].gameObject.SetActive(true);
            }

            for (var i = ContentList.Count; i < Slots.Length; i++)
            {
                Slots[i].InventoryItem = default;
                Slots[i].sprite = null;
                Slots[i].gameObject.SetActive(false);
            }
        }

        public InventoryItem GetInventoryItem(string itemName)
        {
            return _itemDatabaseSo.Items.FirstOrDefault(x => x.ItemName == itemName);
        }
    }
}