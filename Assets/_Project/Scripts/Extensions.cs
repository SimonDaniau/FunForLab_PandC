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
using System.Linq;
using FunForLab.Localization;
using FunForLab.OrbitCamera;
using UnityEngine;

namespace FunForLab
{
    public static class Extensions
    {
        public static bool IsOrbitAParentOf(this Orbit source, Orbit target)
        {
            if (source == target) return true;

            bool foundSelf = false;
            var nextChild = target;

            if (!nextChild.Parent.IsGlobal) // target parent isn't global => target is a subOrbit
            {
                while (!nextChild.Parent.IsGlobal) // while target is subOrbit
                {
                    if (nextChild.Parent == source) // other is common to our orbitChain, we go deeper
                    {
                        foundSelf = true;
                        break;
                    }

                    nextChild = nextChild.Parent;
                }
            }
            return foundSelf;
        }
        
        public static bool IsOrbitAChildOf(this Orbit source, Orbit target)
        {
            return IsOrbitAParentOf(target, source);
        }


        public static Vector3 GetLocalPosition(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(3);
        }

        public static Vector3 GetRightDir(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(0);
        }

        public static Vector3 GetUptDir(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(1);
        }

        public static Vector3 GetForwardDir(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(2);
        }

        public static Matrix4x4 SetLocalPosition(this Matrix4x4 matrix, Vector3 pos)
        {
            Matrix4x4 result = new Matrix4x4();
            result.SetTRS(pos, matrix.GetLocalRotation(), Vector3.one);
            return result;
        }

        public static Quaternion GetLocalRotation(this Matrix4x4 matrix)
        {
            return Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        }

        public static Matrix4x4 SetLocalRotation(this Matrix4x4 matrix, Quaternion rot)
        {
            Matrix4x4 result = new Matrix4x4();
            result.SetTRS(matrix.GetLocalPosition(), rot, Vector3.one);
            return result;
        }

        public static Matrix4x4 CombinedMatrix(Matrix4x4 parent, Matrix4x4 child)
        {
            return parent * child;
        }

        public static float Half(float a, float b) => Mathf.Lerp(a, b, .5f);

        public static T[] Add<T>(this T[] array, T item)
        {
            T[] tmp = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                tmp[i] = array[i];
            }

            tmp[array.Length] = item;
            return tmp;
        }

        public static void Add<T>(ref T[] array, T item) => array = array.Add(item);

        public static T[] AddRange<T>(this T[] array, T[] items)
        {
            T[] tmp = new T[array.Length + items.Length];
            for (int i = 0; i < array.Length; i++)
            {
                tmp[i] = array[i];
            }

            for (int i = array.Length; i < array.Length + items.Length; i++)
            {
                tmp[i] = items[i - array.Length];
            }

            return tmp;
        }

        public static void AddRange<T>(ref T[] array, T[] items) => array = array.AddRange(items);

        public static Transform Closest(this List<Transform> list, Transform t, out float dist)
        {
            var res = list.OrderBy(x => Vector3.Distance(x.position, t.position)).First();
            dist = Vector3.Distance(res.position, t.position);
            return res;
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            System.Random rnd = new System.Random();
            return source.OrderBy((item) => rnd.Next());
        }

        public static float Random(this Vector2 range)
        {
            return UnityEngine.Random.Range(range.x, range.y);
        }

        public static float Random(this Vector2 range, float extrapolationPercent)
        {
            return UnityEngine.Random.Range(range.x * (1 - extrapolationPercent), range.y * (1 + extrapolationPercent));
        }

        public static string Localize(this string value, Localizator.Languages destination,
            Localizator.Languages source = Localizator.Languages.EN)
        {
            foreach (var item in Localizator.GetLocalizations())
            {
                if (item.Value[(int) source] == value) return item.Value[(int) destination];
            }

            return "";
        }

        public static string Localize(this string value, Localizator.Languages source = Localizator.Languages.EN)
        {
            foreach (var item in Localizator.GetLocalizations())
            {
                if (item.Value[(int) source] == value) return item.Value[(int) Localizator.CurrentLanguages];
            }

            return "";
        }

        public static string Localize(this string value)
        {
            return Localizator.Localize(value);
        }

        public static Color AlphaSet(this Color color, float value)
        {
            return new Color(color.r, color.g, color.b, value);
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        public static float EaseInOut(float t)
        {
            return t < 0.5f ? (4 * t * t * t) : (1 - Mathf.Pow(-2 * t + 2, 3) / 2f);
        }

        public static void DisplaySemiSphere(Vector3 position, Quaternion rotation, float radius, float fill,
            Vector3 globalOffset, int halfResolution, Color color)
        {
            float limitAngle = fill * 360f;
            float stepLimited = limitAngle / (halfResolution * 2);
            float step = 360f / (halfResolution * 2);

            float x0 = Mathf.Sin(0) * radius;
            float z0 = Mathf.Cos(0) * radius;
            Vector3 l0R = new Vector3(x0, 0, z0);
            Vector3 l0 = position + globalOffset + rotation * l0R;

            for (int im = -halfResolution; im < halfResolution; im++)
            {
                float x1 = Mathf.Sin(stepLimited * im * Mathf.Deg2Rad) * radius;
                float z1 = Mathf.Cos(stepLimited * im * Mathf.Deg2Rad) * radius;
                float x2 =
                    Mathf.Sin(stepLimited * (im + 1 > halfResolution ? -halfResolution : im + 1) * Mathf.Deg2Rad) *
                    radius;
                float z2 =
                    Mathf.Cos(stepLimited * (im + 1 > halfResolution ? -halfResolution : im + 1) * Mathf.Deg2Rad) *
                    radius;

                Vector3 l1R = new Vector3(x1, 0, z1);
                Vector3 l2R = new Vector3(x2, 0, z2);

                Vector3 lm1 = position + globalOffset + rotation * l1R;
                Vector3 lm2 = position + globalOffset + rotation * l2R;

                Vector3 lc1 = position + globalOffset + rotation * Quaternion.Euler(0, 0, 90) * l1R;
                Vector3 lc2 = position + globalOffset + rotation * Quaternion.Euler(0, 0, 90) * l2R;

                Gizmos.color = color;
                Gizmos.DrawLine(lm1, lm2);
                Gizmos.DrawLine(lc1, lc2);
            }

            for (int il = -halfResolution; il < halfResolution; il++)
            {
                float lradius = Mathf.Sin(limitAngle * Mathf.Deg2Rad / 2f) * radius;
                float lOffset = radius - (1 - Mathf.Cos(limitAngle * Mathf.Deg2Rad / 2f)) * radius;
                float x1 = Mathf.Sin(step * il * Mathf.Deg2Rad) * lradius;
                float z1 = Mathf.Cos(step * il * Mathf.Deg2Rad) * lradius;
                float x2 = Mathf.Sin(step * (il + 1 > halfResolution ? -halfResolution : il + 1) * Mathf.Deg2Rad) *
                           lradius;
                float z2 = Mathf.Cos(step * (il + 1 > halfResolution ? -halfResolution : il + 1) * Mathf.Deg2Rad) *
                           lradius;

                Vector3 ll1R = new Vector3(x1, lOffset, z1);
                Vector3 ll1 = position + globalOffset + rotation * Quaternion.Euler(90, 0, 0) * ll1R;
                Vector3 ll2R = new Vector3(x2, lOffset, z2);
                Vector3 ll2 = position + globalOffset + rotation * Quaternion.Euler(90, 0, 0) * ll2R;

                Gizmos.color = color;
                Gizmos.DrawLine(ll1, ll2);
            }

            Gizmos.color = color;
            Gizmos.DrawLine(position + globalOffset, l0);
        }
    }
}