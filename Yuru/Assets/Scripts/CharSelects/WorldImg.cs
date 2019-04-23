using System;
using Systems.Chars;
using UnityEngine;
using UnityEngine.UI;

namespace CharSelects {
    public class WorldImg : MonoBehaviour {
        [SerializeField] private Image[] ameimg;
        [SerializeField] private Image[] egyimg;
        [SerializeField] private Image[] chilimg;
        [SerializeField] private Image[] japimg;

        public void OnSelect(CharName name, int player) {
            switch (name) {
                case CharName.AmericanHero:
                    ameimg[player].enabled = true;
                    break;
                case CharName.MJ:
                    egyimg[player].enabled = true;
                    break;
                case CharName.Moaian:
                    chilimg[player].enabled = true;
                    break;
                case CharName.Osushi:
                    japimg[player].enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(name), name, null);
            }
        }

        public void Remove(CharName name, int player) {
            switch (name) {
                case CharName.AmericanHero:
                    ameimg[player].enabled = false;
                    break;
                case CharName.MJ:
                    egyimg[player].enabled = false;
                    break;
                case CharName.Moaian:
                    chilimg[player].enabled = false;
                    break;
                case CharName.Osushi:
                    japimg[player].enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(name), name, null);
            }
        }
    }
}
