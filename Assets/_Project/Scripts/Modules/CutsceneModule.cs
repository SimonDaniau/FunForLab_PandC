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
using FunForLab.OrbitCamera;
using FunForLab.Scenario;
using FunForLab.UI;
using UnityEngine;

namespace FunForLab.Modules
{
    public class CutsceneModule : MonoBehaviour
    {
        [Header("HospitalFront")]
        public Transform Hf_CamPose1;

        public Transform Hf_CamPose2;

        [Header("BloodDrawingPatientNurse")]
        public Transform Bdpn_CamPose;

        public GameObject Bdpn_MalePatient;
        public GameObject Bdpn_FemalePatient;

        public List<string> _prompts;

        public SceneType CurrentScene;
        public Pose CurrentScenePose;

        public enum SceneType
        {
            Origin,
            BloodDrawingPatientNurse,
            HospitalFront
        }

        public class State
        {
            public Vector2Int Dialog;
            public bool Playing;
            public bool Finished;
        }

        public State CurrentState { get; private set; }

        public Action<State> OnStateChange;

        void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void SetCamera(SceneType type)
        {
            CurrentScene = type;
            switch (type)
            {
                case SceneType.HospitalFront:
                    CurrentScenePose = new Pose(Hf_CamPose1.position, Hf_CamPose1.rotation);
                    break;
                case SceneType.BloodDrawingPatientNurse:
                    CurrentScenePose = new Pose(Bdpn_CamPose.position, Bdpn_CamPose.rotation);
                    break;
            }

            OrbitController.Instance.SetOrbitControllerPositionAndRotation(CurrentScenePose.position, CurrentScenePose.rotation);
        }

        public void CameraLock(bool state)
        {
            if (state) OrbitController.Instance.Override++;
            else  OrbitController.Instance.Override--;
        }

        public void PlayCutscene( MissionData data)
        {
            switch (CurrentScene)
            {
                case SceneType.BloodDrawingPatientNurse:
                    StartCoroutine(Play(BloodDrawingPatientNurse()));
                    break;
                case SceneType.HospitalFront:
                    StartCoroutine(Play(HospitalFront(10f)));
                    break;
            }
        }

        public void SetupScene( MissionData data)
        {
            switch (CurrentScene)
            {
                case SceneType.BloodDrawingPatientNurse:
                {
                    Bdpn_MalePatient.SetActive(data.PatientIsMale);
                    Bdpn_FemalePatient.SetActive(!data.PatientIsMale);
                }
                    break;
                case SceneType.HospitalFront:
                    break;
            }
        }

        public void Setup(List<string> prompts)
        {
            Setup();
            CurrentState.Dialog = new Vector2Int(0, prompts.Count);
            _prompts = prompts;
        }

        public void Setup()
        {
            CurrentState = new State();
            CurrentState.Finished = false;
            OnStateChange?.Invoke(CurrentState);
        }

        private IEnumerator Play(IEnumerator cutscene)
        {
            CurrentState.Playing = true;
            OnStateChange?.Invoke(CurrentState);
            yield return cutscene;
            CurrentState.Playing = false;
            CurrentState.Finished = true;
            OnStateChange?.Invoke(CurrentState);
        }

        public IEnumerator BloodDrawingPatientNurse()
        {
            yield return new WaitForSeconds(.2f);
            yield return AdvanceDialog(CurrentState, () => Input.anyKeyDown);
        }

        public IEnumerator HospitalFront(float duration = 6f, float initialDialogDelay = 1f)
        {
            MissionWindow window = MissionWindow.Instance;
            bool isWindowNotNull = window != null;
            yield return new WaitForSeconds(.2f);
            bool firstPrompt = false;
            float timeElapsed = 0f;
            float dialogTimeElapsed = 0f;
            float skipTimeElapsed = 0f;
            float timeToSkip = .75f;
            var orbitController = OrbitController.Instance;
            float dialogTimeStep = ( duration - initialDialogDelay ) / ( CurrentState.Dialog.y + 1f );

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float localPercent = timeElapsed / duration;

                orbitController.SetOrbitControllerPositionAndRotation(
                    Vector3.Lerp(Hf_CamPose1.position, Hf_CamPose2.position, localPercent),
                    Quaternion.Slerp(Hf_CamPose1.rotation, Hf_CamPose2.rotation, localPercent));
                if ( timeElapsed > initialDialogDelay)
                {
                    if (!firstPrompt)
                    {
                        MissionManager.Instance.ChangeDescription(_prompts[CurrentState.Dialog.x]);
                        firstPrompt = true;
                    }

                    dialogTimeElapsed += Time.deltaTime;
                    if (dialogTimeElapsed >= dialogTimeStep)
                    {
                        dialogTimeElapsed -= dialogTimeStep;
                        AdvanceDialog(CurrentState);
                    }
                }


                if (isWindowNotNull)
                {
                    if (Input.anyKey)
                    {
                        skipTimeElapsed += Time.deltaTime;
                        if (skipTimeElapsed > timeToSkip)
                        {
                            timeElapsed = duration;
                        }
                    }
                    else if (skipTimeElapsed > 0)
                    {
                        skipTimeElapsed -= Time.deltaTime * 4f;
                    }

                    window.SetSkipAmount(Mathf.Clamp01(skipTimeElapsed / timeToSkip));
                }

                yield return null;
            }
        }

        private IEnumerator AdvanceDialog(State currentState, Func<bool> criteria)
        {
            MissionManager.Instance.DisplayKeyPressNotifier(false);
            MissionManager.Instance.ChangeDescription(_prompts[CurrentState.Dialog.x]);
            while (currentState.Dialog.x < currentState.Dialog.y)
            {
                if (criteria())
                {
                    if (currentState.Dialog.x < currentState.Dialog.y - 1)
                        MissionManager.Instance.ChangeDescription(_prompts[currentState.Dialog.x + 1]);

                    currentState.Dialog.x++;
                    OnStateChange?.Invoke(currentState);
                    MissionManager.Instance.DisplayKeyPressNotifier(false);
                    yield return new WaitForSeconds(.5f);
                }

                MissionManager.Instance.DisplayKeyPressNotifier(true);
                yield return null;
            }

            MissionManager.Instance.DisplayKeyPressNotifier(false);
        }

        private void AdvanceDialog(State currentState)
        {
            if (currentState.Dialog.x < currentState.Dialog.y)
            {
                if (currentState.Dialog.x < currentState.Dialog.y - 1)
                    MissionManager.Instance.ChangeDescription(_prompts[currentState.Dialog.x + 1]);

                currentState.Dialog.x++;
                OnStateChange?.Invoke(currentState);
            }
        }
    }
}