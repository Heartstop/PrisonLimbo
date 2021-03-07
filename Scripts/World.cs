using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using Godot;
using PrisonLimbo.Scripts;

public class World : TileMap
{
	public override void _Ready()
	{
		
	}

	public bool CanMove<T>(T requestingEntity, Direction direction) where T : WorldEntity
	{
		var newPos = requestingEntity.MapPosition + direction.ToVector2();
		return CanMove(requestingEntity, newPos);
	}
	
	public bool CanMove<T>(T requestingEntity, Vector2 target) where T : WorldEntity
	{
		var startPos = requestingEntity.MapPosition;
		var cellIndex = GetCellv(target);
		if (cellIndex != default)
			return false;

		return GetEntities(target)
			.All(we => we.CanEnter(requestingEntity));
	}

	public IEnumerable<WorldEntity> GetEntities(Vector2 position)
	{
		return GetChildren()
			.OfType<WorldEntity>()
			.Where(we => we.MapPosition == position);
	}
}
