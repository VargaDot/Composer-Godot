extends Node2D

func _ready():
	ComposerGD.AddScene("MainMenu", "res://src/main_menu.tscn", {
		"instant_load":true,
		"instant_create":true,
		"scene_parent":self
	})

	await ComposerGD.SceneLoaded
	print("Scene MainMenu has been loaded.")

	await ComposerGD.SceneCreated
	print("Scene MainMenu has been created.")
