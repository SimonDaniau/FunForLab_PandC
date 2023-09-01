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
using UnityEngine;

namespace FunForLab.Modules
{
    public class DebugCommandBase
    {
        private string _commandId;
        private string _commandDesc;
        private string _commandFormat;

        public string CommandId => _commandId;
        public string CommandDesc => _commandDesc;
        public string CommandFormat => _commandFormat;

        public DebugCommandBase(string id, string desc, string format)
        {
            _commandId = id;
            _commandDesc = desc;
            _commandFormat = format;
        }
    }

    public class DebugCommand : DebugCommandBase
    {
        private Action _command;

        public DebugCommand (string id, string desc, string format, Action command) : base(id, desc, format)
        {
            _command = command;
        }

        public void Invoke()
        {
            _command.Invoke();
        }
    }

    public class DebugCommand<T1> : DebugCommandBase
    {
        private Action<T1> _command;

        public DebugCommand (string id, string desc, string format, Action<T1> command) : base(id, desc, format)
        {
            _command = command;
        }

        public void Invoke(T1 value)
        {
            _command.Invoke(value);
        }
    }

    public class DebugCommand<T1,T2> : DebugCommandBase
    {
        private Action<T1,T2> _command;

        public DebugCommand (string id, string desc, string format, Action<T1,T2> command) : base(id, desc, format)
        {
            _command = command;
        }

        public void Invoke(T1 value1, T2 value2)
        {
            _command.Invoke(value1, value2);
        }
    }
}