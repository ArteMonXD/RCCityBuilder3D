using Domain.Models;
using R3;
using UnityEngine;

namespace Presentation.Gameplay.Views
{
    public class GridCellTrigger : MonoBehaviour
    {
        private GridPosition _position;
        private Subject<GridPosition> _clickSubject;

        public void Initialize(GridPosition position, Subject<GridPosition> clickSubject)
        {
            _position = position;
            _clickSubject = clickSubject;
        }

        private void OnMouseDown()
        {
            _clickSubject.OnNext(_position);
        }
    }
}
