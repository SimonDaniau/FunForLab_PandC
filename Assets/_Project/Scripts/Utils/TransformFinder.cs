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
using System.Collections;
using System.Collections.Generic;
using FunForLab.OrbitCamera;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace FunForLab
{
    public class TransformFinder : MonoBehaviour
    {
        public Image Arrow;
        public Transform Target;
        private Camera _camera;
        private Bounds _bounds;
        private Ray _bottomLeft;
        private Ray _topLeft;
        private Ray _topRight;
        private Ray _bottomRight;
        private Ray _mid;
        private bool _showArrow;
        private OrbitController _orbitController;

        private void Start()
        {
            _camera = Camera.main;
            _orbitController = OrbitController.Instance;
        }

        void Update()
        {
            _bottomLeft = _camera.ViewportPointToRay(new Vector3(0, 0, 0));
            _topLeft = _camera.ViewportPointToRay(new Vector3(0, 1, 0));
            _topRight = _camera.ViewportPointToRay(new Vector3(1, 1, 0));
            _bottomRight = _camera.ViewportPointToRay(new Vector3(1, 0, 0));
            _mid = _camera.ViewportPointToRay(new Vector3(.5f, .5f, 0));

            var botLeft = GetPointAtHeight(_bottomLeft, 0);
            var botRight = GetPointAtHeight(_bottomRight, 0);
            var topLeft = GetPointAtHeight(_topLeft, 0);
            var topRight = GetPointAtHeight(_topRight, 0);
            var mid = GetPointAtHeight(_mid, 0);

            Vector3 size = new Vector3(Vector3.Distance(( botRight + topRight ) / 2f, ( botLeft + topLeft ) / 2f),
                1, Vector3.Distance(( botRight + botLeft ) / 2f, ( topRight + topLeft ) / 2f));
            _bounds = new Bounds(mid, size);

            //_showArrow = !_bounds.Contains(new Vector3(Target.position.x, .5f, Target.position.z));


            Vector3 dir = ( Target.position - mid ).normalized;
            Vector3 camForward = Vector3.Cross(_camera.transform.right, Vector3.up);
            Vector3 screenMid = (Vector3) ( new Vector2(Screen.width, Screen.height) * .5f );


            float signedAngle = Vector3.SignedAngle(-Vector3.forward, dir, -Vector3.up) -
                Vector3.SignedAngle(camForward, Vector3.forward, Vector3.up);
            Quaternion rot = Quaternion.AngleAxis(signedAngle, Vector3.forward);

            Arrow.transform.rotation = rot;
            var screenDir = rot * -Vector3.up;
            Arrow.transform.position =
                screenMid +
                ( Screen.width - ( Screen.width / 6f ) ) * screenDir.x * .5f * Vector3.right +
                ( Screen.height - ( Screen.height / 7f ) ) * screenDir.y * .5f * Vector3.up;

            Arrow.transform.localScale = Vector3.one *
                Mathf.Lerp(0f, 2.5f,
                    Mathf.InverseLerp(5, 50, Vector3.Distance(Target.position, mid)));

            if (_orbitController != null)
            {
                if (_orbitController.OrbitData.IsOrbit)
                    Arrow.transform.localScale = Vector3.zero;
            }

            // Arrow.gameObject.SetActive(_showArrow);
        }

        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(GetPointAtHeight(_bottomLeft, 0), GetPointAtHeight(_bottomRight, 0));
            Gizmos.DrawLine(GetPointAtHeight(_bottomRight, 0), GetPointAtHeight(_topRight, 0));
            Gizmos.DrawLine(GetPointAtHeight(_topRight, 0), GetPointAtHeight(_topLeft, 0));
            Gizmos.DrawLine(GetPointAtHeight(_topLeft, 0), GetPointAtHeight(_bottomLeft, 0));
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }*/

        public static Vector3 GetPointAtHeight(Ray ray, float height)
        {
            return ray.origin + ( ( ( ray.origin.y - height ) / -ray.direction.y ) * ray.direction );
        }
    }
}