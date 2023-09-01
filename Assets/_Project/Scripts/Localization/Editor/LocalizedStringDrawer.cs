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
using FunForLab.Localization;
using UnityEditor;
using UnityEngine;

namespace FunForLab._Project.Scripts.Localization.Editor
{
    [CustomPropertyDrawer(typeof(LocalizedString))]
    public class LocalizedStringDrawer : PropertyDrawer
    {
        private bool dropDown;
        private float height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (dropDown)
            {
                return height + 25;
            }

            return  20;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            label.text = "Localized String";
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width -= 34;
            position.height = 18;

            Rect valueRect = new Rect(position);
            valueRect.x += 15;
            valueRect.width -= 15;

            Rect foldButtonRect = new Rect(position);
            foldButtonRect.width = 15;

            dropDown = EditorGUI.Foldout(foldButtonRect, dropDown, "");
            position.x += 15;
            position.width -= 15;

            SerializedProperty key = property.FindPropertyRelative("key");
            key.stringValue = EditorGUI.TextField(position, key.stringValue);

            SerializedProperty context = property.FindPropertyRelative("contextInfo");

            position.x += position.width + 2;
            position.width = 17;
            position.height = 17;

            Texture searchIcon = (Texture) Resources.Load("search");
            GUIContent searchContent = new GUIContent(searchIcon);

            if (GUI.Button(position, searchContent))
            {
                TextLocalizerSearchWindow.Open(key.stringValue, key);
            }

            position.x += position.width + 2;

            Texture storeIcon = (Texture) Resources.Load("store");
            GUIContent storeContent = new GUIContent(storeIcon);

            if (GUI.Button(position, storeContent))
            {
                TextLocalizerEditWindow.Open(key.stringValue);
            }

            if (dropDown)
            {
                var value = property.name + " : " + Localizator.Localize(key.stringValue);
                GUIStyle style = GUI.skin.box;
                height = style.CalcHeight(new GUIContent(value), valueRect.width);

                valueRect.height = height;
                valueRect.y += 21;
                EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
            }

            EditorGUI.EndProperty();
        }
    }
}