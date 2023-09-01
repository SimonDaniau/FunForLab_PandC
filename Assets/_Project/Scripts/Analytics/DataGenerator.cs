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
using System.Linq;
using UnityEngine;
using static FunForLab.Extensions;
using Random = UnityEngine.Random;

namespace FunForLab.Analytics
{
    public class DataGenerator : MonoBehaviour
    {
        public static DataGenerator Instance;
        private static HashSet<int> AllSamplesID;
        private static HashSet<int> AllPatientsID;

        private void Awake()
        {
            Instance = this;
            AllSamplesID = new HashSet<int>();
            AllPatientsID = new HashSet<int>();
        }

        private int GetUniqueSampleID()
        {
            int res = Random.Range(0, (int) Math.Pow(2, 14));
            while (AllSamplesID.Contains(res))
            {
                res = Random.Range(0, (int) Math.Pow(2, 14));
            }

            AllSamplesID.Add(res);
            return res;
        }

        private int GetUniquePatientID()
        {
            int res = Random.Range(0, (int) Math.Pow(2, 14));
            while (AllPatientsID.Contains(res))
            {
                res = Random.Range(0, (int) Math.Pow(2, 14));
            }

            AllPatientsID.Add(res);
            return res;
        }

        public PatientData GetNewPatientData(ref Data data)
        {
            PatientData res = new PatientData();
            res.PatientID = GetUniquePatientID();
            res.TubesID = new List<int>();
            res.ReadingsByTubesDictionary = new Dictionary<int, List<Enums.ReadingType>>();
            res.IsMale = data.Male;
            res.Birthday = new DateTime( DateTime.Today.Year - Random.Range(1, 110), Random.Range(1, 13), Random.Range(1, 31));
            res.Height = Mathf.Clamp(Maths.HeightRangeFromAge(res.Age, res.IsMale).Random(), 51f, 213f) ;
            res.Weight = Mathf.Clamp( Maths.WeightRangeFromHeight(res.Height, res.IsMale).Random(.2f), 1f, 150f);
            var readings  = new List<Enums.ReadingType>();
            readings.Add(data.ReadingType);
            res.TubesID.Add(data.SampleID);
            res.ReadingsByTubesDictionary.Add(data.SampleID, readings);
            data.PatientID = res.PatientID;
            return res;
        }

        public PatientData GetNewPatientData()
        {
            /* public int PatientID;
         public List<int> TubesID;
         public Dictionary<int, List<Enums.ReadingType>> ReadingsByTubesDictionary;
         public bool IsMale;
         public DateTime Birthday;
         public int Age => DateTime.Today.Year - Birthday.Year;
         public float Weight;
         public float Height;*/
            PatientData res = new PatientData();
            res.TubesID = new List<int>();
            res.PatientID = GetUniquePatientID();
            res.ReadingsByTubesDictionary = new Dictionary<int, List<Enums.ReadingType>>();
            res.IsMale = Random.Range(0, 100) > 50;
            res.Birthday = new DateTime( DateTime.Today.Year - Random.Range(1, 110), Random.Range(1, 13), Random.Range(1, 31));
            res.Height = Mathf.Clamp(Maths.HeightRangeFromAge(res.Age, res.IsMale).Random(), 51f, 213f) ;
            res.Weight = Maths.WeightRangeFromHeight(res.Height, res.IsMale).Random(.2f);
            int n = Random.Range(0, 4);
            for (int i = 0 ; i > n; i++)
            {
                var reading = (Enums.ReadingType) Random.Range(0, Enum.GetValues( typeof(Enums.ReadingType)).Length - 1);
                var readings  = new List<Enums.ReadingType>();
                readings.Add(reading);
                CapColors.Caps capColor = CapColors.Caps.Invalid;
                switch (reading)
                {
                    case Enums.ReadingType.HematologyCompleteBloodCount:
                        capColor = CapColors.Caps.Purple;
                        break;
                }

                var tube = GetNewData(capColor, res.IsMale);
                res.TubesID.Add(tube.SampleID);
                res.ReadingsByTubesDictionary.Add(tube.SampleID, readings);
            }

            // var cap  = (CapColors.Caps) Random.Range(0, Enum.GetValues( typeof(CapColors.Caps)).Length - 1);
            return res;
        }

        public Data GetNewData(CapColors.Caps capColor, bool male)
        {
            Data res = new Data();
            res.CapColor = capColor;
            res.TubeName = Data.GetTubeName(capColor);
            res.SampleID = GetUniqueSampleID();
            res.Male = male;
            if (male)
            {
                res.Centrifugated = false;
                res.BloodVolume = RR(Enums.HematologyMeasures.Volume, 3f, 4.5f, Enums.Unit.mL);
                if (res.CapColor == CapColors.Caps.Yellow || res.CapColor == CapColors.Caps.LightGreen)
                {
                    res.BloodVolume.Value *= 0.9f;
                }

                res.RBC = RR(Enums.HematologyMeasures.RBC, 4.2f, 5.7f, Enums.Unit.X10_6_uL);
                res.PCV = RR(Enums.HematologyMeasures.PCV, 40, 50, Enums.Unit.Percent);
                res.Hgb = RR(Enums.HematologyMeasures.Hgb, 13.6f, 16.9f, Enums.Unit.g_dL);
                res.MCV = RR(Enums.HematologyMeasures.MCV, 82.5f, 98f, Enums.Unit.fL);


                float nRBC = res.RBC.Value;
                float nPCV = res.PCV.Value;
                float nHgb = res.Hgb.Value;
                ( nPCV, nRBC, nHgb ) = CorrectLevelsEstimation(res, nRBC, nPCV, nHgb);

                res.RBC.Value = Mathf.Lerp(nRBC, res.RBC.Value, 0);
                res.PCV.Value = Mathf.Lerp(nPCV, res.PCV.Value, 0);
                res.Hgb.Value = Mathf.Lerp(nHgb, res.Hgb.Value, 0);
                res.Platelets = RR(Enums.HematologyMeasures.Platelets, 152f, 324f, Enums.Unit.X10_3_uL);
                res.WBC = RR(Enums.HematologyMeasures.WBC, 3.8f, 10.4f, Enums.Unit.X10_3_uL);
                res.Neutrophils = RR(Enums.HematologyMeasures.Neutrophils, 38f, 54f, Enums.Unit.Percent);
                res.Lymphocytes = RR(Enums.HematologyMeasures.Lymphocytes, 28, 50, Enums.Unit.Percent);
                res.Eosinophils = RR(Enums.HematologyMeasures.Eosinophils, 0.5f, 3.5f, Enums.Unit.Percent);
                res.Basophils = RR(Enums.HematologyMeasures.Basophils, 2.5f, 7.5f, Enums.Unit.Percent);
                res.Monocytes = RR(Enums.HematologyMeasures.Monocytes, 4f, 12f, Enums.Unit.Percent);
                float factor = 100f / new [] { res.Neutrophils, res.Lymphocytes, res.Eosinophils, res.Basophils, res.Monocytes }.Select(x => x.Value).Sum();
                res.Neutrophils.Value *= factor;
                res.Lymphocytes.Value *= factor;
                res.Eosinophils.Value *= factor;
                res.Basophils.Value *= factor;
                res.Monocytes.Value *= factor;
            }
            else
            {
                res.BloodVolume = RR(Enums.HematologyMeasures.Volume, 3f, 4.5f, Enums.Unit.mL);
                res.Centrifugated = false;
                if (res.CapColor == CapColors.Caps.Yellow || res.CapColor == CapColors.Caps.LightGreen)
                {
                    res.BloodVolume.Value *= 0.9f;
                }

                res.RBC = RR(Enums.HematologyMeasures.RBC, 3.8f, 5f, Enums.Unit.X10_6_uL);
                res.PCV = RR(Enums.HematologyMeasures.PCV, 35, 43f, Enums.Unit.Percent);
                res.Hgb = RR(Enums.HematologyMeasures.Hgb, 11.9f, 14.8f, Enums.Unit.g_dL);
                res.MCV = RR(Enums.HematologyMeasures.MCV, 82.5f, 98f, Enums.Unit.fL);

                float nRBC = res.RBC.Value;
                float nPCV = res.PCV.Value;
                float nHgb = res.Hgb.Value;

                ( nPCV, nRBC, nHgb ) = CorrectLevelsEstimation(res, nRBC, nPCV, nHgb);

                res.RBC.Value = Mathf.Lerp(nRBC, res.RBC.Value, 0.2f);
                res.PCV.Value = Mathf.Lerp(nPCV, res.PCV.Value, 0.2f);
                res.Hgb.Value = Mathf.Lerp(nHgb, res.Hgb.Value, 0.2f);

                res.Platelets = RR(Enums.HematologyMeasures.Platelets, 153f, 361f, Enums.Unit.X10_3_uL);
                res.WBC = RR(Enums.HematologyMeasures.WBC, 3.8f, 10.4f, Enums.Unit.X10_3_uL);
                res.Neutrophils = RR(Enums.HematologyMeasures.Neutrophils, 36.4f, 50.4f, Enums.Unit.Percent);
                res.Lymphocytes = RR(Enums.HematologyMeasures.Lymphocytes, 31.5f, 52.1f, Enums.Unit.Percent);
                res.Eosinophils = RR(Enums.HematologyMeasures.Eosinophils, 0.8f, 3.2f, Enums.Unit.Percent);
                res.Basophils = RR(Enums.HematologyMeasures.Basophils, 2.4f, 6.2f, Enums.Unit.Percent);
                res.Monocytes = RR(Enums.HematologyMeasures.Monocytes, 6.6f, 13.4f, Enums.Unit.Percent);
                float factor = 100f / new [] { res.Neutrophils, res.Lymphocytes, res.Eosinophils, res.Basophils, res.Monocytes }.Select(x => x.Value).Sum();
                res.Neutrophils.Value *= factor;
                res.Lymphocytes.Value *= factor;
                res.Eosinophils.Value *= factor;
                res.Basophils.Value *= factor;
                res.Monocytes.Value *= factor;
            }

            return Recalculate(res);
        }

        private (float nPCV, float nRBC, float nHgb) CorrectLevelsEstimation(Enums.HematologyMeasures m, Data res, float nRBC, float nPCV, float nHgb)
        {
            if (m == Enums.HematologyMeasures.RBC)
            {
                nPCV = ( res.MCV.Value / 100f ) * (  nRBC / 10f );
                nHgb = Half( ( nPCV / 100f ) / 3f,  nRBC * 3f);
            }
            else if (m == Enums.HematologyMeasures.PCV)
            {
                nRBC  = (  ( nPCV / 100f ) * 10f ) / ( res.MCV.Value / 100f );
                nHgb = Half( ( nPCV / 100f ) / 3f,  nRBC * 3f);
            }
            else
            {
                nPCV = nHgb * 3f;
                nRBC  = ( ( nPCV / 100f ) * 10f ) / ( res.MCV.Value / 100f );
            }

            nRBC = Half(nRBC, res.RBC.Value);
            nPCV = Half(nPCV, res.PCV.Value);
            nHgb = Half(nHgb, res.Hgb.Value);
            return ( nPCV, nRBC, nHgb );
        }

        private (float nPCV, float nRBC, float nHgb) CorrectLevelsEstimation(Data res, float nRBC, float nPCV, float nHgb)
        {
            var rdm = Random.Range(0f, 1f);
            if (rdm < 0.333f)
            {
                nPCV = ( res.MCV.Value / 100f ) * (  nRBC / 10f );
                nHgb = Half( ( nPCV / 100f ) / 3f,  nRBC * 3f);
            }
            else if (rdm < 0.666f)
            {
                nRBC  = (  ( nPCV / 100f ) * 10f ) / ( res.MCV.Value / 100f );
                nHgb = Half( ( nPCV / 100f ) / 3f,  nRBC * 3f);
            }
            else
            {
                nPCV = nHgb * 3f;
                nRBC  = ( ( nPCV / 100f ) * 10f ) / ( res.MCV.Value / 100f );
            }

            nRBC = Half(nRBC, res.RBC.Value);
            nPCV = Half(nPCV, res.PCV.Value);
            nHgb = Half(nHgb, res.Hgb.Value);
            return ( nPCV, nRBC, nHgb );
        }

        public Data Recalculate(Data d)
        {
            if (d.Male)
            {
                d.MCH = new Measure(Enums.HematologyMeasures.MCH, 10f * d.Hgb.Value / d.RBC.Value, new Vector2(18.7f, 22.7f), Enums.Unit.pg);
                d.MCHC = new Measure( Enums.HematologyMeasures.MCHC, d.Hgb.Value / ( d.PCV.Value / 100f ), new Vector2(33f, 36f), Enums.Unit.g_dL);
            }
            else
            {
                d.MCH = new Measure(Enums.HematologyMeasures.MCH, 10f * d.Hgb.Value / d.RBC.Value, new Vector2(17.1f, 23.5f), Enums.Unit.pg);
                d.MCHC = new Measure( Enums.HematologyMeasures.MCHC, d.Hgb.Value / ( d.PCV.Value / 100f ), new Vector2(28.7f, 35.7f), Enums.Unit.g_dL);
            }

            return d;
        }

        public Data GetNewData(bool male)
        {
            return GetNewData((CapColors.Caps) Random.Range(0, Enum.GetValues( typeof(CapColors.Caps)).Length - 1), male);
        }

        public Data GetNewData(CapColors.Caps capColor)
        {
            return GetNewData(capColor, Random.Range(0f, 1f) > .5f);
        }

        public Data GetNewData()
        {
            var cap  = (CapColors.Caps) Random.Range(0, Enum.GetValues( typeof(CapColors.Caps)).Length - 1);
            var male = Random.Range(0f, 1f) > .5f;
            return GetNewData(cap, male);
        }

        public Data ProcessDiseaseTag(Data d, DiseaseTag disease)
        {
            DiseaseMeasurePoint p = default;
            if (disease.GetDetermination() == DiseaseTag.CriteriaDetermination.Or)
            {
                p = disease.Criterias.Where(x => x.Male == d.Male).Random();
                d =  ChangeDataParameterMatchingDisease(d, p);
            }

            if (disease.GetDetermination() == DiseaseTag.CriteriaDetermination.And)
            {
                foreach (var point in disease.Criterias.Where(x => x.Male == d.Male))
                {
                    d =  ChangeDataParameterMatchingDisease(d, point);
                }
            }

            d.DiseaseTagList.Add(disease.DiseaseName);
            return d;
        }

        public Data ProcessDiseaseTag(DiseaseTag disease) => ProcessDiseaseTag(GetNewData(), disease);
        public Data ProcessDiseaseTag(CapColors.Caps capColor, DiseaseTag disease) => ProcessDiseaseTag(GetNewData(capColor), disease);
        public Data ProcessDiseaseTag(bool male, DiseaseTag disease) => ProcessDiseaseTag(GetNewData(male), disease);
        public Data ProcessDiseaseTag( CapColors.Caps capColor, bool male, DiseaseTag disease) => ProcessDiseaseTag(GetNewData(capColor, male), disease);

        public Data ChangeDataParameterMatchingDisease(Data d, DiseaseMeasurePoint point)
        {
            Vector2 range;
            switch (point.HematologyParameter)
            {
                case Enums.HematologyMeasures.RBC :
                {
                    range = d.RBC.NormalRange;
                    d.RBC = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.RBC.Unit);
                    d.RBC.NormalRange = range;
                    float nRBC =  d.RBC.Value;
                    float nPCV =  d.PCV.Value;
                    float nHgb =  d.Hgb.Value;
                    ( nPCV, nRBC, nHgb ) = CorrectLevelsEstimation( Enums.HematologyMeasures.RBC, d, nRBC, nPCV, nHgb);
                    d.RBC.Value = Mathf.Lerp(nRBC, d.RBC.Value, 0.8f);
                    d.PCV.Value = Mathf.Lerp(nPCV, d.PCV.Value, 0.8f);
                    d.Hgb.Value = Mathf.Lerp(nHgb, d.Hgb.Value, 0.8f);
                }
                    break;
                case Enums.HematologyMeasures.PCV :
                {
                    range = d.PCV.NormalRange;
                    d.PCV = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.PCV.Unit);
                    d.PCV.NormalRange = range;
                    float nRBC =  d.RBC.Value;
                    float nPCV =  d.PCV.Value;
                    float nHgb =  d.Hgb.Value;
                    ( nPCV, nRBC, nHgb ) = CorrectLevelsEstimation( Enums.HematologyMeasures.PCV, d , nRBC, nPCV, nHgb);
                    d.RBC.Value = Mathf.Lerp(nRBC, d.RBC.Value, 0.8f);
                    d.PCV.Value = Mathf.Lerp(nPCV, d.PCV.Value, 0.8f);
                    d.Hgb.Value = Mathf.Lerp(nHgb, d.Hgb.Value, 0.8f);
                }
                    break;
                case Enums.HematologyMeasures.Hgb :
                {
                    range = d.Hgb.NormalRange;
                    d.Hgb = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.Hgb.Unit);
                    d.Hgb.NormalRange = range;
                }
                    break;
                case Enums.HematologyMeasures.MCV :
                    d.MCV = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.MCV.Unit);
                    break;
                case Enums.HematologyMeasures.Platelets :
                    d.Platelets = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.Platelets.Unit);
                    break;
                case Enums.HematologyMeasures.WBC :
                    d.WBC = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.WBC.Unit);
                    break;
                case Enums.HematologyMeasures.Basophils :
                    d.Basophils = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.Basophils.Unit);
                    break;
                case Enums.HematologyMeasures.Neutrophils :
                    d.Neutrophils = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.Neutrophils.Unit);
                    break;
                case Enums.HematologyMeasures.Eosinophils :
                    d.Eosinophils = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.Eosinophils.Unit);
                    break;
                case Enums.HematologyMeasures.Lymphocytes :
                    d.Lymphocytes = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.Lymphocytes.Unit);
                    break;
                case Enums.HematologyMeasures.Monocytes :
                    d.Monocytes = RR(point.HematologyParameter, point.DiseaseGenerationRange, d.Monocytes.Unit);
                    break;
            }

            return d;
        }

        public Measure RR(Enums.HematologyMeasures m, float a, float b, Enums.Unit u) => new Measure(m, Random.Range(a, b), new Vector2(a, b), u);
        public Measure RR(Enums.HematologyMeasures m, Vector2 range, Enums.Unit u) => new Measure(m, Random.Range(range.x, range.y), range, u);
    }
}