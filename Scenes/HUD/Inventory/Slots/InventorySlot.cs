using Godot;
using System;

public partial class InventorySlot : CenterContainer
{
	
	// Actions for item selection
	public enum InventorySlotAction{
		ACTION,
	}

	// Properties
	public int _number = 0;

	// Childs Variables
	private Sprite2D _icon;
	private Label _itemNumber;
	public string _name;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("InventorySlots");
		_icon = GetNode<Sprite2D>("TextureButton/Texture");
		_itemNumber = GetNode<Label>("TextureButton/Label");
		
		// Initial actions
		_itemNumber.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Function that Writes number of Items
	public void IncreaseNumber()
	{	
		_number += 1;
		if (_itemNumber.Visible == false){
			_itemNumber.Visible = true;
		}
		_itemNumber.Text = _number.ToString();
	}

	public void DecreaseNumber()
	{
		_number -= 1;
		if (_number == 0){
			_itemNumber.Visible = false;
			TakeItemOff();
		}
		_itemNumber.Text = _number.ToString();
	}

	// Function that gets the name of a item
	public void GetItemName(string name)
	{
		_name = name;
	}

	// Function that reads and puts the item texture
	public void GetTexture(Texture2D texture)
	{
		_icon.Texture = texture;
	}

	// Function that takes off a item of inventory
	public void TakeItemOff()
	{
		_icon.Texture = null;
		_name = "";
	}

	// Function that returns true if the slot is empty and false otherwise
	public bool IsEmpty()
	{
		return _icon.Texture == null;
	}
	

}
