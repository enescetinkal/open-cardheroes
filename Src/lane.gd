extends Area2D

@export var isWater = false
@export var isHeights = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	# Changes the lane color according to the type of lane type selected.
	if isWater == true :
		$PlaceholderRect.color = Color(0,0,0.75,1)
	if isHeights == true :
		$PlaceholderRect.color = Color(0.75,0.25,0,1)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
