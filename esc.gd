extends Node3D

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	# Enable processing every frame
	set_process(true)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	# Check if the "key_exit" action is pressed
	if Input.is_action_pressed("Esc"):
		get_tree().quit()
