extends Node
## Performance Monitor Singleton for tracking FPS and system metrics
## Access via PerformanceMonitor.function_name()

var _fps_history: Array[float] = []
var _fps_history_size: int = 60  # 1 second at 60fps
var _fps_history_index: int = 0
var _frame_time_history: Array[float] = []  # Type annotation added
var _fps_sum: float = 0.0
var _frame_time_sum: float = 0.0
var _samples_since_recalc: int = 0

# Cached metrics
var average_fps: float = 60.0
var min_fps: float = 60.0
var max_fps: float = 60.0
var average_frame_time: float = 0.016  # ~60fps


func _ready() -> void:
	print("📊 PerformanceMonitor initialized")


func _process(delta: float) -> void:
	_update_metrics(delta)


## Update performance metrics
func _update_metrics(delta: float) -> void:
	var current_fps = Engine.get_frames_per_second()
	
	# Add to history
	if _fps_history.size() < _fps_history_size:
		_fps_history.append(current_fps)
		_frame_time_history.append(delta)
	else:
		_fps_history[_fps_history_index] = current_fps
		_frame_time_history[_fps_history_index] = delta
		_fps_history_index = (_fps_history_index + 1) % _fps_history_size
	
	# Calculate metrics
	if _fps_history.size() > 0:
		if _samples_since_recalc >= _fps_history_size:
			_recalculate_sums()
			_samples_since_recalc = 0
		var history_size = _fps_history.size()
		
		# Division by zero check before calculating averages
		if history_size > 0:
			average_fps = _fps_sum / history_size
			average_frame_time = _frame_time_sum / history_size
		
		# Manual min calculation (Array.min() doesn't exist in GDScript)
		var min_val = _fps_history[0]
		for i in range(_fps_history.size()):
			if _fps_history[i] < min_val:
				min_val = _fps_history[i]
		min_fps = min_val
		
		# Manual max calculation (Array.max() doesn't exist in GDScript)
		var max_val = _fps_history[0]
		for i in range(_fps_history.size()):
			if _fps_history[i] > max_val:
				max_val = _fps_history[i]
		max_fps = max_val


## Recalculate rolling sums to reduce floating-point drift
func _recalculate_sums() -> void:
	_fps_sum = 0.0
	_frame_time_sum = 0.0
	for index in range(_fps_history.size()):
		_fps_sum += _fps_history[index]
		_frame_time_sum += _frame_time_history[index]


## Get formatted performance stats
func get_stats_string() -> String:
	return "FPS: %.1f (avg: %.1f, min: %.1f, max: %.1f) | Frame: %.2fms" % [
		Engine.get_frames_per_second(),
		average_fps,
		min_fps,
		max_fps,
		average_frame_time * 1000.0
	]


## Get memory usage in MB
func get_memory_usage_mb() -> float:
	return Performance.get_monitor(Performance.MEMORY_STATIC) / 1024.0 / 1024.0


## Get formatted memory string
func get_memory_string() -> String:
	return "Memory: %.1f MB" % get_memory_usage_mb()


## Check if performance is good (above 30 FPS)
func is_performance_good() -> bool:
	return average_fps >= 30.0


## Check if performance is excellent (above 55 FPS)
func is_performance_excellent() -> bool:
	return average_fps >= 55.0


## Get all Godot performance metrics
func get_detailed_metrics() -> Dictionary:
	return {
		"fps": Engine.get_frames_per_second(),
		"average_fps": average_fps,
		"min_fps": min_fps,
		"max_fps": max_fps,
		"frame_time_ms": average_frame_time * 1000.0,
		"memory_static_mb": Performance.get_monitor(Performance.MEMORY_STATIC) / 1024.0 / 1024.0,
		"memory_dynamic_mb": Performance.get_monitor(Performance.MEMORY_DYNAMIC) / 1024.0 / 1024.0,
		"objects_in_scene": Performance.get_monitor(Performance.OBJECT_COUNT),
		"nodes_in_scene": Performance.get_monitor(Performance.OBJECT_NODE_COUNT),
		"draw_calls": Performance.get_monitor(Performance.RENDER_TOTAL_DRAW_CALLS_IN_FRAME),
		"physics_2d_active_objects": Performance.get_monitor(Performance.PHYSICS_2D_ACTIVE_OBJECTS),
		"physics_3d_active_objects": Performance.get_monitor(Performance.PHYSICS_3D_ACTIVE_OBJECTS),
	}
