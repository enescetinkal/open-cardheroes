extends Area2D
var mouseInCollision = false

func _ready():
	pass
	


func _process(delta: float) -> void:
	#Checks if the mouse is in the card and if the left mouse is clicked.
	if Input.is_action_pressed("mouse_leftB") and mouseInCollision == true :
		position = get_global_mouse_position() +- Vector2(55, 40)

# Changes a variale if the mouse is in or out of the PlaceHolderRect.
func _on_placeholder_rect_mouse_entered() -> void:
	mouseInCollision = true
	
func _on_placeholder_rect_mouse_exited() -> void:
	mouseInCollision = false
