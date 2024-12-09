extends Area2D

@export var card = "res://Src/Cards/dog_basic.json"

signal card_parsed
var parsed_card: Dictionary
var mouseInCollision = false
var cardInLane = false
var cardPlayedAtLane = false
var troopPlayed = Globals.troopPlayed

var health
var attack

func _ready():
	if card:
		var file = FileAccess.open(card, FileAccess.READ)
		var json_text = file.get_as_text()
		file.close()
		var json = JSON.new()
		var result = json.parse(json_text)
		parsed_card = json.get_data() 
		card_parsed.emit(parsed_card)
	
func _process(delta: float) -> void:
	#Checks if the mouse is in the card and if the left mouse is clicked.
	if Input.is_action_pressed("mouse_leftB") and mouseInCollision == true :
		position = get_global_mouse_position() +- Vector2(55, 40)
		print("Card is now being dragged by mouse.")
	
	if cardInLane == true and Input.is_action_just_released("mouse_leftB"):
		print("Card is played at a lane.")
		troopPlayed = true
		queue_free()

# Changes a variale if the mouse is in or out of the PlaceHolderRect.
func _on_placeholder_rect_mouse_entered() -> void:
	mouseInCollision = true
	print("Mouse is in card.")
	
func _on_placeholder_rect_mouse_exited() -> void:
	mouseInCollision = false
	print("Mouse is out of the card.")

# Detect if a card enteres a lane.
func _on_area_entered(area: Area2D) -> void:
	print("Card is in a lane.")
	cardInLane = true

func _on_area_exited(area: Area2D) -> void:
	print("Card is out of a lane.")
	cardInLane = false
