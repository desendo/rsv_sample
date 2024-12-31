using Game.Services;
using UnityEngine;

namespace Game.Rule.Physics
{
    public class UnityPhysics2dUpdateRule
    {
        private readonly GameConfig _gameConfig;
        private readonly PhysicsService _physicsService;

        public UnityPhysics2dUpdateRule(IUpdateProvider updateProvider, GameConfig gameConfig,
            PhysicsService physicsService, HeroService heroService)
        {
            _gameConfig = gameConfig;
            _physicsService = physicsService;
            updateProvider.OnFixedTick.Subscribe(Fixed);
            _physicsService.PhysicsGMultiplier.Subscribe(x => UpdateGravity());
            _physicsService.PhysicsGBase.Subscribe(x => UpdateGravity());
            heroService.Hero.InventoryShown.Subscribe(OnInventoryShown);
        }

        private void OnInventoryShown(bool obj)
        {
            _physicsService.PhysicsGMultiplier.Value = obj ? _gameConfig.InventoryScale : 1;
        }

        private void UpdateGravity()
        {
            var y = _physicsService.PhysicsGMultiplier.Value * _physicsService.PhysicsGBase.Value;
            Physics2D.gravity = new Vector2(0, y);
        }

        private void Fixed(float dt)
        {
            if (_physicsService.IsPhysics2dUpdateEnabled.Value)
                Physics2D.Simulate(dt);
        }
    }
}