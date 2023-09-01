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
    public class SampleScannerDialogue : MonoBehaviour
    {
        [SerializeField] private SampleScanInfo_SO _sampleScanInfo;
        [SerializeField] private TextMeshProUGUI _sampleIDText;
        [SerializeField] private TextMeshProUGUI _demographicText;
        [SerializeField] private TextMeshProUGUI _requestedAnalysisText;
        [SerializeField] private TextMeshProUGUI _reqItemsText;
        [SerializeField] private Transform _idleHolder;
        [SerializeField] private Transform _infoHolder;

        public void ShowIdleScreen()
        {
            _idleHolder.gameObject.SetActive(true);
            _infoHolder.gameObject.SetActive(false);
        }

        public void ShowScanInfo()
        {
            SetupInfo();
            _idleHolder.gameObject.SetActive(false);
            _infoHolder.gameObject.SetActive(true);
        }

        [ContextMenu("Setup")]
        private void SetupInfo()
        {
            /*
            _sampleIDText.text = "Sample ID : " + _sampleScanInfo.SampleID.ToString();
            _demographicText.text = "Demographic : " + _sampleScanInfo.Demographic;
            _requestedAnalysisText.text = "Requested Analysis :";
            _reqItemsText.text = "";
            foreach (var item in _sampleScanInfo.AllRequestedAnalysis)
            {
                _reqItemsText.text += "- " + item + "\n";
            }
            */
            _sampleIDText.text = _sampleScanInfo.SampleKey.Localize();
            _demographicText.text = _sampleScanInfo.DemoKey.Localize();
            _requestedAnalysisText.text = _sampleScanInfo.ReqTitleKey.Localize();
            _reqItemsText.text = "- " + _sampleScanInfo.ReqContentKey.Localize();
        }
    }
}