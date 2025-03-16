using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class Inventory : Control
{
	// Instanciatable scenes
	[Export] public PackedScene InventorySlotScene;

	// Declaring properties
	public int cols = 3;

	// Declaring list of slots
	public List<InventorySlot> _slots = new List<InventorySlot>();
	private int _size;
	public int _currentPosition = 0;
	
	// Childs
	private CenterContainer _slotSpace;
	private CustomSignals _customSignals; // custom signal variable

    public override void _Ready()
    {
		// Getting childs
		_slotSpace = GetNode<CenterContainer>("SlotSpace");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.GotItem += OnGotItem;

        // initializing array of slots
		for (int i = 0; i < cols; i++){
			// Appending in slots
			var newSlot = (InventorySlot)InventorySlotScene.Instantiate();
			_slotSpace.AddChild(newSlot);
			newSlot.Hide();
			_slots.Add(newSlot);
		}
		_size = _slots.Count;
		
    }

    public override void _Process(double delta)
    {
		ScrollList();
		ShowCurrentItem();
		
    }

    // Function that scrolls through list
    private void ScrollList()
	{
		int i = 0;
		if (Input.IsActionJustPressed("ui_changeItem")){
			i++;
		}
		_currentPosition = (_currentPosition + i)%_size;
		return;
	}

	// Function that shows the current Item
	private void ShowCurrentItem()
	{
		for (int i = 0; i < _size; i++){
			if (i == _currentPosition){
				_slots[i].Visible = true;
			}
			else{
				_slots[i].Visible = false;
			}
		}
		return;
	}

	// Callback Function when player collect a item
	private void OnGotItem(Area2D item)
	{
		// identifying item type
		GD.Print(item.Name);
		if (item is HealthPotion potion){
			// Updating slots data
			if (potion._isStackable){
				_slots[_currentPosition].IncreaseNumber();
			}
			_slots[_currentPosition].GetItemName(potion._name);
			_slots[_currentPosition].GetTexture(potion._icon);
		}
	}
	
}
