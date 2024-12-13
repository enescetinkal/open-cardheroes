extends Node2D

@onready var troop: Node2D = $Troop

var troopPlayed = Globals.troopPlayed

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	remove_child(troop)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if troopPlayed == {"1stCard": true}: 
		add_child(troop)
