using Game.Services;

namespace Game.Rule.World
{
    public class UpdateWorldResourcesRule
    {
        private readonly UpdateProvider _updateProvider;
        private readonly WorldResourcesService _worldResourcesService;

        public UpdateWorldResourcesRule(UpdateProvider updateProvider, WorldResourcesService worldResourcesService)
        {
            _updateProvider = updateProvider;
            _worldResourcesService = worldResourcesService;
            _updateProvider.OnTick.Subscribe(Callback);
        }

        private void Callback(float obj)
        {
            foreach (var worldResourceModel in _worldResourcesService) worldResourceModel.Resources.Update(obj);
        }
    }
}