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
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestInstancing : MonoBehaviour
{
    MaterialPropertyBlock _prop;
    private static readonly int TopColor = Shader.PropertyToID("_TopColor");
    private static readonly int MidColor = Shader.PropertyToID("_MidColor");
    private static readonly int BotColor = Shader.PropertyToID("_BotColor");
    private static readonly int FirstLevel = Shader.PropertyToID("_FirstLevel");
    private static readonly int SecondLevel = Shader.PropertyToID("_SecondLevel");
    private static readonly int ThirdLevel = Shader.PropertyToID("_ThirdLevel");

    [InlineProperty] [SerializeField] private TubeData _data;

    private void Start()
    {
        if (_prop == null)
            _prop = new MaterialPropertyBlock();
        Renderer meshRenderer = GetComponentInChildren<Renderer>();

        Color b = GetRandomColor();
        Color m = GetRandomColor();
        Color t = GetRandomColor();
        float l1 = GetRandomLevel(0);
        float l2 = GetRandomLevel(l1);
        float l3 = GetRandomLevel(l2);

        _prop.SetColor(BotColor, b);
        _prop.SetColor(MidColor, m);
        _prop.SetColor(TopColor, t);
        _prop.SetFloat(FirstLevel, l1);
        _prop.SetFloat(SecondLevel, l2);
        _prop.SetFloat(ThirdLevel, l3);

        meshRenderer.SetPropertyBlock(_prop);

        _data = new TubeData();
        _data.BotColor = b;
        _data.MidColor = m;
        _data.TopColor = t;
        _data.FirstLevel = l1;
        _data.SecondLevel = l2;
        _data.ThirdLevel = l3;
    }

    [Button] private void UpdateColor()
    {
        if (_prop == null)
            _prop = new MaterialPropertyBlock();
        Renderer rend = GetComponentInChildren<Renderer>();

        _prop.SetColor(BotColor, _data.BotColor);
        _prop.SetColor(MidColor, _data.MidColor);
        _prop.SetColor(TopColor, _data.TopColor);
        _prop.SetFloat(FirstLevel, _data.FirstLevel);
        _prop.SetFloat(SecondLevel, _data.SecondLevel);
        _prop.SetFloat(ThirdLevel, _data.ThirdLevel);

        rend.SetPropertyBlock(_prop);
    }

    static Color GetRandomColor()
    {
        Color col = Color.HSVToRGB(Random.value, 1, .9f);
        col.a = Random.value;
        return col;
    }

    static float GetRandomLevel(float previous)
    {
        return  previous + Mathf.Max(Random.value * .3f, .1f) ;
    }

    [System.Serializable]
    struct TubeData
    {
        public float FirstLevel;
        public float SecondLevel;
        public float ThirdLevel;
        public Color BotColor;
        public Color MidColor;
        public Color TopColor;
    }
}