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
using FunForLab.Localization;
using UnityEditor;
using UnityEngine;

namespace FunForLab._Project.Scripts.Localization.Editor
{
    public class TextLocalizerEditWindow : EditorWindow
    {
        public static void Open(string key)
        {
            TextLocalizerEditWindow window = new TextLocalizerEditWindow();
            window.titleContent = new GUIContent("Localizer window");
            window.ShowUtility();
            window.key = key;
        }

        public string key;
        public string value;

        public void OnGUI()
        {
            key = EditorGUILayout.TextField("Key :", key);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value:", GUILayout.MaxWidth(50));
            EditorStyles.textArea.wordWrap = true;
            value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Add"))
            {
                if (Localizator.Localize(key) != string.Empty)
                {
                    Localizator.Replace(key, value);
                }
                else
                {
                    Localizator.Add(key, value);
                }
            }

            minSize = new Vector2(460, 250);
            maxSize = minSize;
        }
    }

    public class TextLocalizerSearchWindow : EditorWindow
    {
        public SerializedProperty keyPointer;
        public string value;
        public Vector2 scroll;
        public Dictionary<string, string> dictionary;

        private static TextLocalizerSearchWindow _window;

        public static void Open(string keyText, SerializedProperty keySP)
        {
            _window = new TextLocalizerSearchWindow();
            _window.titleContent = new GUIContent("Localizer Search");

            Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
            _window.ShowAsDropDown(r, new Vector2(500, 300));
            _window.value = keyText;
            _window.keyPointer = keySP;
        }
#if UNITY_EDITOR
        public static void Close()
        {
            ( (EditorWindow) _window ).Close();
        }
#endif

        private void OnEnable()
        {
            dictionary = Localizator.GetEditorDictionary();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
            value = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();

            GetSearchResults();
        }

        void GetSearchResults()
        {
            if (value == null) return;
            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var element in dictionary)
            {
                if (element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
                {
                    EditorGUILayout.BeginHorizontal("Box");
                    Texture closeIcon = (Texture) Resources.Load("close");
                    GUIContent content = new GUIContent(closeIcon);

                    /*if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localization, are you sure ?", "Do it"))
                    {
                        Localizator.Remove(element.Key);
                        AssetDatabase.Refresh();
                        Localizator.Init();
                        dictionary = Localizator.GetEditorDictionary();
                    }
                }*/

                    if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                    {
                        keyPointer.stringValue = element.Key;
                        keyPointer.serializedObject.ApplyModifiedProperties();
                        Close();
                    }

                    EditorGUILayout.TextField(element.Key);
                    EditorGUILayout.LabelField(element.Value);
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}