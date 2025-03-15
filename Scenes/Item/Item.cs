using Godot;
using System;
using System.Reflection.Metadata;

public partial class Item : Node
{
	// Exporting commum properties of all items
	public string _name = "";
	public Texture2D _icon = null;
	public bool _isStackable = false;

	// Custom signal
	private CustomSignals _customSignals;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Items");

		// Getting the custom signal reff and connecting event
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.GotItem += HandleGotItem;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Callback Function of GotItem event
	private void HandleGotItem(Area2D item)
	{
		// Identifying item type
		if (item is HealthPotion healthPotion){
			// Getting item properties
			_name = healthPotion._name;
			_icon = healthPotion._icon;
			_isStackable = healthPotion._isStackable;
			GD.Print(_name, _icon, _isStackable);
			
		}
		else{
			_name = "";
		}

	}
}
