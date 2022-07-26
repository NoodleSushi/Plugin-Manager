using Godot;

namespace PluginManager
{
    public static class Resources
    {
        public static readonly Texture ICON_FOLDER = GD.Load<Texture>("res://icons/Folder.svg");
        public static readonly Texture ICON_FOLDER_CLOSE = GD.Load<Texture>("res://icons/TreeFolderClose.png");
        public static readonly Texture ICON_FOLDER_OPEN = GD.Load<Texture>("res://icons/TreeFolderOpen.png");
        public static readonly Texture ICON_NODE = GD.Load<Texture>("res://icons/Node.svg");
        public static readonly Texture ICON_LIVE = GD.Load<Texture>("res://icons/Live.png");
        public static readonly Texture ICON_FL = GD.Load<Texture>("res://icons/FL.png");
        public static readonly Texture ICON_BOX_OPEN = GD.Load<Texture>("res://icons/BoxOpen.svg");
        public static readonly Texture ICON_BOX_CLOSE = GD.Load<Texture>("res://icons/BoxClose.svg");
        public static readonly Texture ICON_REMOVE = GD.Load<Texture>("res://icons/Remove.svg");
        public static readonly Texture ICON_ADD = GD.Load<Texture>("res://icons/Add.svg");
        public static readonly Texture ICON_SEPARATOR = GD.Load<Texture>("res://icons/Separator.png");
        public static readonly Texture ICON_TAG = GD.Load<Texture>("res://icons/Groups.svg");
        public static readonly Texture ICON_VISIBLE_ON = GD.Load<Texture>("res://icons/VisibleOn.svg");
        public static readonly Texture ICON_VISIBLE_OFF = GD.Load<Texture>("res://icons/VisibleOff.svg");
    }
}
