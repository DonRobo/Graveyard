using System;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace HDT.Plugins.Graveyard
{
	public class DiscardView : NormalView
	{
		private Dictionary<Card, HearthstoneTextBlock> _chances;

		public static bool isValid()
		{
			return Core.Game.Player.PlayerCardList.FindIndex(card => card.Id == HearthDb.CardIds.Collectible.Warlock.CruelDinomancer) > -1;
		}

		public DiscardView()
		{
			// Section Label
			Label.Text = "Discarded";

			_chances = new Dictionary<Card, HearthstoneTextBlock>();
		}

		new public bool Update(Card card)
		{
			if (!base.Update(card))
			{
				return false;
			}

			// Silverware Golem and Clutchmother Zaras are still counted as discarded, even when their effects trigger
			var count = (double)Cards.Aggregate(0, (total, c) => total + c.Count);
			for (var i = 0; i < Cards.Count(); i++)
			{
				if (!_chances.ContainsKey(Cards[i]))
				{
					var chance = new HearthstoneTextBlock();
					chance.FontSize = 18;
					chance.TextAlignment = TextAlignment.Left;
					var grid = (View.Items.GetItemAt(i) as UserControl).Content as Grid;
					grid.Width = 260;
					(grid.Children[0] as Rectangle).HorizontalAlignment = HorizontalAlignment.Right;
					(grid.Children[1] as Rectangle).Width = 260;
					grid.Children.Add(chance);
					_chances.Add(Cards[i], chance);
				}

				_chances[Cards[i]].Text = $"{Math.Round(Cards[i].Count / count * 100)}%";
			}

			return true;
		}
	}
}
