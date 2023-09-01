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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FunForLab.UI.BetterUI
{
    [RequireComponent(typeof(Image))]
    public class TabExtension : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        public Color TabInactive;
        public Color TabActive;
        public Color TabHighlight;

        public Color TextInactive;
        public Color TextActive;

        private Image _tab;
        private TabButton _tabButton;
        private bool _active;


        [ContextMenu("AutoAssign Text Component")]
        private void AutoAssignText()
        {
            var components =   GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var item in components)
            {
                if (item.transform.parent == transform)
                {
                    Text = item;
                    break;
                }
            }
        }

        private void Awake()
        {
            _tabButton = GetComponent<TabButton>();
            _tab = GetComponent<Image>();
            _tabButton.OnTabSelected.AddListener(OnSelection);
            _tabButton.OnTabDeselected.AddListener(OnDeselection);
            _tabButton.OnTabPointerEnter.AddListener(OnPointerEnter);
            _tabButton.OnTabPointerExit.AddListener(OnPointerExit);
            OnDeselection();
        }

        public void OnSelection()
        {
            _active = true;
            _tab.color = TabActive;
            Text.color = TextActive;
        }

        public void OnDeselection()
        {
            _active = false;
            _tab.color = TabInactive;
            Text.color = TextInactive;
        }

        public void OnPointerEnter()
        {
            _tab.color = Color.Lerp(_active ? TabActive : TabInactive, TabHighlight.AlphaSet(1f), TabHighlight.a);
        }

        public void OnPointerExit()
        {
            _tab.color = _active ? TabActive : TabInactive;
        }
    }
}