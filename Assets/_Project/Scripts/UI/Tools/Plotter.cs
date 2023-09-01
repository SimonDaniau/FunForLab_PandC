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
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FunForLab.UI.Tools
{
#if UNITY_EDITOR
    public class DataPoint
    {
        public string Label;
        public float Time;
        public float Height;
        public Color Color;

        public DataPoint(float height, Color color, string label = "")
        {
            Label = label;
            Height = height;
            Color = color;
            Time = Plotter.Realtime;
        }
    }

    public class Plotter : EditorWindow
    {
        public static List<DataPoint> Points;
        public static Vector2 SizeDelta = new Vector2(300, 300);
        public static float Realtime;
        private static float _startTime;
        private static bool _recording;
        private static bool _dataStored;

        private static Plotter _window;

        [MenuItem("Window/Plotter")]
        static void Create()
        {
            _window = (Plotter) EditorWindow.GetWindow(typeof(Plotter));
            _window.Show();
            _window.Repaint();
        }

        public void Update()
        {
            if (_window == null) _window = (Plotter) EditorWindow.GetWindow(typeof(Plotter));
            if (_recording) _window.Repaint();
        }

        public void OnGUI()
        {
            if (_recording)
                Realtime = (float) EditorApplication.timeSinceStartup - _startTime;
            var rect = EditorGUILayout.BeginHorizontal();
            Vector2 topLeft = new Vector2(rect.x, rect.y);
            Vector2 topRight = new Vector2(rect.width, rect.y);
            if (Points == null) Points = new List<DataPoint>();

            for (int i = 0 ; i < Points.Count; i++)
            {
                Handles.color = Points[i].Color;
                Handles.DrawLine(new Vector2(( Points[i].Time * SizeDelta.x  + topLeft.x ) / Realtime, topLeft.y + SizeDelta.y ),
                    new Vector2(( Points[i].Time * SizeDelta.x  + topLeft.x ) / Realtime,  topLeft.y + SizeDelta.y  - Points[i].Height));
            }

            Handles.color = Color.green;
            Handles.DrawLine(new Vector2( topLeft.x, topLeft.y + SizeDelta.y ), new Vector2(SizeDelta.x, topLeft.y + SizeDelta.y));
            Handles.DrawLine(new Vector2( topLeft.x, topLeft.y ), new Vector2(topLeft.x, topLeft.y + SizeDelta.y));
            Handles.DrawLine(new Vector2( SizeDelta.x, topLeft.y ), new Vector2(SizeDelta.x, topLeft.y + SizeDelta.y));

            if (GUILayout.Button("Record"))
            {
                Record();
            }

            if (GUILayout.Button("Stop"))
            {
                Stop();
            }

            if (GUILayout.Button("Add"))
            {
                if (_recording)
                    Points.Add(new DataPoint( 20, Color.HSVToRGB(Random.value, 1, 1)));
            }

            if (GUILayout.Button("Reset"))
            {
                Reset();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        public static void AddPoint(float height, Color color)
        {
            if (_recording)
                Points.Add(new DataPoint( height, color));
        }

        public static void Reset()
        {
            _recording = false;
            _dataStored = false;
            Points.Clear();
            _startTime = 0;
        }

        public static void Record()
        {
            _recording = true;
            if (_dataStored) Reset();
            _dataStored = true;
            _startTime = (float) EditorApplication.timeSinceStartup;
        }

        public static void Stop()
        {
            _recording = false;
        }
    }
    #endif
}