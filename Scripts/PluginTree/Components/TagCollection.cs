﻿using Godot;
using PluginManager.Editor;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PluginManager.PluginTree.Components
{
    public class TagCollection : BaseOptional
    {
        private List<Tag> _TagList = new();
        public ReadOnlyCollection<Tag> TagList => _TagList.AsReadOnly();

        public void SetTagList(List<Tag> newTagList)
        {
            _TagList = newTagList;
        }

        public void AddTag(Tag tag)
        {
            if (!HasTag(tag))
            {
                _TagList.Add(tag);
                tag.Connect(nameof(Tag.Deleting), this, nameof(RemoveTag), new Godot.Collections.Array(tag));
                tag.Connect(nameof(Tag.VisibilityChanged), this, nameof(OnTagVisibilityChanged));
                DeferredUpdateTreeItems();
            }
        }

        public bool HasTag(Tag tag)
        {
            return _TagList.Contains(tag);
        }

        public void RemoveTag(Tag tag)
        {
            tag.Disconnect(nameof(Tag.Deleting), this, nameof(RemoveTag));
            tag.Disconnect(nameof(Tag.VisibilityChanged), this, nameof(OnTagVisibilityChanged));
            _TagList.Remove(tag);
            DeferredUpdateTreeItems();
        }

        public void ToggleTag(Tag tag)
        {
            if (!HasTag(tag))
                AddTag(tag);
            else
                RemoveTag(tag);
        }

        private void DeferredUpdateTreeItems()
        {
            TreeEntity.DeferredUpdateTreeItem();
            if (TreeEntity is TreeFolder treeFolder)
                treeFolder.DeferredUpdateTreeItemChildren();
        }

        public override void ModifyTreeItem(TreeItem treeItem)
        {
            TreeEntity.Dimmed = (_TagList.Count > 0);
            foreach (Tag tag in _TagList)
            {
                if (tag.Visible)
                {
                    TreeEntity.Dimmed = false;
                    break;
                }
            }
        }

        public override string GetName() => "Tag Collection";

        public override string SerializeIdentifier() => "tag";

        public override void GenerateProperties()
        {
            base.GenerateProperties();
            if (!Active)
                return;
            Tree tree = new()
            {
                HideRoot = true,
                RectMinSize = new Vector2(0, 128)
            };
            tree.Connect("button_pressed", this, nameof(OnTreeButtonPressed));
            TreeItem root = tree.CreateItem();
            foreach (Tag tag in _TagList)
            {
                TreeItem newItem = tree.CreateItem(root);
                newItem.SetText(0, tag.Name);
                newItem.AddButton(0, Resources.ICON_REMOVE);
                newItem.SetMetadata(0, tag);
            }
            EditorServer.Instance.AddProperty(tree);
        }

        private void OnTreeButtonPressed(TreeItem item, int column, int id)
        {
            if (item.GetMetadata(0) is Tag tag)
            {
                RemoveTag(tag);
                TreeEntity.DeferredGenerateProperties();
            }
        }

        private void OnTagVisibilityChanged(bool visible)
        {
            DeferredUpdateTreeItems();
        }

        public override void Serialize(JObject jobj, TreeEntityLookup TEL)
        {
            base.Serialize(jobj, TEL);
            if (_TagList.Count > 0)
            {
                jobj.Add(
                    "tags",
                    new JArray(
                        _TagList.Select<Tag, int>(tag => PluginServer.Instance.TagList.IndexOf(tag))
                    )
                );
            }
        }

        public override void Deserialize(JObject jobj, TreeEntityLookup TEL)
        {
            base.Deserialize(jobj, TEL);
            if (jobj.ContainsKey("tags"))
            {
                JArray jTagList = (JArray)jobj["tags"];
                foreach (int jTagID in jTagList.Select(v => (int)v))
                {
                    _TagList.Add(PluginServer.Instance.TagList[jTagID]);
                }
            }
        }

        public override Component Clone()
        {
            TagCollection newComp = new();
            newComp.SetTagList(new List<Tag>(this._TagList));
            return newComp;
        }
    }
}
