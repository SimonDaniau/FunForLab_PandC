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
using FunForLab.Dialogue;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FunForLab
{
    public class LanguageGetter : MonoBehaviour
    {
        private DialogueSystemController _dialogueSystemController;
        public string PlayerName;
        public static LanguageGetter Instance;

        /*
         public enum Languages
        {
            NL,
            EN,
            FR,
            DE
        }
         */
        private void Awake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(this);
        }

        public void SetRobotName(string name)
        {
            SetNewName("Robot", name);
        }

        public void SetPlayerName(string name)
        {
            SetNewName("Player", name);
        }

        public void SetNewName(string parameter, string name)
        {
            if (LuaController.Instance != null)
                LuaController.Instance.RunLua("Actor[\"" + parameter + "\"].Display_Name = " + "\"" + name + "\"");
        }

        void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            SceneManager.sceneLoaded += (Scene s, LoadSceneMode m) =>
            {
                SetLanguage();
                if (SceneManager.GetActiveScene().name == "Tutorial")
                    SetRobotName("Robot");
                else
                    SetRobotName("AILA");
                PlayerName = PlayerPrefs.GetString("PlayerName", "Sam");
                var parameter = "Player";
                var name = PlayerName;
                SetNewName(parameter, name);
            };
            SetLanguage();
            if (SceneManager.GetActiveScene().name == "Tutorial")
                SetRobotName("Robot");
            else
                SetRobotName("AILA");
            PlayerName = PlayerPrefs.GetString("PlayerName", "Sam");
            var parameter = "Player";
            var name = PlayerName;
            SetNewName(parameter, name);
        }

        private void SetLanguage()
        {
            var current = PlayerPrefs.GetInt("Languages", 1);

            _dialogueSystemController = FindObjectOfType<DialogueSystemController>();
            if (_dialogueSystemController != null)
            {
                _dialogueSystemController.SetLanguage(new[] { "Netherlands", "en", "French", "German", }[current]);
                DialogueLua.SetVariable("AudioLang", new[] { "nl", "en", "fr", "de", }[current]);
            }
        }
    }
}