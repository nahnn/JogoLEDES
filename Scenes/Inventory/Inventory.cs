using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Control
{
	private InventoryItem _inventoryItemScene;

	// Declaring rows and cols
	public int rows = 1;
	public int cols = 3;

	private GridContainer _inventoryGrid;

	[Export] public PackedScene InventorySlotScene;
	private List<InventorySlot> _slots;

    public override void _Ready()
    {
        // initializing array of slots
		
    }
}
