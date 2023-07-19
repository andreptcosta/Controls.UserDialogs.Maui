﻿using Foundation;

using Microsoft.Maui.Platform;

using UIKit;

namespace Maui.Controls.UserDialogs;

public class ActionSheetBuilder
{
    public static double DefaultOptionIconSize { get; set; } = 24;
    public static double DefaultDestructiveIconSize { get; set; } = 24;
    public static double DefaultCancelIconSize { get; set; } = 24;

    public double OptionIconSize { get; set; } = DefaultOptionIconSize;
    public double DestructiveIconSize { get; set; } = DefaultDestructiveIconSize;
    public double CancelIconSize { get; set; } = DefaultCancelIconSize;

    public virtual UIAlertController Build(ActionSheetConfig config)
    {
        var alert = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);

        config.Options.ToList().ForEach(o => alert.AddAction(GetOptionAction(config, o)));

        alert.AddAction(GetDestructiveAction(config));
        alert.AddAction(GetCancelAction(config));

        alert.SetValueForKey(GetTitle(config), new NSString("attributedTitle"));

        alert.SetValueForKey(GetMessage(config), new NSString("attributedMessage"));

        if (config.UserInterfaceStyle is not null)
        {
            alert.OverrideUserInterfaceStyle = config.UserInterfaceStyle.Value.ToNative();
        }

        return alert;
    }

    protected virtual NSAttributedString GetTitle(ActionSheetConfig config)
    {
        UIFont titleFont;
        if (config.FontFamily is null)
        {
            titleFont = UIFont.SystemFontOfSize(config.TitleFontSize, UIFontWeight.Bold);
        }
        else titleFont = UIFont.FromName(config.FontFamily, config.TitleFontSize);

        var attributedString = new NSMutableAttributedString(config.Title, titleFont, config.TitleColor?.ToPlatform());

        return attributedString;
    }

    protected virtual NSAttributedString GetMessage(ActionSheetConfig config)
    {
        UIFont messageFont;
        if (config.FontFamily is null)
        {
            messageFont = UIFont.SystemFontOfSize(config.MessageFontSize);
        }
        else messageFont = UIFont.FromName(config.FontFamily, config.MessageFontSize);

        var attributedString = new NSMutableAttributedString(config.Message, messageFont, config.MessageColor?.ToPlatform());

        return attributedString;
    }

    protected virtual UIAlertAction GetOptionAction(ActionSheetConfig config, ActionSheetOption option)
    {
        var action = UIAlertAction.Create(option.Text, UIAlertActionStyle.Default, x => option.Action?.Invoke());

        if (config.ActionSheetOptionTextColor is not null)
        {
            action.SetValueForKey(config.ActionSheetOptionTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        if (option.Icon is not null)
        {
            var img = new UIImage(option.Icon).ScaleTo(OptionIconSize);
            action.SetValueForKey(img, new NSString("image"));
        }

        return action;
    }

    protected virtual UIAlertAction GetDestructiveAction(ActionSheetConfig config)
    {
        var action = UIAlertAction.Create(config.Destructive.Text, UIAlertActionStyle.Destructive, x => config.Destructive.Action?.Invoke());

        if (config.DestructiveButtonTextColor is not null)
        {
            action.SetValueForKey(config.DestructiveButtonTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        if (config.Destructive.Icon is not null)
        {
            var img = new UIImage(config.Destructive.Icon).ScaleTo(DestructiveIconSize);
            action.SetValueForKey(img, new NSString("image"));
        }

        return action;
    }

    protected virtual UIAlertAction GetCancelAction(ActionSheetConfig config)
    {
        var action = UIAlertAction.Create(config.Cancel.Text, UIAlertActionStyle.Cancel, x => config.Cancel.Action?.Invoke());

        if (config.NegativeButtonTextColor is not null)
        {
            action.SetValueForKey(config.NegativeButtonTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        if (config.Cancel.Icon is not null)
        {
            var img = new UIImage(config.Cancel.Icon).ScaleTo(CancelIconSize);
            action.SetValueForKey(img, new NSString("image"));
        }

        return action;
    }
}