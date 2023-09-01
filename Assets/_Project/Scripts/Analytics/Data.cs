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
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FunForLab.Analytics
{
    [System.Serializable]
    public struct Measure
    {
        public Enums.HematologyMeasures Parameter;
        public float Value;
        public Vector2 NormalRange;
        public Enums.Unit Unit;

        public Measure(Enums.HematologyMeasures parameter, float value, Enums.Unit unit)
        {
            Parameter = parameter;
            Value = value;
            Unit = unit;
            NormalRange = Vector2.zero;
        }

        public Measure(Enums.HematologyMeasures parameter, float value, Vector2 normalRange, Enums.Unit unit)
        {
            Parameter = parameter;
            Value = value;
            Unit = unit;
            NormalRange = normalRange;
        }

        public int Check()
        {
            if (Value > NormalRange.y) return 1;
            if (Value < NormalRange.x) return -1;
            return 0;
        }

        public int CheckValue(float value)
        {
            if (value > NormalRange.y) return 1;
            if (value < NormalRange.x) return -1;
            return 0;
        }

        public int CheckRange(Vector2 range)
        {
            if (Value > range.y) return 1;
            if (Value < range.x) return -1;
            return 0;
        }
    }


    [System.Serializable]
    public class Data
    {
        public int SampleID;
        public int PatientID;
        public string TubeName;
        public CapColors.Caps CapColor;
        public Enums.ReadingType ReadingType;
        public bool Male;
        public bool Centrifugated;
        public Measure BloodVolume; // M - Volue de sang dans l'echantillon
        public Measure RBC; //M - Nombre de globules rouge/10^6 par microlitre
        public Measure PCV; //M - proportion de cellules dans volume de sang - hematocrite
        public Measure Hgb; //M - poids de l'hemoglobine dans un volume de sang
        public Measure MCV; //M - volume globulaire moyen
        public Measure MCH; //C - teneur corpusculaire moyenne en hémoblobine (hemoglobine (g/L) / nombre globules rouge dans volume(n/L))
        public Measure MCHC; //C - concentration corpusculaire moyenne en hémoglobine (hemoglobine / hematocrite)
        public Measure Platelets; //M - Nombre de plaquettes/10^3 par microlitre

        public Measure WBC; //M - Nombre de globules blanc/10^3 par microlitre

        //types de globules blancs
        public Measure Basophils;
        public Measure Neutrophils;
        public Measure Eosinophils;
        public Measure Lymphocytes;
        public Measure Monocytes;

        public List<string> DiseaseTagList;

        public Data()
        {
            DiseaseTagList = new List<string>();
        }

        public static float GetDataParameter(Data d, Enums.HematologyMeasures hematologyMeasure)
        {
            switch (hematologyMeasure)
            {
                case Enums.HematologyMeasures.RBC : return d.RBC.Value;
                case Enums.HematologyMeasures.PCV : return d.PCV.Value;
                case Enums.HematologyMeasures.Hgb : return d.Hgb.Value;
                case Enums.HematologyMeasures.MCV : return d.MCV.Value;
                case Enums.HematologyMeasures.Platelets : return d.Platelets.Value;
                case Enums.HematologyMeasures.WBC : return d.WBC.Value;
                case Enums.HematologyMeasures.Basophils : return d.Basophils.Value;
                case Enums.HematologyMeasures.Neutrophils : return d.Neutrophils.Value;
                case Enums.HematologyMeasures.Eosinophils : return d.Eosinophils.Value;
                case Enums.HematologyMeasures.Lymphocytes : return d.Lymphocytes.Value;
                case Enums.HematologyMeasures.Monocytes : return d.Monocytes.Value;
                case Enums.HematologyMeasures.MCH: return d.MCH.Value;
                case Enums.HematologyMeasures.MCHC: return d.MCHC.Value;
                default:
                    throw new NotImplementedException($"{hematologyMeasure} not implemented in parameter getter");
            }
        }

        public string GetReading(Measure measure, float additiveError, float factorError)
        {
            return $"{measure.Parameter.ToString()} : {measure.Value * factorError + additiveError} {measure.Unit.ToString()}";
        }

        public string GetFullReading( float error)
        {
            return
                GetReading(WBC ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(RBC ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(Hgb ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(PCV ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(MCV ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(MCH ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(MCHC ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(Platelets ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(Neutrophils ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(Lymphocytes ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(Monocytes ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(Eosinophils ,  Random.Range(-error, error), 1 + Random.Range(-error, error)) +
                "\n" +
                GetReading(Basophils ,  Random.Range(-error, error), 1 + Random.Range(-error, error))
                ;
        }

        public string GetFullReadingFlags()
        {
            string msg = "";
            msg += ( WBC.Check() > 0 ? "*" : WBC.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( RBC.Check() > 0 ? "*" : RBC.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( Hgb.Check() > 0 ? "*" : Hgb.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( PCV.Check() > 0 ? "*" : PCV.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( MCV.Check() > 0 ? "*" : MCV.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( MCH.Check() > 0 ? "*" : MCH.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( MCHC.Check() > 0 ? "*" : MCHC.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( Platelets.Check() > 0 ? "*" : Platelets.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( Neutrophils.Check() > 0 ? "*" : Neutrophils.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( Lymphocytes.Check() > 0 ? "*" : Lymphocytes.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( Monocytes.Check() > 0 ? "*" : Monocytes.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( Eosinophils.Check() > 0 ? "*" : Eosinophils.Check() < 0 ? "*" : "" ) +  "\n";
            msg += ( Basophils.Check() > 0 ? "*" : Basophils.Check() < 0 ? "*" : "" ) +  "\n";
            return msg;
        }

        public string GetFullReadingParameter()
        {
            return
                GetReadingParameter(WBC) +
                "\n" +
                GetReadingParameter(RBC) +
                "\n" +
                GetReadingParameter(Hgb) +
                "\n" +
                GetReadingParameter(PCV) +
                "\n" +
                GetReadingParameter(MCV) +
                "\n" +
                GetReadingParameter(MCH) +
                "\n" +
                GetReadingParameter(MCHC) +
                "\n" +
                GetReadingParameter(Platelets) +
                "\n" +
                GetReadingParameter(Neutrophils) +
                "\n" +
                GetReadingParameter(Lymphocytes) +
                "\n" +
                GetReadingParameter(Monocytes) +
                "\n" +
                GetReadingParameter(Eosinophils) +
                "\n" +
                GetReadingParameter(Basophils)
                ;
        }

        public string GetFullReadingValue()
        {
            return
                GetReadingValue(WBC) +
                "\n" +
                GetReadingValue(RBC) +
                "\n" +
                GetReadingValue(Hgb) +
                "\n" +
                GetReadingValue(PCV) +
                "\n" +
                GetReadingValue(MCV) +
                "\n" +
                GetReadingValue(MCH) +
                "\n" +
                GetReadingValue(MCHC) +
                "\n" +
                GetReadingValue(Platelets) +
                "\n" +
                GetReadingValue(Neutrophils) +
                "\n" +
                GetReadingValue(Lymphocytes) +
                "\n" +
                GetReadingValue(Monocytes) +
                "\n" +
                GetReadingValue(Eosinophils) +
                "\n" +
                GetReadingValue(Basophils)
                ;
        }

        public string GetFullReadingUnit()
        {
            return
                GetReadingUnit(WBC) +
                "\n" +
                GetReadingUnit(RBC) +
                "\n" +
                GetReadingUnit(Hgb) +
                "\n" +
                GetReadingUnit(PCV) +
                "\n" +
                GetReadingUnit(MCV) +
                "\n" +
                GetReadingUnit(MCH) +
                "\n" +
                GetReadingUnit(MCHC) +
                "\n" +
                GetReadingUnit(Platelets) +
                "\n" +
                GetReadingUnit(Neutrophils) +
                "\n" +
                GetReadingUnit(Lymphocytes) +
                "\n" +
                GetReadingUnit(Monocytes) +
                "\n" +
                GetReadingUnit(Eosinophils) +
                "\n" +
                GetReadingUnit(Basophils)
                ;
        }

        public string GetFullReadingRange()
        {
            return
                GetReadingRange(WBC) +
                "\n" +
                GetReadingRange(RBC) +
                "\n" +
                GetReadingRange(Hgb) +
                "\n" +
                GetReadingRange(PCV) +
                "\n" +
                GetReadingRange(MCV) +
                "\n" +
                GetReadingRange(MCH) +
                "\n" +
                GetReadingRange(MCHC) +
                "\n" +
                GetReadingRange(Platelets) +
                "\n" +
                GetReadingRange(Neutrophils) +
                "\n" +
                GetReadingRange(Lymphocytes) +
                "\n" +
                GetReadingRange(Monocytes) +
                "\n" +
                GetReadingRange(Eosinophils) +
                "\n" +
                GetReadingRange(Basophils)
                ;
        }
        public string GetReadingRange(Measure measure)
        {
            return $"{measure.NormalRange.x.ToString("F")} - {measure.NormalRange.y.ToString("F")}";
        }

        public string GetReadingParameter(Measure measure)
        {
            var str = measure.Parameter.ToString();
            return $"{str.Substring(0, Mathf.Min(str.Length, 5))}";
        }

        public string GetReadingValue(Measure measure)
        {
            return $"{measure.Value.ToString("F")}";
        }

        public string GetReadingUnit(Measure measure)
        {
            switch (measure.Unit)
            {
                case Enums.Unit.g_dL:
                    return "g / dL";
                case Enums.Unit.X10_3_uL:
                    return "x10^3 / uL";
                case Enums.Unit.X10_6_uL:
                    return "x10^6 / uL";
                case Enums.Unit.Percent:
                    return "%";
                default:
                    return measure.Unit.ToString();
            }
        }

        private Data(CapColors.Caps capColor)
        {
            CapColor = capColor;
        }

        public Data None => new Data(CapColor = CapColors.Caps.Invalid);

        public static Color GetCapColor(Data d)
        {
            return Color.black;
        }

        public static string GetTubeName (CapColors.Caps capColor)
        {
            return new [] { "SERUM", "HEPARIN", "PST", "CITRATE", "EDTA", "", "SST", "GLUCOSE", "INVALID" }[(int) capColor];
        }

        public static Color GetBloodColor(Data d, int state = 1)
        {
            Color c = new Color();
            Color[] values1 = new []
            {
                new Color(60f / 256f, 10f / 256f, 18f / 256f),
                new Color(106f / 256f, 8f / 256f, 15f / 256f),
                new Color(146f / 256f, 6f / 256f, 12f / 256f),
                new Color(179f / 256f, 14f / 256f, 14f / 256f),
                new Color(203f / 256f, 14f / 256f, 5f / 256f),
                new Color(225f / 256f, 19f / 256f, 18f / 256f),
                new Color(234f / 256f, 37f / 256f, 33f / 256f),
                new Color(241f / 256f, 43f / 256f, 42f / 256f),
            };
            Color[] values2 = new []
            {
                new Color(22f / 256f, 6f / 256f, 6f / 256f),
                new Color(45f / 256f, 5f / 256f, 1f / 256f),
                new Color(55f / 256f, 5f / 256f, 1f / 256f),
                new Color(64f / 256f, 3f / 256f, 0f / 256f),
                new Color(79f / 256f, 6f / 256f, 4f / 256f),
                new Color(94f / 256f, 8f / 256f, 7f / 256f),
                new Color(112f / 256f, 8f / 256f, 7f / 256f),
                new Color(132f / 256f, 6f / 256f, 9f / 256f),
            };
            var values = values2;
            float[] keys = new [] { 18f, 16f, 14f, 12f, 10f, 8f, 6f, 4f };
            Keyframe[] keyframes = new Keyframe[8];
            for (int i = 0; i < keyframes.Length; i++)
            {
                keyframes[i] = GetColorKeyed(keys[i], values[i], 0);
            }

            var curveR = new AnimationCurve(keyframes);
            for (int i = 0; i < keyframes.Length; i++)
            {
                keyframes[i] = GetColorKeyed(keys[i], values[i], 1);
            }

            var curveG = new AnimationCurve(keyframes);
            for (int i = 0; i < keyframes.Length; i++)
            {
                keyframes[i] = GetColorKeyed(keys[i], values[i], 2);
            }

            var curveB = new AnimationCurve(keyframes);

            c.r = curveR.Evaluate(d.Hgb.Value);
            c.g = curveG.Evaluate(d.Hgb.Value);
            c.b = curveB.Evaluate(d.Hgb.Value);
            c.a = 0.95f;
            return c;

            Keyframe GetColorKeyed(float key, Color col, int channel)
            {
                return new Keyframe { time = key, value = channel == 0 ? col.r : channel == 1 ? col.g : col.b };
            }
        }
    }
}