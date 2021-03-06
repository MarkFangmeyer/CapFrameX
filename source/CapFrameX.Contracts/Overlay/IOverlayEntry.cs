﻿using CapFrameX.Contracts.Sensor;
using System;

namespace CapFrameX.Contracts.Overlay
{
    public enum LimitState
    {
        Lower,
        Upper,
        None,
        Undefined
    }

    public interface IOverlayEntry
    {
        string Identifier { get; }

        EOverlayEntryType OverlayEntryType { get; }

        string Description { get; }

        object Value { get; set; }

        string ValueFormat { get; set; }

        string FormattedValue { get; }

        bool ShowOnOverlay { get; set; }

        bool ShowOnOverlayIsEnabled { get; set; }

        string GroupName { get; set; }

        bool ShowGraph { get; set; }

        bool ShowGraphIsEnabled { get; set; }

        /// <summary>
        /// Value standard color
        /// </summary>
        string Color { get; set; }

        int ValueFontSize { get; set; }

        string FormattedGroupName { get; }

        string GroupNameFormat { get; set; }

        string UpperLimitValue { get; set; }

        string LowerLimitValue { get; set; }

        string GroupColor { get; set; }

        int GroupFontSize { get; set; }

        int GroupSeparators { get; set; }

        string UpperLimitColor { get; set; }

        string LowerLimitColor { get; set; }

        bool FormatChanged { get; set; }

        bool IsNumeric { get; set; }

        string ValueAlignmentAndDigits { get; set; }

        string ValueUnitFormat { get; set; }

        LimitState LastLimitState { get; set; }

        Action<string> UpdateGroupName { get; set; }

        Action PropertyChangedAction { set; get; }

        Action<string, bool> UpdateShowOnOverlay { set; get; }

        IOverlayEntry Clone();
    }
}
