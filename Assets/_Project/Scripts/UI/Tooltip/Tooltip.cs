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

namespace FunForLab.UI.Tooltip
{
    [ExecuteInEditMode()]
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI HeaderField;

        public TextMeshProUGUI ContentField;

        public LayoutElement LayoutElement;

        private RectTransform _rectTransform;

        public int CharacterWrapLimit;
        private float _cursorOffsetX;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _cursorOffsetX = 32f / Screen.width;
        }

        public void SetText( string content, string header = "")
        {
            HeaderField.gameObject.SetActive(!string.IsNullOrEmpty(header));
            HeaderField.text = header;
            ContentField.text = content;
            int headerLength = HeaderField.text.Length;
            int contentLength = ContentField.text.Length;
            LayoutElement.enabled = headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit;
        }

        private void OnEnable()
        {
            if (HeaderField == null) HeaderField = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (ContentField == null) ContentField = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (LayoutElement == null) LayoutElement = GetComponent<LayoutElement>();
        }

        private void Update()
        {
            if (Application.isEditor)
            {
                int headerLength = HeaderField.text.Length;
                int contentLength = ContentField.text.Length;
                LayoutElement.enabled = headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit;
            }

            Vector2 position = Input.mousePosition;

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            _rectTransform.pivot = new Vector2(pivotX > .5f ? ( 1 + _cursorOffsetX ) : ( 0 - _cursorOffsetX * 2 ), pivotY > .5f ? 1 : 0);

            transform.position = position;
        }
    }
}