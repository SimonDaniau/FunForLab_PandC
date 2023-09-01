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
using FunForLab.Localization;
using FunForLab.Scenario;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FunForLab.UI
{
    public class MenuSelector : MonoBehaviour
    {
        public TMP_Dropdown Dropdown;

        [Header("Tabs")]
        public TextMeshProUGUI HomeText;

        public TextMeshProUGUI SelectionText;
        public TextMeshProUGUI SettingsText;

        [Header("HomePage")]
        public TextMeshProUGUI QuitText;

        public TextMeshProUGUI CreditsText;
        public TextMeshProUGUI TutorialText;
        public TextMeshProUGUI ScenarioContinueText;
        
        [Header("SelectionPage")]
        public TextMeshProUGUI PrologueSelectionText;
        public TextMeshProUGUI TutorialSelectionText;
        public TextMeshProUGUI IntroductionSelectionText;
        public TextMeshProUGUI FirstCaseSelectionText;
        public TextMeshProUGUI SecondCaseSelectionText;
        public TextMeshProUGUI ThirdCaseSelectionText;
        public TextMeshProUGUI EpilogueSelectionText;

        [Header("SettingsPage")]
        public TextMeshProUGUI LanguageText;

        private void Awake()
        {
            Dropdown.options = new List<TMP_Dropdown.OptionData>();
            foreach (var item in Enum.GetNames(typeof(Localizator.Languages)))
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(item));
            }
            Dropdown.onValueChanged.AddListener((x) =>
            {
                PlayerPrefs.SetInt("Languages", Dropdown.value);
                //Tabs
                HomeText.text =  "Menu_Tab_Home".Localize();
                SettingsText.text =  "Menu_Tab_Settings".Localize();
                SelectionText.text =  "Menu_Tab_Selection".Localize();
                //Home
                QuitText.text =  "Menu_Label_Quit".Localize();
                CreditsText.text = "Menu_Label_Credits".Localize();
                TutorialText.text = "Menu_Label_Tutorial".Localize();
                ScenarioContinueText.text = "Menu_Label_Continue".Localize();
                //Settings
                LanguageText.text = "Menu_Label_Language".Localize();
                //Selection
                PrologueSelectionText.text = "Menu_Selection_Prologue".Localize();
                TutorialSelectionText.text = "Menu_Selection_Tutorial".Localize();
                IntroductionSelectionText.text = "Menu_Selection_Introduction".Localize();
                FirstCaseSelectionText.text = "Menu_Selection_FirstCase".Localize();
                SecondCaseSelectionText.text = "Menu_Selection_SecondCase".Localize();
                ThirdCaseSelectionText.text = "Menu_Selection_ThirdCase".Localize();
                EpilogueSelectionText.text = "Menu_Selection_Epilogue".Localize();
            });
            Dropdown.value = (PlayerPrefs.GetInt("Languages", 1));
        }

        public void SetScenario(ScenarioNameComponent mission)
        {
           /* var current = PlayerPrefs.GetInt("Languages", 1);
            if ( current == 0 || current == 3 ) return;

            PlayerPrefs.SetInt("CurrentScenario", (int) mission.ScenarioNameSelected);
            var op = SceneManager.LoadSceneAsync(ScenarioChanger.ScenarioToSceneName(mission.ScenarioNameSelected));
            //op.completed += x => MissionManager.Instance.StartCoroutine(MissionManager.Instance.Initialize());
       */
           
           ScenarioChanger.ChangeScenario(mission);
        }

        public void Resume()
        {
            ScenarioChanger.Instance.Resume();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }

        public void Credits()
        {
           
        }
    }
}