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
using FunForLab.Dialogue;
using FunForLab.Modules;
using FunForLab.OrbitCamera;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FunForLab.Automaton
{
    public class Menu : MonoBehaviour
    {
        public GameObject ButtonPrefab;
        public Transform ButtonsGroupHolder;
        protected Camera _mainCamera;

        protected Dictionary<string, bool> _functionVisibilityDict;
        protected bool _dragOverride;
        public bool CanShrinkGrow = true;
        protected virtual void Awake()
        {
            _mainCamera = Camera.main;
        }
        public void Grow(float duration = .5f, float delay = .7f)
        {
            if (CanShrinkGrow) StartCoroutine(GrowRoutine(duration, delay));
        }

        public void Shrink(float duration = .5f, float delay = 0f)
        {
            if (CanShrinkGrow)  StartCoroutine(ShrinkRoutine(duration, delay));
        }
        public IEnumerator GrowRoutine(float duration , float delay )
        {
            float timeElapsed = 0f;
            yield return new WaitForSeconds(delay);
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float localPercent = timeElapsed / duration;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, localPercent);
                yield return null;
            }

            transform.localScale =  Vector3.one;
        }

        public IEnumerator ShrinkRoutine(float duration , float delay )
        {
            float timeElapsed = 0f;
            yield return new WaitForSeconds(delay);
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float localPercent = timeElapsed / duration;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, localPercent);
                yield return null;
            }

            transform.localScale =  Vector3.zero;
        }
        public void Grow() => Grow(.5f, .7f);
        public void Shrink() => Shrink(.5f, 0f);
        public GameObject AddButton(string functionName, string luaCode, string luaEvent,
            List<HematologyAutomaton.ItemAction> events, HematologyAutomaton source, string luaCondition)
        {
            
            var go = Instantiate(ButtonPrefab, ButtonsGroupHolder);
            Button b = go.GetComponent<Button>();
            if (b == null)
                b = go.GetComponentInChildren<Button>(true);
            b.GetComponentInChildren<TextMeshProUGUI>(true).text = functionName.Localize();
            if (luaCode != "")
                b.onClick.AddListener(() =>
                {
                    if (LuaController.Instance.CheckLua(luaCondition))
                        LuaController.Instance.RunLua(luaCode);
                });
            if (luaEvent != "")
                b.onClick.AddListener(() =>
                {
                    if (LuaController.Instance.CheckLua(luaCondition))
                    {
                        var item = events.FirstOrDefault(x => x.ActionName == luaEvent);
                        if (item != null)
                        {
                            item.ActionEvent.Invoke();
                            source.DelayedEvent(() => item.DelayedEvent?.Invoke(), item.delay);
                        }
                    }
                });
            var trigger = b.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                AddEventTriggerListener(trigger, EventTriggerType.PointerDown, OnBeginDrag);
                AddEventTriggerListener(trigger, EventTriggerType.PointerUp, OnEndDrag);
            }
           
            if (_functionVisibilityDict == null) _functionVisibilityDict = new Dictionary<string, bool>();
            if (_functionVisibilityDict.ContainsKey(functionName)) return go;
            _functionVisibilityDict.Add(functionName, true);
            return go;
        }

        public GameObject AddButton(string functionName, Action function, string luaCondition)
        {
            var go = Instantiate(ButtonPrefab, ButtonsGroupHolder);
            Button b = go.GetComponent<Button>();
            if (b == null)
                b = go.GetComponentInChildren<Button>(true);
            b.GetComponentInChildren<TextMeshProUGUI>(true).text = functionName.Localize();
            if (function != null)
                b.onClick.AddListener(() =>
                    {
                        if (LuaController.Instance.CheckLua(luaCondition))
                            function?.Invoke();
                        else
                            HideMenu();
                    }
                );
            var trigger = b.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                AddEventTriggerListener(trigger, EventTriggerType.PointerDown, OnBeginDrag);
                AddEventTriggerListener(trigger, EventTriggerType.PointerUp, OnEndDrag);
            }

            if (_functionVisibilityDict == null) _functionVisibilityDict = new Dictionary<string, bool>();
            if (_functionVisibilityDict.ContainsKey(functionName)) return go;
            _functionVisibilityDict.Add(functionName, true);
            return go;
        }

        public GameObject AddButton(string functionName, Action function)
        {
            var go = Instantiate(ButtonPrefab, ButtonsGroupHolder);
            Button b = go.GetComponent<Button>();
            if (b == null)
                b = go.GetComponentInChildren<Button>(true);
            var tmpComponent = b.GetComponentInChildren<TextMeshProUGUI>(true);
            tmpComponent.text = functionName.Localize();
            if (function != null)
                b.onClick.AddListener(() => function?.Invoke());
            var trigger = b.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                AddEventTriggerListener(trigger, EventTriggerType.PointerDown, OnBeginDrag);
                AddEventTriggerListener(trigger, EventTriggerType.PointerUp, OnEndDrag);
            }

            if (_functionVisibilityDict == null) _functionVisibilityDict = new Dictionary<string, bool>();
            if (_functionVisibilityDict.ContainsKey(functionName)) return go;
            _functionVisibilityDict.Add(functionName, true);
            return go;
        }

        [ContextMenu("AddDummyButton")]
        public void AddDummyButton()
        {
            AddButton("Dum", () => { });
        }

        public void RemoveButton(GameObject button)
        {
            Button b = button.GetComponent<Button>();
            if (b == null)
                b = GetComponentInChildren<Button>(true);
            if (b == null) return;
            b.onClick.RemoveAllListeners();

            var trigger = button.GetComponent<EventTrigger>();
            if (trigger != null) RemoveEventTriggerListener(trigger);

            var textmesh = button.GetComponentInChildren<TextMeshProUGUI>(true);
            if (textmesh != null)
            {
                if (_functionVisibilityDict != null)
                    _functionVisibilityDict.Remove(textmesh.text);
            }

            Destroy(button);
        }
        public void Clear()
        {
            if (_functionVisibilityDict == null) _functionVisibilityDict = new Dictionary<string, bool>();
            _functionVisibilityDict.Clear();
            var items = ButtonsGroupHolder.gameObject.GetComponentsInChildren<Transform>();
            for (var i = 0; i < items.Length; i++)
            {
                if (items[i].gameObject != ButtonsGroupHolder.gameObject)
                    RemoveButton(items[i].gameObject);
            }
        }

        public void ShowMenu() => ButtonsGroupHolder.gameObject.SetActive(true);
        public void HideMenu() => ButtonsGroupHolder.gameObject.SetActive(false);
        public void SetFunctionVisibility(string functionName, bool state)
        {
            _functionVisibilityDict[functionName] = state;
        }

        public bool GetFunctionAllowed(string functionName)
        {
            return _functionVisibilityDict[functionName];
        }

        public void OnBeginDrag(BaseEventData data)
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray))
            {
                OrbitController.Instance.Override++;
                _dragOverride = true;
            }
        }

        public void OnEndDrag(BaseEventData data)
        {
            if (_dragOverride)
            {
                _dragOverride = false;
                OrbitController.Instance.Override--;
            }
        }

        public static void AddEventTriggerListener(EventTrigger trigger,
            EventTriggerType eventType,
            System.Action<BaseEventData> callback)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
            trigger.triggers.Add(entry);
        }

        public static void RemoveEventTriggerListener(EventTrigger trigger)
        {
            trigger.triggers.Clear();
        }

        [System.Serializable]
        public class AutomatonFunction
        {
            public string FunctionName;
            [HideInInspector] public Button Button;
            public UnityEvent Function;
        }
    }
}