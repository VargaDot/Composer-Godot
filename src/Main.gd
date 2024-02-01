extends Node2D

func _ready():
	ComposerGD.AddScene("MainMenu", "res://src/main_menu.tscn", {
		"instant_load":true,
		"instant_create":true,
		"scene_parent":self
	})

	await Composer.SceneLoaded
	print("Scene MainMenu has been loaded.")

	await Composer.SceneCreated
	print("Scene MainMenu has been created.")
