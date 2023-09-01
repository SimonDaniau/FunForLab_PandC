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
using System.Linq;
using FunForLab.Dialogue;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

namespace FunForLab.Modules
{
    public class DebugModule : MonoBehaviour
    {
        private bool _showConsole;
        private bool _firstFrame;
        public KeyCode ConsoleKey;
        private string _input;

        public static DebugCommand TEST_COMMAND;
        public static DebugCommand<string> TEST_PARAM_COMMAND;
        public static DebugCommand<string, bool> DS_SET_BOOL;
        public static DebugCommand<string> DS_UPDATE_INDICATOR;

        public List<object> CommandList;
        [Header("Lua")] public string LuaAction;
        public string LuaConditionTester;
        public string LuaEvent;
        public bool RunLua;
        public string ConditionResult;
        [Header("Conv")] public string ConversationToStart;
        public bool StartConversation;
        
        public bool StopConversations;

        public List<EventItem> Events;

        [System.Serializable]
        public class EventItem
        {
            public string eventName;
            public string LuaAction;
            public UnityEvent ActionEvent;
        }


        private void Awake()
        {
            DS_SET_BOOL = new DebugCommand<string, bool>("DS_SET_BOOL", "Sets dialogue system booleans",
                "DS_SET_BOOL <variable name> {0 : false, 1 : true}",
                (x, y) => DialogueLua.SetVariable(x, y));
            DS_UPDATE_INDICATOR = new DebugCommand<string>("DS_UPDATE_INDICATOR", "Updates indicators of quest",
                "DS_UPDATE_INDICATOR <quest name>",
                (x) => QuestLog.UpdateQuestIndicators(x));
            CommandList = new List<object>
            {
                DS_SET_BOOL,
                DS_UPDATE_INDICATOR
            };
        }

        private void Update()
        {
            if (StopConversations)
            {
                StopConversations = false;
                DialogueManager.StopAllConversations();
            }
            if (StartConversation)
            {
                StartConversation = false;
                DialogueManager.instance.StartConversation(ConversationToStart);
            }

            if (RunLua)
            {
                RunLua = false;
                LuaController.Instance.RunLua(LuaAction);
                ConditionResult = LuaController.Instance.CheckLua(LuaConditionTester).ToString();
                if (LuaEvent != "")
                {
                    var action = Events.FirstOrDefault(x => x.eventName == LuaEvent);
                    if (action != null)
                    {
                        LuaController.Instance.RunLua(action.LuaAction);
                        action.ActionEvent.Invoke();
                    }
                }
            }

            if (Input.GetKeyDown(ConsoleKey) && !_showConsole)
            {
                _showConsole = true;
                _firstFrame = true;
            }

            if (_showConsole && Input.GetKeyDown(KeyCode.Return))
            {
                HandleInput();
                _input = "";
            }
        }

        public void HandleInput(string input)
        {
            _input = input;
            HandleInput();
            _input = "";
        }

        private void HandleInput()
        {
            if (_input == "") return;
            string[] properties = _input.Split(' ');
            for (int i = 0; i < CommandList.Count; i++)
            {
                DebugCommandBase commandBase = CommandList[i] as DebugCommandBase;
                if (properties[0] == commandBase.CommandId)
                {
                    if (CommandList[i] as DebugCommand != null)
                    {
                        (CommandList[i] as DebugCommand).Invoke();
                        break;
                    }
                    else if (CommandList[i] as DebugCommand<string> != null)
                    {
                        (CommandList[i] as DebugCommand<string>).Invoke(properties[1]);
                        break;
                    }
                    else if (CommandList[i] as DebugCommand<string, bool> != null)
                    {
                        (CommandList[i] as DebugCommand<string, bool>).Invoke(properties[1],
                            int.Parse(properties[2]) == 1);
                        break;
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (!_showConsole) return;

            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.character == '\n')
                {
                    HandleInput();
                    _input = "";
                }
                else if (Event.current.keyCode == ConsoleKey)
                {
                    _showConsole = false;
                }
            }

            float y = 0;
            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            GUI.SetNextControlName("Console");
            _input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), _input);
            if (_firstFrame)
            {
                GUI.FocusControl("Console");
                _firstFrame = false;
            }
        }
    }
}