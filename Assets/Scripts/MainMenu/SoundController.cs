using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EducaNuclear
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("VolumeMaster"))
            {
                volumeSlider.value = PlayerPrefs.GetFloat("VolumeMaster");
            }
        }

        public void VolumeMaster(float volume)
        {
            PlayerPrefs.SetFloat("VolumeMaster", volume);
            AudioListener.volume = volume;
        }
    }
}
