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
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FunForLab.Analytics
{
    public class TubeDataLink : MonoBehaviour
    {
        public CapColors CapColors;
        public Transform Tube;
        public Transform Cap;
        public Transform Sticker;
        public bool GenerateOnStart = true;
        public bool GenerateWithColor;
        public bool EnableSticker;
        public bool EnableCap = true;
        public bool DataSet;
        public CapColors.Caps DefaultCapColor;
        MaterialPropertyBlock _prop;

        private static readonly int BarCode = Shader.PropertyToID("_BarCode");
        private static readonly int TopColor = Shader.PropertyToID("_TopColor");
        private static readonly int MidColor = Shader.PropertyToID("_MidColor");
        private static readonly int BotColor = Shader.PropertyToID("_BotColor");
        private static readonly int FirstLevel = Shader.PropertyToID("_FirstLevel");
        private static readonly int SecondLevel = Shader.PropertyToID("_SecondLevel");
        private static readonly int ThirdLevel = Shader.PropertyToID("_ThirdLevel");
        private Renderer _tubeMeshRenderer;
        private Renderer _stickerRenderer;
        private Renderer _capMeshRenderer;
        [SerializeField] private Data _data;
        public Data Data => _data;
        [SerializeField] private TubeData _tubeData;

        private void Setup()
        {
            if (_prop == null)
                _prop = new MaterialPropertyBlock();
            if (_tubeMeshRenderer == null)
                _tubeMeshRenderer = Tube.GetComponentInChildren<Renderer>();
            if (_capMeshRenderer == null)
                _capMeshRenderer = Cap.GetComponentInChildren<Renderer>();
            if (_stickerRenderer == null)
                _stickerRenderer = Sticker.GetComponentInChildren<Renderer>();
        }

        private void Start()
        {
            if (DataSet) return;
            if (GenerateOnStart)
                GenerateTube();
            else if (GenerateWithColor)
                GenerateTube(DefaultCapColor);
            else
                SetEmpty();
        }

        [Button] public void UnCap()
        {
            _capMeshRenderer.enabled = false;
        }

        [Button(name: "Regenerate")]  public void GenerateTube()
        {
            GenerateTube(new [] { CapColors.Caps.Red, CapColors.Caps.Green, CapColors.Caps.LightGreen, CapColors.Caps.Blue, CapColors.Caps.Purple, CapColors.Caps.Grey });
        }

        [Button]
        public void SetEmpty()
        {
            if (_prop == null)
                Setup();
            Color b = Color.white;
            Color m = Color.white;
            Color t = Color.white;
            float l1 = 0;
            float l2 = 0;
            float l3 = 0;

            _tubeData = new TubeData(false,false,0,l1, l2, l3, b, m, t);
            DisplayData(_tubeData);
        }

        public void UnEmpty()
        {
            if (DataSet)
            {
                SetData(_data);
            }
            else
            {
                GenerateTube();
            }
        }

        public void DisplayData (TubeData tubeData)
        {
            _prop.SetColor(BotColor, tubeData.BotColor);
            _prop.SetColor(MidColor, tubeData.MidColor);
            _prop.SetColor(TopColor, tubeData.TopColor);
            _prop.SetFloat(FirstLevel, tubeData.FirstLevel);
            _prop.SetFloat(SecondLevel, tubeData.SecondLevel);
            _prop.SetFloat(ThirdLevel, tubeData.ThirdLevel);

            _tubeMeshRenderer.SetPropertyBlock(_prop);
            _prop.SetInt(BarCode, 1 + 4 + ( tubeData.Barcode * 8 ) + 1310720);
            _stickerRenderer.SetPropertyBlock(_prop);
            _stickerRenderer.enabled = EnableSticker;
            _capMeshRenderer.enabled = EnableCap;
        }

        public void DisplayData(Data data, TubeData tubeData)
        {
            if (_prop == null)
                Setup();
            Color b = Color.white;
            Color m = Color.white;
            Color t = Color.white;
            float l1 = 0;
            float l2 = 0;
            float l3 = 0;
            _capMeshRenderer.sharedMaterial = CapColors.Colors[(int) data.CapColor];


            if (data.CapColor == CapColors.Caps.Yellow || data.CapColor == CapColors.Caps.LightGreen)
            {
                float buffer = Random.Range(.08f, .12f);
                if (data.Centrifugated)
                {
                    b = Data.GetBloodColor(data);
                    m = new Color(198f / 256f, 167f / 256f, 64f / 256f, 0.98f);
                    t =  new Color(248f / 256f, 237f / 256f, 162f / 256f, 0.5f);
                    l1 = Mathf.InverseLerp(0, 6f, data.BloodVolume.Value * data.PCV.Value * 0.01f);
                    l2 = l1 + buffer;
                    l3 = Mathf.InverseLerp(0, 6f, data.BloodVolume.Value + buffer);
                }
                else
                {
                    m = new Color(198f / 256f, 167f / 256f, 64f / 256f, 0.98f);
                    t = Data.GetBloodColor(data);
                    l2 = buffer;
                    l3 = Mathf.InverseLerp(0, 6f, data.BloodVolume.Value + buffer);
                }
            }
            else
            {
                t = Data.GetBloodColor(data);
                l3 = Mathf.InverseLerp(0, 6f, data.BloodVolume.Value);
            }

            _tubeData = new TubeData(tubeData.CapEnabled, tubeData.Centrifugated, data.SampleID, l1, l2, l3, b, m, t);
            DisplayData(_tubeData);
        }

        public void SetData(Data data, bool capEnabled = true)
        {
            DataSet = true;
            if (_prop == null)
                Setup();
            _data = data;
            _tubeData = new TubeData();
            _tubeData.Barcode = data.SampleID;
            _tubeData.CapEnabled = capEnabled;
            DisplayData( _data, _tubeData);
        }

        public void GenerateTube(CapColors.Caps[] caps, bool centrifugated = false, bool capEnabled = true)
        {
            _data = DataGenerator.Instance.GetNewData(caps.Random());
            SetData(_data, capEnabled);
        }

        public void GenerateTube(CapColors.Caps cap, bool centrifugated = false, bool capEnabled = true)
        {
            _data = DataGenerator.Instance.GetNewData(cap);
            SetData(_data, capEnabled);
        }

        [Button] public void Centrifugate()
        {
            if (_data.Centrifugated) return;
            if ( !( _data.CapColor == CapColors.Caps.Yellow || _data.CapColor == CapColors.Caps.LightGreen )) return;


            _data.Centrifugated = true;
            float buffer = _tubeData.SecondLevel;
            Color b = _tubeData.TopColor;
            Color m = new Color(198f / 256f, 167f / 256f, 64f / 256f, 0.98f);
            Color t =  new Color(248f / 256f, 237f / 256f, 162f / 256f, 0.5f);
            float l1 = Mathf.InverseLerp(0, 6f, _data.BloodVolume.Value * _data.PCV.Value * 0.01f);
            float l2 = l1 + buffer;
            float l3 = Mathf.InverseLerp(0, 6f, _data.BloodVolume.Value + buffer);
            _tubeData = new TubeData(_data.Centrifugated, l1, l2, l3, b, m, t);
            DisplayData(_tubeData);
        }

        [System.Serializable]
        public struct TubeData
        {
            public int Barcode;
            public bool Centrifugated;
            public bool CapEnabled;
            public float FirstLevel;
            public float SecondLevel;
            public float ThirdLevel;
            public Color BotColor;
            public Color MidColor;
            public Color TopColor;


            public TubeData(bool capEnabled, bool centrifugated, int barcode, float firstLevel, float secondLevel, float thirdLevel, Color botColor, Color midColor,
                Color topColor)
            {
                Centrifugated = centrifugated;
                CapEnabled = capEnabled;
                Barcode = barcode;
                FirstLevel = firstLevel;
                SecondLevel = secondLevel;
                ThirdLevel = thirdLevel;
                BotColor = botColor;
                MidColor = midColor;
                TopColor = topColor;
            }

            public TubeData(bool centrifugated, int barcode, float firstLevel, float secondLevel, float thirdLevel, Color botColor, Color midColor, Color topColor)
            {
                Centrifugated = centrifugated;
                CapEnabled = true;
                Barcode = barcode;
                FirstLevel = firstLevel;
                SecondLevel = secondLevel;
                ThirdLevel = thirdLevel;
                BotColor = botColor;
                MidColor = midColor;
                TopColor = topColor;
            }

            public TubeData(bool centrifugated, float firstLevel, float secondLevel, float thirdLevel, Color botColor, Color midColor, Color topColor)
            {
                Centrifugated = centrifugated;
                CapEnabled = true;
                Barcode = 0;
                FirstLevel = firstLevel;
                SecondLevel = secondLevel;
                ThirdLevel = thirdLevel;
                BotColor = botColor;
                MidColor = midColor;
                TopColor = topColor;
            }
        }
    }

    public static class EnumerableHelper<E>
    {
        private static System.Random r;

        static EnumerableHelper()
        {
            r = new System.Random();
        }

        public static T Random<T>(IEnumerable<T> input)
        {
            return input.ElementAt(r.Next(input.Count()));
        }
    }

    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> input)
        {
            return EnumerableHelper<T>.Random(input);
        }
    }
}