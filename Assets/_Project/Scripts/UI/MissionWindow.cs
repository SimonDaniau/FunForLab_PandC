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
using FunForLab.Scenario;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FunForLab.UI
{
    public class MissionWindow : MonoBehaviour
    {
        public static MissionWindow Instance;

        [Header("SquareBox")]
        public Image SbCompletionBar;

        public GameObject SbCompletionBarHolder;
        public Image SbKeyPressNotifier;
        public TextMeshProUGUI SbMissionBox;
        public TextMeshProUGUI SbChapterBox;
        public GameObject SbHolder;

        [Header("WideBox")]
        public Image WbKeyPressNotifier;

        public Image WbSkipNotifier;
        public TextMeshProUGUI WbMissionBox;
        public TextMeshProUGUI WbChapterBox;
        public GameObject WbHolder;

        private bool _showChapter;

        public enum WindowType
        {
            None,
            Square,
            SquareNoCompletion,
            Wide
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable ()
        {
            SbHolder.SetActive(true);
            WbHolder.SetActive(false);
            SbCompletionBar.transform.localScale = new Vector3(0, 1f, 1f);
            MissionManager.Instance.OnDescriptionChange += OnObjectiveChanged;
            MissionManager.Instance.OnNotifyKeyPressChange += OnNotifyKeyPress;
        }

        private void OnDestroy()
        {
            MissionManager.Instance.OnDescriptionChange -= OnObjectiveChanged;
            MissionManager.Instance.OnNotifyKeyPressChange -= OnNotifyKeyPress;
        }

        private void OnNotifyKeyPress(bool state)
        {
            SbKeyPressNotifier.gameObject.SetActive(state);
            WbKeyPressNotifier.gameObject.SetActive(state);
        }

        private void OnObjectiveChanged(string desc)
        {
            SbCompletionBar.transform.localScale = new Vector3(MissionManager.Instance.CurrentMissionCompletion, 1f, 1f);
            SetChapterTexts(_showChapter ? MissionManager.Instance.ObjectiveStatus : "");
            SetMissionTexts(MissionManager.Instance.ObjectiveDescription);
        }

        public void SetSkipAmount(float value)
        {
            WbSkipNotifier.fillAmount = value;
        }

        private void SetChapterTexts(string text)
        {
            SbChapterBox.text = text;
            WbChapterBox.text = text;
        }

        private void SetMissionTexts(string text)
        {
        
            SbMissionBox.text = text;
            WbMissionBox.text = text;
        }

        public void ChangeWindow(WindowType type, bool showChapter)
        {
            _showChapter = showChapter;
            SetChapterTexts(_showChapter ? MissionManager.Instance.ObjectiveStatus : "");

            switch (type)
            {
                case WindowType.None:
                {
                    SbHolder.SetActive(false);
                    WbHolder.SetActive(false);
                }
                    break;
                case WindowType.SquareNoCompletion:
                case WindowType.Square:
                {
                    SbHolder.SetActive(true);
                    SbCompletionBarHolder.SetActive(type == WindowType.Square);
                    WbHolder.SetActive(false);
                }
                    break;
                case WindowType.Wide:
                {
                    SbHolder.SetActive(false);
                    WbHolder.SetActive(true);
                }
                    break;
            }
        }
    }
}