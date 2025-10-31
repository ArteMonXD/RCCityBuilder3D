using UnityEngine;
using UnityEngine.UIElements;
using R3;
using VContainer;
using Domain.Enums;

namespace Presentation.Application.Views
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private VisualElement _root;
        private Label _goldText;
        private VisualElement _buildingInfoPanel;
        private Label _buildingName;
        private Label _buildingLevel;

        public ReactiveCommand<BuildingType> OnBuildingSelected { get; } = new();
        public ReactiveCommand<BuildingAction> OnBuildingAction { get; } = new();
        public ReactiveCommand<SystemAction> OnSystemAction { get; } = new();

        private void Start()
        {
            _root = uiDocument.rootVisualElement;
            InitializeElements();
            RegisterCallbacks();
        }

        private void InitializeElements()
        {
            _goldText = _root.Q<Label>("gold-text");
            _buildingInfoPanel = _root.Q<VisualElement>("building-info");
            _buildingName = _root.Q<Label>("building-name");
            _buildingLevel = _root.Q<Label>("building-level");
        }

        private void RegisterCallbacks()
        {
            //  нопки строительства
            _root.Q<Button>("house-button").clicked += () => OnBuildingSelected.Execute(BuildingType.House);
            _root.Q<Button>("farm-button").clicked += () => OnBuildingSelected.Execute(BuildingType.Farm);
            _root.Q<Button>("mine-button").clicked += () => OnBuildingSelected.Execute(BuildingType.Mine);

            //  нопки действий с здани€ми
            _root.Q<Button>("upgrade-button").clicked += () => OnBuildingAction.Execute(BuildingAction.Upgrade);
            _root.Q<Button>("move-button").clicked += () => OnBuildingAction.Execute(BuildingAction.Move);
            _root.Q<Button>("remove-button").clicked += () => OnBuildingAction.Execute(BuildingAction.Remove);

            // —истемные кнопки
            _root.Q<Button>("save-button").clicked += () => OnSystemAction.Execute(SystemAction.Save);
            _root.Q<Button>("load-button").clicked += () => OnSystemAction.Execute(SystemAction.Load);
        }

        public void UpdateGold(int gold)
        {
            _goldText.text = gold.ToString();
        }

        public void ShowBuildingInfo(string buildingName, int level)
        {
            _buildingInfoPanel.style.display = DisplayStyle.Flex;
            _buildingName.text = buildingName;
            _buildingLevel.text = $"Level: {level}";
        }

        public void HideBuildingInfo()
        {
            _buildingInfoPanel.style.display = DisplayStyle.None;
        }
    }

    public enum BuildingAction { Upgrade, Move, Remove }
    public enum SystemAction { Save, Load }
}