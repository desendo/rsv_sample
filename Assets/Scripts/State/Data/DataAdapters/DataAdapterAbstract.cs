namespace Game.State.Data.DataAdapters
{
    public interface IDataAdapter<TModel, TData>
    {
        void DataToModel(in TData data, in TModel model);
        void ModelToData(in TModel model, in TData data);
    }
}