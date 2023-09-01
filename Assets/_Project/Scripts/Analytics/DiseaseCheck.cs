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
using System.Linq;

namespace FunForLab.Analytics
{
    public class DiseaseCheck
    {
        private static DiseaseTag _diseaseTag;

        public DiseaseCheck Check(DiseaseTag diseaseTag)
        {
            _diseaseTag = diseaseTag;
            return this;
        }

        public bool ForDisease(Data d, out DiseaseMeasurePoint point)
        {
            var correctCriterias =  _diseaseTag.Criterias.Where(x => x.Male == d.Male);
            point = new DiseaseMeasurePoint();
            if (_diseaseTag.Determination == DiseaseTag.CriteriaDetermination.Or)
            {
                foreach (var criteria in correctCriterias)
                {
                    if ( DiseaseConditionCheck(d, criteria))
                    {
                        point = criteria;
                        return true;
                    }
                }

                return false;
            }

            throw new NotImplementedException($"{_diseaseTag.Determination} not implemented in 1 point DiseaseCheck");
        }

        public bool ForDisease(Data d)
        {
            var correctCriterias =  _diseaseTag.Criterias.Where(x => x.Male == d.Male);

            if (_diseaseTag.Determination == DiseaseTag.CriteriaDetermination.Or)
            {
                foreach (var criteria in correctCriterias)
                {
                    if ( DiseaseConditionCheck(d, criteria)) return true;
                }

                return false;
            }

            if (_diseaseTag.Determination == DiseaseTag.CriteriaDetermination.And)
            {
                foreach (var criteria in correctCriterias)
                {
                    if (!DiseaseConditionCheck(d, criteria)) return false;
                }

                return true;
            }

            throw new NotImplementedException($"{_diseaseTag.Determination} not implemented in DiseaseCheck");
        }

        private bool DiseaseConditionCheck(Data d, DiseaseMeasurePoint criteria)
        {
            switch (criteria.Operator )
            {
                case DiseaseMeasurePoint.Operators.LessThan :
                    return Data.GetDataParameter(d, criteria.HematologyParameter) < criteria.Value;
                case DiseaseMeasurePoint.Operators.MoreThan :
                    return Data.GetDataParameter(d, criteria.HematologyParameter) > criteria.Value;
                case DiseaseMeasurePoint.Operators.Between :
                    return  Data.GetDataParameter(d, criteria.HematologyParameter) > criteria.Range.x && Data.GetDataParameter(d, criteria.HematologyParameter) < criteria.Range.y;
                default: throw new NotImplementedException($"{criteria.Operator} not implemented in ConditionCheck");
            }
        }
    }
}