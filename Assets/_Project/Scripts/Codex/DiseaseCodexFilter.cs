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
using System.Collections.Generic;
using System.Linq;
using FunForLab.Analytics;
using UnityEngine;

namespace FunForLab.Codex
{
    public static class DiseaseCodexFilter
    {
        private static List<DiseaseTag> _results;
        private static List<DiseaseTag> _allTags;
        private static string _loadedPath;

        public static void Init(string path = "Diseases")
        {
            _allTags = Resources.LoadAll<DiseaseTag>(path).ToList();
            if (_allTags == null)
                throw new NullReferenceException("Could not Resources.LoadAll @Resources/" + path);
            if (_allTags.Count == 0)
                throw new Exception("Resources.LoadAll @Resources/" + path + "returns an empty List");
            _loadedPath = path;
        }

        public static List<DiseaseTag> GetAll(string path = "Diseases")
        {
            if (_allTags == null || _allTags.Count == 0 || path != _loadedPath) Init(path);
            return _allTags.ToList();
        }

        public static List<DiseaseTag> RequestByParameter(List<Enums.HematologyMeasures> containedParameter,
            List<Enums.HematologyMeasures> notContainedParameter, string path = "Diseases")
        {
            if (_allTags == null || _allTags.Count == 0 || path != _loadedPath) Init(path);

            if (containedParameter.Count > 0)
                _results = _allTags
                    .Where(x => x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.HematologyAnalysis) >= 1)
                    .Where(x => x.Criterias.Count(y => containedParameter.Contains(y.HematologyParameter)) >= 1)
                    .Where(x => x.Criterias.Count(y => notContainedParameter.Contains(y.HematologyParameter)) == 0)
                    .ToList();
            else
                _results = _allTags
                    .Where(x => x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.HematologyAnalysis) >= 1)
                    .Where(x => x.Criterias.Count(y => notContainedParameter.Contains(y.HematologyParameter)) == 0)
                    .ToList();
            return _results;
        }

        public static List<DiseaseTag> RequestByParameter(List<Enums.MicroscopicMeasures> containedParameter,
            List<Enums.MicroscopicMeasures> notContainedParameter, string path = "Diseases")
        {
            if (_allTags == null || _allTags.Count == 0 || path != _loadedPath) Init(path);

            if (containedParameter.Count > 0)
                _results = _allTags
                    .Where(x => x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.MicroscopeAnalysis) >= 1)
                    .Where(x => x.Criterias.Count(y => containedParameter.Contains(y.MicroscopicParameter)) >= 1)
                    .Where(x => x.Criterias.Count(y => notContainedParameter.Contains(y.MicroscopicParameter)) == 0)
                    .ToList();
            else
                _results = _allTags
                    .Where(x => x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.MicroscopeAnalysis) >= 1)
                    .Where(x => x.Criterias.Count(y => notContainedParameter.Contains(y.MicroscopicParameter)) == 0)
                    .ToList();
            return _results;
        }

        public static List<DiseaseTag> RequestByParameter(List<Enums.MultistixMeasures> containedParameter,
            List<Enums.MultistixMeasures> notContainedParameter, string path = "Diseases")
        {
            if (_allTags == null || _allTags.Count == 0 || path != _loadedPath) Init(path);

            if (containedParameter.Count > 0)
                _results = _allTags
                    .Where(x => x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.MultistixAnalysis) >= 1)
                    .Where(x => x.Criterias.Count(y => containedParameter.Contains(y.MultistixParameter)) >= 1)
                    .Where(x => x.Criterias.Count(y => notContainedParameter.Contains(y.MultistixParameter)) == 0)
                    .ToList();
            else
                _results = _allTags
                    .Where(x => x.Criterias.Count(y => y.AnalysisType == Enums.AnalysisType.MultistixAnalysis) >= 1)
                    .Where(x => x.Criterias.Count(y => notContainedParameter.Contains(y.MultistixParameter)) == 0)
                    .ToList();
            return _results;
        }
    }
}