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
using UnityEngine;
using UnityEngine.UI;

namespace FunForLab.UI.Blur
{
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("UI/Blur Panel")]
    public class BlurPanel : Image
    {
        public bool animate;
        public float time = 0.5f;
        public float delay = 0f;
        public float blurAmount = 3f;

        private CanvasGroup canvas;
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            color = Color.black * 0.1f;
        }
#endif
        protected override void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Application.isPlaying)
            {
                material.SetFloat("_Size", 0);
                canvas.alpha = 0;
                StartCoroutine(UpdateBlur(delay, time, 0, blurAmount));
            }
        }

        IEnumerator UpdateBlur(float delay, float duration, float startValue, float endValue)
        {
            yield return new WaitForSeconds(delay);
            float timeElapsed = 0;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float localPercent = timeElapsed / duration;
                material.SetFloat("_Size", Mathf.Lerp(startValue, endValue, localPercent));
                canvas.alpha = Mathf.Lerp(startValue, endValue, localPercent);
                yield return null;
            }
        }
    }
}