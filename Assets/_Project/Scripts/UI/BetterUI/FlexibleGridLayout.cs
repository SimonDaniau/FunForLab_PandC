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
using UnityEngine.UI;

namespace FunForLab.UI.BetterUI
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns,
        }

        public bool PaddingInPercent;
        public FitType Fit;
        public int Rows;
        public int Columns;
        public Vector2 CellSize;
        public Vector2 Spacing;

        public bool FitX;
        public bool FitY;

        public bool ResizeContainerToFitContent;

        [ContextMenu("Switch Percent")]
        public void SwitchPercent()
        {
            if (PaddingInPercent)
            {
                padding.left =  Mathf.RoundToInt(( padding.left * rectTransform.rect.width ) / 100);
                padding.right =  Mathf.RoundToInt(( padding.right * rectTransform.rect.width ) / 100);
                padding.top =  Mathf.RoundToInt(( padding.top * rectTransform.rect.height ) / 100);
                padding.bottom =  Mathf.RoundToInt(( padding.bottom * rectTransform.rect.height ) / 100);
                PaddingInPercent = false;
            }
            else
            {
                padding.left =  Mathf.RoundToInt(( padding.left / rectTransform.rect.width ) * 100);
                padding.right =  Mathf.RoundToInt(( padding.right / rectTransform.rect.width ) * 100);
                padding.top =  Mathf.RoundToInt(( padding.top / rectTransform.rect.height ) * 100);
                padding.bottom =  Mathf.RoundToInt(( padding.bottom / rectTransform.rect.height ) * 100);
                PaddingInPercent = true;
            }
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            int childCount = rectChildren.Count;
            foreach (var item in rectChildren)
            {
                var elem = item.GetComponent<UILayoutExtractor>();
                if (elem != null) childCount--;
            }

            Vector4 newPadding = new Vector4();

            newPadding.x = PaddingInPercent ? Mathf.RoundToInt(( padding.left * rectTransform.rect.width ) / 100 ) : padding.left;
            newPadding.y = PaddingInPercent ? Mathf.RoundToInt(( padding.right * rectTransform.rect.width ) / 100 ) : padding.right;
            newPadding.z = PaddingInPercent ? Mathf.RoundToInt(( padding.top * rectTransform.rect.height ) / 100 ) : padding.top;
            newPadding.w = PaddingInPercent ? Mathf.RoundToInt(( padding.bottom * rectTransform.rect.height ) / 100 ) : padding.bottom;


            if (Fit == FitType.Width || Fit == FitType.Height || Fit == FitType.Uniform )
            {
                FitX = true;
                FitY = true;
                float sqrt = Mathf.Sqrt(childCount);
                Rows = Mathf.CeilToInt(sqrt);
                Columns = Mathf.CeilToInt(sqrt);
            }

            if (Fit == FitType.Width || Fit == FitType.FixedColumns)
            {
                Rows = Mathf.CeilToInt(childCount / (float) Columns);
            }

            if (Fit == FitType.Height || Fit == FitType.FixedRows)
            {
                Columns = Mathf.CeilToInt(childCount / (float) Rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = parentWidth / (float) Columns -
                ( ( Spacing.x / (float) Columns ) * ( Columns - 1 ) ) -
                ( newPadding.x / (float) Columns ) -
                ( newPadding.y / (float) Columns );
            float cellHeight = parentHeight / (float) Rows - ( ( Spacing.y / (float) Rows ) * ( Rows - 1 ) )  - ( newPadding.z / (float) Rows ) - ( newPadding.w / (float) Rows );

            CellSize.x = FitX ? cellWidth : CellSize.x;
            CellSize.y = FitY ? cellHeight : CellSize.y;

            int columnCount = 0;
            int rowCount = 0;


            for (int i = 0; i < childCount; i++)
            {
                rowCount = i / Columns;
                columnCount = i % Columns;
                var item = rectChildren[i];

                var xPos = ( CellSize.x * columnCount ) + ( Spacing.x * columnCount ) + newPadding.x;
                var yPos = ( CellSize.y * rowCount ) + ( Spacing.y * rowCount ) + newPadding.z;

                SetChildAlongAxis(item, 0, xPos, CellSize.x);
                SetChildAlongAxis(item, 1, yPos, CellSize.y);
            }

            if (ResizeContainerToFitContent)
            {
                rectTransform.sizeDelta = new Vector2(( CellSize.x + Spacing.x ) * columnCount + newPadding.x, ( CellSize.y + Spacing.y ) * ( rowCount + 1 )  + newPadding.z);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}