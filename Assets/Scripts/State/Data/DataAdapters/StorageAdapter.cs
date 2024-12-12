using System.Linq;
using Game.State.Models;

namespace Game.State.Data.DataAdapters
{
    public class StorageAdapter : IDataAdapter<StorageModel, StorageData>
    {
        public void DataToModel(in StorageData data, in StorageModel model)
        {
            model.Items.Clear();
            model.Width.Value = data.Width;
            model.Height.Value = data.Height;

            foreach (var dataItem in data.Items)
            {
                var entry = Di.Instance.Get<GameConfig>().ItemParameters.FirstOrDefault(x => x.Id == dataItem.TypeId);
                var item = new StorageItemModel
                {
                    TypeId = { Value = dataItem.TypeId },
                    UId = dataItem.UId,
                    ViewRotation =
                    {
                        Value = dataItem.ViewRotation
                    },
                    ViewPosition =
                    {
                        Value = dataItem.ViewPosition
                    },
                    Mass = { Value = entry.Mass},
                    Scale = { Value = entry.Size}
                };
                model.Items.Add(item);
            }
        }

        public void ModelToData(in StorageModel model, in StorageData data)
        {
            data.Items.Clear();
            data.Width = model.Width.Value;
            data.Height = model.Height.Value;
            foreach (var item in model.Items)
            {
                item.PreSave
                    .Invoke();
                data.Items.Add(new ItemData
                {
                    TypeId = item.TypeId.Value,
                    UId = item.UId,
                    ViewRotation = item.ViewRotation.Value,
                    ViewPosition = item.ViewPosition.Value,
                });
            }
        }
    }
}