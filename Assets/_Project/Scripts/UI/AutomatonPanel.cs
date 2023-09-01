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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FunForLab
{
    public class AutomatonPanel : MonoBehaviour
    {
        public class SampleResult
        {
            public string WBC;
            public string RBC;
            public string HB;
            public string HCT;
            public string MCV;
            public string MCH;
            public string MCHC;
            public string PLT;
            public string ESR;

            public float fWBC;
            public float fRBC;
            public float fHB;
            public float fHCT;
            public float fMCV;
            public float fMCH;
            public float fMCHC;
            public float fPLT;
            public float fESR;

            public string nWBC;
            public string nRBC;
            public string nHB;
            public string nHCT;
            public string nMCV;
            public string nMCH;
            public string nMCHC;
            public string nPLT;
            public string nESR;

            public string uWBC;
            public string uRBC;
            public string uHB;
            public string uHCT;
            public string uMCV;
            public string uMCH;
            public string uMCHC;
            public string uPLT;
            public string uESR;

            public string lWBC;
            public string lRBC;
            public string lHB;
            public string lHCT;
            public string lMCV;
            public string lMCH;
            public string lMCHC;
            public string lPLT;
            public string lESR;

            public bool bWBC;
            public bool bRBC;
            public bool bHB;
            public bool bHCT;
            public bool bMCV;
            public bool bMCH;
            public bool bMCHC;
            public bool bPLT;
            public bool bESR;

            public SampleResult(bool male, string wbc, string rbc, string hb, string hct, string mcv, string mch,
                string mchc, string plt, string esr)
            {
                uWBC = "/nL";
                uRBC = "/pL";
                uHB = "g/L";
                uHCT = "%";
                uMCV = "fL";
                uMCH = "pg";
                uMCHC = "g/L";
                uPLT = "/nL";
                uESR = "mm/h";

                lWBC = "WBC";
                lRBC = "RBC";
                lHB = "HB";
                lHCT = "HCT";
                lMCV = "MCV";
                lMCH = "MCH";
                lMCHC = "MCHC";
                lPLT = "PLT";
                lESR = "ESR";

                fWBC = -1f;
                fRBC = -1f;
                fHB = -1f;
                fHCT = -1f;
                fMCV = -1f;
                fMCH = -1f;
                fMCHC = -1f;
                fPLT = -1f;
                fESR = -1f;

                WBC = wbc;
                RBC = rbc;
                HB = hb;
                HCT = hct;
                MCV = mcv;
                MCH = mch;
                MCHC = mchc;
                PLT = plt;
                ESR = esr;

                nWBC = "4-10 /nL";
                nRBC = male ? "4.4-6.0" : "4.2-5.5";
                nHB = male ? "140-180" : "120-160";
                nHCT = male ? "40-54" : "37-47";
                nMCV = "82-97 fL";
                nMCH = "27-36 pg";
                nMCHC = "320-360 g/L";
                nPLT = "140-400 /nL";
                nESR = male ? "<22 mm/h" : "<24 mm/h";

                bWBC = false;
                bRBC = false;
                bHB = false;
                bHCT = false;
                bMCV = false;
                bMCH = false;
                bMCHC = false;
                bPLT = false;
                bESR = false;
            }

            public SampleResult(bool male, float wbc, float rbc, float hb, float hct, float mcv, float mch,
                float mchc, float plt, float esr)
            {
                uWBC = "/nL";
                uRBC = "/pL";
                uHB = "g/L";
                uHCT = "%";
                uMCV = "fL";
                uMCH = "pg";
                uMCHC = "g/L";
                uPLT = "/nL";
                uESR = "mm/h";

                lWBC = "WBC";
                lRBC = "RBC";
                lHB = "HB";
                lHCT = "HCT";
                lMCV = "MCV";
                lMCH = "MCH";
                lMCHC = "MCHC";
                lPLT = "PLT";
                lESR = "ESR";

                WBC = "";
                RBC = "";
                HB = "";
                HCT = "";
                MCV = "";
                MCH = "";
                MCHC = "";
                PLT = "";
                ESR = "";

                fWBC = wbc;
                fRBC = rbc;
                fHB = hb;
                fHCT = hct;
                fMCV = mcv;
                fMCH = mch;
                fMCHC = mchc;
                fPLT = plt;
                fESR = esr;

                nWBC = "4.0 ~ 10.0";
                nRBC = male ? "4.4 ~ 6.0" : "4.2 ~ 5.5";
                nHB = male ? "140.0 ~ 180.0" : "120.0 ~ 160.0";
                nHCT = male ? "40.0 ~ 54.0" : "37.0 ~ 47.0";
                nMCV = "82.0 ~ 97.0";
                nMCH = "27.0 ~ 36.0";
                nMCHC = "320.0 ~ 360.0";
                nPLT = "140.0 ~ 400.0";
                nESR = male ? "<22" : "<24";

                bWBC = wbc < 4f || wbc > 10f;
                bRBC = male ? (rbc < 4.4f || rbc > 6f) : (rbc < 4.2f || rbc > 5.5f);
                bHB = male ? (hb < 140f || hb > 180f) : (hb < 120f || hb > 160f);
                bHCT = male ? (hct < 40f || hct > 54f) : (hct < 37f || hct > 47f);
                bMCV = mcv < 82f || mcv > 97f;
                bMCH = mch < 27f || mch > 36f;
                bMCHC = mchc < 320f || mchc > 360f;
                bPLT = plt < 140f || plt > 400f;
                bESR = male ? esr > 22f : esr > 24f;
            }

            public static SampleResult Blank(bool male) =>
                new SampleResult(male, "xxx", "xxx", "xxx", "xxx", "xxx", "xxx", "xxx", "xxx", "xxx");

            public static SampleResult OwnBlood() =>
                new SampleResult(true, 4.8f, 4.5f, 145.3f, 42.5f, 89.3f, 32.1f, 345.4f, 212.0f, 13.0f); // normal

            public static SampleResult Case1Bernie() =>
                new SampleResult(true, 4.9f, 3.4f, 87f, 26.4f, 87.4f, 28.5f, 330.0f, 383.0f, 30.0f); // anemia

            public static SampleResult Case3Julia() =>
                new SampleResult(false, 9.5f, 5.2f, 172f, 50.0f, 96.4f, 33.0f, 344.0f, 379.0f, 12.0f); // diabetes
        }

        [Header("Status")] public Bubble Reagents;

        public Bubble Control;
        public Bubble Calibration;
        public Image ReagentLevelA;
        public Image ReagentLevelB;
        public Image ReagentLevelC;
        public Image ReagentLevelD;


        [Header("Analysis")] public TextMeshProUGUI PatientLabel;

        public Color DefaultColor = Color.black;
        public Color HighlightColor = Color.red;

        public TextMeshProUGUI[] WBCTexts;
        public TextMeshProUGUI[] RBCTexts;
        public TextMeshProUGUI[] HBTexts;
        public TextMeshProUGUI[] HCTTexts;
        public TextMeshProUGUI[] MCVTexts;
        public TextMeshProUGUI[] MCHTexts;
        public TextMeshProUGUI[] MCHCTexts;
        public TextMeshProUGUI[] PLTTexts;
        public TextMeshProUGUI[] ESRTexts;

        public void SwitchToAnalysisResults()
        {
        }

        [ContextMenu("Blank")]
        private void SetBlank() => SetAnalysisSheet(SampleResult.Blank(Random.Range(0, 100) > 50));

        [ContextMenu("Own")]
        private void SetOwnBlood() => SetAnalysisSheet(SampleResult.OwnBlood());


        public void SetAnalysisSheet(string name)
        {
            switch (name)
            {
                case "Case3Julia":
                    SetAnalysisSheet(SampleResult.Case3Julia());
                    return;
                case "Case1Bernie":
                    SetAnalysisSheet(SampleResult.Case1Bernie());
                    return;
                case "OwnBlood":
                    SetAnalysisSheet(SampleResult.OwnBlood());
                    return;
                case "Blank":
                    SetBlank();
                    return;
            }
        }

        public void SetAnalysisSheet(SampleResult sample)
        {
            if (sample.fWBC > 0)
            {
                WBCTexts[1].text = SetParameterText2(sample.lWBC, sample.fWBC.ToString("F1"), sample.uWBC, sample.nWBC);
                RBCTexts[1].text = SetParameterText2(sample.lRBC, sample.fRBC.ToString("F1"), sample.uRBC, sample.nRBC);
                HBTexts[1].text = SetParameterText2(sample.lHB, sample.fHB.ToString("F1"), sample.uHB, sample.nHB);
                HCTTexts[1].text = SetParameterText2(sample.lHCT, sample.fHCT.ToString("F1"), sample.uHCT, sample.nHCT);
                MCVTexts[1].text = SetParameterText2(sample.lMCV, sample.fMCV.ToString("F1"), sample.uMCV, sample.nMCV);
                MCHTexts[1].text = SetParameterText2(sample.lMCH, sample.fMCH.ToString("F1"), sample.uMCH, sample.nMCH);
                MCHCTexts[1].text = SetParameterText2(sample.lMCHC, sample.fMCHC.ToString("F1"), sample.uMCHC, sample.nMCHC);
                PLTTexts[1].text = SetParameterText2(sample.lPLT, sample.fPLT.ToString("F1"), sample.uPLT, sample.nPLT);
                ESRTexts[1].text = SetParameterText2(sample.lESR, sample.fESR.ToString("F1"), sample.uESR, sample.nESR);
            }
            else
            {
                WBCTexts[1].text = SetParameterText2(sample.lWBC, sample.WBC, sample.uWBC, sample.nWBC);
                RBCTexts[1].text = SetParameterText2(sample.lRBC, sample.RBC, sample.uRBC, sample.nRBC);
                HBTexts[1].text = SetParameterText2(sample.lHB, sample.HB, sample.uHB, sample.nHB);
                HCTTexts[1].text = SetParameterText2(sample.lHCT, sample.HCT, sample.uHCT, sample.nHCT);
                MCVTexts[1].text = SetParameterText2(sample.lMCV, sample.MCV, sample.uMCV, sample.nMCV);
                MCHTexts[1].text = SetParameterText2(sample.lMCH, sample.MCH, sample.uMCH, sample.nMCH);
                MCHCTexts[1].text = SetParameterText2(sample.lMCHC, sample.MCHC, sample.uMCHC, sample.nMCHC);
                PLTTexts[1].text = SetParameterText2(sample.lPLT, sample.PLT, sample.uPLT, sample.nPLT);
                ESRTexts[1].text = SetParameterText2(sample.lESR, sample.ESR, sample.uESR, sample.nESR);
            }

            //HeaderText.text = SetHeaderText();
            for (int i = 0; i < 4; i++)
            {
                WBCTexts[i].color = sample.bWBC ? HighlightColor : DefaultColor;
                RBCTexts[i].color = sample.bRBC ? HighlightColor : DefaultColor;
                HBTexts[i].color = sample.bHB ? HighlightColor : DefaultColor;
                HCTTexts[i].color = sample.bHCT ? HighlightColor : DefaultColor;
                MCVTexts[i].color = sample.bMCV ? HighlightColor : DefaultColor;
                MCHTexts[i].color = sample.bMCH ? HighlightColor : DefaultColor;
                MCHCTexts[i].color = sample.bMCHC ? HighlightColor : DefaultColor;
                PLTTexts[i].color = sample.bPLT ? HighlightColor : DefaultColor;
                ESRTexts[i].color = sample.bESR ? HighlightColor : DefaultColor;
            }

            //HeaderText.color = DefaultColor;
        }

        string SetParameterText(string label, string value, string unit, string normal)
        {
            return label + " " + value + " " + unit + " | " + normal + " " + unit;
        }

        string SetParameterText2(string label, string value, string unit, string normal)
        {
            return value + " " + unit;
        }

        string SetHeaderText()
        {
            return "label value unit | normal unit";
        }

        public void SetControlStatus(bool ok)
        {
            Control.SetStatus(ok);
        }
    }
}