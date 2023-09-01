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
using FunForLab.Analytics;
using FunForLab.Dialogue;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FunForLab.Codex
{
    public class DiseaseCodex : MonoBehaviour
    {
        public TMP_Dropdown AnalysisType;
        public TMP_Dropdown Parameter;
        public TextMeshProUGUI DiseaseTitle;
        public TextMeshProUGUI DiseaseDescription;
        public TextMeshProUGUI DiseaseRanges;

        public Transform ContentHolder;
        public GameObject ListButtonPrefab;

        public Transform SearchContentHolder;

        public List<DiseaseTag> Results;
        public List<DiseaseTag> ValidationList;
        public TMP_InputField SearchBar;
        public Button Validate;
        public Button ButtonValidationHoldingTag;
        public DiseaseTag TagToBeValidated;
        private string _inputField;
        public Color ColorSelectedForValidation;
        public List<DiseaseTag> CorrectList;
        public ItemAction OnValidateCorrectEvent;
        public ItemAction OnValidateIncorrectEvent;
        [Serializable]
        public class ItemAction
        {
            public string LuaAction;
            public UnityEvent EventAction;
        }


        private void Awake()
        {
            DiseaseCodexFilter.Init();
            AnalysisType.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("All"),
                /*
                new TMP_Dropdown.OptionData(Enums.AnalysisType.HematologyAnalysis.ToString()),
                new TMP_Dropdown.OptionData(Enums.AnalysisType.MicroscopeAnalysis.ToString()),
                new TMP_Dropdown.OptionData(Enums.AnalysisType.MultistixAnalysis.ToString())
                */
                new TMP_Dropdown.OptionData("Codex_Analysis_Type_Hematology".Localize()),
                new TMP_Dropdown.OptionData("Codex_Analysis_Type_Microscope".Localize()),
                new TMP_Dropdown.OptionData("Codex_Analysis_Type_Multistix".Localize())
                
            };
            AnalysisType.onValueChanged.AddListener((x) => RefreshParameterDropDown());
            Parameter.onValueChanged.AddListener((x) => ApplyFilter());
            RefreshParameterDropDown();
            ClearList(SearchContentHolder);
            _inputField = "";
            ApplyFilter();
            UpdateSearchedList(_inputField);
        }

        private void Update()
        {
            string oldInputField = _inputField;
            _inputField = SearchBar.text;
            if (_inputField.Length != oldInputField.Length)
            {
                UpdateSearchedList(_inputField);
            }
        }

        public void UpdateSearchedList(string inputField)
        {
            ClearList(SearchContentHolder);
            if (inputField.Length > 0)
                ValidationList = DiseaseCodexFilter.GetAll()
                    .FindAll(w => w.DiseaseName.ToLower().Contains(inputField.ToLower()));
            else
                ValidationList = DiseaseCodexFilter.GetAll();
            foreach (var item in ValidationList)
            {
                var button = AddTagToContentList(item, SearchContentHolder, SelectTagForValidation);
                if (TagToBeValidated != null && TagToBeValidated == item)
                {
                    SelectTagForValidation(item, button);
                }
            }
        }

        public void RefreshParameterDropDown()
        {
            List<TMP_Dropdown.OptionData> optionDatasList = new List<TMP_Dropdown.OptionData>();
            optionDatasList.Add(new TMP_Dropdown.OptionData("Any"));
            switch (AnalysisType.value)
            {
                case 2:
                {
                    foreach (var item in Enum.GetNames(typeof(Enums.MicroscopicMeasures)))
                        optionDatasList.Add(new TMP_Dropdown.OptionData(item));
                }
                    break;
                case 3:
                {
                    foreach (var item in Enum.GetNames(typeof(Enums.MultistixMeasures)))
                        optionDatasList.Add(new TMP_Dropdown.OptionData(item));
                }
                    break;
                case 1:
                {
                    foreach (var item in Enum.GetNames(typeof(Enums.HematologyMeasures)))
                        optionDatasList.Add(new TMP_Dropdown.OptionData(item));
                }
                    break;
            }

            Parameter.options = optionDatasList;
            ApplyFilter();
        }

        [Button("ApplyFilter")]
        public void ApplyFilter()
        {
            ClearList(ContentHolder);
            switch (AnalysisType.value)
            {
                case 0:
                    Results = DiseaseCodexFilter.GetAll();
                    break;
                case 2:
                {
                    if (Parameter.options[Parameter.value].text != "Any")
                        Results = DiseaseCodexFilter.RequestByParameter(
                            new List<Enums.MicroscopicMeasures>()
                            {
                                (Enums.MicroscopicMeasures) Enum.Parse(typeof(Enums.MicroscopicMeasures),
                                    Parameter.options[Parameter.value].text)
                            },
                            new List<Enums.MicroscopicMeasures>());
                    else
                        Results = DiseaseCodexFilter.GetAll().Where(x =>
                                x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.MicroscopeAnalysis) >= 1)
                            .ToList();
                }
                    break;
                case 3:
                {
                    if (Parameter.options[Parameter.value].text != "Any")
                        Results = DiseaseCodexFilter.RequestByParameter(
                            new List<Enums.MultistixMeasures>()
                            {
                                (Enums.MultistixMeasures) Enum.Parse(typeof(Enums.MultistixMeasures),
                                    Parameter.options[Parameter.value].text)
                            },
                            new List<Enums.MultistixMeasures>());
                    else
                        Results = DiseaseCodexFilter.GetAll().Where(x =>
                                x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.MultistixAnalysis) >= 1)
                            .ToList();
                }
                    break;
                case 1:
                {
                    if (Parameter.options[Parameter.value].text != "Any")
                        Results = DiseaseCodexFilter.RequestByParameter(
                            new List<Enums.HematologyMeasures>()
                            {
                                (Enums.HematologyMeasures) Enum.Parse(typeof(Enums.HematologyMeasures),
                                    Parameter.options[Parameter.value].text)
                            },
                            new List<Enums.HematologyMeasures>());
                    else
                        Results = DiseaseCodexFilter.GetAll().Where(x =>
                                x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.HematologyAnalysis) >= 1)
                            .ToList();
                }
                    break;
            }

            foreach (var item in Results) AddTagToContentList(item, ContentHolder, DisplayTagInfo);
        }

        [Button("Clear")]
        public void ClearList(Transform holder)
        {
            Results = new List<DiseaseTag>();
            DiseaseTitle.text = "";
            DiseaseDescription.text = "";
            for (int i = 0; i < holder.childCount; i++)
            {
                Destroy(holder.GetChild(i).gameObject);
            }
        }

        public Button AddTagToContentList(DiseaseTag tagData, Transform holder,
            Action<DiseaseTag, Button> callback = null)
        {
            GameObject go = Instantiate(ListButtonPrefab, holder);
            TextMeshProUGUI textComponent = go.GetComponentInChildren<TextMeshProUGUI>();
            Button buttonComponent = go.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => callback?.Invoke(tagData, buttonComponent));
            textComponent.text = tagData.DiseaseNameKey.Localize();
            return buttonComponent;
        }

        public void DisplayTagInfo(DiseaseTag tagData, Button button)
        {
            DiseaseTitle.text = tagData.DiseaseNameKey.Localize();
            DiseaseDescription.text = tagData.DiseaseDescriptionKey.Localize();
        }

        public void SelectTagForValidation(DiseaseTag tagData, Button button)
        {
            if (ButtonValidationHoldingTag != null)
            {
                var defaultBlock = ColorBlock.defaultColorBlock;
                defaultBlock.normalColor *= new Color(1, 1, 1, 0);
                ButtonValidationHoldingTag.colors = defaultBlock;
            }

            TagToBeValidated = tagData;
            ButtonValidationHoldingTag = button;
            var block = ColorBlock.defaultColorBlock;
            block.normalColor *= ColorSelectedForValidation;
            block.highlightedColor *= ColorSelectedForValidation;
            block.pressedColor *= ColorSelectedForValidation;
            block.selectedColor *= ColorSelectedForValidation;
            block.disabledColor *= ColorSelectedForValidation;
            ButtonValidationHoldingTag.colors = block;
        }

        public void DeterminePatientHealthy()
        {
            if (CorrectList.Count == 0)
            {
                OnValidateCorrectEvent.EventAction?.Invoke();
                LuaController.Instance.RunLua(OnValidateCorrectEvent.LuaAction);
            }
            else
            {
                OnValidateIncorrectEvent.EventAction?.Invoke();
                LuaController.Instance.RunLua(OnValidateIncorrectEvent.LuaAction);
            }
        }

        public void ValidateSelectedButton()
        {
            if (ButtonValidationHoldingTag == null) return;

            if (CorrectList.Contains(TagToBeValidated))
            {
                OnValidateCorrectEvent.EventAction?.Invoke();
                LuaController.Instance.RunLua(OnValidateCorrectEvent.LuaAction);
            }
            else
            {
                OnValidateIncorrectEvent.EventAction?.Invoke();
                LuaController.Instance.RunLua(OnValidateIncorrectEvent.LuaAction);
            }
        }
    }
}