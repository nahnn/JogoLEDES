using Godot;
using System;

public partial class CustomSignals : Node
{
	// Declaring Signals
	// Signal emitted when player got a Item
	[Signal] public delegate void GotItemEventHandler(Area2D item);
	[Signal] public delegate void DieEventHandler();
}
