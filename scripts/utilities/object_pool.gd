extends Node
## Object Pool for reusable game objects
## Reduces memory allocation and garbage collection overhead
## Use this for frequently spawned/destroyed objects like particles, projectiles, enemies

class_name ObjectPool

var _pool: Array = []
var _scene: PackedScene
var _initial_size: int = 10
var _parent: Node = null

## Initialize the pool with a scene and initial size
func initialize(scene: PackedScene, initial_size: int = 10, parent: Node = null) -> void:
	_scene = scene
	_initial_size = initial_size
	_parent = parent if parent != null else self
	
	# Validate parent is in scene tree
	if _parent and not _parent.is_node_ready():
		push_warning("ObjectPool: parent node is not in scene tree, using self as parent")
		_parent = self
	
	# Pre-populate the pool
	for i in range(_initial_size):
		var instance = _scene.instantiate()
		instance.set_process(false)
		instance.set_physics_process(false)
		instance.visible = false
		_parent.add_child(instance)
		_pool.append(instance)


## Get an object from the pool
func get_object() -> Node:
	var obj: Node = null
	
	if _pool.is_empty():
		# Pool is empty, create new instance
		if _parent == null or not is_instance_valid(_parent):
			push_error("ObjectPool: parent node is no longer valid, cannot create new instance")
			return null
		obj = _scene.instantiate()
		_parent.add_child(obj)
	else:
		# Reuse object from pool
		obj = _pool.pop_back()
		# Validate popped object is a valid Node
		if obj == null or not is_instance_valid(obj):
			push_warning("ObjectPool: popped object is invalid, creating new instance")
			if _parent != null and is_instance_valid(_parent):
				obj = _scene.instantiate()
				_parent.add_child(obj)
			else:
				push_error("ObjectPool: cannot create replacement, parent is invalid")
				return null
	
	# Activate the object
	obj.set_process(true)
	obj.set_physics_process(true)
	obj.visible = true
	
	return obj


## Return an object to the pool
func return_object(obj: Node) -> void:
	if obj == null or not is_instance_valid(obj):
		return
	
	# Deactivate the object
	obj.set_process(false)
	obj.set_physics_process(false)
	obj.visible = false
	
	# Reset transform
	if obj is Node3D:
		obj.global_position = Vector3.ZERO
		obj.rotation = Vector3.ZERO
	elif obj is Node2D:
		obj.global_position = Vector2.ZERO
		obj.rotation = 0.0
	
	# Add back to pool
	_pool.append(obj)


## Clear the pool and free all objects
func clear() -> void:
	for obj in _pool:
		if is_instance_valid(obj):
			obj.queue_free()
	_pool.clear()


## Get the current pool size
func get_pool_size() -> int:
	return _pool.size()


## Get total managed objects (pool + active)
func get_total_objects() -> int:
	var active_count = 0
	if _parent:
		for child in _parent.get_children():
			if child is Node and child.visible:
				active_count += 1
	return _pool.size() + active_count
