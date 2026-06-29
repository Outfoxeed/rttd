using System;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class BuildingComponent : UnitOrderComponentVisitor
{
    public enum State { MissingResources, InConstruction, Constructed }
    public State CurrentState { get; private set; } = State.MissingResources;
    public float CurrentWorkRatio => _currentWorkRatio; 

    [Export] private ResourcesData _buildingCost;
    [Export] private int _neededWorkAmount = 10;
    private float _currentWorkAmount = 0f;
    private float _currentWorkRatio = 0f;

    [Export] private Slider _progressSlider;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        IEntityComponent[] entityComponents = GetEntity().GetAllComponents();
        foreach (IEntityComponent entityComponent in entityComponents)
        {
            if (entityComponent == this)
                continue;
            
            entityComponent.Enabled = false;
        }
        GetEntity().Modulate = Colors.Black;

        if (_progressSlider.IsValid())
        {
            _progressSlider.Visible = false;
            _progressSlider.MinValue = 0f;
            _progressSlider.MaxValue = 1f;
        }
    }

    public bool TryGetMissingResources()
    {
        if (CurrentState is not State.MissingResources)
            return false;

        bool success = PlayerResourcesManager.Instance.Resources.TryBuy(_buildingCost);
        if(success) CurrentState = State.InConstruction;
        return success;
    }
    
    public bool AddConstructionProgress(float workAmount)
    {
        if (CurrentState is not State.InConstruction)
            return false;
        
        _currentWorkAmount += workAmount;
        _currentWorkRatio = Math.Clamp(_currentWorkAmount / _neededWorkAmount, 0f, 1f);
        GetEntity().Modulate = Colors.Black.Lerp(Colors.White, _currentWorkRatio);
        if (_progressSlider.IsValid())
        {
            _progressSlider.Value = _currentWorkRatio;
            _progressSlider.Visible = true;
        }
        
        if (_currentWorkAmount < _neededWorkAmount)
            return false;
        
        CurrentState = State.Constructed;
        IEntityComponent[] entityComponents = GetEntity().GetAllComponents();
        foreach (IEntityComponent entityComponent in entityComponents)
        {
            entityComponent.Enabled = true;
        }
        if (_progressSlider.IsValid()) _progressSlider.Visible = false;
        return true;
    }
    
    public override bool CanVisitImpl(UnitComponent target, OrderMode orderMode)
    {
        if (!target.GetEntity().HasComponent<WorkerComponent>())
            return false;

        switch (CurrentState)
        {
            case State.MissingResources:
                return HasPlayerEnoughResources();
            case State.InConstruction:
                return true;
            case State.Constructed:
                return false;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override bool TryVisitImpl(UnitComponent target, OrderMode orderMode)
    {
        target.QueueCommand(new CompositeUnitCommand([
            new MoveToEntityCommand(GetEntity()),
            new BuildCommand(this)
        ]), orderMode);
        return true;
    }

    private bool HasPlayerEnoughResources()
    {
        PlayerResourcesManager playerResourcesManager = PlayerResourcesManager.Instance;
        return playerResourcesManager.Resources.HasResources(_buildingCost);
    }
}