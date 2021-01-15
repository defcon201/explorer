using Cinemachine;
using DCL.SettingsController;
using UnityEngine;
using UnityEngine.UI;

namespace DCL.SettingsPanelHUD.Controls
{
    [CreateAssetMenu(menuName = "Settings/Controllers/Controls/Mouse Sensivity", fileName = "MouseSensivityControlController")]
    public class MouseSensivityControlController : SettingsControlController
    {
        internal const float FIRST_PERSON_MIN_SPEED = 25f;
        internal const float FIRST_PERSON_MAX_SPEED = 350f;
        internal const float THIRD_PERSON_X_MIN_SPEED = 100f;
        internal const float THIRD_PERSON_X_MAX_SPEED = 450f;
        internal const float THIRD_PERSON_Y_MIN_SPEED = 0.5f;
        internal const float THIRD_PERSON_Y_MAX_SPEED = 3f;

        private CinemachinePOV povCamera;
        private Slider mouseSensitivitySlider;        

        public override void Initialize(ISettingsControlView settingsControlView)
        {
            base.Initialize(settingsControlView);

            povCamera = GeneralSettingsReferences.i.firstPersonCamera.GetCinemachineComponent<CinemachinePOV>();
            mouseSensitivitySlider = ((SliderSettingsControlView)view).sliderControl;
        }

        public override object GetStoredValue()
        {
            return Mathf.Lerp(mouseSensitivitySlider.minValue, mouseSensitivitySlider.maxValue, currentGeneralSettings.mouseSensitivity);
        }

        public override void OnControlChanged(object newValue)
        {
            currentGeneralSettings.mouseSensitivity = RemapMouseSensitivityTo01((float)newValue);

            var povSpeed = Mathf.Lerp(FIRST_PERSON_MIN_SPEED, FIRST_PERSON_MAX_SPEED, currentGeneralSettings.mouseSensitivity);
            povCamera.m_HorizontalAxis.m_MaxSpeed = povSpeed;
            povCamera.m_VerticalAxis.m_MaxSpeed = povSpeed;
            GeneralSettingsReferences.i.thirdPersonCamera.m_XAxis.m_MaxSpeed = Mathf.Lerp(THIRD_PERSON_X_MIN_SPEED, THIRD_PERSON_X_MAX_SPEED, currentGeneralSettings.mouseSensitivity);
            GeneralSettingsReferences.i.thirdPersonCamera.m_YAxis.m_MaxSpeed = Mathf.Lerp(THIRD_PERSON_Y_MIN_SPEED, THIRD_PERSON_Y_MAX_SPEED, currentGeneralSettings.mouseSensitivity);
        }

        private float RemapMouseSensitivityTo01(float value)
        {
            return (value - mouseSensitivitySlider.minValue)
                / (mouseSensitivitySlider.maxValue - mouseSensitivitySlider.minValue)
                * (1 - 0) + 0; //(value - from1) / (to1 - from1) * (to2 - from2) + from2
        }
    }
}