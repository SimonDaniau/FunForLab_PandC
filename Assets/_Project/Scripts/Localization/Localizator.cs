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
using UnityEngine;

namespace FunForLab.Localization
{
    public class Localizator
    {
        public enum Languages
        {
            NL,
            EN,
            FR,
            DE
        }
    
        public static Languages CurrentLanguages;
        public static CSVLoader CsvLoader;
        public static Dictionary<string, string[]> Localizations;
        public static bool ContentProcessed;

        public static Dictionary<string, string[]> GetLocalizations()
        {
            if (CsvLoader == null)
            {
                Init();
            }

            return Localizations;
        }

        public static Dictionary<string, string> GetEditorDictionary()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            if (CsvLoader == null)
            {
                Init();
            }

            foreach (var pair in Localizations)
            {
                res.Add(pair.Key, pair.Value[(int) Languages.EN]);
            }

            return res;
        }

        public static void Init()
        {
            ContentProcessed = false;
            CurrentLanguages = (Languages) PlayerPrefs.GetInt("Languages", (int) Languages.EN);
            Localizations = new Dictionary<string, string[]>();
            if (CsvLoader == null)
            {
                CsvLoader = new CSVLoader();
            }

            Localizations = CsvLoader.Load("FFL Localization - MainTable");
            ContentProcessed = true;
        }
#if UNITY_EDITOR
        public static void Add(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if (CsvLoader == null)
            {
                Init();
            }

            CsvLoader.Add(key, value);
        }

        public static void Replace(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if (CsvLoader == null)
            {
                Init();
            }

            CsvLoader.Edit(key, value);
        }

        public static void Remove(string key)
        {
            if (CsvLoader == null)
            {
                Init();
            }

            CsvLoader.Remove(key);
        }

#endif
        public static string Localize(string[] values)
        {
            CurrentLanguages = (Languages) PlayerPrefs.GetInt("Languages", (int) Languages.EN);
            if (CsvLoader == null)
            {
                Init();
            }

            return values[(int) CurrentLanguages];
        }

        public static string Localize(string key)
        {
            CurrentLanguages = (Languages) PlayerPrefs.GetInt("Languages", (int) Languages.EN);
            if (CsvLoader == null)
            {
                Init();
            }

            if (Localizations.ContainsKey(key))
                return Localizations[key][(int) CurrentLanguages].Replace("\"", "");
            return "";
        }

        public static string Localize(string value, Languages destination, Languages source = Languages.EN)
        {
            if (CsvLoader == null)
            {
                Init();
            }

            foreach (var item in Localizations)
            {
                if (item.Value[(int) source] == value) return item.Value[(int) destination];
            }

            return "";
        }

        public static string Localize(string value, Languages source = Languages.EN)
        {
            CurrentLanguages = (Languages) PlayerPrefs.GetInt("Languages", (int) Languages.EN);
            if (CsvLoader == null)
            {
                Init();
            }

            foreach (var item in Localizations)
            {
                if (item.Value[(int) source] == value) return item.Value[(int) CurrentLanguages];
            }

            return "";
        }
    }
}