using Godot;
using System;

public partial class InventorySlot : CenterContainer
{
	// Exporting a packed scene so item can be instanciated 
	[Export] public PackedScene InventoryItemScene;

	// _hintItem is most like a restriction to the slot to only accept the type
	// of item represented by itself
	[Export] public InventoryItem _item;

	// Actions for item selection
	public enum InventorySlotAction{
		SELECT,
	}

	[Signal] public delegate void SlotInputEventHandler(InventorySlot which, InventorySlotAction action);
	[Signal] public delegate void SlotHoveredEventHandler(InventorySlot which, bool isHovering); 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("InventorySlots");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Callback function when the slot is pressed
	private void OnTextureButtonGuiInput(InputEvent Event)
	{
		if (Event is InputEventMouseButton _mouseEvent && Event.IsPressed()){
			if (_mouseEvent.ButtonIndex == MouseButton.Left){
				var action = InventorySlotAction.SELECT;
				EmitSignal(SignalName.SlotInput, this, (int)action);
			}
		}
	}

	// Callback function on mouse entered the button
	private void OnTextureButtonMouseEntered()
	{
		EmitSignal(SignalName.SlotHovered, this, true);
	}

	// Callback function on mouse exited the button
	private void OnTextureButtonMouseExited()
	{
		EmitSignal(SignalName.SlotHovered, this, false);
	}



}
