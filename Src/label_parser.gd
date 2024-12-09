extends Label


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func card_parsed(info):
	pass


func _on_card_card_parsed(info) -> void:
	text = str(info["stats"]["cost"]) + " cost " + str(info["stats"]["attack"]) + "/" + str(info["stats"]["health"])
	for i in info["stats"]["tribes"]:
		add_to_group(i)
