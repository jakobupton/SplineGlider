extends Node3D

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	# Enable processing every frame
	set_process(true)

# Called every frame
func _process(_delta) -> void:
	# Check if the "Esc" key is pressed
	if Input.is_action_pressed("ui_cancel"):
		get_tree().quit()
