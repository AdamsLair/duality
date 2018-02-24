﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Resources;
using Duality.Editor.Plugins.SceneView.Properties;

namespace Duality.Editor.Plugins.SceneView.TreeModels
{
	public abstract class NodeBase : Node
	{
		public enum PrefabLinkState
		{
			None,
			Active,
			Broken
		}

		private PrefabLinkState linkState = PrefabLinkState.None;
		private string linkLastPath = null;
		private bool linkLastExisted = false;

		protected virtual int TypeSortIndex { get { return 0; } }
		public PrefabLinkState LinkState
		{
			get { return this.linkState; }
		}

		public NodeBase(string name) : base(name)
		{
		}
		public bool UpdateLinkState(bool checkFileChanged)
		{
			PrefabLinkState lastState = this.linkState;

			ComponentNode cmpNode = this as ComponentNode;
			GameObjectNode objNode = (cmpNode != null ? cmpNode.Parent : this) as GameObjectNode;

			PrefabLink prefabLink = objNode != null ? objNode.Obj.AffectedByPrefabLink : null;
			bool affectedByPrefabLink = prefabLink != null;
			if (cmpNode != null) affectedByPrefabLink = affectedByPrefabLink && prefabLink.AffectsObject(cmpNode.Component);
			if (objNode != null) affectedByPrefabLink = affectedByPrefabLink && prefabLink.AffectsObject(objNode.Obj);

			string filePath = affectedByPrefabLink ? prefabLink.Prefab.Path : null;
			bool fileExists = this.linkLastExisted;
			if (checkFileChanged || this.linkLastPath != filePath)
				fileExists = File.Exists(filePath);

			// Prefab-linked entities
			if (affectedByPrefabLink && fileExists) //prefabLink.Prefab.IsAvailable) // Not sufficient - might be loaded but with a broken path
				this.linkState = PrefabLinkState.Active;
			else if (cmpNode == null && objNode.Obj.PrefabLink != null)
				this.linkState = PrefabLinkState.Broken;
			else
				this.linkState = PrefabLinkState.None;

			this.linkLastExisted = fileExists;
			this.linkLastPath = filePath;
			return this.linkState != lastState;
		}

		public static int Compare(NodeBase first, NodeBase second)
		{
			return first.TypeSortIndex - second.TypeSortIndex;
		}
	}
	public class GameObjectNode : NodeBase
	{
		private	GameObject	obj				= null;
		private	bool		customIcon		= false;
		private	bool		customIconSet	= false;

		protected override int TypeSortIndex
		{
			get { return 1; }
		}
		public GameObject Obj
		{
			get { return this.obj; }
		}
		public bool UseCustomIcon
		{
			get { return this.customIcon; }
		}
		public bool CustomIconSet
		{
			get { return this.customIconSet; }
		}

		public GameObjectNode(GameObject obj, bool customIcon) : base(obj.Name)
		{
			this.obj = obj;
			this.customIcon = customIcon;
			this.UpdateIcon();
		}
		public void UpdateIcon(Component ignoreComponent = null)
		{
			if (this.customIcon)
			{
				// Find the most unique / significant Component
				Type representant = null;
				int bestScore = int.MinValue;
				foreach (Component component in this.obj.Components)
				{
					if (component == ignoreComponent) continue;
					Type type = component.GetType();
					
					if (type.GetEditorImage() == null)
						continue;

					int requirementScore = Component.RequireMap.GetRequirements(type).Count();
					int matchingNameScore = this.obj.Name.Contains(type.Name) ? 100 : 0;
					int tieBreakerScore = (int)type.Name[0] - (int)' ';
					int score = 1000 * (requirementScore + matchingNameScore) - tieBreakerScore;
					if (score > bestScore)
					{
						bestScore = score;
						representant = type;
					}
				}

				this.customIconSet = (representant != null);
				Image img = (representant ?? typeof(GameObject)).GetEditorImage();
				if (this.LinkState != PrefabLinkState.None)
				{
					img = EditorHelper.GetImageWithOverlay(img, this.LinkState == PrefabLinkState.Active ? 
						SceneViewResCache.OverlayLink : 
						SceneViewResCache.OverlayLinkBroken);
				}
				this.Image = img;
			}
			else
			{
				Image img = typeof(GameObject).GetEditorImage();
				if (this.LinkState != PrefabLinkState.None)
				{
					img = EditorHelper.GetImageWithOverlay(img, this.LinkState == PrefabLinkState.Active ? 
						SceneViewResCache.OverlayLink : 
						SceneViewResCache.OverlayLinkBroken);
				}
				this.Image = img;
			}
		}
	}
	public class ComponentNode : NodeBase
	{
		private	Component	cmp	= null;

		protected override int TypeSortIndex
		{
			get { return 0; }				
		}
		public Component Component
		{
			get { return this.cmp; }
		}

		public ComponentNode(Component cmp) : base(cmp.GetType().Name)
		{
			this.cmp = cmp;
			this.UpdateIcon();
		}
		public void UpdateIcon()
		{
			this.Image = GetTypeImage(cmp.GetType());
		}

		public static Image GetTypeImage(Type type)
		{
			return type.GetEditorImage();
		}
	}
}
