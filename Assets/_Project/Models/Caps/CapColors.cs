
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

[CreateAssetMenu(fileName = "CapColors", menuName = "Data/CapColors", order = 1)]
public class CapColors : ScriptableObject
{
    public enum Caps
    {
        Red, // [SERUM / spray-coated silica /cap13*75-4|cap13*100-5|stop16*100-10 /]tube sec - pas d'anti coagulant -> sérum aprés centrifugation utilisé pour (sérologie,biochimie(ionogramme,urée,créatinine,choléstérol),allergie,autoimmunité,hormonologie,cancerologie)
        Green, // [HEPARIN / spray-coated (lithium/sodium)heparin /cap13*75-4/]tube avec anticoagulant héparine de lithium ou sodium -> plasma,lactates, methémoglobine
        LightGreen, // PST / spray-coated (lithium/sodium)heparin + polymer gel /cap13*75-3|cap13*100-4.5/]tube avec anticoagulant héparine de lithium ou sodium -> plasma,lactates, methémoglobine
        Blue, // [CITRATE / sodium citrate solution /cap13*75-4.5/]tube avec anticoagulant citrate de sodium -> analyses coagulation 
        Purple, // [EDTA / spray-coated edta /cap13*75-[2-3-4]/]tube avec anticoagulant EDTA -> numération (gbbl,gbro,plaqu),groupe sanguin,hemoglobine glyquée
        LightPurple, // -> pédiatrie -> petit tubes
        Yellow, //[SST / spray-coated silica + polymer gel /cap13*75-3.5|cap13*100-5|cap13*100-4 /] comme tube rouge + gel pour séparer cellules et liquides -> plasma
        Grey, // [GLUCOSE / glycolytic inhibitor /cap13*75-4|cap13*75-2/] fluorure de sodium/oxalate de potassium -> dosage de la glycémie dans plasma
        Invalid
    }
    public Material[] Colors;
}
