using Game.Services;
using Game.State.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Signals
{
    public class UIViewSignals
    {
        public struct OpenPopupRequest<T> where T : IUIView
        {
        }

        public struct ClosePopupRequest<T> where T : IUIView
        {
        }

        public struct QuickSaveGameRequest
        {
        }

        public struct QuickLoadGameRequest
        {
        }

        public struct RestartGameRequest
        {
        }

        public class CancelDialogRequest
        {
        }

        public class TogglePause
        {
        }

        public class SetLanguageRequest
        {
            public SetLanguageRequest(SystemLanguage lang)
            {
                Lang = lang;
            }

            public SystemLanguage Lang { get; }
        }

        public class ActualizeBagItemPositionRequest
        {
            public ActualizeBagItemPositionRequest(int uid, Vector3 position, Quaternion rotation)
            {
                Uid = uid;
                Position = position;
                Rotation = rotation;
            }

            public int Uid { get; }
            public Vector3 Position { get; }
            public Quaternion Rotation { get; }
        }

        public class ConsumeItemHeroStorageRequest
        {
            public ConsumeItemHeroStorageRequest(int uid)
            {
                Uid = uid;
            }

            public int Uid { get; }
        }

        public class DropItemHeroStorageRequest
        {
            public DropItemHeroStorageRequest(int uid)
            {
                Uid = uid;
            }

            public int Uid { get; }
        }

        public struct ZoomMapRequest
        {
            public bool Plus { get; }

            public ZoomMapRequest(bool plus)
            {
                Plus = plus;
            }
        }

        public struct ToggleViewShownRequest<T>
        {
        }

        public class HintRequest
        {
            public HintRequest(IModel model)
            {
                Model = model;
            }

            public IModel Model { get; }
        }

        public class UnHintRequest
        {
            public UnHintRequest(IModel model)
            {
                this.model = model;
            }

            public IModel model { get; }
        }

        public struct ToggleInventoryShownRequest
        {
        }

        public struct ToggleMapShownRequest
        {
        }

        public class DialogOptionRequest
        {
            public DialogOptionRequest(int index)
            {
                Index = index;
            }

            public int Index { get; }
        }
    }

    public class WorldViewSignals
    {
        public struct SelectRequest
        {
            public SelectRequest(IModel model)
            {
                Model = model;
            }

            public IModel Model { get; }
        }

        public struct ActionRequest
        {
            public ActionRequest(IModel model)
            {
                Model = model;
            }

            public IModel Model { get; }
        }

        public struct HoverRequest
        {
            public HoverRequest(IModel model)
            {
                Model = model;
            }

            public IModel Model { get; }
        }

        public struct UnHoverRequest
        {
            public UnHoverRequest(IModel model)
            {
                Model = model;
            }

            public IModel Model { get; }
        }

        public struct GroundClick
        {
            public GroundClick(PointerEventData.InputButton button, Vector3 position)
            {
                Button = button;
                Position = position;
            }

            public PointerEventData.InputButton Button { get; }
            public Vector3 Position { get; }
        }
    }
}