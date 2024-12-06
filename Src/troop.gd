extends Node2D

var Attack: int = 2
var Health: int = 2
@onready var troop: Node2D = $"."
var troopPlayed = Globals.troopPlayed

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	remove_child(troop)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if troopPlayed == true:
		add_child(troop)
