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
using System.Reflection;
using FunForLab.UI.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace FunForLab
{
    [Serializable]
    public class IDObject
    {
        public GameObject GameObject;
        public long ID;

        public IDObject(GameObject obj, long id)
        {
            GameObject = obj;
            ID = id;
        }
    }

    public class Notepad : EditorWindow, IHasCustomMenu
    {
        private bool deleteMode;
        private static Notepad _window;
        private static GameObject[] cachedAll;
        private static int scrollIndex;
        [SerializeField] private string currentScene;
        [SerializeField] private List<string> notes = new List<string>();
        [SerializeField] private List<string> notes2 = new List<string>();
        [SerializeField] private Vector2 scrollPos;
        [SerializeField] private string _newNote;
        [SerializeField] private List<IDObject> sources = new List<IDObject>();


        [MenuItem("Window/Notepad")]
        static void Create()
        {
            _window = (Notepad) EditorWindow.GetWindow(typeof(Notepad));
            _window.Show();
        }


        void Reset()
        {
            notes.Clear();
            notes2.Clear();
            sources.Clear();
            var data = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString("Notepad", data);
        }

        public static void HierarchyChanged()
        {
            if (_window.currentScene != SceneManager.GetActiveScene().name)
            {
                _window.currentScene = SceneManager.GetActiveScene().name;
                Debug.Log("SceneChanged:" + _window.currentScene);
                cachedAll = null;
                foreach (var item in _window.sources)
                {
                    item.GameObject = GetGameObjectFromFileID(item.ID);
                }
            }
        }

        public void OnEnable()
        {
            var data = EditorPrefs.GetString("Notepad", JsonUtility.ToJson(this, false));
            JsonUtility.FromJsonOverwrite(data, this);
            EditorApplication.hierarchyChanged += HierarchyChanged;
        }

        public void OnDisable()
        {
            var data = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString("Notepad", data);
            notes.Clear();
            notes2.Clear();
            sources.Clear();
            EditorApplication.hierarchyChanged -= HierarchyChanged;
        }

        private static long GetFileIDFromGameObject(GameObject go)
        {
            PropertyInfo inspectorModeInfo =
                typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            SerializedObject serializedObject = new SerializedObject(go);
            inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

            SerializedProperty localIdProp =
                serializedObject.FindProperty("m_LocalIdentfierInFile"); //note the misspelling!

            return localIdProp.longValue;
        }

        private static GameObject GetGameObjectFromFileID(long fileID) // also called local identifier
        {
            GameObject resultGo = null;
            if (cachedAll == null)
                cachedAll = Resources.FindObjectsOfTypeAll<GameObject>();
            var some = new List<GameObject>();
            foreach (var go in cachedAll)
            {
                if (go.GetComponent<NotepadIdentifier>() != null)
                {
                    some.Add(go);
                }
            }

            var gameObjects = some;
            // Test every gameobjects
            foreach (var go in gameObjects)
            {
#if UNITY_EDITOR
                PropertyInfo inspectorModeInfo =
                    typeof(UnityEditor.SerializedObject).GetProperty("inspectorMode",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(go);
                inspectorModeInfo.SetValue(serializedObject, UnityEditor.InspectorMode.Debug, null);
                UnityEditor.SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");
#endif
                if (localIdProp.longValue == fileID) resultGo = go;
            }

            return resultGo;
        }

        public void OnGUI()
        {
            // GUILayout.BeginHorizontal();
            // scrollIndex = EditorGUILayout.IntField(scrollIndex);


            float f = 0;
            for (var i = 0; i < notes2.Count; i++)
            {
                var item = notes2[i];
                f += GUIStyle.none.CalcSize(new GUIContent(item)).y;
            }

            if (_window == null) Create();
            var winHeight = _window.position.height;
            var contentHeight = 20 * notes2.Count + f;
            var bias = 90;
            var scrollBarMax = Mathf.Max(0, -(winHeight - contentHeight - bias));
            var scrollViewPort = winHeight - bias;
            // EditorGUILayout.LabelField(winHeight.ToString());
            // EditorGUILayout.LabelField(scrollPos.y.ToString());
            // EditorGUILayout.LabelField(contentHeight.ToString());
            // EditorGUILayout.LabelField(scrollBarMax.ToString());
            // EditorGUILayout.LabelField(scrollViewPort.ToString());

            //var contentHeightUpToIndex = GetScrollHeightAtIndex(scrollIndex);
            // EditorGUILayout.LabelField(contentHeightUpToIndex.ToString());
            // GUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("CopyJsonToClipboard", GUILayout.ExpandWidth(false)))
            {
                GUIUtility.systemCopyBuffer = JsonUtility.ToJson(this, false);
            }

            if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false)))
            {
                foreach (var item in sources)
                {
                    item.GameObject = GetGameObjectFromFileID(item.ID);
                }
            }
            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
            {
                notes.Clear();
                notes2.Clear();
                sources.Clear();
            }

            if (GUILayout.Button("LoadJsonFromClipboard", GUILayout.ExpandWidth(false)))
            {
                var data = GUIUtility.systemCopyBuffer;
                JsonUtility.FromJsonOverwrite(data, this);
                foreach (var item in sources)
                {
                    item.GameObject = GetGameObjectFromFileID(item.ID);
                }
            }

            deleteMode = GUILayout.Toggle(deleteMode, "DeleteMode");
            EditorGUILayout.EndHorizontal();


            // Create an input field for entering new notes
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Note", GUILayout.ExpandWidth(false)))
            {
                // Add the new note to the list
                notes.Add(_newNote);
                notes2.Add("");
                sources.Add(new IDObject(default, default));
                _newNote = "";
                EditorGUI.FocusTextInControl("");
            }

            _newNote = EditorGUILayout.TextField(_newNote, GUILayout.ExpandWidth(true));

            EditorGUILayout.EndHorizontal();

            // Create a scrollable list to display the notes
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            GUILayout.Space(20);
            for (int i = 0; i < notes.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                var tmp = (GameObject) EditorGUILayout.ObjectField(sources[i].GameObject, typeof(GameObject),
                    GUILayout.Width(100));
                if (tmp != sources[i].GameObject)
                {
                    if (tmp == null)
                    {
                        if (sources[i].GameObject != null)
                        {
                            var comp = sources[i].GameObject.GetComponent<NotepadIdentifier>();
                            DestroyImmediate(comp);
                        }

                        sources[i].GameObject = null;
                        sources[i].ID = 0;
                    }
                    else
                    {
                        sources[i].ID = GetFileIDFromGameObject(tmp);

                        var comp = tmp.GetComponent<NotepadIdentifier>();
                        if (comp == null)
                        {
                            EditorUtility.SetDirty(tmp);
                            var idComp = tmp.AddComponent<NotepadIdentifier>();
                            idComp.FileId = sources[i].ID;
                        }
                        else
                        {
                            sources[i].ID = comp.FileId;
                        }

                        NotepadIdentifier identifier;

                        if (!ReferenceEquals(sources[i], null))
                            if (sources[i].GameObject != null)
                                identifier = sources[i].GameObject.GetComponent<NotepadIdentifier>();
                            else
                                identifier = null;
                        else
                            identifier = null;

                        //var isMissing = ReferenceEquals(identifier, null) ? false : (identifier ? false : true);
                        if (identifier != null) Destroy(identifier);

                        sources[i].GameObject = tmp;
                    }
                }


                notes[i] = EditorGUILayout.TextField(notes[i], GUILayout.MaxWidth(150));
                if (deleteMode)
                {
                    if (GUILayout.Button("x", GUILayout.MaxWidth(20)))
                    {
                        notes.Remove(notes[i]);
                    }
                }
                if (i>0 &&GUILayout.Button("/\\", GUILayout.MaxWidth(20)))
                {
                    var ntTmp = notes[i];
                    notes[i] = notes[i-1];
                    notes[i - 1] = ntTmp;
                    
                    var nt2Tmp = notes2[i];
                    notes2[i] = notes2[i-1];
                    notes2[i - 1] = nt2Tmp;

                    var scTmp = sources[i];
                    sources[i] = sources[i-1];
                    sources[i - 1] = scTmp;
                }
                if (i <notes.Count -1 && GUILayout.Button("\\/", GUILayout.MaxWidth(20)))
                {
                    var ntTmp = notes[i];
                    notes[i] = notes[i+1];
                    notes[i + 1] = ntTmp;
                    
                    var nt2Tmp = notes2[i];
                    notes2[i] = notes2[i+1];
                    notes2[i + 1] = nt2Tmp;

                    var scTmp = sources[i];
                    sources[i] = sources[i+1];
                    sources[i + 1] = scTmp;
                }

                EditorGUILayout.EndHorizontal();
                notes2[i] = EditorGUILayout.TextArea(notes2[i]);
            }

            GUILayout.Space(200);
            EditorGUILayout.EndScrollView();

            //if (GUILayout.Button("Close"))
            //{
            //    _window.Close();
            //}
        }

        private float GetScrollHeightAtIndex(int index)
        {
            if (index >= notes2.Count) return -1;
            float contentHeightUpToIndex = 0;
            for (var i = 0; i < index; i++)
            {
                var item = notes2[i];
                contentHeightUpToIndex += GUIStyle.none.CalcSize(new GUIContent(item)).y;
            }

            contentHeightUpToIndex += index * 20;
            return contentHeightUpToIndex;
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            GUIContent content = new GUIContent("Reset");
            menu.AddItem(content, false, Reset);
        }
    }
}