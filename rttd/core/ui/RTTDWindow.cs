using System;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class RTTDWindow : Control
{
    public Action<RTTDWindow> OnClosed;
    public Action<RTTDWindow, bool> OnFolded;

    [Export] private Control _mainBar;
    [Export] private BaseButton _closeButton;
    [Export] private bool _foldable;
    [Export] private BaseButton _foldButton;
    [Export] private Control _contentContainer;

    private bool _folded = false;
    private bool _isMouseHoveringMainBar = false;
    private bool _isDragging;
    private Vector2 _dragOffset = Vector2.Zero;

    public override void _EnterTree()
    {
        base._EnterTree();

        if (_mainBar is not null)
        {
            _mainBar.MouseEntered += OnMainBarMouseEntered;
            _mainBar.MouseExited += OnMainBarMouseExited;
        }
        if (_closeButton is not null) _closeButton.Pressed += OnCloseButtonPressed;
        if (_foldButton is not null)
        {
            _foldButton.SetVisible(_foldable);
            _foldButton.Pressed += OnFoldButtonPressed;
        }

        _folded = false;
    }


    public override void _ExitTree()
    {
        base._ExitTree();
        
        if (_mainBar is not null)
        {
            _mainBar.MouseEntered -= OnMainBarMouseEntered;
            _mainBar.MouseExited -= OnMainBarMouseExited;
        }
        if (_closeButton is not null) _closeButton.Pressed -= OnCloseButtonPressed;
        if(_foldButton is not null) _foldButton.Pressed -= OnFoldButtonPressed;
    }

    public override void _GuiInput(InputEvent @event)
    {
        base._GuiInput(@event);
        
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } eventMouseButton)
        {
            if (!_isDragging && _isMouseHoveringMainBar)
            {
                _isDragging = eventMouseButton.IsPressed();
                _dragOffset = GetViewport().GetMousePosition() - GetPosition();
                GD.Print($"{_dragOffset} // mousePOs:{GetViewport().GetMousePosition()} // position:{GetPosition()}");
                GetViewport().SetInputAsHandled();
                return;
            }
            
            if (_isDragging && !eventMouseButton.IsPressed())
            {
                _isDragging = false;
                GetViewport().SetInputAsHandled();
                return;
            }
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_isDragging)
        {
            Rect2 visibleRect = GetViewport().GetVisibleRect();
            Vector2 newPosition = GetViewport().GetMousePosition() - _dragOffset;
            newPosition.X = Math.Clamp(newPosition.X, 0, visibleRect.End.X - Size.X);
            newPosition.Y = Math.Clamp(newPosition.Y, 0, visibleRect.End.Y - Size.Y);
            SetPosition(newPosition);
        }
    }

    public void Close()
    {
        SetVisible(false);
        QueueFree();
        
        OnClosed?.Invoke(this);
    }

    private void Fold()
    {
        _folded = !_folded;
        _contentContainer?.SetVisible(!_folded);
        
        OnFolded?.Invoke(this, _folded);
    }

    private void OnFoldButtonPressed() => Fold();
    private void OnCloseButtonPressed() => Close();

    private void OnMainBarMouseExited() => _isMouseHoveringMainBar = false;
    private void OnMainBarMouseEntered() => _isMouseHoveringMainBar = true;
}
