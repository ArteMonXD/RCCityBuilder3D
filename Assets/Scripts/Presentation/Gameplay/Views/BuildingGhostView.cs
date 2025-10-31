using Domain.Enums;
using Domain.Models;
using R3;
using UnityEngine;

namespace Presentation.Gameplay.Views
{
    public class BuildingGhostView : MonoBehaviour
    {
        [SerializeField] private GameObject ghostPrefab;
        [SerializeField] private Material validMaterial;
        [SerializeField] private Material invalidMaterial;

        private GameObject _currentGhost;
        private Renderer _ghostRenderer;
        private BuildingType _currentType;

        public void ShowGhost(BuildingType buildingType, GridPosition position, bool isValid)
        {
            if (_currentGhost == null || _currentType != buildingType)
            {
                DestroyCurrentGhost();
                CreateGhost(buildingType);
            }

            var worldPos = new Vector3(position.X, 0, position.Z);
            _currentGhost.transform.position = worldPos;

            _ghostRenderer.material = isValid ? validMaterial : invalidMaterial;
            _currentGhost.SetActive(true);
        }

        public void HideGhost()
        {
            if (_currentGhost != null)
            {
                _currentGhost.SetActive(false);
            }
        }

        private void CreateGhost(BuildingType buildingType)
        {
            _currentGhost = Instantiate(ghostPrefab);
            _ghostRenderer = _currentGhost.GetComponent<Renderer>();
            _currentType = buildingType;

            // Настраиваем внешний вид в зависимости от типа здания
            var scale = GetBuildingScale(buildingType);
            _currentGhost.transform.localScale = scale;
        }

        private void DestroyCurrentGhost()
        {
            if (_currentGhost != null)
            {
                Destroy(_currentGhost);
                _currentGhost = null;
                _ghostRenderer = null;
            }
        }

        private Vector3 GetBuildingScale(BuildingType type) => type switch
        {
            BuildingType.House => new Vector3(1, 1, 1),
            BuildingType.Farm => new Vector3(2, 1, 2),
            BuildingType.Mine => new Vector3(1.5f, 1.5f, 1.5f),
            _ => Vector3.one
        };
    }
}
