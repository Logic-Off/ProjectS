using UnityEngine;
using UnityEngine.UI;

namespace ProjectDawn.Navigation.Sample.BoardDefense
{
    public class BuildActionBar : MonoBehaviour
    {
        [SerializeField]
        Button m_ArcherTowerButton;

        [SerializeField]
        Button m_BarracksTowerButton;

        [SerializeField]
        Text m_GoldLabel;

        State m_State;

        public bool CheckAndResetButtonState(out State state)
        {
            state = m_State;
            m_State = State.None;
            return state != State.None;
        }

        void Awake()
        {
            m_ArcherTowerButton.onClick.AddListener(() => m_State = State.ArcherTower);
            m_BarracksTowerButton.onClick.AddListener(() => m_State = State.BarracksTower);
        }

        public enum State
        {
            None,
            ArcherTower,
            BarracksTower,
        }
    }
}
