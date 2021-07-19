using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class MultiSelectionPromptTests
    {
        private class CustomItem
        {
            public int X { get; set; }
            public int Y { get; set; }

            public class Comparer : IEqualityComparer<CustomItem>
            {
                public bool Equals(CustomItem x, CustomItem y)
                {
                    return x.X == y.X && x.Y == y.Y;
                }

                public int GetHashCode([DisallowNull] CustomItem obj)
                {
                    throw new NotImplementedException();
                }
            }
        }

        [Fact]
        public void Should_Not_Mark_Item_As_Selected_By_Default()
        {
            // Given
            var prompt = new MultiSelectionPrompt<int>();

            // When
            var choice = prompt.AddChoice(32);

            // Then
            choice.IsSelected.ShouldBeFalse();
        }

        [Fact]
        public void Should_Mark_Item_As_Selected()
        {
            // Given
            var prompt = new MultiSelectionPrompt<int>();
            var choice = prompt.AddChoice(32);

            // When
            prompt.Select(32);

            // Then
            choice.IsSelected.ShouldBeTrue();
        }

        [Fact]
        public void Should_Mark_Custom_Item_As_Selected_If_The_Same_Reference_Is_Used()
        {
            // Given
            var prompt = new MultiSelectionPrompt<CustomItem>();
            var item = new CustomItem { X = 18, Y = 32 };
            var choice = prompt.AddChoice(item);

            // When
            prompt.Select(item);

            // Then
            choice.IsSelected.ShouldBeTrue();
        }

        [Fact]
        public void Should_Mark_Custom_Item_As_Selected_If_A_Comparer_Is_Provided()
        {
            // Given
            var prompt = new MultiSelectionPrompt<CustomItem>(new CustomItem.Comparer());
            var choice = prompt.AddChoice(new CustomItem { X = 18, Y = 32 });

            // When
            prompt.Select(new CustomItem { X = 18, Y = 32 });

            // Then
            choice.IsSelected.ShouldBeTrue();
        }
        
        [Fact]
        public void Should_Mark_Multiple_Custom_Items_As_Selected_If_The_Same_Reference_Is_Used()
        {
            // Given
            var prompt = new MultiSelectionPrompt<CustomItem>();
            var item1 = new CustomItem { X = 18, Y = 32 };
            var item2 = new CustomItem { X = 19, Y = 33 };
            var choice1 = prompt.AddChoice(item1);
            var choice2 = prompt.AddChoice(item2);

            // When
            prompt.Select(item1, item2);

            // Then
            choice1.IsSelected.ShouldBeTrue();
            choice2.IsSelected.ShouldBeTrue();
        }
        
        [Fact]
        public void Should_Mark_Multiple_Custom_Items_As_Selected_If_The_Same_Reference_Is_Used_Using_The_Enumerable()
        {
            // Given
            var prompt = new MultiSelectionPrompt<CustomItem>();
            var item1 = new CustomItem { X = 18, Y = 32 };
            var item2 = new CustomItem { X = 19, Y = 33 };
            var choice1 = prompt.AddChoice(item1);
            var choice2 = prompt.AddChoice(item2);

            // When
            prompt.Select(new[]{item1, item2});

            // Then
            choice1.IsSelected.ShouldBeTrue();
            choice2.IsSelected.ShouldBeTrue();
        }
        
    }
}
