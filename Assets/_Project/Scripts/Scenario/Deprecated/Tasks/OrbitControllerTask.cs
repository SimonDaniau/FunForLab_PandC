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
using FunForLab.OrbitCamera;

namespace FunForLab.Scenario.Tasks
{
    public class OrbitControllerTask : Objective
    {
        public enum OrbitSubTask
        {
            OrbitMoveIn,
            OrbitMoveOut,
            OrbitMoveAround
        }

        private OrbitSubTask _subTask;
        private OrbitController _controller;
        private string _description;
        private float _disp;

        public override string Description() => _description;

        public override bool Criteria()
        {
            switch (_subTask)
            {
                case OrbitSubTask.OrbitMoveIn:
                {
                    if (_controller.inTransition && _controller.CurrentOrbit != null)
                        return true;
                }
                    break;
                case OrbitSubTask.OrbitMoveOut:
                {
                    if (_controller.inTransition && _controller.CurrentOrbit.IsGlobal)
                        return true;
                }
                    break;
                case OrbitSubTask.OrbitMoveAround:
                {
                    _disp += _controller.OrbitData.Velocity.magnitude;
                    if (_disp > 60f) return true;
                }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return false;
        }


        public OrbitControllerTask(OrbitController controller, string description, OrbitSubTask type)
        {
            _controller = controller;
            _description = description;
            _subTask = type;
            _disp = 0;
        }
    }
}