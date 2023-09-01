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
using System.Globalization;
using System.Linq;
using FunForLab.Analytics;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace FunForLab.Modules
{
    public class DisplayModule : MonoBehaviour
    {
        public Transform Canvas;
        public Enums.DisplayType Type;

        [ShowIfGroup("B1" , Condition = "@Type == Enums.DisplayType.HematologyCompleteBloodCount")]
        [BoxGroup("B1/HematologyCompleteBloodCount")]
        public GameObject HematologyCBCGroup;

        [BoxGroup("B1/HematologyCompleteBloodCount")]
        public TextMeshProUGUI HematologyCBCItemText;

        [BoxGroup("B1/HematologyCompleteBloodCount")]
        public TextMeshProUGUI HematologyCBCDataText;

        [BoxGroup("B1/HematologyCompleteBloodCount")]
        public TextMeshProUGUI HematologyCBCUnitText;

        [BoxGroup("B1/HematologyCompleteBloodCount")]
        public TextMeshProUGUI HematologyCBCRangeText;

        [BoxGroup("B1/HematologyCompleteBloodCount")]
        public TextMeshProUGUI HematologyCBCFlagText;

        [ShowIfGroup("B2" , Condition = "@Type == Enums.DisplayType.PatientData")]
        [BoxGroup("B2/PatientData")]
        public GameObject PatientDataGroup;

        [BoxGroup("B2/PatientData")]
        public TextMeshProUGUI PatientDataIDText;

        [BoxGroup("B2/PatientData")]
        public TextMeshProUGUI PatientDataMaleText;

        [BoxGroup("B2/PatientData")]
        public TextMeshProUGUI PatientDataBirthdayText;

        [BoxGroup("B2/PatientData")]
        public TextMeshProUGUI PatientDataAgeText;

        [BoxGroup("B2/PatientData")]
        public TextMeshProUGUI PatientDataHeightText;

        [BoxGroup("B2/PatientData")]
        public TextMeshProUGUI PatientDataWeightText;

        [BoxGroup("B2/PatientData")]
        public TextMeshProUGUI PatientDataPreviousReadingsText;

        [ShowIfGroup("B3" , Condition = "@Type == Enums.DisplayType.Console")]
        [BoxGroup("B3/Console")]
        public GameObject ConsoleGroup;

        [BoxGroup("B3/Console")]
        public TextMeshProUGUI ConsoleText;

        private Camera _mainCam;

        private void Awake()
        {
            _mainCam = Camera.main;
        }

        public void DisplayReading(Enums.ReadingType type, Data data)
        {
            switch (type)
            {
                case Enums.ReadingType.HematologyCompleteBloodCount:
                {
                    if (Type != Enums.DisplayType.HematologyCompleteBloodCount) return;
                    HematologyCBCItemText.text = data.GetFullReadingParameter();
                    HematologyCBCDataText.text =  data.GetFullReadingValue();
                    HematologyCBCUnitText.text =  data.GetFullReadingUnit();
                    HematologyCBCRangeText.text = data.GetFullReadingRange();
                    HematologyCBCFlagText.text = data.GetFullReadingFlags();
                    //print(data.Male);
                    //foreach (var item in data.DiseaseTagList) print(item);
                }
                    break;
            }
        }

        public void DisplayPatientData(PatientData data)
        {
            if (Type != Enums.DisplayType.PatientData) return;
            PatientDataIDText.text = data.PatientID.ToString();
            PatientDataMaleText.text = data.IsMale ? "Male" : "Female" ;
            PatientDataBirthdayText.text = data.Birthday.ToString("yyyy MMMM dd",new CultureInfo("en-GB"));
            PatientDataAgeText.text = data.Age.ToString();
            PatientDataHeightText.text = data.Height.ToString("F");
            PatientDataWeightText.text = data.Weight.ToString("F");
            string msg = "";
            foreach (var item in data.TubesID)
            {
                msg +=  $"Sample N°{item} : {data.ReadingsByTubesDictionary[item].FirstOrDefault().ToString()}\n";
            }

            PatientDataPreviousReadingsText.text = msg;
        }

        public void Clear()
        {
            switch (Type)
            {
                case Enums.DisplayType.HematologyCompleteBloodCount:
                {
                    HematologyCBCItemText.text = "";
                    HematologyCBCDataText.text = "";
                    HematologyCBCUnitText.text = "";
                    HematologyCBCRangeText.text = "";
                    HematologyCBCFlagText.text = "";
                }
                    break;
                case Enums.DisplayType.PatientData:
                {
                    PatientDataIDText.text = "";
                    PatientDataMaleText.text = "";
                    PatientDataBirthdayText.text = "";
                    PatientDataAgeText.text = "";
                    PatientDataHeightText.text = "";
                    PatientDataWeightText.text = "";
                    PatientDataPreviousReadingsText.text = "";
                }
                    break;
            }
        }

        private void Update()
        {
            bool inFront = Vector3.Dot(Canvas.forward, ( _mainCam.transform.position - Canvas.position ).normalized) <= 0;
            Canvas.gameObject.SetActive(inFront);
        }
    }
}