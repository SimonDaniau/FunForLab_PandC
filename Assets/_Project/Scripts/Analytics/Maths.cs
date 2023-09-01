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

namespace FunForLab.Analytics
{
    public static class Maths
    {
        public static float Remap(this float value, float oldA, float oldB, float newA, float newB)
        {
            return Mathf.Lerp(newA, newB, Mathf.InverseLerp(oldA, oldB, value));
        }

        public static Vector3 ClampMagnitude(this Vector3 vector, float minLength, float maxLength, float correctDot)
        {
            float dot = Vector3.Dot(vector, Vector3.up);
            Vector3 result = vector.ClampMagnitude(minLength, maxLength);
            return Mathf.Sign(dot) == Mathf.Sign(correctDot) ? result : -result;
        }

        public static Vector3 ClampMagnitude(this Vector3 vector, float minLength, float maxLength)
        {
            float magnitude = vector.magnitude;
            if ( magnitude <= maxLength )
            {
                if ( magnitude >= minLength )
                    return vector;

                float num1 = magnitude;
                float num2 = vector.x / num1;
                float num3 = vector.y / num1;
                float num4 = vector.z / num1;
                return new Vector3(num2 * minLength, num3 * minLength, num4 * minLength);
            }
            else
            {
                float num1 = magnitude;
                float num2 = vector.x / num1;
                float num3 = vector.y / num1;
                float num4 = vector.z / num1;
                return new Vector3(num2 * maxLength, num3 * maxLength, num4 * maxLength);
            }
        }

        public static Vector2 HeightRangeFromAge(int age, bool male)
        {
            Vector2 res;
            var yAge = Mathf.Min(age, 18f);
            if (male)
            {
                float hm =
                    67.7f +
                    10.1f * yAge -
                    0.713f * Mathf.Pow(yAge, 2) +
                    0.0586f * Mathf.Pow(yAge, 3) -
                    1.77f * Mathf.Pow(10, -3) * Mathf.Pow(yAge, 4);
                float he =
                    -0.256f +
                    4.59f * yAge -
                    0.672f * Mathf.Pow(yAge, 2) +
                    0.0463f * Mathf.Pow(yAge, 3) -
                    1.19f * Mathf.Pow(10, -3) * Mathf.Pow(yAge, 4);

                res.x = hm - he;
                res.y = hm + he;
            }
            else
            {
                float hf =
                    67.5f +
                    9.67f * yAge -
                    0.436f * Mathf.Pow(yAge, 2) +
                    0.0307f * Mathf.Pow(yAge, 3) -
                    1.13f * Mathf.Pow(10, -3) * Mathf.Pow(yAge, 4);
                float he =
                    5.45f -
                    0.116f * yAge +
                    0.188f * Mathf.Pow(yAge, 2) -
                    0.0161f * Mathf.Pow(yAge, 3) +
                    3.89f * Mathf.Pow(10, -4) * Mathf.Pow(yAge, 4);

                res.x = hf - he;
                res.y = hf + he;
            }

            if (age > 40)
            {
                res.x -= .1f * ( age - 40 );
                res.y -= .1f * ( age - 40 );
            }

            if (age > 60)
            {
                res.x -= .15f * ( age - 60 );
                res.y -= .15f * ( age - 60 );
            }

            return res;
        }

        //heightRange [51cm - 213cm]
        public static Vector2 WeightRangeFromHeight(float height, bool male)
        {
            Vector2 res;
            if (male)
            {
                //-74.9\ +\ 3.11x\ -\ 0.0426x^{2}\ +\ 2.54\cdot10^{-4}x^{3}-4.84\cdot10^{-7}x^{4}
                //-10.8\ +\ 0.452x\ -\ 6.12\cdot10^{-3}x^{2}+3.52\cdot10^{-5}x^{3}\ -\ 6.64\cdot10^{-8}x^{4}
                float pm =
                    - 74.9f +
                    3.11f * height -
                    0.0426f * Mathf.Pow(height, 2) +
                    2.54f * Mathf.Pow(10, -4) * Mathf.Pow(height, 3) -
                    4.84f * Mathf.Pow(10, -7) * Mathf.Pow(height, 4);
                float em =
                    - 10.8f +
                    0.452f * height -
                    6.12f * Mathf.Pow(10, -3) * Mathf.Pow(height, 2) +
                    3.52f * Mathf.Pow(10, -5) * Mathf.Pow(height, 3) -
                    6.64f * Mathf.Pow(10, -8) * Mathf.Pow(height, 4);

                res.x = pm - em;
                res.y = pm + em;
            }
            else
            {
                //-51.4\ +\ 2.21x\ \ -0.0311x^{2\ }+1.94\cdot10^{-4}x^{3}-3.8\cdot10^{-7}x^{4}
                //-6.99\ +\ 0.303x\ -\ 4.13\cdot10^{-3}x^{2}+2.44\cdot10^{-5}x^{3}\ -\ 4.66\cdot10^{-8}x^{4}
                float pf =
                    - 51.4f +
                    2.21f * height -
                    0.0311f * Mathf.Pow(height, 2) +
                    1.94f * Mathf.Pow(10, -4) * Mathf.Pow(height, 3) -
                    3.8f * Mathf.Pow(10, -7) * Mathf.Pow(height, 4);
                float ef =
                    - 6.99f +
                    0.303f * height -
                    4.13f * Mathf.Pow(10, -3) * Mathf.Pow(height, 2) +
                    2.44f * Mathf.Pow(10, -5) * Mathf.Pow(height, 3) -
                    4.66f * Mathf.Pow(10, -8) * Mathf.Pow(height, 4);

                res.x = pf - ef;
                res.y = pf + ef;
            }

            return res;
        }
    }
}