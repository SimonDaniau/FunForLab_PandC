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
using UnityEngine;

namespace FunForLab.UI.BetterUI
{
    public class CameraBackgroundDrift : MonoBehaviour
    {
        [SerializeField] private Vector2 _min;
        [SerializeField] private Vector2 _max;
        [SerializeField] private Vector2 _yRotationRange;
        [SerializeField] private float _lerpSpeed = 0.05f;

        private Vector3 _newPosition;
        private Quaternion _newRotation;

        private void Awake()
        {
            _newPosition = transform.position;
            _newRotation = transform.rotation;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * _lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * _lerpSpeed);
            if (Vector3.Distance(transform.position, _newPosition) < 1f)
            {
                GetNewPosition();
            }
        }

        private void GetNewPosition()
        {
            var xPos = Random.Range(_min.x, _max.x);
            var zPos = Random.Range(_min.y, _max.y);
            _newRotation = Quaternion.Euler(0, Random.Range(_yRotationRange.x, _yRotationRange.y), 0);
            _newPosition = new Vector3(xPos, 0, zPos);
        }
    }
}