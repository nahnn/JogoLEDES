using Godot;
using System;

public partial class Inventory : Control
{
	// Declaring items properties
	public int _healthPotionAmount = 0;

	// Childs variables
	private TextureRect _itemsMenu;
	private Sprite2D _item1Image;
	private Sprite2D _item2Image;
	private Sprite2D _item3Image;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Getting childs
		_itemsMenu = GetNode<TextureRect>("ColorRect/ItemsMenu");
		_item1Image = _itemsMenu.GetNode<Sprite2D>("HealthPotion/Image");
		_item2Image = _itemsMenu.GetNode<Sprite2D>("Item2/Image");
		_item3Image = _itemsMenu.GetNode<Sprite2D>("Item3/Image");

		// Setting initial behaviours
		_itemsMenu.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
