extends Node

signal scene_began_loading(scene_name:String)
signal scene_loaded(scene_name:String)
signal scenes_all_loaded()
signal scene_loading_process_updated(scene_name:String, progress:float)
signal scene_created(scene_name:String)
signal scene_enabled(scene_name:String)
signal scene_disabled(scene_name:String)
signal scene_removed(scene_name:String)
signal scene_disposed(scene_name:String)
signal scene_replaced(old_scene_name:String ,new_scene_name:String)

var scene_manifest:Dictionary
