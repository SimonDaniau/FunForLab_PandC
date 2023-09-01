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

namespace FunForLab.OrbitCamera
{
    public static class OrbitCalculation
    {
        public static Vector3 GetPositionFromRotationAxis(Orbit orb, Vector2 rotationAxis, float angleLimit)
        {
            Vector2 virtualLimiter =
                new Vector2(Mathf.InverseLerp(-angleLimit, angleLimit, rotationAxis.x), Mathf.InverseLerp(-angleLimit, angleLimit, rotationAxis.y)) * 2f -
                Vector2.one;
            virtualLimiter = ( Vector2.one + ( virtualLimiter.magnitude > 1 ? virtualLimiter.normalized : virtualLimiter ) ) / 2f;
            rotationAxis.x = Mathf.Lerp(-angleLimit, angleLimit, virtualLimiter.x);
            rotationAxis.y = Mathf.Lerp(-angleLimit, angleLimit, virtualLimiter.y);
            Quaternion toRotation = Quaternion.Euler(orb.transform.rotation.eulerAngles.x + rotationAxis.x, orb.transform.rotation.eulerAngles.y + rotationAxis.y, 0);
            Quaternion rotation = toRotation * Quaternion.Euler(orb.LocalRotation + new Vector3(180, 0, 180));

            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -orb.Radius) + orb.GetCenter();
            return position;
        }

        public static Vector2 GetRotationAxisFromPosition(Orbit orb, Vector3 pos)
        {
            var angleLimit = orb.ArcFillPercent * 180f;
            Quaternion finalRot = Quaternion.FromToRotation(orb.GetNormalDirection(), pos - orb.GetCenter());

            Vector2 ret = new Vector2(finalRot.eulerAngles.x, finalRot.eulerAngles.y);
            ret.x = ret.x > 180 ? ret.x - 360 : ret.x;
            ret.y = ret.y > 180 ? ret.y - 360 : ret.y;
            Vector2 virtualLimiter =
                new Vector2(Mathf.InverseLerp(-angleLimit, angleLimit, ret.x), Mathf.InverseLerp(-angleLimit, angleLimit, ret.y)) * 2f -
                Vector2.one;
            virtualLimiter = ( Vector2.one + ( virtualLimiter.magnitude > 1 ? virtualLimiter.normalized : virtualLimiter ) ) / 2f;
            ret.x = Mathf.Lerp(-angleLimit, angleLimit, virtualLimiter.x);
            ret.y = Mathf.Lerp(-angleLimit, angleLimit, virtualLimiter.y);
            return ret;
        }

        public static void ComputeOrbitalVelocity(Orbit currentOrbit, Matrix4x4 orbitMatrix, float mouseDragSpeed, ref Vector2 rotationAxis, ref Vector2 velocity,
            out Vector3 targetPos, out Quaternion targetRot)
        {
            rotationAxis += velocity;
            var angleLimit = currentOrbit.ArcFillPercent * 180f;
            rotationAxis.x = Extensions.ClampAngle(rotationAxis.x, -angleLimit, angleLimit);
            rotationAxis.y = Extensions.ClampAngle(rotationAxis.y, -angleLimit, angleLimit);
            Vector3 position = GetPositionFromRotationAxis(currentOrbit, rotationAxis, angleLimit);

            targetRot = Quaternion.LookRotation(currentOrbit.GetCenter() - position,
                Vector3.Lerp(Vector3.up, Vector3.Cross( currentOrbit.GetNormalDirection(), currentOrbit.transform.right),
                    Mathf.Clamp01(Vector3.Dot(-Vector3.up, orbitMatrix.GetForwardDir())) ));
            targetPos = position;
            velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * mouseDragSpeed);
            velocity.y = Mathf.Lerp(velocity.y, 0, Time.deltaTime * mouseDragSpeed);
        }
        
        public static Vector3 GetClosestValidPosFrom(float radius, Vector3 getCenter, Transform transform1, Vector3 localRotation, float arcFillPercent, Vector3 point)
        {
            var angleLimit = arcFillPercent * 180f;
            Vector3 d1 = Quaternion.Euler(localRotation) * transform1.forward;
            Vector3 d2 = ( point - getCenter ).normalized;
            Vector3 axis = Vector3.Cross(d1, d2).normalized;
            float angle = Vector3.SignedAngle(d1, d2, axis);
            angle = Mathf.Clamp(angle, -angleLimit, angleLimit);
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);
            return rotation * transform1.rotation * Quaternion.Euler(localRotation) * new Vector3(0.0f, 0.0f, radius) + getCenter;
        }
        
        public static bool IsTargetCommonOrbit( Orbit current, Orbit target)
        {
            if (current == null || target == null || target.Parent == null) return false;
            bool foundSelf = false;

            if (!target.Parent.IsGlobal) // target parent isn't global => target is a subOrbit
            {
                while (!target.Parent.IsGlobal) // while target is subOrbit
                {
                    if (target.Parent == current) // other is common to our orbitChain, we go deeper
                    {
                        foundSelf = true;
                        break;
                    }

                    target = target.Parent;
                }
            }

            return foundSelf;
        }

        public static Vector3 GetSimsPointOnGround(Matrix4x4 dummySimsMatrix, Quaternion newRotation, Vector3 camPos)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = new Ray(camPos + newRotation * dummySimsMatrix.GetLocalPosition(), newRotation * dummySimsMatrix.GetLocalRotation() * Vector3.forward);
            if (plane.Raycast(ray, out var dist))
            {
                return ray.GetPoint(dist);
            }

            return Vector3.zero;
        }
    }
}