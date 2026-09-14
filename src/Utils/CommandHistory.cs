using System;
using System.Collections.Generic;
using System.Linq;
using PromptStructTool.Models;

namespace PromptStructTool.Utils
{
    /// <summary>
    /// Manages undo/redo history using the Command pattern
    /// </summary>
    public class CommandHistory
    {
        private readonly Stack<PromptData> undoStack = new();
        private readonly Stack<PromptData> redoStack = new();
        private int maxHistorySize = 50;

        /// <summary>
        /// Record a state change
        /// </summary>
        public void Record(PromptData state)
        {
            undoStack.Push(state.Clone());
            redoStack.Clear(); // Clear redo stack when new command is issued

            // Limit history size to prevent memory bloat
            while (undoStack.Count > maxHistorySize)
            {
                var items = undoStack.ToList();
                undoStack.Clear();
                for (int i = items.Count - 1; i >= 1; i--)
                {
                    undoStack.Push(items[i]);
                }
            }
        }

        /// <summary>
        /// Check if undo is available
        /// </summary>
        public bool CanUndo => undoStack.Count > 0;

        /// <summary>
        /// Check if redo is available
        /// </summary>
        public bool CanRedo => redoStack.Count > 0;

        /// <summary>
        /// Undo to previous state
        /// </summary>
        public PromptData? Undo()
        {
            if (!CanUndo) return null;

            var state = undoStack.Pop();
            redoStack.Push(state);
            return state;
        }

        /// <summary>
        /// Redo to next state
        /// </summary>
        public PromptData? Redo()
        {
            if (!CanRedo) return null;

            var state = redoStack.Pop();
            undoStack.Push(state);
            return state;
        }

        /// <summary>
        /// Clear all history
        /// </summary>
        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}
