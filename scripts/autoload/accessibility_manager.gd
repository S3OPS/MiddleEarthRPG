extends Node

## AccessibilityManager
## Manages accessibility settings and options

signal accessibility_setting_changed(setting_name: String, value)

# Accessibility settings
var settings = {
	# Visual
	"colorblind_mode": "none",  # none, protanopia, deuteranopia, tritanopia
	"high_contrast": false,
	"text_size": 1.0,  # 0.8 - 1.5
	"subtitles_enabled": true,
	"subtitles_size": 1.0,
	"reduce_motion": false,
	"screen_shake": true,
	
	# Audio
	"master_volume": 1.0,
	"music_volume": 0.8,
	"sfx_volume": 1.0,
	"voice_volume": 1.0,
	"audio_cues_enabled": false,  # Additional audio feedback for important events
	
	# Input
	"mouse_sensitivity": 1.0,
	"gamepad_sensitivity": 1.0,
	"invert_y_axis": false,
	"hold_to_sprint": false,  # false = toggle sprint
	"auto_aim_assist": false,
	"button_remapping": {},
	
	# Gameplay
	"auto_save_frequency": 300,  # seconds (5 minutes)
	"combat_auto_lock": false,
	"damage_numbers": true,
	"quest_markers": true,
	"tutorial_hints": true
}

func _ready() -> void:
	print("♿ Accessibility Manager initialized")

func set_setting(setting_name: String, value) -> void:
	"""Set an accessibility setting"""
	if setting_name not in settings:
		push_error("Unknown accessibility setting: " + setting_name)
		return
	
	settings[setting_name] = value
	accessibility_setting_changed.emit(setting_name, value)
	_apply_setting(setting_name, value)
	
	print("Accessibility setting changed: ", setting_name, " = ", value)

func get_setting(setting_name: String):
	"""Get an accessibility setting"""
	return settings.get(setting_name, null)

func _apply_setting(setting_name: String, value) -> void:
	"""Apply a setting change immediately"""
	match setting_name:
		"colorblind_mode":
			_apply_colorblind_mode(value)
		"high_contrast":
			_apply_high_contrast(value)
		"text_size":
			_apply_text_size(value)
		"reduce_motion":
			_apply_reduce_motion(value)
		"screen_shake":
			_apply_screen_shake(value)
		"master_volume":
			_apply_volume("Master", value)
		"music_volume":
			_apply_volume("Music", value)
		"sfx_volume":
			_apply_volume("SFX", value)
		"voice_volume":
			_apply_volume("Voice", value)
		"mouse_sensitivity":
			_apply_mouse_sensitivity(value)
		_:
			pass  # Setting will be used when needed

func _apply_colorblind_mode(mode: String) -> void:
	"""Apply colorblind shader/filters"""
	# TODO: Apply shader when rendering system is implemented
	print("Colorblind mode: ", mode)

func _apply_high_contrast(enabled: bool) -> void:
	"""Apply high contrast theme"""
	# TODO: Apply high contrast UI theme
	print("High contrast: ", enabled)

func _apply_text_size(size: float) -> void:
	"""Apply text size scaling"""
	# TODO: Scale all UI text
	print("Text size: ", size)

func _apply_reduce_motion(enabled: bool) -> void:
	"""Reduce or disable motion effects"""
	# TODO: Disable camera shake, reduce animations
	print("Reduce motion: ", enabled)

func _apply_screen_shake(enabled: bool) -> void:
	"""Enable or disable screen shake"""
	# TODO: Control camera shake effects
	print("Screen shake: ", enabled)

func _apply_volume(bus_name: String, volume: float) -> void:
	"""Apply audio volume to bus"""
	var bus_idx = AudioServer.get_bus_index(bus_name)
	if bus_idx >= 0:
		AudioServer.set_bus_volume_db(bus_idx, linear_to_db(volume))

func _apply_mouse_sensitivity(sensitivity: float) -> void:
	"""Apply mouse sensitivity"""
	# TODO: Update player camera controller
	print("Mouse sensitivity: ", sensitivity)

# Preset configurations
func apply_preset(preset_name: String) -> void:
	"""Apply a preset configuration"""
	match preset_name:
		"high_visibility":
			set_setting("high_contrast", true)
			set_setting("text_size", 1.3)
			set_setting("subtitles_size", 1.3)
			set_setting("damage_numbers", true)
			set_setting("quest_markers", true)
		"low_vision":
			set_setting("text_size", 1.5)
			set_setting("subtitles_size", 1.5)
			set_setting("audio_cues_enabled", true)
			set_setting("high_contrast", true)
		"motion_sensitive":
			set_setting("reduce_motion", true)
			set_setting("screen_shake", false)
			set_setting("mouse_sensitivity", 0.7)
		"easy_controls":
			set_setting("hold_to_sprint", false)
			set_setting("auto_aim_assist", true)
			set_setting("combat_auto_lock", true)
		_:
			print("Unknown preset: ", preset_name)

func reset_to_defaults() -> void:
	"""Reset all settings to defaults"""
	settings = {
		"colorblind_mode": "none",
		"high_contrast": false,
		"text_size": 1.0,
		"subtitles_enabled": true,
		"subtitles_size": 1.0,
		"reduce_motion": false,
		"screen_shake": true,
		"master_volume": 1.0,
		"music_volume": 0.8,
		"sfx_volume": 1.0,
		"voice_volume": 1.0,
		"audio_cues_enabled": false,
		"mouse_sensitivity": 1.0,
		"gamepad_sensitivity": 1.0,
		"invert_y_axis": false,
		"hold_to_sprint": false,
		"auto_aim_assist": false,
		"button_remapping": {},
		"auto_save_frequency": 300,
		"combat_auto_lock": false,
		"damage_numbers": true,
		"quest_markers": true,
		"tutorial_hints": true
	}
	
	# Apply all defaults
	for setting_name in settings:
		_apply_setting(setting_name, settings[setting_name])
	
	print("Reset all accessibility settings to defaults")

func save_data() -> Dictionary:
	"""Save accessibility settings"""
	return {
		"settings": settings
	}

func load_data(data: Dictionary) -> void:
	"""Load accessibility settings"""
	var saved_settings = data.get("settings", {})
	
	for setting_name in saved_settings:
		if setting_name in settings:
			settings[setting_name] = saved_settings[setting_name]
			_apply_setting(setting_name, settings[setting_name])
