using System.Collections.Generic;
using System.Text;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class RTSSelectionSystem : Node2D
{
    public IReadOnlyList<UnitComponent> Selected => _selected;
    
    private Rect2 _selectionRect;
    private bool _selecting;
    private readonly List<UnitComponent> _selected = new();

    [Export] private Node2D _selectionVisual;
    [Export] private float _selectionVisualBaseSize = 64f;

    public override void _EnterTree()
    {
        base._EnterTree();
        _selectionVisual.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } mouseButton)
        {
            if (mouseButton.Pressed)
            {
                _selectionRect = new Rect2(GetGlobalMousePosition(), Vector2.Zero).Grow(8);
                UpdateSelectedList();
                if (_selected.Count > 0)
                {
                    while (_selected.Count > 1)
                    {
                        _selected.RemoveAt(_selected.Count - 1);
                    }
                    PrintSelectedUnits();
                }
                else
                {
                    _selecting = true;
                    _selectionRect.Position = GetGlobalMousePosition();
                    _selectionRect.Size = Vector2.Zero;
                    _selectionVisual.Visible = true;
                }
                
                GetViewport().SetInputAsHandled();
            }
            else if (_selecting)
            {
                _selecting = false;
                _selectionRect.End = GetGlobalMousePosition();
                UpdateSelectedList();
                _selectionVisual.Visible = false;

                GetViewport().SetInputAsHandled();
                PrintSelectedUnits();
            }
        }
        else if (@event is InputEventMouseMotion && _selecting)
        {
            _selectionRect.End = GetGlobalMousePosition();
            UpdateSelectedList();
            
            GetViewport().SetInputAsHandled();
        }
    }

    private void PrintSelectedUnits()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Selected ({_selected.Count}): ");
        foreach (UnitComponent rtsEntityComponent in _selected)
        {
            sb.Append(rtsEntityComponent.GetEntity().Name);
            sb.Append(", ");
        }
        GD.Print(sb.ToString());
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_selecting)
        {
            _selectionVisual.GlobalPosition = _selectionRect.Position;
            _selectionVisual.Scale = _selectionRect.Size / _selectionVisualBaseSize;
        }
    }

    private void UpdateSelectedList()
    {
        _selected.Clear();
        foreach (UnitComponent entityComponent in UnitComponent.AllUnits)
        {
            if (_selectionRect.Abs().Intersects(entityComponent.GetEntity().GetWorldRect()))
            {
                _selected.Add(entityComponent);
            }
        }
    }
}