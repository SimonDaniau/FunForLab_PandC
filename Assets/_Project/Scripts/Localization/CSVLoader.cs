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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FunForLab.Localization
{
    public class CSVLoader
    {
        public static TextAsset TextAssetData;
        private static char lineSeparator = '\n';
        private static char surround = '"';
        private static string[] fieldSeparator = { "\",\"" };
        public event Action<Dictionary<string, string[]>> LocalizationLoaded;

        public Dictionary<string, string[]> Load(string path)
        {
            TextAssetData = Resources.Load<TextAsset>(path);
            Dictionary<string, string[]> content = ProcessContent();
            LocalizationLoaded?.Invoke(content);
            return content;
        }

        public Dictionary<string, string[]> ProcessContent()
        {
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();
            string[] lines = TextAssetData.text.Split(lineSeparator);

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                line = line.Replace('|', '\n');
                string[] fields = CSVParser.Split(line);
                for (int f = 0; f < fields.Length; f++)
                {
                    fields[f] = fields[f].TrimStart(' ', surround);
                    fields[f] = fields[f].TrimEnd(surround);
                }

                var key = fields[0];
                if (result.ContainsKey(key))
                {
                    continue;
                }

                result.Add(fields[0], new [] { fields[1], fields[2], fields[3], fields[4] });
            }

            return result;
        }
#if UNITY_EDITOR
        public void Add(string key, string value)
        {
            string appended = string.Format("\n\"{0}\",\"\",\"{1}\",\"\"", key, value);
            File.AppendAllText("Assets/Localization/Resources/FFL Localization - MainTable.csv", appended);
            UnityEditor.AssetDatabase.Refresh();
        }

        public void Remove(string key)
        {
            string[] lines = TextAssetData.text.Split(lineSeparator);

            string[] keys = new String[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                keys[i] = line.Split(fieldSeparator, StringSplitOptions.None)[0];
            }

            int index = -1;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].Contains(key))
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                string[] newLines;
                newLines = lines.Where(w => w != lines[index]).ToArray();
                string replaced = string.Join(lineSeparator.ToString(), newLines);
                File.WriteAllText("Assets/Localization/Resources/FFL Localization - MainTable.csv", replaced);
            }
        }

        public void Edit(string key, string value)
        {
            Remove(key);
            Add(key, value);
        }
#endif
    }
}