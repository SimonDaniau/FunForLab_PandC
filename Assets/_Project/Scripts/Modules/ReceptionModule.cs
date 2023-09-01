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
using System.Collections.Generic;
using FunForLab.Analytics;
using FunForLab.Interactables;
using FunForLab.Inventory;
using FunForLab.OrbitCamera;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace FunForLab.Modules
{
    public class ReceptionModule : HighlightableObject
    {
        [SerializeField] private GameObject _tubePrefab;
        [SerializeField] private Transform _tubeHolder;
        [SerializeField] private Transform _rack;
        [SerializeField] private Orbit _tubeOrbit;
        private Queue<Data> _tubesToGet;
        public bool IsFull { get; private set; }

        public void AddToTubeQueue(Data data)
        {
            if (_tubesToGet == null) _tubesToGet = new Queue<Data>();
            _tubesToGet.Enqueue(data);
        }


        public Tube SetNextTube(DiseaseTag d)
        {
            if (IsFull) return null;
            IsFull = true;
            if (_tubesToGet.Count <= 0) return null;
            var tubeData = _tubesToGet.Dequeue();
            var go = Instantiate(_tubePrefab, _tubeHolder.position,  _tubeHolder.rotation * Quaternion.Euler(new Vector3(0, Random.Range(160, 200), 0)));
            var tube = go.GetComponent<Tube>();
            var dataLink = go.GetComponent<TubeDataLink>();
            var buttonHandle = go.AddComponent<ButtonHandle>();
            buttonHandle.AddParentOrbit(_tubeOrbit);
            buttonHandle.MouseDownEvent = new UnityEvent();
            buttonHandle.MouseDownEvent.AddListener(() =>
            {
                GetNextTube();
                TransferToPlayer(tube);
            });
            dataLink.SetData(tubeData);
            tube.DataLink = dataLink;
            _rack.gameObject.SetActive(true);
            return tube;
        }

        [Button]
        public void GetNextTube()
        {
            IsFull = false;
            _rack.gameObject.SetActive(false);
        }

        public void TransferToPlayer(Tube tube)
        {
            var pickable = tube.GetComponent<Pickable>();
            if (pickable != null)
            {
                if (!pickable.TransferToInventory())
                {
                    Debug.Log("ContentList full");
                }
            }
            else
            {
                Destroy(tube.gameObject);
            }
        }
    }
}